using System;
using System.Collections.Generic;
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

namespace Website.Controllers
{
    [HandleError, Authorize]
    public class PhotoController : Controller
    {
        //
        // GET: /Photo/

        public ActionResult Index()
        {
            var viewModels = SharedDataService.GetAllPlansAlbumsWithAlbumId();
            return View(viewModels);
        }
        private const string SessionPhotosAttached = "PhotoController_PhotosAttached";
        private const string SessionPhotosUnAttached = "PhotoController_PhotosUnAttached";
        private static ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
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

        public ActionResult SiteAlbum(Guid planAlbumId)
        {
            AttachedPhotos = null;
            UnAttachedPhotos = null;
            PlanSiteAlbumViewModel model = SharedDataService.GetSiteAlbumForPlan(planAlbumId);
            return View(model);
        }

        public ActionResult GetPhotosForSite(Guid siteId)
        {
            
            return Content("");
        }
        public ActionResult GetSitePhotosBySiteId(Guid siteId, Guid planCityId, Guid planDetailsId)
        {

            string photoList = "";
            IEnumerable<PhotoViewModel> photos = SharedDataService.GetSitePhotosBySiteId(siteId);
            
            foreach (var photoViewModel in photos)
            {
                photoList +=
                    @"<li class=""ui-widget-content ui-corner-tr"">
		                        <span class=""ui-widget-header siteImagesli"">" +

                    (photoViewModel.Title.Length > 12 ? photoViewModel.Title.Substring(0, 12) + ".." : photoViewModel.Title) + @"</span> " +
                    @"<img src=""" + Url.Content("~/Resources/Images/SitePhotos/planId_" + planDetailsId + "/" + photoViewModel.PhotoId + "_" + photoViewModel.PhotoName + @"""") + @" id=""" + photoViewModel.PhotoId + @""" alt=""" + photoViewModel.Title + @""" width=""96"" height=""72""  /> " +
                             "</li >";
            }
            if (AttachedPhotos.ContainsKey(siteId))
            {
                foreach (var dto in AttachedPhotos[siteId])
                {
                    photoList +=
                        @"<li class=""ui-widget-content ui-corner-tr"">
		                        <span class=""ui-widget-header siteImagesli"">" +
                       (dto.Title.Length > 12 ? dto.Title.Substring(0, 12) + ".." : dto.Title) + @"</span> " +
                        @"<img src=""" + Url.Content("~/Resources/Images/SitePhotos/planId_" + planDetailsId + "/" + dto.FilePath + @"""") + @" id=""" + dto.PhotoId + @""" alt=""" + dto.Title + @""" width=""96"" height=""72""  /> " +
                                 "</li >";
                }
            }
            return Content(photoList);
        }

        public ActionResult GetUnattachedSitePhotosByCityId(Guid planCityId, Guid planDetailsId)
        {
            IEnumerable<PhotoViewModel> photos = SharedDataService.GetUnattachedPhotosForCity(planCityId);
            string photoList = "";
            foreach (var photoViewModel in photos)
            {
                photoList += @"<li class=""ui-widget-content ui-corner-tr"">
		                        <span class=""ui-widget-header siteImagesli"">" + (photoViewModel.Title.Length > 12 ? photoViewModel.Title.Substring(0, 12) + ".." : photoViewModel.Title) + @"</span> " +
                                @"<img src=""" + Url.Content("~/Resources/Images/SitePhotos/planId_" + planDetailsId + "/" + photoViewModel.PhotoId + "_" + photoViewModel.PhotoName + @"""") + @""" id=""" + photoViewModel.PhotoId + @""" alt=""" + photoViewModel.Title + @""" width=""106"" height=""82"" /> " +
                             "</li >";
            }
            return Content(photoList);
        }
        public ActionResult AttachSite(Guid siteId, string filePath, string title, string tags, Guid photoId, Guid planCityId)
        {
            if (AttachedPhotos.ContainsKey(siteId))
            {
                AttachedPhotos[siteId].Add(new PhotoDetailDTO() { FilePath = filePath, PhotoId = photoId, PlanSiteId = siteId, Tags = tags, Title = title , PlanCityId= planCityId});
            }
            else
            {
                Dictionary<Guid, List<PhotoDetailDTO>> pics = AttachedPhotos;
                var lst = new List<PhotoDetailDTO>();
                lst.Add(new PhotoDetailDTO() { FilePath = filePath, PhotoId = photoId, PlanSiteId = siteId, Tags = tags, Title = title, PlanCityId = planCityId });
                pics.Add(siteId, lst);
                AttachedPhotos = pics;
            }
            return Content("Success");
        }
        public ActionResult UnAttachSite(Guid siteId, string filePath, string title, string tags, Guid photoId, Guid planCityId)
        {
            if (UnAttachedPhotos.ContainsKey(siteId))
            {
                UnAttachedPhotos[siteId].Add(new PhotoDetailDTO() { FilePath = filePath, PhotoId = photoId, PlanSiteId = siteId, Tags = tags, Title = title, PlanCityId= planCityId });
            }
            else
            {
                Dictionary<Guid, List<PhotoDetailDTO>> pics = UnAttachedPhotos;
                var lst = new List<PhotoDetailDTO>();
                lst.Add(new PhotoDetailDTO() { FilePath = filePath, PhotoId = photoId, PlanSiteId = siteId, Tags = tags, Title = title, PlanCityId = planCityId });
                pics.Add(siteId, lst);
                UnAttachedPhotos = pics;
            }
            return Content("Success");
        }
        public ActionResult SaveForm(Guid planAlbumId, Guid planDetailsId, Guid planCityId)
        {
            try
            {
                if(AttachedPhotos.Count==0 && UnAttachedPhotos.Count==0)
                    return Content("Success");
                var command = new AddOrRemovePhotosFromPlanSite();
                command.PlanAlbumId = planAlbumId;
                List<PhotoDetailDTO> photoDetailDtos = new List<PhotoDetailDTO>();
                foreach (var photo in AttachedPhotos)
                {
                    photoDetailDtos.AddRange(photo.Value);
                }
                command.AttachedPhotos = photoDetailDtos;
                foreach (var unAttachedPhoto in UnAttachedPhotos)
                {
                    command.RemovedPhotos =
                        unAttachedPhoto.Value.ConvertAll(
                            x =>
                            new PhotoDetailDTO()
                                {
                                    FilePath = x.FilePath,
                                    PhotoId = x.PhotoId,
                                    PlanSiteId = x.PlanSiteId,
                                    PlanCityId = x.PlanCityId,
                                    Tags = x.Tags,
                                    Title = x.Title
                                });
                }
                if (command.RemovedPhotos == null)
                    command.RemovedPhotos = new List<PhotoDetailDTO>();
                var service = new MyNotesCommandServiceClient();
                service.AddOrRemovePhotosFromPlanSite(command);

                AttachedPhotos = null;
                UnAttachedPhotos = null;
            }
            catch (Exception e)
            {
                _log.Error(e.Message, e);
                throw;
            }
            return Content("Success");
        }

        public ActionResult PlanPhotoViewer(Guid planAlbumId)
        {
            AttachedPhotos = null;
            PlanSiteAlbumViewModel model = SharedDataService.GetSiteAlbumForPlan(planAlbumId);
            return View(model);
        }
        public ActionResult PhotoViewerIndex()
        {
            var viewModels = SharedDataService.GetAllPlansAlbumsWithAlbumId();
            return View(viewModels);
        }

        public ActionResult GetSitePhotosBySiteIdforViewer(Guid siteId, Guid planCityId, Guid planDetailsId)
        {
             string photoList = "";
            IEnumerable<PhotoViewModel> photos = SharedDataService.GetSitePhotosBySiteId(siteId);

            foreach (var photoViewModel in photos)
            {
                string imgId = planDetailsId + "/" + photoViewModel.PhotoId + "_" + photoViewModel.PhotoName;
                photoList +=                   
                   
                    @"<li class=""ui-widget-content ui-corner-tr"">
		                        <span class=""ui-widget-header siteImagesli"">" +
                    (photoViewModel.Title.Length > 12 ? photoViewModel.Title.Substring(0, 12) + ".." : photoViewModel.Title) + @"</span> " +
                    @"<a style=""""height:300px"""" href=""" + Url.Content("~/Resources/Images/SitePhotos/PlanId_"+ imgId  )+ @"""> <img src=""" + Url.Content("~/Resources/Images/SitePhotos/PlanId_" + imgId + @"""") + @" id=""" + photoViewModel.PhotoId + @""" alt=""" + photoViewModel.Title + @""" width=""140"" height=""126""  /> " +
                             "  </a> </li >";
               
            }
            return Content(photoList);
        }
        public ActionResult GetSiteListForCity(Guid planCityId)
        {
            Dictionary<Guid, string> photos = SharedDataService.GetSitesForCity(planCityId);
            string siteList = "";
            foreach (var photo in photos)
            {
                siteList += @"<div style='vertical-align:baseline; height:2em; width:100%; margin:0 0 0 5px' id='div_"+ photo.Key +@"'>  <a href= javascript:GetPhotosForSite('" +photo.Key + @"')> "+ photo.Value + "</a>  </div> <br />";
            }
            return Content(siteList);
        }
    }
}
