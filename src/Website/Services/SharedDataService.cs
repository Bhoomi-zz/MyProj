using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReadModel;
using Website.ViewModel;
using Website.ViewModel.Plan;
using PlanSite = ReadModel.PlanSite;
using Website.ViewModel.Photo;
using Website.ViewModel.Email;
namespace Website.Services
{
    public static class SharedDataService
    {
        public static IEnumerable<bmContact> GetAllCustomers()
        {
            IEnumerable<bmContact> customers;
            using (var context = new MyNotesReadModelEntities())
            {
                var query = from item in context.bmContacts
                            orderby item.sName
                            select item;

                customers = query.ToArray();
            }
            return customers;
        }
        
        internal static Dictionary<string, string> GetContactPersons(string filterValue, string contactId)
        {
            Dictionary<string, string> contactPersons;
            using (var context = new MyNotesReadModelEntities())
            {
                var query = from item in context.bmAddresses
                            where item.sAddressOfIDFK == contactId //&& item.sContactPerson.Contains(filterValue) 
                            select new {item.sAddressID, item.sContactPerson};

                 contactPersons = query.ToDictionary(v=> v.sAddressID, v=> v.sContactPerson);
            }
            return contactPersons;  
        }
        internal static string GetContactPersonNameById(string contactPersonId)
        {
            string contactPerson;
            using (var context = new MyNotesReadModelEntities())
            {
                var query = from item in context.bmAddresses
                            where item.sAddressID == contactPersonId 
                            orderby item.sContactPerson
                            select new { item.sContactPerson }
                            ;

                contactPerson = query.ToDictionary(v => v.sContactPerson, v => v.sContactPerson).FirstOrDefault().Value; 
            }
            return contactPerson;  
        }

        public static IEnumerable<bmUser> GetAllUsers()
        {
            IEnumerable<bmUser> users;
            using (var context = new MyNotesReadModelEntities())
            {
                var query = from item in context.bmUsers
                            orderby item.sUserName
                            select item;

                users = query.ToArray();
            }
            return users;
        }
        public static Dictionary<string,string> GetAllUsersDictionary()
        {
            return GetAllUsers().ToDictionary(v => v.sUserID, v => v.sUserName);
        }
        public static BriefAllocationViewModelForEdit GetAllocatedBrief(Guid guid)
        {
            BriefAllocationViewModelForEdit briefAllocationViewModelForEdit = new BriefAllocationViewModelForEdit();
            using (var context = new MyNotesReadModelEntities())
            {
                var briefAllocation = context.BriefAllocations.Single(item => item.BriefAllocationId == guid);
                briefAllocationViewModelForEdit.PlanId = briefAllocation.BriefAllocationId;
                if (briefAllocation.CreatedOn != null)
                {
                    briefAllocationViewModelForEdit.CreatedOn = Convert.ToDateTime(briefAllocation.CreatedOn);
                }
                briefAllocationViewModelForEdit.HeadPlannerId = briefAllocation.HeadPlannerID;
                briefAllocationViewModelForEdit.BriefNo = Convert.ToInt32(briefAllocation.BriefNo);
            }
            briefAllocationViewModelForEdit.RegionAndCities = GetAllocatedRegionAndCitiesForBrief(guid);
            return briefAllocationViewModelForEdit;
        }
        public static List<RegionsAndCitiesViewModel> GetAllocatedRegionAndCitiesForBrief(Guid guid)
        {
            IEnumerable<AllocatedRegionAndCity> allocatedRegionAndCities;
            var regionsAndCities = new List<RegionsAndCitiesViewModel>();
            using (var context = new MyNotesReadModelEntities())
            {
                var query = (from item in context.AllocatedRegionAndCities
                                            join location in context.bmLocationHierarchy_bmLocation 
                                            on item.LocationID equals location.sLocationID
                                            join user in context.bmUsers
                                            on item.PlannerID equals user.sUserID
                                            where item.BriefAllocationId == guid
                                            select new { item.AllocatedRegionAndCitiesId, item.BriefAllocationId, item.LocationID, item.PlannerID, item.Region, location.sLocationName, user.sUserName, item.budget}
                                           );
                regionsAndCities =
                    query.Select(
                        x =>
                        new RegionsAndCitiesViewModel
                            {
                                RegionsAndCitiesId = x.AllocatedRegionAndCitiesId,
                                BriefAllocationId = (Guid) x.BriefAllocationId,
                                PlannerId = x.PlannerID,
                                LocationId = x.LocationID,
                                Region = x.Region,
                                LocationName = x.sLocationName,
                                PlannerName = x.sUserName,
                                Budget = (decimal)x.budget
                            }).ToList();
                
            }
            return regionsAndCities;
        }
        public static List<RegionsAndCitiesViewModel> GetAllocatedRegionAndCitiesForBriefByPlanNo(Guid planId)
        {
            IEnumerable<AllocatedRegionAndCity> allocatedRegionAndCities;
            var regionsAndCities = new List<RegionsAndCitiesViewModel>();
            using (var context = new MyNotesReadModelEntities())
            {
                var query = (from item in context.AllocatedRegionAndCities
                             join location in context.bmLocationHierarchy_bmLocation                             
                             on item.LocationID equals location.sLocationID
                             join plan in context.PlanDetails
                             on item.BriefAllocation.BriefNo equals plan.BriefNo
                             join user in context.bmUsers
                             on item.PlannerID equals user.sUserID
                             where plan.PlanDetailsId == planId
                             select new { item.AllocatedRegionAndCitiesId, item.BriefAllocationId, item.LocationID, item.PlannerID, item.Region, location.sLocationName, user.sUserName, item.budget }
                                           );
                regionsAndCities =
                    query.Select(
                        x =>
                        new RegionsAndCitiesViewModel
                        {
                            RegionsAndCitiesId = x.AllocatedRegionAndCitiesId,
                            BriefAllocationId = (Guid)x.BriefAllocationId,
                            PlannerId = x.PlannerID,
                            LocationId = x.LocationID,
                            Region = x.Region,
                            LocationName = x.sLocationName,
                            PlannerName = x.sUserName,
                            Budget = (decimal)x.budget
                        }).ToList();

            }
            return regionsAndCities;
        }
        public static Dictionary<string,string > GetLocationByName(string filterValue)
        {
            Dictionary<string, string> locations;
            using (var context = new MyNotesReadModelEntities())
            {
                var query = from item in context.bmLocationHierarchy_bmLocation
                            where item.sLocationName.StartsWith(filterValue)
                            orderby item.sLocationName
                            select new { item.sLocationID, item.sLocationName };

                locations = query.ToDictionary(v => v.sLocationID, v => v.sLocationName);
            }
            //FE:FedEx;IN:InTime;TN:TNT;AR:ARAMEX
            return locations;  
        }
        public static Dictionary<string, string> GetLocationByName(string filterValue, string region)
        {
            Dictionary<string, string> locations;
            using (var context = new MyNotesReadModelEntities())
            {
                var query = from item in context.bmLocationHierarchy_bmLocation
                            where item.sLocationName.StartsWith(filterValue)// && item.Region == region
                            orderby item.sLocationName
                            select new { item.sLocationID, item.sLocationName };

                locations = query.ToDictionary(v => v.sLocationID, v => v.sLocationName);
            }
            //FE:FedEx;IN:InTime;TN:TNT;AR:ARAMEX
            return locations;
        }
        public static IEnumerable<PlanIndexViewModel> GetAllPlans()
        {
            List<PlanIndexViewModel> items;
            using (var context = new MyNotesReadModelEntities())
            {
                var query = (from item in context.PlanDetails
                             join us in context.bmUsers
                             on item.HeadPlannerID equals us.sUserID
                             join contact in context.bmContacts
                             on item.ContactId equals contact.sContactId
                             orderby item.PlanNo descending 
                             select new { item.PlanDetailsId, item.PlanNo, item.BriefNo, item.budget, item.CreatedOn, item.StartDate, item.EndDate, us.sUserName , contact.sName});

                items = query.Select(x => new PlanIndexViewModel
                {
                    PlanDetailsId = x.PlanDetailsId,
                    BriefNo = x.BriefNo,
                    budget = x.budget,
                    CreatedOn = x.CreatedOn,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    HeadPlanner = x.sUserName,
                    PlanNo = x.PlanNo,
                    ClientName = x.sName
                }).ToList();
            }
            return items.ToArray();
        }
        public static IEnumerable<PlanAlbumIndexViewModel> GetAllPlansAlbumsWithAlbumId()
        {
            List<PlanAlbumIndexViewModel> items;
            using (var context = new MyNotesReadModelEntities())
            {
                var query = (from item in context.PlanAlbums
                             join contact in context.bmContacts
                             on item.PlanDetail.ContactId equals contact.sContactId
                             
                             orderby item.PlanDetail.PlanNo descending 

                             select new { item.PlanDetail.PlanDetailsId, item.planAlbumId,  item.PlanDetail.PlanNo, contact.sName });

                items = query.Select(x => new PlanAlbumIndexViewModel
                                              {
                                                  PlanDetailsId = x.PlanDetailsId,
                                                  PlanAlbumId = x.planAlbumId,
                                                  PlanNo = x.PlanNo,
                                                  ClientName = x.sName
                }).ToList();
            }
            return items.ToArray(); 
        }
        public static IEnumerable<PlanIndexViewModel> GetRecentPlans()
        {
            List<PlanIndexViewModel> items;
            DateTime date = DateTime.Today.Subtract(new TimeSpan(7, 0, 0));
            using (var context = new MyNotesReadModelEntities())
            {
                var query = (from item in context.PlanDetails
                             join us in context.bmUsers
                             on item.HeadPlannerID equals us.sUserID
                             join contact in context.bmContacts
                             on item.ContactId equals contact.sContactId
                             where item.UpdatedOn >= date 
                             orderby item.PlanNo descending 
                             select new { item.PlanDetailsId, item.PlanNo, item.BriefNo, item.budget, item.CreatedOn, item.StartDate, item.EndDate, us.sUserName, contact.sName });

                items = query.Select(x => new PlanIndexViewModel
                {
                    PlanDetailsId = x.PlanDetailsId,
                    BriefNo = x.BriefNo,
                    budget = x.budget,
                    CreatedOn = x.CreatedOn,
                    EndDate = x.EndDate,
                    HeadPlanner = x.sUserName,
                    PlanNo = x.PlanNo,
                    ClientName = x.sName
                }).ToList();
            }
            return items.ToArray();
        }

        public static PlanEditViewModel GetPlanById(Guid id)
        {
            PlanEditViewModel planEditViewModel;
            using (var context = new MyNotesReadModelEntities())
            {
                var query = from item in context.PlanDetails
                            join user in context.bmUsers
                            on item.HeadPlannerID equals user.sUserID
                            join contact in context.bmContacts
                            on item.ContactId equals contact.sContactId
                            where item.PlanDetailsId == id
                            select new { item.BriefNo, item.budget , item.CreatedOn, item.EndDate, item.HeadPlannerID, item.PlanDetailsId, item.PlanNo,  item.StartDate, HeadPlannerName = user.sUserName, item.ContactId, contact.sName};
                
                planEditViewModel = new PlanEditViewModel();
                foreach (var curr in query)
                {
                    planEditViewModel.BriefNo = Convert.ToInt32(curr.BriefNo);
                    planEditViewModel.Budget = decimal.Parse(curr.budget.ToString());
                    planEditViewModel.CreatedOn = curr.CreatedOn;
                    planEditViewModel.EndDate = curr.EndDate;
                    planEditViewModel.HeadPlannerId = curr.HeadPlannerID;
                    planEditViewModel.PlanDetailsId = curr.PlanDetailsId;
                    planEditViewModel.PlanNo = int.Parse(curr.PlanNo.ToString());
                    planEditViewModel.HeadPlannerName = curr.HeadPlannerName;
                    planEditViewModel.StartDate = curr.StartDate;
                    planEditViewModel.ClientId = curr.ContactId;
                    planEditViewModel.ClientName = curr.sName;
                }
            }
            return planEditViewModel;            
        }

        internal static IEnumerable<PlanRegionViewModel> GetAllRegionsForPlan(Guid id)
        {
            IEnumerable<PlanRegionViewModel> planRegions1;
            using (var context = new MyNotesReadModelEntities())
            {

                var query = (from item1 in context.PlanRegions
                             join user1 in context.bmUsers
                            on item1.PlannerId equals user1.sUserID
                             where item1.PlanDetailsId == id 
                             orderby item1.Region
                             select new { item1.planRegionId, item1.Region, PlannerId = item1.PlannerId, PlannerName = user1.sUserName, item1.Budget});

                planRegions1 = query.Select(x => new PlanRegionViewModel { PlanRegionId =  x.planRegionId, Budget =  x.Budget ?? 0, PlannerId = x.PlannerId, PlannerName = x.PlannerName, Region = x.Region }).ToArray();
                foreach (var planRegionViewModel in planRegions1)
                {
                    planRegionViewModel.CityCount = (from item in context.PlanCities
                                                     where
                                                         item.PlanDetailsId == id &&
                                                         item.PlanRegion == planRegionViewModel.Region
                                                     select item).Count();

                    planRegionViewModel.SiteCount = (from item in context.PlanSites
                                                     where
                                                         item.PlanCity.PlanDetailsId == id &&
                                                         item.PlanCity.PlanRegion == planRegionViewModel.Region && item.IsInActive == false
                                                     select item).Count();
                }
            }
            return planRegions1;
        }

        internal static IEnumerable<PlanCityViewModel> GetAllCitiesForPlanRegion(Guid guid, string region)
        {
            IEnumerable<PlanCityViewModel> planCityViewModels = null;
            using (var context = new MyNotesReadModelEntities())
            {
                var query = (from item1 in context.PlanCities
                             join user1 in context.bmUsers
                            on item1.PlannerID equals user1.sUserID
                             join location in context.bmLocationHierarchy_bmLocation
                             on item1.LocationID equals location.sLocationID
                             where item1.PlanDetailsId == guid && (item1.PlanRegion == region || region == "<All>")
                             orderby location.sLocationName
                             select new { item1.PlanCitiesId, item1.PlanRegion, PlannerId = item1.PlannerID, PlannerName = user1.sUserName, item1.Budget, item1.LocationID, LocationName = location.sLocationName, plansites = item1.PlanSites });

                planCityViewModels = query.Select(x => new PlanCityViewModel() { PlanCitiesId = x.PlanCitiesId, Budget = x.Budget ?? 0, PlannerId = x.PlannerId, PlannerName = x.PlannerName, Region = x.PlanRegion, LocationId = x.LocationID, LocationName = x.LocationName, SiteCount =  x.plansites.Where(x1 => x1.IsInActive == false).Count() }).ToArray();
            }
            return planCityViewModels;
        }

        internal static string GetRegionInfoForPlanRegion(Guid PlanDetailsId, string region)
        {
            string result = "";
            using (var context = new MyNotesReadModelEntities())
            { 
                var query = (from item1 in context.PlanRegions
                             join user1 in context.bmUsers
                             on item1.PlannerId equals user1.sUserID
                             where item1.PlanDetailsId == PlanDetailsId && item1.Region == region
                             orderby item1.Region
                             select new { user1.sUserName,  item1.Budget });
                if (query.FirstOrDefault() != null)
                   result = query.FirstOrDefault().Budget + ";" + query.FirstOrDefault().sUserName;
            }
            return result;
        }
        internal static string CheckIfPlanAlreadyGenerated(string briefNo, Guid planDetailsId)
        {
            int IsPresent = 0;
           using (var context = new MyNotesReadModelEntities())
           {
               var query = (from item in context.PlanDetails
                            where item.BriefNo == briefNo && item.PlanDetailsId != planDetailsId
                            select item.PlanNo);
               IsPresent = query.FirstOrDefault() > 0 ? 1 : 0;
           }
            return IsPresent.ToString() + ";";
        }

        internal static string GetClientInfoByBriefId(string briefNo)
        {
            string result = "";
            using (var context = new MyNotesReadModelEntities())
            {
                var query = (from item1 in context.Briefs
                             join contact in context.bmContacts
                             on item1.sContactIDFK equals contact.sContactId
                             where item1.sBriefNo == briefNo
                             select new { item1.sContactIDFK, contact.sName });
                if (query.FirstOrDefault() != null)
                    result = query.FirstOrDefault().sContactIDFK+ ";" + query.FirstOrDefault().sName;
            }
            return result;
        }

        internal static PlanSiteViewModel GetPlanSiteViewModelByPlanId(Guid planDetailId)
        {
            PlanSiteViewModel planSiteViewModels = null;
            using (var context = new MyNotesReadModelEntities())
            {
                PlanEditViewModel planById = GetPlanById(planDetailId);
                planSiteViewModels = new PlanSiteViewModel()
                                         {
                                             BriefNo = planById.BriefNo.ToString(),
                                             Budget = planById.Budget,
                                             EndDate = planById.EndDate,
                                             StartDate = planById.StartDate,
                                             PlanDetailsId = planById.PlanDetailsId,
                                             PlanNo = planById.PlanNo
                                         };

            }
            return planSiteViewModels;
        }

        internal static Dictionary<string, string> GetVendorByName(string q)
        {
            Dictionary<string, string > vendors;
            using (var context = new MyNotesReadModelEntities())
            {
                var query = from item in context.bmContacts
                            where item.sContactNatureIdFK == "Acc_Supplier" && item.sName.StartsWith(q)
                            orderby item.sName
                            select item;

                vendors = query.ToDictionary(v => v.sContactId, v => v.sName);
            }
            return vendors;
        }

        internal static IEnumerable<ViewModel.Plan.PlanSite> GetAllSitesForPlanRegion(Guid planDetailsId, string region, string locationId)
        {
            IEnumerable<ViewModel.Plan.PlanSite> planSites;
            using (var context = new MyNotesReadModelEntities())
            {
                var query = from item in context.PlanSites
                            join vendor in context.bmContacts
                                on item.DisplayVendor equals vendor.sContactId
                            where
                                item.PlanCity.PlanRegion == region && item.PlanCity.PlanDetailsId == planDetailsId &&
                                (item.PlanCity.LocationID == locationId || locationId=="<All>") && item.IsInActive == false
                            orderby item.siteNo
                            select new {item, vendor.sName};

                planSites = query.Select(x => new ViewModel.Plan.PlanSite() { Addressline1 = x.item.addressline1, Addressline2 = x.item.addressline2, 
                    Addressline3 = x.item.addressline1, Days = x.item.Days ?? 0, DisplayCost = x.item.DisplayCost ?? 0, DisplayFromDate = x.item.DisplayFromDate, 
                    DisplayRate = x.item.DisplayRate ?? 0, DisplayToDate = x.item.DisplayToDate, DisplayVendor = x.item.DisplayVendor,
                    EndDate = x.item.EndDate, StartDate = x.item.StartDate, H = x.item.H ?? 0, V = x.item.V ?? 0, SiteSizeInSqFt = x.item.SiteSizeInSqFt ?? 0, Illumination = x.item.illumination, 
                    MediaType = x.item.MediaType, NoOfFaces = x.item.NoOfFaces ?? 0, PlanSiteId = x.item.planSiteId, Qty = x.item.Qty ?? 0, SiteName = x.item.siteName, 
                    SiteNo = x.item.siteNo ?? 0, SiteSize = x.item.siteSize, SizeRatio = x.item.SizeRatio, DisplayVendorName = x.sName, CityId = x.item.PlanCity.LocationID,
                    PlanCityId = x.item.PlanCity.PlanCitiesId}).ToArray();
            }
            return planSites;
        }

        internal static string GetLocationInfoForPlan(Guid PlanDetailsId, string region, string locationId)
        {
            string result = "";
            using (var context = new MyNotesReadModelEntities())
            {
                var query = (from item1 in context.PlanCities
                             where item1.PlanDetailsId == PlanDetailsId && item1.PlanRegion == region && item1.LocationID == locationId
                             select new { item1.PlanCitiesId, item1.Budget });
                if (query.FirstOrDefault() != null)
                    result = query.FirstOrDefault().PlanCitiesId + ";" + query.FirstOrDefault().Budget;
            }
            return result;           
        }

        internal static string GetSiteDetails(string siteNameOrNo, string type)
        {
            string result = "", siteName="";
            int siteNo = 0;
            if(type.Equals("SiteNo"))
            {
                siteNo = Convert.ToInt32(siteNameOrNo);
            }
            else
            {
                siteName = siteNameOrNo;
            }
            using (var context = new MyNotesReadModelEntities())
            {
                var query = (from item1 in context.SiteMasters
                             where item1.siteNo == siteNo || item1.siteName == siteName
                             select item1);


                if (query.FirstOrDefault() != null)
                {
                    var sitemaster = query.FirstOrDefault();
                    string vendorName = (from item in context.bmContacts where item.sContactId == sitemaster.vendor  select item.sName ).FirstOrDefault();
                    result = sitemaster.siteNo + ";" + sitemaster.siteName + ";" + sitemaster.vendor + ";" + vendorName + ";" + sitemaster.addressline1 + ";" +
                             sitemaster.addressline2 + ";" + sitemaster.addressline3
                             + ";" + sitemaster.siteSize + ";" + sitemaster.illumination + ";" + sitemaster.H + ";" +
                             sitemaster.V + ";" + sitemaster.SizeRatio +
                             ";" + sitemaster.MediaType + ";" + sitemaster.NoOfFaces;
                }

            }
            return result; 
        }

        internal static Dictionary<string, string > GetSiteBySiteNo(string q)
        {
            Dictionary<string, string > sites;
            using (var context = new MyNotesReadModelEntities())
            {
                var query = from item in context.SiteMasters
                            where item.siteNoChar.StartsWith(q)
                            select new { item.siteNoChar};

                sites = new Dictionary<string, string>();
                foreach (var x in query)
                {
                    if (!sites.ContainsKey(x.siteNoChar))
                        sites.Add(x.siteNoChar, x.siteNoChar);
                }
            }
            return sites;             
        }

        internal static Dictionary<string, string> GetSiteBySiteName(string q)
        {
            Dictionary<string, string> sites;
            using (var context = new MyNotesReadModelEntities())
            {
                var query = from item in context.SiteMasters
                            where item.siteName.StartsWith(q)
                            select new { item.siteName};
                sites = new Dictionary<string, string>();
                foreach (var x in query)
                {
                    if(!sites.ContainsKey(x.siteName))
                        sites.Add(x.siteName, x.siteName);
                }
            }
            return sites; 
        }

        internal static IEnumerable<PlanAddDisplayInfoViewModel> GetAllPlansForDisplayInfoIndex()
        {
            IEnumerable<ViewModel.Plan.PlanAddDisplayInfoViewModel> infoViewModels;
            using (var context = new MyNotesReadModelEntities())
            {
                var query = from item in context.PlanDetails
                            orderby item.PlanNo descending 
                            select new { item };

                infoViewModels = query.Select(x => new ViewModel.Plan.PlanAddDisplayInfoViewModel() {PlanDetailsId = x.item.PlanDetailsId, BriefNo = x.item.BriefNo, EndDate= x.item.EndDate, StartDate = x.item.StartDate, PlanNo = x.item.PlanNo }).ToArray();
            }
            return infoViewModels;           
        }

        internal static PlanAddDisplayInfoViewModel GetPlanDisplayInfoByPlanId(Guid Id)
        {
            ViewModel.Plan.PlanAddDisplayInfoViewModel infoViewModel;
            using (var context = new MyNotesReadModelEntities())
            {
                var query = from item in context.PlanDetails
                            orderby item.PlanNo descending
                            where item.PlanDetailsId == Id
                            select new { item };
                infoViewModel = query.Select(x => new ViewModel.Plan.PlanAddDisplayInfoViewModel() { PlanDetailsId = x.item.PlanDetailsId, BriefNo = x.item.BriefNo, EndDate = x.item.EndDate, StartDate = x.item.StartDate, PlanNo = x.item.PlanNo }).FirstOrDefault();
            }
            return infoViewModel;
        }

        internal static IEnumerable<ViewModel.Plan.PlanSite> GetAllSitesForPlanRegionForDisplay(Guid planDetailsId, string region, string locationId)
        {
            IEnumerable<ViewModel.Plan.PlanSite> planSites;
            using (var context = new MyNotesReadModelEntities())
            {
                var query = from item in context.PlanSites
                            join vendor in context.bmContacts
                                on item.DisplayVendor equals vendor.sContactId
                            where
                                item.PlanCity.PlanRegion == region && item.PlanCity.PlanDetailsId == planDetailsId &&
                                item.PlanCity.LocationID == locationId && item.IsInActive == false
                            orderby item.siteName
                            select new { item, vendor.sName };

                planSites = query.Select(x => new ViewModel.Plan.PlanSite() { Addressline1 = x.item.addressline1, Addressline2 = x.item.addressline2, Addressline3 = x.item.addressline1, DisplayRate = x.item.DisplayCost ?? 0 , DisplayFromDate = x.item.DisplayFromDate, DisplayToDate = x.item.DisplayToDate, DisplayVendor = x.item.DisplayVendor, DisplayVendorName = x.sName, PlanCityId = x.item.PlanCity.PlanCitiesId, PlanSiteId = x.item.planSiteId, SiteName  =  x.item.siteName, SiteNo = x.item.siteNo ??0 , SiteSizeInSqFt = x.item.H * x.item.V * x.item.Qty ?? 0, DisplayClientCost = x.item.DisplayClientCost ??0, DisplayClientRate =  x.item.DisplayClientRate ??0, DisplayCost = x.item.DisplayCost ??0, ChargingBasis = x.item.chargingBasis}).ToArray();
            }
            return planSites; 
        }

        internal static PlanAddMountingInfoViewModel GetPlanMountingInfoByPlanId(Guid Id)
        {
            ViewModel.Plan.PlanAddMountingInfoViewModel infoViewModel;
            using (var context = new MyNotesReadModelEntities())
            {
                var query = from item in context.PlanDetails
                            orderby item.PlanNo descending
                            where item.PlanDetailsId == Id
                            select new { item };
                infoViewModel = query.Select(x => new ViewModel.Plan.PlanAddMountingInfoViewModel() { PlanDetailsId = x.item.PlanDetailsId, BriefNo = x.item.BriefNo, EndDate = x.item.EndDate, StartDate = x.item.StartDate, PlanNo = x.item.PlanNo }).FirstOrDefault();
            }
            return infoViewModel;
        }

        internal static IEnumerable<ViewModel.Plan.PlanSite> GetAllSitesForPlanRegionForMounting(Guid planDetailsId, string region, string locationId)
        {
            IEnumerable<ViewModel.Plan.PlanSite> planSites;
            using (var context = new MyNotesReadModelEntities())
            {
                var query = from item in context.PlanSites
                            //join vendor in context.bmContacts
                            //    on item.MountingVendor equals vendor.sContactId
                            where
                                item.PlanCity.PlanRegion == region && item.PlanCity.PlanDetailsId == planDetailsId &&
                                item.PlanCity.LocationID == locationId && item.IsInActive == false
                            orderby item.siteName
                            select new { item};

                planSites = query.Select(x => new ViewModel.Plan.PlanSite() { Addressline1 = x.item.addressline1, Addressline2 = x.item.addressline2, Addressline3 = x.item.addressline1, MountingRate = x.item.MountingRate?? 0, MountingCost = x.item.MountingCost ??0, MountingVendor = x.item.MountingVendor, PlanCityId = x.item.PlanCity.PlanCitiesId, PlanSiteId = x.item.planSiteId, SiteName = x.item.siteName, SiteNo = x.item.siteNo ?? 0, SiteSizeInSqFt = x.item.H * x.item.V * x.item.Qty ?? 0 }).ToArray();
            }
            return planSites; 
        }

        internal static IEnumerable<PlanAddMountingInfoViewModel> GetAllPlansForMountingInfoIndex()
        {
            IEnumerable<ViewModel.Plan.PlanAddMountingInfoViewModel> infoViewModels;
            using (var context = new MyNotesReadModelEntities())
            {
                var query = from item in context.PlanDetails
                            orderby item.PlanNo descending
                            select new { item };

                infoViewModels = query.Select(x => new ViewModel.Plan.PlanAddMountingInfoViewModel() { PlanDetailsId = x.item.PlanDetailsId, BriefNo = x.item.BriefNo, EndDate = x.item.EndDate, StartDate = x.item.StartDate, PlanNo = x.item.PlanNo }).ToArray();
            }
            return infoViewModels;     
        }

        internal static IEnumerable<PlanEditViewModel> GetAllPlansForPlanRegionIndex()
        {
            List<ViewModel.Plan.PlanEditViewModel> plans;
            using (var context = new MyNotesReadModelEntities())
            {
                var query = from item in context.PlanDetails
                            join user in context.bmUsers
                            on item.HeadPlannerID equals user.sUserID
                            join contact in context.bmContacts
                            on item.ContactId equals contact.sContactId
                            orderby item.PlanNo descending
                            select new { item, user.sUserName , contact.sName};
                plans = new List<PlanEditViewModel>();
                PlanEditViewModel plan;
                foreach (var x in query)
                {
                    plan = new PlanEditViewModel();
                    plan.PlanNo = x.item.PlanNo;
                    plan.BriefNo = Convert.ToInt32(x.item.BriefNo);
                    plan.Budget = x.item.budget ??0;
                    plan.CreatedOn = x.item.CreatedOn;
                    plan.HeadPlannerName = x.sUserName;
                    plan.PlanDetailsId = x.item.PlanDetailsId;
                    plan.StartDate = x.item.StartDate;
                    plan.EndDate = x.item.EndDate;
                    plan.ClientName = x.sName;
                    plans.Add(plan);
                }
            }
            return plans.ToArray();
        }

        internal static SiteVendorAssignmentViewModel GetPlanSiteVendorAssignmentByPlanId(Guid planDetailsId)
        {
            ViewModel.Plan.SiteVendorAssignmentViewModel plan;
            using (var context = new MyNotesReadModelEntities())
            {
                var query = from item in context.PlanDetails
                            join user in context.bmUsers
                            on item.HeadPlannerID equals user.sUserID
                            join contact in context.bmContacts
                            on item.ContactId equals contact.sContactId
                            where item.PlanDetailsId == planDetailsId
                            orderby item.PlanNo descending
                            select new { item, user.sUserName, contact.sName };
                plan = new SiteVendorAssignmentViewModel();
                foreach (var x in query)
                {
                    plan = new SiteVendorAssignmentViewModel();
                    plan.PlanNo = x.item.PlanNo ;
                    plan.BriefNo = x.item.BriefNo;
                    plan.PlanBudget = Convert.ToInt32( x.item.budget ?? 0);
                    plan.ClientName = x.sName;
                    plan.HeadPlannerName = x.sUserName;
                    plan.PlanDetailsId = x.item.PlanDetailsId;
                    plan.StartDate = x.item.StartDate;
                    plan.EndDate = x.item.EndDate;
                    plan.Activity = "";
                }
            }
            return plan;
        }

        internal static IEnumerable<ViewModel.Plan.PlanSite> GetAllSitesForPlan(Guid planDetailsId)
        {
            IEnumerable<ViewModel.Plan.PlanSite> planSites;
            using (var context = new MyNotesReadModelEntities())
            {
                var query = from item in context.PlanSites
                            join city in context.bmLocationHierarchy_bmLocation 
                            on item.PlanCity.LocationID equals city.sLocationID
                            where item.PlanCity.PlanDetailsId == planDetailsId && item.IsInActive == false
                            orderby item.PlanCity.PlanRegion, city.sLocationName
                            select new { item , city.sLocationName};

                planSites = query.Select(x => new ViewModel.Plan.PlanSite()
                                                  {
                                                      DisplayVendor = x.item.DisplayVendor,
                                                      DisplayVendorName = x.item.DisplayVendorContact.sName,
                                                      DisplayCost = x.item.DisplayCost ?? 0,
                                                      DisplayRate = x.item.DisplayRate ?? 0,
                                                      DisplayClientRate = x.item.DisplayClientRate ??0,
                                                      DisplayClientCost = x.item.DisplayClientCost ??0,
                                                      DisplayFromDate = x.item.DisplayFromDate,
                                                      DisplayToDate = x.item.DisplayToDate,
                                                      MountingRate = x.item.MountingRate ?? 0,
                                                      MountingCost = x.item.MountingCost ??0,
                                                      MountingVendor = x.item.MountingVendor,
                                                      MountingVendorName = x.item.MountingVendorContact.sName,
                                                      MountingClientRate = x.item.MountingClientRate ??0,
                                                      MountingClientCost = x.item.MountingClientCost ??0,
                                                      PlanCityId = x.item.PlanCity.PlanCitiesId,
                                                      PlanSiteId = x.item.planSiteId,
                                                      SiteName = x.item.siteName,
                                                      SiteNo = x.item.siteNo ?? 0,
                                                      SiteSizeInSqFt = x.item.H * x.item.V * x.item.Qty ?? 0,
                                                      MediaType = x.item.MediaType,
                                                      H = x.item.H ?? 0,
                                                      V = x.item.V ?? 0,
                                                      Days = x.item.Days ??0,
                                                      Qty =  x.item.Qty ??0,
                                                      FabricationCost = x.item.FabricationCost ?? 0,
                                                      FabricationVendor = x.item.FabricationVendor,
                                                      FabricationVendorName = x.item.FabricationVendorContact.sName,
                                                      FabricationRate = x.item.FabricationRate ?? 0,
                                                      FabricationClientRate = x.item.FabricationClientRate ??0,
                                                      FabricationClientCost = x.item.FabricationClientCost ??0,
                                                      PrintingVendor = x.item.PrintVendor,
                                                      PrintingVendorName = x.item.PrintVendorContact.sName,
                                                      PrintingCost = x.item.PrintCost ?? 0,
                                                      PrintingRate = x.item.PrintRate ?? 0,
                                                      PrintingClientRate = x.item.PrintClientRate ??0,
                                                      PrintingClientCost = x.item.PrintClientCost ??0,
                                                      SiteSize = x.item.siteSize,
                                                      SizeRatio = x.item.SizeRatio,
                                                      Region = x.item.PlanCity.PlanRegion,
                                                      CityId = x.item.PlanCity.LocationID,
                                                      CityName = x.sLocationName,
                                                      Illumination = x.item.illumination,
                                                      StartDate = x.item.StartDate,
                                                      EndDate = x.item.EndDate,
                                                      Activity = "",
                                                      IsDirty = false,
                                                      DisplayStatus = x.item.DisplayStatus,
                                                      MountingStatus = x.item.MountingStatus,
                                                      PrintingStatus = x.item.PrintingStatus,
                                                      FabricationStatus = x.item.FabricationStatus,
                                                      ChargingBasis = x.item.chargingBasis,
                                                  }).ToArray();
                foreach (var site in planSites)
                {
                    if(site.DisplayVendor != "")
                    {
                        site.Activity = "Display,";
                    }
                }
            }
            return planSites; 
        }

        internal static PlanSiteAlbumViewModel GetSiteAlbumForPlan(Guid planAlbumId)
        {
            PlanSiteAlbumViewModel albumViewModel;
            using (var context = new MyNotesReadModelEntities())
            {
                var query = from item in context.PlanAlbums
                            join contact in context.bmContacts
                            on item.PlanDetail.ContactId equals contact.sContactId
                            where item.planAlbumId == planAlbumId
                            select new {  item.PlanDetail, contact.sName };
                albumViewModel =
                    query.Select(
                        x => new PlanSiteAlbumViewModel() { PlanId = x.PlanDetail.PlanDetailsId, PlanNo = x.PlanDetail.PlanNo , ClientName = x.sName, PlanAlbumId = planAlbumId }).FirstOrDefault();
                if (albumViewModel != null)
                {
                    albumViewModel.Regions = GetAllRegionsForPlan(albumViewModel.PlanId);
                    albumViewModel.Cities = GetAllCitiesForPlanForPhoto(albumViewModel.PlanId, "<All>");

                    var query1 = from site in context.PlanSites
                                 where site.PlanCity.PlanDetailsId == albumViewModel.PlanId && site.IsInActive == false
                                 select new {site.planSiteId, site.siteName, site.PlanCity.PlanCitiesId, site.IsInActive};
                    albumViewModel.Sites =
                        query1.Where(x1=>x1.IsInActive == false).Select(x => new SiteAlbum() {SiteId = x.planSiteId, SiteName = x.siteName, PlanCityId = x.PlanCitiesId}).ToList();
                }

            }
            return albumViewModel;
        }

        internal static IEnumerable<PlanCityforPhotosViewModel> GetAllCitiesForPlanForPhoto(Guid guid, string region)
        {
            IEnumerable<PlanCityforPhotosViewModel> planCityViewModels = null;
            using (var context = new MyNotesReadModelEntities())
            {
                var query = (from item1 in context.PlanCities
                             join user1 in context.bmUsers
                            on item1.PlannerID equals user1.sUserID
                             join location in context.bmLocationHierarchy_bmLocation
                             on item1.LocationID equals location.sLocationID                             
                             where item1.PlanDetailsId == guid && (item1.PlanRegion == region || region == "<All>") 
                             orderby location.sLocationName
                             select new { item1.PlanCitiesId, item1.PlanRegion, item1.PhotoAgId, item1.LocationID, LocationName = location.sLocationName, item1.PlanSites });

                //planCityViewModels = query.Select(x => new PlanCityforPhotosViewModel() { PlanCitiesId = x.PlanCitiesId, PhotoAgId = x.PhotoAgId ?? Guid.Empty, Region = x.PlanRegion, LocationName = x.LocationName, SiteCount = x.siteCount }).ToArray();
                List<PlanCityforPhotosViewModel> lst= new List<PlanCityforPhotosViewModel>();
                foreach (var v in query)
                {
                    lst.Add(new PlanCityforPhotosViewModel(){ PlanCitiesId = v.PlanCitiesId, PhotoAgId = v.PhotoAgId ?? Guid.Empty, Region = v.PlanRegion, 
                                LocationName = v.LocationName, SiteCount = v.PlanSites.Where(x=>x.IsInActive==false).Count() });
                    
                }
                planCityViewModels = lst; 
            }
            return planCityViewModels;
        }

        internal static IEnumerable<PhotoViewModel> GetSitePhotosBySiteId(Guid siteId)
        {
            IEnumerable<PhotoViewModel> photoViewModels;
            using (var context = new MyNotesReadModelEntities())
            {
                var query = from item in context.PlanAlbumSitePhotos
                            where item.PlanSiteId == siteId
                            select new { item };
                photoViewModels = query.Select(x => new PhotoViewModel() { PhotoId = x.item.PlanAlbumSitePhotosId, PhotoName = x.item.PhotoName, Tags = x.item.Tags, Title = x.item.Title }).ToList();
            }
            return photoViewModels;
        }

        internal static IEnumerable<PhotoViewModel> GetUnattachedPhotosForCity(Guid planCityId)
        {
            IEnumerable<PhotoViewModel> photoViewModels;
            using (var context = new MyNotesReadModelEntities())
            {
                var query = from item in context.PlanAlbumSitePhotos
                            where item.PlanCityId == planCityId && item.PlanSite == null
                            select new { item };
                photoViewModels = query.Select(x => new PhotoViewModel() { PhotoId = x.item.PlanAlbumSitePhotosId, PhotoName = x.item.PhotoName, Tags = x.item.Tags, Title = x.item.Title, PlanAlbumId = x.item.planAlbumId ?? Guid.Empty }).ToList();
            }
            return photoViewModels;
        }

        internal static PlanPhotoUploadingViewModel GetPlanForPhotoUploading(Guid planDetailId, Guid planAlbumId)
        {
            PlanPhotoUploadingViewModel photoViewModels;
            using (var context = new MyNotesReadModelEntities())
            {
                var query = from item in context.PlanAlbums
                            join contact in context.bmContacts
                            on item.PlanDetail.ContactId equals contact.sContactId
                            where item.PlanDetail.PlanDetailsId == planDetailId
                            select new { item, contact.sName};
                photoViewModels = query.Select(x => new PlanPhotoUploadingViewModel() { PlanAlbumId = planAlbumId,  AlbumNo = x.item.AlbumNo ?? 0, planDetailsId = x.item.PlanDetail.PlanDetailsId, ClientName = x.sName, PlanNo = x.item.PlanDetail.PlanNo }).Where(x => x.PlanAlbumId == planAlbumId).FirstOrDefault();

                //var query1 = from item1 in context.PlanCities
                //             join location in context.bmLocationHierarchy_bmLocation
                //                 on item1.LocationID equals location.sLocationID
                //             where item1.PlanDetailsId == planDetailId
                //             select new {item1, location.sLocationName};

                //photoViewModels.Cities =
                //    query1.Select(
                //        x =>
                //        new PlanCityforPhotosViewModel()
                //            {
                //                LocationName = x.sLocationName,
                //                PlanCitiesId = x.item1.PlanCitiesId,
                //                Region = x.item1.PlanRegion,
                //                SiteCount = x.item1.PlanSites.Count()
                //            }).ToList();
            }
            return photoViewModels;
        }

        internal static IEnumerable<PhotoViewModel> GetAllAttachedPhotosForPlan(Guid planDetailId)
        {
            IEnumerable<PhotoViewModel> photoViewModels;
            using (var context = new MyNotesReadModelEntities())
            {
                var query = from item in context.PlanAlbumSitePhotos
                            where item.PlanAlbum.planDetailsId == planDetailId 
                            select new { item };
                photoViewModels = query.Select(x => new PhotoViewModel() { PhotoId = x.item.PlanAlbumSitePhotosId, PhotoName = x.item.PhotoName, Tags = x.item.Tags, Title = x.item.Title, PlanAlbumId = x.item.planAlbumId ?? Guid.Empty }).ToList();
            }
            return photoViewModels;
        }

        internal static IEnumerable<PhotoViewModel> GetCityPhotosByCityId(Guid planCityId)
        {
            IEnumerable<PhotoViewModel> photoViewModels;
            using (var context = new MyNotesReadModelEntities())
            {
                var query = from item in context.PlanAlbumSitePhotos
                            where item.PlanCityId == planCityId 
                            select new { item };
                photoViewModels = query.Select(x => new PhotoViewModel() { PhotoId = x.item.PlanAlbumSitePhotosId, PhotoName = x.item.PhotoName, Tags = x.item.Tags, Title = x.item.Title, PlanAlbumId = x.item.planAlbumId ?? Guid.Empty }).ToList();
            }
            return photoViewModels;
        }

        internal static IEnumerable<PlanAlbumViewModel> GetAllPlansWhoseAlbumIsNotCreated()
        {
            IEnumerable<PlanAlbumViewModel> planAlbumViewModels = null;
            using (var context = new MyNotesReadModelEntities())
            {
                var query = from item in context.PlanDetails
                            join contact in context.bmContacts
                            on item.ContactId equals  contact.sContactId
                            where item.PlanAlbums.Count == 0
                            select new { item , sName = contact.sName};
                planAlbumViewModels = query.Select(x => new PlanAlbumViewModel() { PlanNo = x.item.PlanNo,  ClientName = x.sName,  PlanDetailsId = x.item.PlanDetailsId }).ToList();
            }
            return planAlbumViewModels;
        }

        internal static PlanAlbumViewModel GetPlanAlbumViewModelByPlanId(Guid planDetailsId)
        {
            PlanAlbumViewModel planAlbumViewModels = null;
            using (var context = new MyNotesReadModelEntities())
            {
                var query = from item in context.PlanDetails
                            join contact in context.bmContacts
                            on item.ContactId equals contact.sContactId
                            where item.PlanDetailsId == planDetailsId
                            select new { item, contact.sName };
                planAlbumViewModels = query.Select(x => new PlanAlbumViewModel() { PlanNo = x.item.PlanNo, ClientName = x.sName, PlanDetailsId = x.item.PlanDetailsId }).FirstOrDefault();
            }
            return planAlbumViewModels;
        }
        internal static IEnumerable<PhotoViewModel> GetAllUnAttachedPhotosForCity(Guid planAlbumId)
        {
            IEnumerable<PhotoViewModel> photomodels = null;
            using (var context = new MyNotesReadModelEntities())
            {
                var query = from item in context.PlanAlbumSitePhotos
                            where item.planAlbumId == planAlbumId && item.PlanCityId== null
                            select new { item};
                photomodels = query.Select(x => new PhotoViewModel() { PhotoId= x.item.PlanAlbumSitePhotosId, PhotoName = x.item.PhotoName, PlanAlbumId = x.item.planAlbumId ?? Guid.Empty, Tags= x.item.Tags, Title= x.item.Title }).ToList();
            }
            return photomodels;
        }
        internal static Dictionary<Guid, string > GetSitesForCity(Guid planCityId)
        {
            Dictionary<Guid, string> sites;
            using (var context = new MyNotesReadModelEntities())
            {
                var query = from item in context.PlanSites
                            where item.PlanCity.PlanCitiesId == planCityId && item.IsInActive == false
                            select new { item };
                 sites= query.ToDictionary(x1 => x1.item.planSiteId, x1 => x1.item.siteName);
            }
            return sites;
        }
        internal static IEnumerable<ViewModel.Email.EmailDataViewModel> GetEmailDataViewModel(Guid id)
        {
            IEnumerable<ViewModel.Email.EmailDataViewModel> models;
            using (var context = new MyNotesReadModelEntities())
            {
                var query = (from item in context.PlanDetails
                             join contact in context.bmContacts
                                 on item.ContactId equals contact.sContactId
                             join user in context.bmUsers
                                 on item.HeadPlannerID equals user.sUserID
                             join city in context.PlanCities
                                 on item.PlanDetailsId equals city.PlanDetailsId
                             join location in context.bmLocationHierarchy_bmLocation
                                 on city.LocationID equals location.sLocationID
                             where item.PlanDetailsId == id
                             select
                                 new
                                 {
                                     item.PlanNo,
                                     user.sUserName,
                                     item.budget,
                                     contact.sName,
                                     location.sLocationName,
                                     city.PlanSites
                                 });
                var list = new List<EmailDataViewModel>();
                foreach (var v1 in query)
                {
                    list.AddRange(v1.PlanSites.Where(x => x.IsInActive == false).Select(site => new EmailDataViewModel()
                    {
                        budget = v1.budget ?? 0,
                        Cost = site.DisplayClientCost + site.FabricationClientCost + site.MountingClientCost + site.PrintClientCost ?? 0,
                        Days = site.Days ?? 0,
                        EndDate = site.EndDate ?? DateTime.Now,
                        H = site.H ?? 0,
                        Illumination = site.illumination,
                        MediaType = site.MediaType,
                        NoOfFaces = site.NoOfFaces ?? 0,
                        planno = v1.PlanNo,
                        Qty = site.Qty ?? 0,
                        siteName = site.siteName,
                        siteNo = site.siteNo ?? 0,
                        siteSize = site.siteSize,
                        SizeRatio = site.SizeRatio,
                        sLocationName = v1.sLocationName,
                        sname = v1.sName,
                        StartDate = site.StartDate ?? DateTime.Now,
                        sUserName = v1.sUserName,
                        V = site.V ?? 0
                    }));
                }
                models = list;
            }
            return models;
        }
        internal static ViewModel.Email.EmailViewModel GetEmailViewModel(Guid id)
        {
            var model = new EmailViewModel();
            using (var context = new MyNotesReadModelEntities())
            {
                var query = (from item in context.PlanDetails
                             join contact in context.bmContacts
                                 on item.ContactId equals contact.sContactId
                             where item.PlanDetailsId == id
                             select new { item.PlanNo, contact.sName, contact.EmailId });
                model =
                    query.Select(x => new EmailViewModel() { Client = x.sName, PlanNo = x.PlanNo, ToAddress = x.EmailId }).FirstOrDefault();
            }
            return model;
        }
    }
}