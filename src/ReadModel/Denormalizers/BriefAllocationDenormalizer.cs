using System;
using System.Collections.Generic;
using System.Linq;
using Events;
using Ncqrs.Eventing.ServiceModel.Bus;

namespace ReadModel.Denormalizers
{
    public class BriefAllocationDenormalizer : IEventHandler<BriefAllocated>, IEventHandler<BriefAllocationModified>
    {

        public void Handle(BriefAllocated evnt)
        {
            using (var context = new MyNotesReadModelEntities())
            {
                var briefAllocation = new BriefAllocation();
                briefAllocation.BriefAllocationId = evnt.PlanId;
                briefAllocation.BriefNo = evnt.BriefNo.ToString();
                briefAllocation.CreatedOn = evnt.CreatedOn;
                briefAllocation.HeadPlannerID = evnt.HeadPlannerId;

                IEnumerator<KeyValuePair< Guid, RegionsAndCitiesAdded>> iEnum = evnt.RegionCities.GetEnumerator();
                while (iEnum.MoveNext())
                {
                    var regionCities = new AllocatedRegionAndCity() { AllocatedRegionAndCitiesId = iEnum.Current.Value.RegionAndCitiesId, LocationID = iEnum.Current.Value.LocationId, PlannerID = iEnum.Current.Value.PlannerId, budget = iEnum.Current.Value.Budget, Region = iEnum.Current.Value.Region, BriefAllocationId = evnt.PlanId };
                    context.AllocatedRegionAndCities.AddObject(regionCities);
                }
                context.BriefAllocations.AddObject(briefAllocation);
                context.SaveChanges();
            }
        }

        public void Handle(BriefAllocationModified evnt)
        {
            Guid id = evnt.PlanId;
            using (var context = new MyNotesReadModelEntities())
            {
                var itemToUpdate = context.BriefAllocations.Single(item => item.BriefAllocationId == id);
                itemToUpdate.BriefAllocationId = evnt.PlanId;
                itemToUpdate.BriefNo = evnt.BriefNo.ToString();
                itemToUpdate.CreatedOn = evnt.CreatedOn;
                itemToUpdate.HeadPlannerID = evnt.HeadPlannerId;

                IEnumerator<KeyValuePair<Guid, RegionsAndCitiesAdded>> iEnum = evnt.RegionCitiesAdded.GetEnumerator();
                while (iEnum.MoveNext())
                {
                    var regionCities = new AllocatedRegionAndCity() { AllocatedRegionAndCitiesId = iEnum.Current.Value.RegionAndCitiesId, LocationID = iEnum.Current.Value.LocationId, PlannerID = iEnum.Current.Value.PlannerId, budget = iEnum.Current.Value.Budget, Region = iEnum.Current.Value.Region, BriefAllocationId = evnt.PlanId };
                    context.AllocatedRegionAndCities.AddObject(regionCities);
                }

                IEnumerator<KeyValuePair<Guid, RegionsAndCitiesModified>> iEnumModified = evnt.RegionCitiesEdited.GetEnumerator();
                while (iEnumModified.MoveNext())
                {
                    var regionCity =
                        context.AllocatedRegionAndCities.Single(
                            item => item.AllocatedRegionAndCitiesId == iEnumModified.Current.Value.RegionAndCitiesId);
                    regionCity.LocationID = iEnumModified.Current.Value.LocationId;
                    regionCity.budget = iEnumModified.Current.Value.Budget;
                    regionCity.PlannerID = iEnumModified.Current.Value.PlannerId;
                    regionCity.Region = iEnumModified.Current.Value.Region;
                }
                context.SaveChanges();
            }
        }
    }
}
