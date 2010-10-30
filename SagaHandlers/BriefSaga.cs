using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ncqrs;
using Ncqrs.Eventing.ServiceModel.Bus;
using Events;
using Ncqrs.Domain;
using Domain;

namespace SagaHandlers
{
    //public class BriefSaga : ISagaHandler<NewBriefAdded>
    //{
    //    public void Handle(NewBriefAdded evnt)
    //    {
    //        //var aggregateRoot = UnitOfWork.Current.GetById(typeof (Plan), evnt.BriefId);
    //        AggregateRoot aggregateRoot = new BriefAllocation(evnt.BriefId, evnt.BriefNo, DateTime.Now, "", null);
    //        UnitOfWork.Current.RegisterDirtyInstance(aggregateRoot);
    //    }
    //}
}
