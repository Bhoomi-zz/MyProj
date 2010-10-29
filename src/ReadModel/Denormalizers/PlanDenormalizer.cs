using System;
using System.Collections.Generic;
using System.Linq;
using Events;
using Ncqrs.Eventing.ServiceModel.Bus;

namespace ReadModel.Denormalizers
{
    public class PlanDenormalizer : IEventHandler<PlanCreated>, IEventHandler<PlanChanged>, IEventHandler<RegionAdded>, IEventHandler<RegionChanged>, IEventHandler<CityAdded>, IEventHandler<CityChanged>, IEventHandler<SiteAdded>, IEventHandler<SiteDetailsChanged>, IEventHandler<SiteDeselected>, IEventHandler<SiteDisplayInfoChanged>, IEventHandler<SiteMountingInfoChanged> , IEventHandler<SitePrintingInfoChanged>, IEventHandler<SiteFabricationInfoChanged>, IEventHandler<PlanUpdated>
    {
        public void Handle(PlanCreated evnt)
        {
            using (var context = new MyNotesReadModelEntities())
            {
                var plan = new PlanDetail();
                plan.PlanDetailsId = evnt.PlanDetailsId;
                plan.BriefNo = evnt.BriefNo.ToString();
                plan.CreatedOn = evnt.CreatedOn;
                plan.HeadPlannerID = evnt.HeadPlannerId;
                plan.budget = evnt.Budget;
                plan.EndDate = evnt.EndDate;
                plan.PlanNo = Series.AutoGenerateNumber(context, "PlanDetails", "PlanNo"); 
                plan.StartDate = evnt.StartDate;
                plan.UpdatedOn = DateTime.Today;
                plan.ContactId = evnt.ClientId;
                context.PlanDetails.AddObject(plan);
                 context.SaveChanges();
            }
        }

        public void Handle(PlanChanged evnt)
        {
            using (var context = new MyNotesReadModelEntities())
            {
                var plan = context.PlanDetails.Single(x => x.PlanDetailsId == evnt.PlanDetailsId);
                plan.PlanDetailsId = evnt.PlanDetailsId;
                plan.BriefNo = evnt.BriefNo.ToString();
                plan.CreatedOn = evnt.CreatedOn;
                plan.HeadPlannerID = evnt.HeadPlannerId;
                plan.budget = evnt.Budget;
                plan.EndDate = evnt.EndDate;
                plan.PlanNo = evnt.PlanNo;
                plan.StartDate = evnt.StartDate;
                plan.UpdatedOn = DateTime.Today;
                plan.ContactId = evnt.ClientId;
                context.SaveChanges();
            }
        }
        public void Handle(PlanUpdated evnt)
        {
            using (var context = new MyNotesReadModelEntities())
            {
                var plan = context.PlanDetails.Single(x => x.PlanDetailsId == evnt.PlanDetailsId);
                plan.UpdatedOn = evnt.UpdatedOn;
                context.SaveChanges();
            }
        }

        public void Handle(RegionAdded evnt)
        {
            using (var context = new MyNotesReadModelEntities())
            {
                var planRegion = new PlanRegion();
                planRegion.planRegionId = evnt.PlanRegionId;
                planRegion.PlanDetailsId = evnt.PlanDetailsId;
                planRegion.PlannerId = evnt.PlannerId;
                planRegion.Region = evnt.Region;
                planRegion.Budget = evnt.Budget;
                context.PlanRegions.AddObject(planRegion);
                context.SaveChanges();
            }
        }

        public void Handle(RegionChanged evnt)
        {
            using (var context = new MyNotesReadModelEntities())
            {
                var planRegion = context.PlanRegions.Single(x => x.planRegionId == evnt.PlanRegionId);
                planRegion.PlannerId = evnt.PlannerId;
                planRegion.Region = evnt.Region;
                planRegion.Budget = evnt.Budget;
                context.SaveChanges();
            }
        }

        public void Handle(CityAdded evnt)
        {
            using (var context = new MyNotesReadModelEntities())
            {
                var planCity = new PlanCity();
                planCity.PlanCitiesId = evnt.PlanCitiesId;
                planCity.PlanDetailsId = evnt.PlanDetailsId;
                planCity.PlannerID = evnt.PlannerId;
                planCity.PlanRegion = evnt.Region;
                planCity.Budget = evnt.Budget;
                planCity.LocationID = evnt.LocationId;
                context.PlanCities.AddObject(planCity);
                context.SaveChanges();
            }
        }

        public void Handle(CityChanged evnt)
        {
            using (var context = new MyNotesReadModelEntities())
            {
                var planCity = context.PlanCities.Single(x => x.PlanCitiesId == evnt.PlanCitiesId);
                planCity.PlannerID = evnt.PlannerId;
                planCity.PlanRegion = evnt.Region;
                planCity.Budget = evnt.Budget;
                planCity.LocationID = evnt.LocationId;
                context.SaveChanges();
            }
        }

        public void Handle(SiteAdded evnt)
        {
            using (var context = new MyNotesReadModelEntities())
            {
                evnt.SiteNo = AddNewSiteIfNotFound(evnt.PlanSiteId, context, evnt.Addressline1, evnt.Addressline2, evnt.Addressline3,
                                  evnt.H, evnt.Illumination, evnt.PlanCityId, evnt.MediaType, evnt.NoOfFaces,
                                  evnt.SiteNo, evnt.SiteName, evnt.SiteSize, evnt.SizeRatio, evnt.V, evnt.DisplayVendor);

                var site = new PlanSite();
                site.addressline1 = evnt.Addressline1;
                site.addressline2 = evnt.Addressline2;
                site.addressline3 = evnt.Addressline3;
                site.Days = Convert.ToInt16(evnt.Days);
                site.DisplayCost = evnt.DisplayCost;
                site.DisplayFromDate = evnt.DisplayFromDate;
                site.DisplayRate = evnt.DisplayRate;
                site.DisplayToDate = evnt.DisplayToDate;
                site.DisplayVendor = evnt.DisplayVendor;
                site.EndDate = evnt.EndDate;
                site.H = Convert.ToInt16(evnt.H);
                site.illumination = evnt.Illumination;
                site.MediaType = evnt.MediaType;
                site.NoOfFaces = Convert.ToInt16(evnt.NoOfFaces);
                site.PlanCity = context.PlanCities.Single(x => x.PlanCitiesId== evnt.PlanCityId);//.PlanCitiesId = evnt.PlanCityId;
                site.planSiteId = evnt.PlanSiteId;
                site.Qty = Convert.ToInt16(evnt.Qty);
                site.siteName = evnt.SiteName;
                site.siteNo = evnt.SiteNo;
                site.siteSize = evnt.SiteSize;
                site.SizeRatio = evnt.SizeRatio;
                site.StartDate = evnt.StartDate;
                site.V = Convert.ToInt16(evnt.V);
                site.IsInActive = false;
                site.SiteSizeInSqFt = evnt.SiteSizeInSqFt;
                context.PlanSites.AddObject(site);
                context.SaveChanges();
            }
        }

        private int AddNewSiteIfNotFound(Guid planSiteId, MyNotesReadModelEntities context, string addressline1, string addressline2, string addressline3, decimal H, string illumination,
                            Guid planCityId, string mediaType, int noOfFaces,int siteNo, string siteName, string  siteSize, string  sizeRatio, int V, string vendor)
        {
            var siteMaster = context.SiteMasters.SingleOrDefault(x => x.siteNo == siteNo);
            if (siteMaster != null) return siteMaster.siteNo ?? 0;

            siteMaster = new SiteMaster();
            siteMaster.siteId = planSiteId;
            siteMaster.siteNo = Series.AutoGenerateNumber(context, "SiteMaster", "SiteNo"); 
            siteMaster.siteName = siteName;
            siteMaster.addressline1 = addressline1;
            siteMaster.addressline2 = addressline2;
            siteMaster.addressline3 = addressline3;
            siteMaster.H = Convert.ToInt16(H);
            siteMaster.illumination = illumination ;
            siteMaster.MediaType = mediaType;
            siteMaster.NoOfFaces = Convert.ToInt16(noOfFaces);
            siteMaster.siteNoChar = siteNo.ToString();
            siteMaster.siteSize = siteSize;
            siteMaster.SizeRatio = sizeRatio;
            siteMaster.V = Convert.ToInt16(V);
            siteMaster.vendor = vendor;
            
            context.SiteMasters.AddObject(siteMaster);
            return siteMaster.siteNo ?? 0;
        }

        public void Handle(SiteDetailsChanged evnt)
        {
            using (var context = new MyNotesReadModelEntities())
            {
                var site = context.PlanSites.Single(x => x.planSiteId== evnt.PlanSiteId);
                site.addressline1 = evnt.Addressline1;
                site.addressline2 = evnt.Addressline2;
                site.addressline3 = evnt.Addressline3;
                site.Days = Convert.ToInt16(evnt.Days);
                site.DisplayCost = evnt.DisplayCost;
                site.DisplayFromDate = evnt.DisplayFromDate;
                site.DisplayRate = evnt.DisplayRate;
                site.DisplayToDate = evnt.DisplayToDate;
                site.DisplayVendor = evnt.DisplayVendor;
                site.EndDate = evnt.EndDate;
                site.H = Convert.ToInt16(evnt.H);
                site.illumination = evnt.Illumination;
                site.MediaType = evnt.MediaType;
                site.NoOfFaces = Convert.ToInt16(evnt.NoOfFaces);
                //site.PlanCity = context.PlanCities.Single(x => x.PlanCitiesId == evnt.PlanCityId);//evnt.PlanCityId;
                site.SiteSizeInSqFt = evnt.SiteSizeInSqFt;
                site.planSiteId = evnt.PlanSiteId;
                site.Qty = Convert.ToInt16(evnt.Qty);
                site.siteName = evnt.SiteName;
                site.siteNo = evnt.SiteNo;
                site.siteSize = evnt.SiteSize;
                site.SizeRatio = evnt.SizeRatio;
                site.StartDate = evnt.StartDate;
                site.V = Convert.ToInt16(evnt.V);
                AddNewSiteIfNotFound(evnt.PlanSiteId, context, evnt.Addressline1, evnt.Addressline2, evnt.Addressline3,
                                    evnt.H, evnt.Illumination, evnt.PlanCityId, evnt.MediaType, evnt.NoOfFaces,
                                    evnt.SiteNo, evnt.SiteName, evnt.SiteSize, evnt.SizeRatio, evnt.V, evnt.DisplayVendor);
                context.SaveChanges();
            }
        }

        public void Handle(SiteDeselected evnt)
        {
            using (var context = new MyNotesReadModelEntities())
            {
                var site = context.PlanSites.Single(x => x.planSiteId == evnt.PlanSiteId);
                site.IsInActive = true;
                context.SaveChanges();
            }
        }

        public void Handle(SiteDisplayInfoChanged evnt)
        {
            using (var context = new MyNotesReadModelEntities())
            {
                var site = context.PlanSites.Single(x => x.planSiteId == evnt.PlanSiteId);
                site.DisplayVendor = evnt.DisplayVendor;
                site.DisplayRate = evnt.DisplayRate;
                site.DisplayCost = evnt.DisplayCost;
                site.StartDate = evnt.StartDate;
                site.EndDate = evnt.EndDate;
                site.DisplayClientRate = evnt.DisplayClientRate;
                site.DisplayClientCost = evnt.DisplayClientCost;
                site.DisplayStatus = evnt.DisplayStatus;
                context.SaveChanges();
            }
        }

        public void Handle(SiteMountingInfoChanged evnt)
        {
            using (var context = new MyNotesReadModelEntities())
            {
                var site = context.PlanSites.Single(x => x.planSiteId == evnt.PlanSiteId);
                site.MountingVendor = evnt.MountingVendor;
                site.MountingRate = evnt.MountingRate;
                site.MountingCost = evnt.MountingCost;
                site.MountingClientRate = evnt.MountingClientRate;
                site.MountingClientCost = evnt.MountingClientCost;
                site.MountingStatus = evnt.MountingStatus;
                context.SaveChanges();
            }
        }

        public void Handle(SitePrintingInfoChanged evnt)
        {
            using (var context = new MyNotesReadModelEntities())
            {
                var site = context.PlanSites.Single(x => x.planSiteId == evnt.PlanSiteId);
                site.PrintVendor = evnt.PrintingVendor;
                site.PrintRate = evnt.PrintingRate;
                site.PrintCost = evnt.PrintingCost;
                site.PrintingStatus = evnt.PrintingStatus;
                site.PrintClientRate = evnt.PrintingClientRate;
                site.PrintClientCost = evnt.PrintingClientCost;
                context.SaveChanges();
            }
        }

        public void Handle(SiteFabricationInfoChanged evnt)
        {
            using (var context = new MyNotesReadModelEntities())
            {
                var site = context.PlanSites.Single(x => x.planSiteId == evnt.PlanSiteId);
                site.FabricationVendor = evnt.FabricationVendor;
                site.FabricationRate = evnt.FabricationRate;
                site.FabricationCost = evnt.FabricationCost;
                site.FabricationStatus = evnt.FabricationStatus;
                site.FabricationClientRate = evnt.FabricationClientRate;
                site.FabricationClientCost = evnt.FabricationClientCost;
                context.SaveChanges();
            }  
        }
    }
}
