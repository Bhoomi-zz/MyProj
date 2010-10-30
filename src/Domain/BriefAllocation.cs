using System;
using System.Collections.Generic;
using Ncqrs;
using Events;
using Ncqrs.Domain;
using CommonDTOs;

namespace Domain
{
    public class BriefAllocation :  AggregateRootMappedByConvention
    {
        public Guid Id
        {
            get { return EventSourceId; }
            set { EventSourceId = value; }
        }
        private Guid _planId;
        private int _briefNo;
        private DateTime? _createdOn;
        private string _headPlannerId;
        private Dictionary<Guid, RegionsAndCities> _regionCities;

        public BriefAllocation()
        {
            _regionCities = new Dictionary<Guid, RegionsAndCities>();
        }

        public BriefAllocation(Guid planId, int briefNo, DateTime? createdOn, string headPlannerId, List<RegionsAndCitiesDTO> regionCities)
        {
            planId = Id;
            var planCreated = new BriefAllocated() { PlanId = planId, BriefNo = briefNo, CreatedOn = createdOn, HeadPlannerId = headPlannerId };
            IEnumerator<CommonDTOs.RegionsAndCitiesDTO> iEnum = regionCities.GetEnumerator();
            while(iEnum.MoveNext())
            {
                planCreated.RegionCities.Add(iEnum.Current.RegionsAndCitiesId, new RegionsAndCitiesAdded() { RegionAndCitiesId = iEnum.Current.RegionsAndCitiesId, LocationId = iEnum.Current.LocationId, PlannerId = iEnum.Current.PlannerId, Budget = iEnum.Current.Budget, Region = iEnum.Current.Region });
            }
            ApplyEvent(planCreated);
        }
        public void OnPlanCreated(BriefAllocated e)
        {
            _planId = e.PlanId;
            _briefNo = e.BriefNo;
            _createdOn = e.CreatedOn;
            _headPlannerId = e.HeadPlannerId;
            IEnumerator<KeyValuePair<Guid, RegionsAndCitiesAdded>> iEnum = e.RegionCities.GetEnumerator();
            
            _regionCities = new Dictionary<Guid, RegionsAndCities>();
            while (iEnum.MoveNext())
            {
                _regionCities.Add(iEnum.Current.Value.RegionAndCitiesId,
                                  new RegionsAndCities
                                      {
                                          RegionAndCitiesId = iEnum.Current.Value.RegionAndCitiesId,
                                          LocationId = iEnum.Current.Value.LocationId,
                                          PlannerId = iEnum.Current.Value.PlannerId,
                                          Budget = iEnum.Current.Value.Budget,
                                          Region = iEnum.Current.Value.Region
                                      });
            }
        }
        public void ChangePlan(int briefNo, DateTime? createdOn, string headPlannerId, List<RegionsAndCitiesDTO> regionCities)
        {
            var planModified = new BriefAllocationModified() {  BriefNo = briefNo, CreatedOn = createdOn, HeadPlannerId = headPlannerId ,PlanId = Id};
            IEnumerator<CommonDTOs.RegionsAndCitiesDTO> iEnum = regionCities.GetEnumerator();
            while (iEnum.MoveNext())
            {
                if (!_regionCities.ContainsKey(iEnum.Current.RegionsAndCitiesId))
                {
                    planModified.RegionCitiesAdded.Add(iEnum.Current.RegionsAndCitiesId,
                                                      new RegionsAndCitiesAdded()
                                                          {
                                                              RegionAndCitiesId = iEnum.Current.RegionsAndCitiesId,
                                                              LocationId = iEnum.Current.LocationId,
                                                              PlannerId = iEnum.Current.PlannerId,
                                                              Budget = iEnum.Current.Budget,
                                                              Region = iEnum.Current.Region
                                                          });
                }
                else
                {
                    RegionsAndCities regionsAndCities = _regionCities[iEnum.Current.RegionsAndCitiesId];
                    if (regionsAndCities.Region != iEnum.Current.Region ||
                        regionsAndCities.Budget != iEnum.Current.Budget ||
                        regionsAndCities.LocationId != iEnum.Current.LocationId ||
                        regionsAndCities.PlannerId != iEnum.Current.PlannerId)
                    {
                        planModified.RegionCitiesEdited.Add(iEnum.Current.RegionsAndCitiesId,
                                                           new RegionsAndCitiesModified()
                                                               {
                                                                   RegionAndCitiesId = iEnum.Current.RegionsAndCitiesId,
                                                                   LocationId = iEnum.Current.LocationId,
                                                                   PlannerId = iEnum.Current.PlannerId,
                                                                   Budget = iEnum.Current.Budget,
                                                                   Region = iEnum.Current.Region
                                                               });
                    }
                }
            }
            ApplyEvent(planModified);
        }
        public void OnPlanModified(BriefAllocationModified e)
        {
            _planId = e.PlanId;
            _briefNo = e.BriefNo;
            _createdOn = e.CreatedOn;
            _headPlannerId = e.HeadPlannerId;
            IEnumerator<KeyValuePair<Guid, RegionsAndCitiesAdded>> iEnum = e.RegionCitiesAdded.GetEnumerator();
            if(_regionCities == null)
                _regionCities = new Dictionary<Guid, RegionsAndCities>();
            while (iEnum.MoveNext())
            {
                _regionCities.Add(iEnum.Current.Value.RegionAndCitiesId,
                                  new RegionsAndCities
                                  {
                                      RegionAndCitiesId = iEnum.Current.Value.RegionAndCitiesId,
                                      LocationId = iEnum.Current.Value.LocationId,
                                      PlannerId = iEnum.Current.Value.PlannerId,
                                      Budget = iEnum.Current.Value.Budget,
                                      Region = iEnum.Current.Value.Region
                                  });
            }

            IEnumerator<KeyValuePair<Guid, RegionsAndCitiesModified>> iEnumModified = e.RegionCitiesEdited.GetEnumerator();
            while (iEnumModified.MoveNext())
            {
                var regionsAndCities = _regionCities[iEnumModified.Current.Value.RegionAndCitiesId];
                regionsAndCities.LocationId = iEnumModified.Current.Value.LocationId;
                regionsAndCities.PlannerId= iEnumModified.Current.Value.PlannerId;
                regionsAndCities.Budget = iEnumModified.Current.Value.Budget;
                regionsAndCities.Region = iEnumModified.Current.Value.Region;
            }
        }
    }
}
