using System;
using System.Collections.Generic;
using Ncqrs;
using Events;
using Ncqrs.Domain;
using CommonDTOs;
namespace Domain
{
    public class Plan : AggregateRootMappedByConvention
    {
        public Guid Id
        {
            get { return EventSourceId; }
            set { EventSourceId = value; }
        }
        private Guid _planId;
        private int _planNo;
        private int _briefNo;
        private DateTime? _createdOn;
        private string _headPlannerId;
        private decimal _budget;
        private DateTime _startDate;
        private DateTime _endDate;
        private string _clientId;
        private Dictionary<Guid, PlanRegion> _planRegions;
        private Dictionary<Guid, PlanRegion> PlanRegions
        {
            get { return _planRegions ?? (_planRegions = new Dictionary<Guid, PlanRegion>()); }
            set { _planRegions = value; }
        }

        private DateTime _updatedOn;
        private Dictionary<Guid, PlanCity> _planCities;
        private Dictionary<Guid, PlanCity> PlanCities
        {
            get { return _planCities ?? (_planCities = new Dictionary<Guid, PlanCity>()); }
            set { _planCities = value; }
        }
        public Plan()
        {

        }
        public Plan(Guid planDetailsId, int briefNo, DateTime? createdOn, string headPlannerId, int planNo, decimal budget, DateTime startDate, DateTime endDate, string clientId)
        {
            planDetailsId = Id;
            ApplyEvent(new PlanCreated() { BriefNo = briefNo, Budget = budget, CreatedOn = createdOn, EndDate = endDate, HeadPlannerId = headPlannerId, PlanDetailsId = planDetailsId, PlanNo = planNo, StartDate =  startDate, ClientId = clientId});
        }
        public void OnPlanCreatedEvent(PlanCreated evnt)
        {
            _planId = evnt.PlanDetailsId;
            _planNo = evnt.PlanNo;
            _briefNo = evnt.BriefNo;
            _createdOn = evnt.CreatedOn;
            _headPlannerId = evnt.HeadPlannerId;
            _budget = evnt.Budget;
            _startDate = evnt.StartDate;
            _endDate = evnt.EndDate;
            _clientId = evnt.ClientId;
        }
        public void ChangePlan(int briefNo, DateTime? createdOn, string headPlannerId, int planNo, decimal budget, DateTime startDate, DateTime endDate, string clientId)
        {
            ApplyEvent(new PlanChanged() { BriefNo = briefNo, Budget = budget, CreatedOn = createdOn, EndDate = endDate, HeadPlannerId = headPlannerId, PlanDetailsId = Id,  PlanNo = planNo, StartDate = startDate , ClientId = clientId});
        }
        public void OnPlanChanged(PlanChanged evnt)
        {
            _planNo = evnt.PlanNo;
            _briefNo = evnt.BriefNo;
            _createdOn = evnt.CreatedOn;
            _headPlannerId = evnt.HeadPlannerId;
            _budget = evnt.Budget;
            _startDate = evnt.StartDate;
            _endDate = evnt.EndDate;
            _clientId = evnt.ClientId;
        }
        public void OnPlanUpdated(PlanUpdated evnt)
        {
            _updatedOn = evnt.UpdatedOn;
        }
        public void AddOrModifyRegionInPlan(Dictionary<Guid, PlanRegionsDTO> regions)
        {
            IEnumerator<KeyValuePair<Guid, PlanRegionsDTO>> iEnum = regions.GetEnumerator();
            while (iEnum.MoveNext())
            {
                if (PlanRegions.ContainsKey(iEnum.Current.Value.PlanRegionId))
                {
                    ApplyEvent(new RegionChanged(){ PlanRegionId = iEnum.Current.Value.PlanRegionId, Budget = iEnum.Current.Value.Budget, PlannerId = iEnum.Current.Value.PlannerId, Region = iEnum.Current.Value.Region, PlanDetailsId = Id});
                }
                else
                {
                    ApplyEvent(new RegionAdded() { PlanRegionId = iEnum.Current.Value.PlanRegionId, Budget = iEnum.Current.Value.Budget, PlannerId = iEnum.Current.Value.PlannerId, Region = iEnum.Current.Value.Region, PlanDetailsId = Id });
                }
            }
            ApplyEvent(new PlanUpdated() { PlanDetailsId = Id, UpdatedOn = DateTime.Today });
        }
        public void OnRegionAdded(RegionAdded regionAdded)
        {
            //if (_planRegions==null)
            //    _planRegions= new Dictionary<Guid, PlanRegion>();
            PlanRegions.Add(regionAdded.PlanRegionId, new PlanRegion {PlanRegionId = regionAdded.PlanRegionId, Budget = regionAdded.Budget, PlannerId = regionAdded.PlannerId, Region =  regionAdded.Region });
        }
        public void OnRegionChanged(RegionChanged regionChanged)
        {
            var changedRegion = _planRegions[regionChanged.PlanRegionId];
            changedRegion.Budget = regionChanged.Budget;
            changedRegion.PlannerId = regionChanged.PlannerId;
            changedRegion.Region = regionChanged.Region;
        }

        public void AddOrModifyCityInPlan(Dictionary<Guid, PlanCityDTO> cities)
        {
            IEnumerator<KeyValuePair<Guid, PlanCityDTO>> iEnum = cities.GetEnumerator();
            while (iEnum.MoveNext())
            {
                if (PlanCities.ContainsKey(iEnum.Current.Value.PlanCitiesId))
                {
                    ApplyEvent(new CityChanged() { PlanCitiesId = iEnum.Current.Value.PlanCitiesId, Budget = iEnum.Current.Value.Budget, PlannerId = iEnum.Current.Value.PlannerId, Region = iEnum.Current.Value.Region, PlanDetailsId = Id, LocationId = iEnum.Current.Value.LocationId });
                }
                else
                {
                    ApplyEvent(new CityAdded() { PlanCitiesId = iEnum.Current.Value.PlanCitiesId, Budget = iEnum.Current.Value.Budget, PlannerId = iEnum.Current.Value.PlannerId, Region = iEnum.Current.Value.Region, PlanDetailsId = Id, LocationId = iEnum.Current.Value.LocationId });
                }
            }
            ApplyEvent(new PlanUpdated() { PlanDetailsId = Id, UpdatedOn = DateTime.Today });
        }
        public void OnCityChanged(CityChanged cityChanged)
        {
            var planCity = _planCities[cityChanged.PlanCitiesId];
            planCity.LocationId = cityChanged.LocationId;
            planCity.Budget = cityChanged.Budget;
            planCity.PlannerId = cityChanged.PlannerId;
            planCity.Region = cityChanged.Region;
        }
        public void OnCityAdded(CityAdded cityAdded)
        {
            //if (_planCities == null)
            //    _planCities = new Dictionary<Guid, PlanCity>();
            PlanCities.Add(cityAdded.PlanCitiesId, new PlanCity { PlanCityId = cityAdded.PlanCitiesId, Budget = cityAdded.Budget, LocationId = cityAdded.LocationId, PlannerId = cityAdded.PlannerId, Region = cityAdded.Region });
        }

        public void AddOrModifySiteInPlan(Dictionary<Guid, PlanSiteDTO> sites, Dictionary<Guid, PlanSiteDTO> deselectedSites)
        {
            IEnumerator<KeyValuePair<Guid, PlanSiteDTO>> iEnum = sites.GetEnumerator();
            while (iEnum.MoveNext())
            {
                if (PlanCities[iEnum.Current.Value.PlanCityId].Sites.ContainsKey(iEnum.Current.Value.PlanSiteId) )
                {
                    ApplyEvent(new SiteDetailsChanged()
                                   {
                                       Addressline1 = iEnum.Current.Value.Addressline1,
                                       Addressline2 = iEnum.Current.Value.Addressline2,
                                       Addressline3 = iEnum.Current.Value.Addressline3,
                                       Days = iEnum.Current.Value.Days,
                                       DisplayCost = iEnum.Current.Value.DisplayCost,
                                       DisplayFromDate = iEnum.Current.Value.DisplayFromDate,
                                       DisplayRate = iEnum.Current.Value.DisplayRate,
                                       DisplayToDate = iEnum.Current.Value.DisplayToDate,
                                       DisplayVendor = iEnum.Current.Value.DisplayVendor,
                                       EndDate = iEnum.Current.Value.EndDate,
                                       H = iEnum.Current.Value.H,
                                       Illumination = iEnum.Current.Value.Illumination,
                                       MediaType = iEnum.Current.Value.MediaType,
                                       NoOfFaces = iEnum.Current.Value.NoOfFaces,
                                       PlanCityId = iEnum.Current.Value.PlanCityId,
                                       PlanSiteId = iEnum.Current.Value.PlanSiteId,
                                       Qty = iEnum.Current.Value.Qty,
                                       SiteName = iEnum.Current.Value.SiteName,
                                       SiteNo = iEnum.Current.Value.SiteNo,
                                       SiteSize = iEnum.Current.Value.SiteSize,
                                       SizeRatio = iEnum.Current.Value.SizeRatio,
                                       StartDate = iEnum.Current.Value.StartDate,
                                       V = iEnum.Current.Value.V,
                                       SiteSizeInSqFt = iEnum.Current.Value.V*iEnum.Current.Value.H*iEnum.Current.Value.Qty,
                                   });
                }
                else
                {
                    ApplyEvent(new SiteAdded()
                    {
                        Addressline1 = iEnum.Current.Value.Addressline1,
                        Addressline2 = iEnum.Current.Value.Addressline2,
                        Addressline3 = iEnum.Current.Value.Addressline3,
                        Days = iEnum.Current.Value.Days,
                        DisplayCost = iEnum.Current.Value.DisplayCost,
                        DisplayFromDate = iEnum.Current.Value.DisplayFromDate,
                        DisplayRate = iEnum.Current.Value.DisplayRate,
                        DisplayToDate = iEnum.Current.Value.DisplayToDate,
                        DisplayVendor = iEnum.Current.Value.DisplayVendor,
                        EndDate = iEnum.Current.Value.EndDate,
                        H = iEnum.Current.Value.H,
                        Illumination = iEnum.Current.Value.Illumination,
                        MediaType = iEnum.Current.Value.MediaType,
                        NoOfFaces = iEnum.Current.Value.NoOfFaces,
                        PlanCityId = iEnum.Current.Value.PlanCityId,
                        PlanSiteId = iEnum.Current.Value.PlanSiteId,
                        Qty = iEnum.Current.Value.Qty,
                        SiteName = iEnum.Current.Value.SiteName,
                        SiteNo = iEnum.Current.Value.SiteNo,
                        SiteSize = iEnum.Current.Value.SiteSize,
                        SizeRatio = iEnum.Current.Value.SizeRatio,
                        StartDate = iEnum.Current.Value.StartDate,
                        V = iEnum.Current.Value.V,
                        SiteSizeInSqFt = iEnum.Current.Value.V * iEnum.Current.Value.H * iEnum.Current.Value.Qty,
                    });
                }
            }
            IEnumerator<KeyValuePair<Guid, PlanSiteDTO>> iEnumDeselected = deselectedSites.GetEnumerator();
            while (iEnumDeselected.MoveNext())
            {
                ApplyEvent(new SiteDeselected()
                               {
                                   PlanSiteId = iEnumDeselected.Current.Value.PlanSiteId,
                                   PlanCityId = iEnumDeselected.Current.Value.PlanCityId,
                                   SiteNo = iEnumDeselected.Current.Value.SiteNo
                               });
            }
            ApplyEvent(new PlanUpdated() { PlanDetailsId = Id, UpdatedOn = DateTime.Today });
        }
        public void OnSiteAdded(SiteAdded evnt)
        {
            var site = new PlanSite();
            site.Addressline1 = evnt.Addressline1;
            site.Addressline2 = evnt.Addressline2;
            site.Addressline3 = evnt.Addressline3;
            site.Days = evnt.Days;
            site.DisplayCost = evnt.DisplayCost;
            site.DisplayFromDate = evnt.DisplayFromDate;
            site.DisplayRate = evnt.DisplayRate;
            site.DisplayToDate = evnt.DisplayToDate;
            site.DisplayVendor = evnt.DisplayVendor;
            site.EndDate = evnt.EndDate;
            site.H = evnt.H;
            site.Illumination = evnt.Illumination;
            site.MediaType = evnt.MediaType;
            site.NoOfFaces = evnt.NoOfFaces;
            site.PlanCityId = evnt.PlanCityId;
            site.PlanSiteId = evnt.PlanSiteId;
            site.Qty = evnt.Qty;
            site.SiteName = evnt.SiteName;
            site.SiteNo = evnt.SiteNo;
            site.SiteSize = evnt.SiteSize;
            site.SizeRatio = evnt.SizeRatio;
            site.StartDate = evnt.StartDate;
            site.V = evnt.V;
            site.SiteSizeInSqFt = evnt.SiteSizeInSqFt;
            PlanCities[evnt.PlanCityId].Sites.Add(evnt.PlanSiteId, site);
        }
        public void OnSiteDetailsAdded(SiteDetailsChanged evnt)
        {
            var site = PlanCities[evnt.PlanCityId].Sites[evnt.PlanSiteId];
            site.Addressline1 = evnt.Addressline1;
            site.Addressline2 = evnt.Addressline2;
            site.Addressline3 = evnt.Addressline3;
            site.Days = evnt.Days;
            site.DisplayCost = evnt.DisplayCost;
            site.DisplayFromDate = evnt.DisplayFromDate;
            site.DisplayRate = evnt.DisplayRate;
            site.DisplayToDate = evnt.DisplayToDate;
            site.DisplayVendor = evnt.DisplayVendor;
            site.EndDate = evnt.EndDate;
            site.H = evnt.H;
            site.Illumination = evnt.Illumination;
            site.MediaType = evnt.MediaType;
            site.NoOfFaces = evnt.NoOfFaces;
            site.PlanCityId = evnt.PlanCityId;
            site.PlanSiteId = evnt.PlanSiteId;
            site.Qty = evnt.Qty;
            site.SiteName = evnt.SiteName;
            site.SiteNo = evnt.SiteNo;
            site.SiteSize = evnt.SiteSize;
            site.SizeRatio = evnt.SizeRatio;
            site.StartDate = evnt.StartDate;
            site.V = evnt.V;
            site.SiteSizeInSqFt = evnt.SiteSizeInSqFt;
        }
        public void OnSiteDeselected(SiteDeselected evnt)
        {
            var planSite = PlanCities[evnt.PlanCityId].Sites[evnt.PlanSiteId];
            PlanCities[evnt.PlanCityId].Sites.Remove(evnt.PlanSiteId);
            if(!PlanCities[evnt.PlanCityId].DeselectedSites.ContainsKey(evnt.PlanSiteId))
                PlanCities[evnt.PlanCityId].DeselectedSites.Add(evnt.PlanSiteId, planSite);
        }

        public void ChangeDisplayInfo(Guid planCityId, Dictionary<Guid, PlanSiteDisplayInfoDTO> sites)
        {
            IEnumerator<KeyValuePair<Guid, PlanSiteDisplayInfoDTO>> iEnum = sites.GetEnumerator();
            while (iEnum.MoveNext())
            {
                ApplyEvent(new SiteDisplayInfoChanged()
                               {
                                   PlanSiteId = iEnum.Current.Value.PlanSiteId,
                                   PlanCityId = iEnum.Current.Value.PlanCityId,
                                   DisplayCost = iEnum.Current.Value.DisplayCost,
                                   DisplayRate = iEnum.Current.Value.DisplayRate,
                                   StartDate = iEnum.Current.Value.DisplayFromDate,
                                   EndDate = iEnum.Current.Value.DisplayToDate,
                                   DisplayVendor = iEnum.Current.Value.DisplayVendor,
                                   DisplayClientRate = iEnum.Current.Value.DisplayClientRate,
                                   DisplayClientCost = iEnum.Current.Value.DisplayClientCost
                               });
            }
        }
        public void OnSiteDisplayInfoChanged( SiteDisplayInfoChanged evnt)
        {
            var site = PlanCities[evnt.PlanCityId].Sites[evnt.PlanSiteId];
            site.DisplayCost = evnt.DisplayCost;
            site.DisplayRate = evnt.DisplayRate;
            site.DisplayFromDate = evnt.StartDate;
            site.DisplayToDate = evnt.EndDate;
            site.DisplayVendor = evnt.DisplayVendor;
            site.DisplayClientRate = evnt.DisplayClientRate;
            site.DisplayClientCost = evnt.DisplayClientCost;
            site.DisplayStatus = evnt.DisplayStatus;
        }

        #region change mounting info
        public void ChangeMountingInfo(Guid planCityId, Dictionary<Guid, PlanSiteMountingInfoDTO> sites)
        {
            IEnumerator<KeyValuePair<Guid, PlanSiteMountingInfoDTO>> iEnum = sites.GetEnumerator();
            while (iEnum.MoveNext())
            {
                ApplyEvent(new SiteMountingInfoChanged()
                {
                    PlanSiteId = iEnum.Current.Value.PlanSiteId,
                    PlanCityId = iEnum.Current.Value.PlanCityId,
                    MountingCost = iEnum.Current.Value.MountingCost,
                    MountingRate = iEnum.Current.Value.MountingRate,
                    MountingVendor = iEnum.Current.Value.MountingVendor,
                    MountingClientRate = iEnum.Current.Value.MountingClientRate,
                    MountingClientCost = iEnum.Current.Value.MountingClientCost,
                });
            }
        }
        public void OnSiteMountingInfoChanged(SiteMountingInfoChanged evnt)
        {
            var site = PlanCities[evnt.PlanCityId].Sites[evnt.PlanSiteId];
            site.MountingCost = evnt.MountingCost;
            site.MountingRate = evnt.MountingRate;
            site.MountingVendor = evnt.MountingVendor;
            site.MountingClientRate = evnt.MountingClientRate;
            site.MountingClientCost = evnt.MountingClientCost;
            site.MountingStatus = evnt.MountingStatus;
        }
        #endregion change mounting info

        #region Vendor-Site Assignment
        public void SiteVendorAssignment(List<SiteVendorAssignmentDTO> sites)
        {
            IEnumerator<SiteVendorAssignmentDTO> iEnum = sites.GetEnumerator();
            while (iEnum.MoveNext())
            {
                switch (iEnum.Current.VendorType)
                {
                    case "Display" : ApplyEvent(new SiteDisplayInfoChanged(){StartDate = iEnum.Current.StartDate, EndDate = iEnum.Current.EndDate, DisplayVendor= iEnum.Current.Vendor, DisplayRate = iEnum.Current.Rate, DisplayCost = iEnum.Current.Cost , PlanCityId = iEnum.Current.PlanCityId, PlanSiteId =  iEnum.Current.PlanSiteId, DisplayStatus = iEnum.Current.Status, DisplayClientRate = iEnum.Current.ClientRate, DisplayClientCost = iEnum.Current.ClientCost});
                             break;
                    case "Mounting": ApplyEvent(new SiteMountingInfoChanged() { MountingVendor = iEnum.Current.Vendor, MountingRate = iEnum.Current.Rate, MountingCost = iEnum.Current.Cost, PlanCityId = iEnum.Current.PlanCityId, PlanSiteId = iEnum.Current.PlanSiteId, MountingStatus = iEnum.Current.Status, MountingClientRate= iEnum.Current.ClientRate, MountingClientCost= iEnum.Current.ClientCost });
                             break;
                    case "Printing": ApplyEvent(new SitePrintingInfoChanged() { PrintingVendor = iEnum.Current.Vendor, PrintingRate = iEnum.Current.Rate, PrintingCost = iEnum.Current.Cost, PlanCityId = iEnum.Current.PlanCityId, PlanSiteId = iEnum.Current.PlanSiteId, PrintingStatus = iEnum.Current.Status, PrintingClientRate = iEnum.Current.ClientRate, PrintingClientCost = iEnum.Current.ClientCost });
                             break;
                    case "Fabrication": ApplyEvent(new SiteFabricationInfoChanged() { FabricationVendor = iEnum.Current.Vendor, FabricationRate = iEnum.Current.Rate, FabricationCost = iEnum.Current.Cost, PlanCityId = iEnum.Current.PlanCityId, PlanSiteId = iEnum.Current.PlanSiteId, FabricationStatus = iEnum.Current.Status, FabricationClientRate = iEnum.Current.ClientRate, FabricationClientCost= iEnum.Current.ClientCost });
                             break; 
                }
            }
            ApplyEvent(new PlanUpdated() { PlanDetailsId = Id, UpdatedOn = DateTime.Today });
        }
        #endregion Vendor-Site Assignment
        public void OnSitePrintingInfoChanged(SitePrintingInfoChanged evnt)
        {
            var site = PlanCities[evnt.PlanCityId].Sites[evnt.PlanSiteId];
            site.PrintingCost = evnt.PrintingCost;
            site.PrintingRate = evnt.PrintingRate;
            site.PrintingVendor = evnt.PrintingVendor;
            site.PrintingStatus = evnt.PrintingStatus;
            site.PrintingClientRate = evnt.PrintingClientRate;
            site.PrintingClientCost = evnt.PrintingClientCost;
        }
        public void OnSiteFabricationInfoChanged(SiteFabricationInfoChanged evnt)
        {
            var site = PlanCities[evnt.PlanCityId].Sites[evnt.PlanSiteId];
            site.FabricationCost = evnt.FabricationCost;
            site.FabricationRate = evnt.FabricationRate;
            site.FabricationVendor = evnt.FabricationVendor;
            site.FabricationStatus = evnt.FabricationStatus;
            site.FabricationClientRate= evnt.FabricationClientRate;
            site.FabricationClientCost = evnt.FabricationClientCost;
        }
    }
}
