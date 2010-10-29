using System;
using Commands;
using Ncqrs;
using Ncqrs.Commanding.ServiceModel;

namespace CommandService
{
    public class MyNotesCommandService : IMyNotesCommandService
    {
        private static ICommandService _service;

        static MyNotesCommandService()
        {
            BootStrapper.BootUp();

            _service = NcqrsEnvironment.Get<ICommandService>();
        }

        public void CreateNewNote(CreateNewNote command)
        {
            _service.Execute(command);
        }

        public void ChangeNoteText(ChangeNoteText command)
        {
            _service.Execute(command);
        }


        public void CreateNewBrief(CreateBrief command)
        {
           _service.Execute(command);
        }

        public void ChangeBrief(ChangeBrief command)
        {
            _service.Execute(command);
        }


        public void CreateBriefAllocation(CreateBriefAllocation command)
        {
            _service.Execute(command);
        }
        public void ChangeBriefAllocation(EditBriefAllocation command)
        {
            _service.Execute(command);
        }

        public void CreatePlan(CreatePlan command)
        {
            _service.Execute(command);
        }


        public void ChangePlan(EditPlan command)
        {
            _service.Execute(command);
        }

        public void CreateRegionInPlan(CreateRegionInPlan command)
        {
            _service.Execute(command);
        }
        public void CreateOrChangeCitiesInPlan(CreateOrChangeCitiesInPlan command)
        {
            _service.Execute(command);
        }

        public void CreateOrChangeSitesInPlan(CreateOrChangeSitesInPlan command)
        {
            _service.Execute(command);
        }

        public void ChangeSiteDisplayInfo(ChangeDisplayInfoForSite command)
        {
            _service.Execute(command);
        }
        public void ChangeSiteMountingInfo(ChangeMountingInfoForSite command)
        {
            _service.Execute(command);
        }
        public void AssignVendorToSitesInfo(AssignVendorsToSite command)
        {
            _service.Execute(command);
        }
      
        public void CreatePlanAlbum(CreatePlanAlbum command)
        {
            _service.Execute(command);
        }

        public void AddOrRemovePhotosFromPlanAlbum(AddOrRemovePhotosFromPlanAlbum command)
        {
            _service.Execute(command);
        }
        public void AddOrRemovePhotosFromCity(AddOrRemovePhotosFromPlanCity command)
        {
            _service.Execute(command);
        }
        public void AddOrRemovePhotosFromPlanSite(AddOrRemovePhotosFromPlanSite command)
        {
            _service.Execute(command);
        }
    }
}
