using System;
using System.Collections.Generic;
using Ncqrs.Eventing.Sourcing;

namespace Events
{
    [Serializable]
    public class BriefAllocated : SourcedEvent
    {
        public Guid PlanId { get; set; }
        public int BriefNo { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string HeadPlannerId { get; set; }
        public Dictionary<Guid, RegionsAndCitiesAdded> RegionCities { get; set; }
        public BriefAllocated()
        {
            RegionCities = new Dictionary<Guid, RegionsAndCitiesAdded>();
        }
    }
    public class BriefAllocationModified : SourcedEvent
    {
        public Guid PlanId { get; set; }
        public int BriefNo { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string HeadPlannerId { get; set; }
        public Dictionary<Guid, RegionsAndCitiesAdded> RegionCitiesAdded { get; set; }
        public Dictionary<Guid, RegionsAndCitiesModified> RegionCitiesEdited { get; set; }

        public BriefAllocationModified()
        {
            RegionCitiesAdded = new Dictionary<Guid, RegionsAndCitiesAdded>();
            RegionCitiesEdited = new Dictionary<Guid, RegionsAndCitiesModified>();
        }
    }
    public class RegionsAndCitiesAdded :SourcedEvent
    {
        public Guid RegionAndCitiesId { get; set; }
        public string Region { get; set; }
        public string LocationId { get; set; }
        public string PlannerId { get; set; }
        public decimal Budget { get; set; }
    }

    public class RegionsAndCitiesModified : SourcedEvent
    {
        public Guid RegionAndCitiesId { get; set; }
        public string Region { get; set; }
        public string LocationId { get; set; }
        public string PlannerId { get; set; }
        public decimal Budget { get; set; }
    }
}
