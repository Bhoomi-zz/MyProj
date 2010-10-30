using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Commands;
using CommonDTOs;
using Ncqrs;
using Website.CommandService;
using Website.Services;
using Website.ViewModel.Photo;
using System.IO;

namespace Website.Controllers
{
    [HandleError, Authorize]
    public class PhotoUploadingController : Controller
    {
        //
        // GET: /PhotoUploading/
        private static ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public ActionResult UploadIndex()
        {
            var viewModels = SharedDataService.GetAllPlansAlbumsWithAlbumId();
            return View("UploadIndex",viewModels);
        }
        private const string SessionPhotosAttached = "PhotoUploadingContoroller_PhotosAttached";
        private const string SessionPhotosUnAttached = "PhotoUploadingController_PhotosUnAttached";
        private const string SessionPhotosUploaded = "PhotoUploadingController_PhotosUploaded";
        private const string SessionPhotosRemoved = "PhotoUploadingController_PhotosRemoved";

        private Dictionary<Guid, List<PhotoDetailDTO>> AttachedPhotos
        {
            get
            {
                if (Session[SessionPhotosAttached] != null)
                    return (Dictionary<Guid, List<PhotoDetailDTO>>)Session[SessionPhotosAttached];
                else
                    return new Dictionary<Guid, List<PhotoDetailDTO>>();
            }
            set
            {
                Session[SessionPhotosAttached] = value;
            }
        }
        private List<PhotoDetailDTO> UploadedPhotos
        {
            get
            {
                if (Session[SessionPhotosUploaded] != null)
                    return  (List<PhotoDetailDTO>)Session[SessionPhotosUploaded];
                else
                    return new List<PhotoDetailDTO>();
            }
            set
            {
                Session[SessionPhotosUploaded] = value;
            }
        }
        private List<PhotoDetailDTO> RemovedPhotos
        {
            get
            {
                if (Session[SessionPhotosRemoved] != null)
                    return (List<PhotoDetailDTO>)Session[SessionPhotosRemoved];
                else
                    return new List<PhotoDetailDTO>();
            }
            set
            {
                Session[SessionPhotosRemoved] = value;
            }
        }
        private Dictionary<Guid, List<PhotoDetailDTO>> UnAttachedPhotos
        {
            get
            {
                if (Session[SessionPhotosUnAttached] != null)
                    return (Dictionary<Guid, List<PhotoDetailDTO>>)Session[SessionPhotosUnAttached];
                else
                    return new Dictionary<Guid, List<PhotoDetailDTO>>();
            }
            set
            {
                Session[SessionPhotosUnAttached] = value;
            }
        }
        public ActionResult UploadPhotoToCity(Guid planDetailId, Guid planAlbumId)
        {
            AttachedPhotos = null;
            UnAttachedPhotos = null;
            UploadedPhotos = null;

            PlanPhotoUploadingViewModel model = SharedDataService.GetPlanForPhotoUploading(planDetailId, planAlbumId);
            model.sessionId = new Random().Next(120000);
            model.Regions = SharedDataService.GetAllRegionsForPlan(planDetailId);
            model.Cities = SharedDataService.GetAllCitiesForPlanForPhoto(planDetailId,"<All>").ToList();
            return View(model);
        }

        public ActionResult GetCityPhotosByCityId(Guid planDetailsId, Guid planCityId, int sessionId)
        {
            string photoList = "";
            IEnumerable<PhotoViewModel> photos = SharedDataService.GetCityPhotosByCityId(planCityId);
            
            //Photos from db
            foreach (var photoViewModel in photos)
            {
                if(UnAttachedPhotos.ContainsKey(planCityId))
                {
                    if(UnAttachedPhotos[planCityId].Find(x=> x.PhotoId == photoViewModel.PhotoId) != null)
                        continue;
                }
                photoList +=
                    @"<li class=""ui-widget-content ui-corner-tr"">
		                        <span class=""ui-widget-header cityImagesli"">" +
                    (photoViewModel.Title.Length > 12 ? photoViewModel.Title.Substring(0, 12) + ".." : photoViewModel.Title) 
                    + @"</span> " +
                    @"<img src=""" + Url.Content("~/Resources/Images/SitePhotos/planId_" + planDetailsId + "/" + photoViewModel.PhotoId  + "_" + photoViewModel.PhotoName + @"""") + @" id=""" + photoViewModel.PhotoId + @""" alt=""" + photoViewModel.Title + @""" width=""96"" height=""72""  /> " +
                             "</li >";
            }
            //Photos attached recently
            if (AttachedPhotos.ContainsKey(planCityId))
            {
                foreach (var dto in AttachedPhotos[planCityId])
                {
                    photoList +=
                        @"<li class=""ui-widget-content ui-corner-tr"">
		                        <span class=""ui-widget-header siteImagesli"">" +
                        (dto.Title.Length > 12 ? dto.Title.Substring(0, 12) + ".." : dto.Title) + @"</span> " +
                        @"<img src=""" + Url.Content("~/Resources/Images/SitePhotos/PlanId_" + planDetailsId + "/" + dto.PhotoId  + "_" + dto.FilePath + @"""") + @" id=""" + dto.PhotoId + @""" alt=""" + dto.Title + @""" width=""96"" height=""72""  /> " + "</li >";
                }
            }
            //Photos uploaded recently
            IEnumerable<PhotoViewModel> newPhotos = SharedDataService.GetAllAttachedPhotosForPlan(planDetailsId);
            List<string> newlyAddedPhotos = Directory.GetFiles(Request.PhysicalApplicationPath +
                new System.Configuration.AppSettingsReader().GetValue("PhotoPath", typeof(string)) + "\\PlanId_" + planDetailsId, "CA_" + sessionId + "_*.*").ToList();

            string picName;

            foreach (string pic in newlyAddedPhotos)
            {
                picName = pic.Substring(pic.LastIndexOf("\\") + 1);
                string[] picNameSplitted = picName.Split('_');
                string picTitle = "";
                for (var s = 0; s < picNameSplitted.Count(); s++)
                {
                    if (s >= 3)
                    {
                        if (picTitle.Length > 0)
                            picTitle += "_";
                        picTitle += picNameSplitted[s];
                    }
                }
                 
                Guid photoId = Guid.Parse(picNameSplitted[2]);
                if (picNameSplitted.Length >= 4)
                {
                    if (AttachedPhotos.ContainsKey(planCityId))
                    {
                        int count = AttachedPhotos[planCityId].Where(x => x.PhotoId == photoId).Count();
                        if (count > 0)
                            continue;
                    }
                    if (UnAttachedPhotos.ContainsKey(planCityId))
                    {
                        int count = UnAttachedPhotos[planCityId].Where(x => x.PhotoId == photoId).Count();
                        if (count > 0)
                            continue;
                    }
                    if (UploadedPhotos.Where(x => x.PhotoId == photoId && x.PlanCityId != planCityId).Count() > 0)
                    {
                        continue;
                    }
                    if (UploadedPhotos.Where(x => x.PhotoId == photoId).Count() == 0)
                    {
                        var lst = UploadedPhotos;
                        lst.Add(new PhotoDetailDTO()
                                    {FilePath = picTitle, PhotoId = photoId, PlanCityId = planCityId, Title = picTitle});
                        UploadedPhotos = lst;
                    }
                }
                photoList += @"<li class=""ui-widget-content ui-corner-tr"">
		                        <span class=""ui-widget-header siteImagesli"">" + (picTitle.Length > 12 ? picTitle.Substring(0, 12) + ".." : picTitle) + @"</span> " +
                                @"<img src=""" + Url.Content("~/Resources/Images/SitePhotos/PlanId_" + planDetailsId + "/CA_" + sessionId + "_" + photoId + "_" + picTitle + @"""") + @" id=""" + photoId + @""" alt=""" + picTitle + @""" width=""106"" height=""82"" /> " + "</li >";
            }
            return Content(photoList);
        }
        public ActionResult GetUnAttachedPhotosForCity(Guid planAlbumId, Guid planDetailId )
        {
            IEnumerable<PhotoViewModel> photos = SharedDataService.GetAllUnAttachedPhotosForCity(planAlbumId);
            string photoList = "";
            foreach (var photoViewModel in photos)
            {
                photoList += @"<li class=""ui-widget-content ui-corner-tr"">
		                        <span class=""ui-widget-header cityImagesli"">" + (photoViewModel.Title.Length > 12 ? photoViewModel.Title.Substring(0, 12) + ".." : photoViewModel.Title) + @"</span> " +
                                @"<img src=""" + Url.Content("~/Resources/Images/SitePhotos/planId_" +  planDetailId + "/" + photoViewModel.PhotoId + "_" + photoViewModel.PhotoName + @"""") + @""" id=""" + photoViewModel.PhotoId + @""" alt=""" + photoViewModel.Title + @""" width=""106"" height=""82"" /> " + "</li >";
               
            }
            return Content(photoList);
        }

        public ActionResult GetUnattachedPlanPhotosByPlanId(Guid planDetailId, string planCityId)
        {
            Guid PlanCityIDVal;
            if (planCityId == "" || planCityId == "undefined")
                PlanCityIDVal = Guid.Empty;
            else
            {
                PlanCityIDVal = Guid.Parse(planCityId);
            }
            IEnumerable<PhotoViewModel> photos = SharedDataService.GetAllAttachedPhotosForPlan(planDetailId);
            List<string> newlyAddedPhotos = Directory.GetFiles(Request.PhysicalApplicationPath + 
                new System.Configuration.AppSettingsReader().GetValue("PhotoPath", typeof (string)) + "\\PlanId_" + planDetailId, "UA_*.*").ToList();
            string photoList = "";
            int i = 0;
            string photoAgid = "";
            foreach (var photoViewModel in photos)
            {
                photoList += @"<li class=""ui-widget-content ui-corner-tr"">
		                        <span class=""ui-widget-header siteImagesli"">" + photoViewModel.Title + @"</span> " +
                                @"<img src=""" + Url.Content("~/Resources/Images/SitePhotos/planId_" + planDetailId + "/" + photoViewModel.PhotoId + "_" + photoViewModel.PhotoName + @"""") + @""" id=""" + photoViewModel.PhotoId + @""" alt=""" + photoViewModel.Title + @""" width=""106"" height=""82"" /> " +"</li >";
                
                photoAgid = photoViewModel.PlanAlbumId.ToString();
            }
            string picName;

            foreach (string pic in newlyAddedPhotos)
            {
                 picName = pic.Substring(pic.LastIndexOf("\\") + 1);
                 string[] picNameSplitted = picName.Split('_');
                 string picTitle = "";
                for (var s = 0; s < picNameSplitted.Count(); s++)
                {
                    if(s>=2)
                    {
                        if (picTitle.Length > 0)
                            picTitle += "_";
                        picTitle += picNameSplitted[s] ;
                    }
                }
                
                if (picNameSplitted.Length >= 3)
                {
                    if (AttachedPhotos.ContainsKey(PlanCityIDVal))
                    {
                        int count = AttachedPhotos[PlanCityIDVal].Where(x => x.PhotoId == Guid.Parse(picNameSplitted[1])).Count();
                        if(count>0)
                            continue;
                    }
                    if (UnAttachedPhotos.Count == 0)
                    {
                        var lst = new List<PhotoDetailDTO>();
                        lst.Add(new PhotoDetailDTO() { FilePath = picTitle, PhotoId = Guid.Parse(picNameSplitted[1]), Title = picTitle });
                        UnAttachedPhotos.Add(Guid.Parse(picNameSplitted[1]), lst);
                    }
                    else
                    {
                        UnAttachedPhotos[planDetailId].Add(new PhotoDetailDTO() { FilePath = picTitle, PhotoId = Guid.Parse(picNameSplitted[1]), Title = picTitle });    
                    }
                }
                photoList += @"<li class=""ui-widget-content ui-corner-tr"">
		                        <span class=""ui-widget-header siteImagesli"">" + picTitle + @"</span> " +
                                @"<img src=""" + Url.Content("~/Resources/Images/SitePhotos/PlanId_" + planDetailId + "/UA_" + picNameSplitted[1] + "_" + picTitle + @"""") + @" id=""" + picNameSplitted[1] + @""" alt=""" + picTitle + @""" width=""106"" height=""82"" /> " + "</li >";
            }

            photoList += @" <input type=""Hidden"" id=""PhotoAgIdForSelectedCity"" value =""" + photoAgid + @"""></input>";
            return Content(photoList);
        }
        public ActionResult AttachCity(Guid planCityId, string filePath, string title, string tags, Guid photoId)
        {
            if (AttachedPhotos.ContainsKey(planCityId))
            {
                AttachedPhotos[planCityId].Add(new PhotoDetailDTO() { FilePath = title, PhotoId = photoId, PlanSiteId = planCityId, Tags = tags, Title = title });
            }
            else
            {
                Dictionary<Guid, List<PhotoDetailDTO>> pics = AttachedPhotos;
                var lst = new List<PhotoDetailDTO>();
                lst.Add(new PhotoDetailDTO() { FilePath = title, PhotoId = photoId, PlanCityId = planCityId, Tags = tags, Title = title });
                pics.Add(planCityId, lst);
                AttachedPhotos = pics;
            }
            return Content("Success");
        }
        public ActionResult UnAttachCity(Guid planCityId, string filePath, string title, string tags, Guid photoId)
        {
            if (UnAttachedPhotos.ContainsKey(planCityId))
            {
                UnAttachedPhotos[planCityId].Add(new PhotoDetailDTO() { FilePath = title, PhotoId = photoId, PlanCityId = planCityId, Tags = tags, Title = title });
            }
            else
            {
                Dictionary<Guid, List<PhotoDetailDTO>> pics = UnAttachedPhotos;
                var lst = new List<PhotoDetailDTO>();
                lst.Add(new PhotoDetailDTO() { FilePath = title, PhotoId = photoId, PlanCityId = planCityId, Tags = tags, Title = title });
                pics.Add(planCityId, lst);
                UnAttachedPhotos = pics;
            }
            return Content("Success");
        }


        public ActionResult SaveForm(Guid planAlbumId, Guid planDetailsId, int sessionId)
        {
            try
            {
                if (AttachedPhotos.Count > 0 || UnAttachedPhotos.Count > 0 || UploadedPhotos.Count > 0)
                {
                    var service = new MyNotesCommandServiceClient();
                    var command = new AddOrRemovePhotosFromPlanCity();
                    command.PlanAlbumId = planAlbumId;
                    var attachedPics = new List<PhotoDetailDTO>();

                    foreach (var attachedPhoto in AttachedPhotos)
                    {
                        foreach (var photoDetailDto in attachedPhoto.Value)
                        {
                            attachedPics.Add(new PhotoDetailDTO()
                                                 {
                                                     FilePath = photoDetailDto.FilePath,
                                                     PhotoId = photoDetailDto.PhotoId,
                                                     PlanCityId = attachedPhoto.Key,
                                                     Tags = photoDetailDto.Tags,
                                                     Title = photoDetailDto.Title
                                                 });
                        }
                    }
                    command.AttachedPhotos = attachedPics;
                    var removedPics = new List<PhotoDetailDTO>();
                    foreach (var unattachedPhoto in UnAttachedPhotos)
                    {
                        foreach (var photoDetailDto in unattachedPhoto.Value)
                        {
                            removedPics.Add(new PhotoDetailDTO()
                                                {
                                                    FilePath = photoDetailDto.FilePath,
                                                    PhotoId = photoDetailDto.PhotoId,
                                                    PlanCityId = unattachedPhoto.Key,
                                                    Tags = photoDetailDto.Tags,
                                                    Title = photoDetailDto.Title
                                                });
                        }
                    }
                    command.RemovedPhotos = removedPics;
                    command.UploadedPhotos = UploadedPhotos;
                    service.AddOrRemovePhotosFromCity(command);

                    string strAppPath = Request.PhysicalApplicationPath +
                                        new System.Configuration.AppSettingsReader().GetValue("PhotoPath",
                                                                                              typeof (string)).
                                            ToString() + "\\PlanId_" + planDetailsId + "\\";
                    foreach (var photoDetailDto in UploadedPhotos)
                    {
                        System.IO.File.Copy(
                            strAppPath + @"\CA_" + sessionId + "_" + photoDetailDto.PhotoId + "_" +
                            photoDetailDto.FilePath,
                            strAppPath + @"\" + photoDetailDto.PhotoId + "_" + photoDetailDto.FilePath);
                        System.IO.File.Delete(strAppPath + @"\CA_" + sessionId + "_" + photoDetailDto.PhotoId + "_" +
                                              photoDetailDto.FilePath);
                    }
                    AttachedPhotos = null;
                    UnAttachedPhotos = null;
                    UploadedPhotos = null;
                }
            }
            catch (Exception e)
            {
                _log.Error(e.Message, e);
                throw;
            }
            return Content("Success");
        }

        public ActionResult UploadPhoto()
       {
           string saveLocation = string.Empty;
           string fileName = string.Empty;
           string subfolder = string.Empty;
           try
           {
               int length = 4096;
               int bytesRead = 0;
               Byte[] buffer = new Byte[length];

               //This works with Chrome/FF/Safari
               // get the name from qqfile url parameter here

               subfolder = Path.Combine(@"Uploads", Request["subfolder"]);
               saveLocation = Server.MapPath(Path.Combine(Request.ApplicationPath, subfolder));

               if (String.IsNullOrEmpty(Request["qqfile"]))
               {
                   // IE
                   fileName = Path.Combine(saveLocation, System.IO.Path.GetFileName(Request.Files[0].FileName));
               }
               else
               {
                   //Webkit, Mozilla
                   fileName = Path.Combine(saveLocation, Request["qqfile"]);
               }

               try
               {
                   if (!Directory.Exists(saveLocation))
                       Directory.CreateDirectory(saveLocation);


                   using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
                   {
                       do
                       {
                           bytesRead = Request.InputStream.Read(buffer, 0, length);
                           fileStream.Write(buffer, 0, bytesRead);
                       }
                       while (bytesRead > 0);
                   }
               }
               catch (UnauthorizedAccessException ex)
               {
                   // log error hinting to set the write permission of ASPNET or the identity accessing the code
                   return Json(new { success = false, message = ex.Message }, "application/json");
               }
           }
           catch (Exception ex)
           {
               return Json(new { success = false, message = ex.Message }, "application/json");
           }

           return Json(new { success = true }, "application/json");
           return Content("");
       }

        public ActionResult UploadPhotoToPlan(Guid planDetailsId, Guid planAlbumId)
        {
            AttachedPhotos = null;
            PlanPhotoUploadingViewModel model = SharedDataService.GetPlanForPhotoUploading(planDetailsId, planAlbumId);
            model.Regions = SharedDataService.GetAllRegionsForPlan(planDetailsId);
            
            return View(model);
        }

        public ActionResult SaveUploadedPhotosToPlan(Guid planDetailsId, Guid planAlbumId )
        {
            try
            {
                if (UploadedPhotos.Count == 0 && RemovedPhotos.Count == 0)
                    return Content("Success");
                var service = new MyNotesCommandServiceClient();
                var command = new AddOrRemovePhotosFromPlanAlbum();
                command.PlanAlbumId = planAlbumId;
                command.AttachedPhotos = UploadedPhotos;
                command.RemovedPhotos = RemovedPhotos;
                service.AddOrRemovePhotosFromPlanAlbum(command);

                string strAppPath = Request.PhysicalApplicationPath +
                                    new System.Configuration.AppSettingsReader().GetValue("PhotoPath", typeof (string)).
                                        ToString() + "\\PlanId_" + planDetailsId + "\\";
                foreach (var photoDetailDto in UploadedPhotos)
                {
                    System.IO.File.Copy(strAppPath + @"\UA_" + photoDetailDto.PhotoId + "_" + photoDetailDto.FilePath,
                                        strAppPath + @"\" + photoDetailDto.PhotoId + "_" + photoDetailDto.FilePath);
                    System.IO.File.Delete(strAppPath + @"\UA_" + photoDetailDto.PhotoId + "_" + photoDetailDto.FilePath);
                }
                foreach (var removedPhoto in RemovedPhotos)
                {
                    System.IO.File.Delete(strAppPath + @"\" + removedPhoto.PhotoId + "*.*");
                }
                //viewModel = SharedDataService.GetPlanForPhotoUploading(viewModel.planDetailsId, viewModel.PlanAlbumId);
                UploadedPhotos = null;
                RemovedPhotos = null;
            }
            catch (Exception e)
            {
                _log.Error(e.Message, e);
                return Content("Failure");
            }
            return Content("Success");
        }

        public ActionResult GetAllPhotosforPlan(Guid planDetailId)
        {
            Guid PlanCityIDVal;
            IEnumerable<PhotoViewModel> photos = SharedDataService.GetAllAttachedPhotosForPlan(planDetailId);
            string path = Request.PhysicalApplicationPath +
                                             new System.Configuration.AppSettingsReader().GetValue("PhotoPath",
                                                                                                   typeof (string));
            if (!Directory.Exists(path + "\\PlanId_" + planDetailId))
            {
                return Content("");
            }
            List<string> newlyAddedPhotos = Directory.GetFiles(path + "\\PlanId_" + planDetailId, "UA_*.*").ToList();
                string photoList = "";
                int i = 0;

                foreach (var photoViewModel in photos)
                {
                    var val = RemovedPhotos.Find(x => x.PhotoId.ToString() == photoViewModel.PhotoId.ToString());
                    if (val != null)
                    {
                        Guid id = val.PhotoId;
                        if (RemovedPhotos.Count > 0 && id != Guid.Empty)
                        {
                            continue;
                        }
                    }
                    photoList +=
                        @"<li class=""ui-widget-content ui-corner-tr""> <span class=""ui-widget-header siteImagesli"">" + (photoViewModel.Title.Length > 12
                             ? photoViewModel.Title.Substring(0, 12) + ".."
                             : photoViewModel.Title) +
                            @"</span>  <img style=""float:right"" src=""" + Url.Content(@"~/Resources/Images//Icons//close.gif""") + @""" alt=""close"" onclick=""removeImage(this)"" ></img>" +
                             @"<img src=""" + Url.Content("~/Resources/Images/SitePhotos/planId_" + planDetailId + "/" + photoViewModel.PhotoId + "_" + photoViewModel.PhotoName + @"""") + @""" id=""" +
                                    photoViewModel.PhotoId + @""" alt=""" + photoViewModel.Title +
                                    @""" width=""120"" height=""92"" /> " + "</li >";
                }
            
            string picName;
            var lst = new List<PhotoDetailDTO>();
            foreach (string pic in newlyAddedPhotos)
            {
                 picName = pic.Substring(pic.LastIndexOf("\\") + 1);
                 string[] picNameSplitted = picName.Split('_');
                 string picTitle = "";
                for (var s = 0; s < picNameSplitted.Count(); s++)
                {
                    if(s>=2)
                    {
                        if (picTitle.Length > 0)
                            picTitle += "_";
                        picTitle += picNameSplitted[s] ;
                    }
                }
                Guid photoid = Guid.Parse(picNameSplitted[1]);
                if (picNameSplitted.Length >= 3)
                {
                    lst.Add(new PhotoDetailDTO() { FilePath = picTitle, PhotoId = Guid.Parse(picNameSplitted[1]), Title = picTitle });    
                }
                photoList += @"<li class=""ui-widget-content ui-corner-tr"">
		                        <span class=""ui-widget-header siteImagesli"">" + (picTitle.Length > 12 ? picTitle.Substring(0, 12) + ".." : picTitle) + @"</span> " +
                                @"<img style=""float:right"" src=""" + Url.Content(@"~/Resources/Images//Icons//close.gif""") + @""" alt=""close"" onclick=""removeImage(this)"" ></img>" +
                                @"<img src=""" + Url.Content("~/Resources/Images/SitePhotos/PlanId_" + planDetailId + "/UA_" + picNameSplitted[1] + "_" + picTitle + @"""") + @" id=""" + picNameSplitted[1] + @""" alt=""" + picTitle + @""" width=""120"" height=""92"" /> " + "</li >";
            }
            UploadedPhotos = lst;
            return Content(photoList);
        }

        public ActionResult RemovePhoto(Guid photoId)
        {
            List<PhotoDetailDTO> lst = RemovedPhotos;
            lst.Add(new PhotoDetailDTO(){PhotoId = photoId});
            RemovedPhotos = lst;
            PhotoDetailDTO dto = UploadedPhotos.Find(x => x.PhotoId == photoId);
            if(dto != null)
            {
                UploadedPhotos.Remove(dto);
            }
            return Content("Success");
        }
        public ActionResult PlansWithoutAlbumsIndex()
        {
            IEnumerable<PlanAlbumViewModel> model = SharedDataService.GetAllPlansWhoseAlbumIsNotCreated();
            return View(model);
        }

        public ActionResult CreatePlanAlbum(Guid planDetailsId)
        {
            PlanAlbumViewModel albumViewModel = SharedDataService.GetPlanAlbumViewModelByPlanId(planDetailsId);
            return View(albumViewModel);
        }

        [HttpPost]
        public ActionResult CreatePlanAlbum(PlanAlbumViewModel planAlbumViewModel)
        {
            var service = new MyNotesCommandServiceClient();
            var command = new CreatePlanAlbum();
            command.PlanDetailId = planAlbumViewModel.PlanDetailsId;
            command.AttachedPhotos = UploadedPhotos;
            service.CreatePlanAlbum(command);
           // planAlbumViewModel = SharedDataService.GetPlanAlbumViewModelByPlanId(planAlbumViewModel.PlanDetailsId);
            return UploadIndex();
        }
    }
}
