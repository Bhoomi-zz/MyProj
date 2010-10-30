using System;
using System.ServiceModel;
using Commands;



namespace CommandService
{
    [ServiceContract]
    public interface IMyNotesCommandService
    {
        [OperationContract]
        void CreateNewNote(CreateNewNote command);

        [OperationContract]
        void ChangeNoteText(ChangeNoteText command);

        [OperationContract]
        void CreateNewBrief(CreateBrief command);

        [OperationContract]
        void ChangeBrief(ChangeBrief command);

        [OperationContract]
        void CreateBriefAllocation(CreateBriefAllocation command);

        [OperationContract]
        void ChangeBriefAllocation(EditBriefAllocation command);

        [OperationContract]
        void CreatePlan(CreatePlan command);

        [OperationContract]
        void ChangePlan(EditPlan command);

        [OperationContract]
        void CreateRegionInPlan(CreateRegionInPlan command);

        [OperationContract]
        void CreateOrChangeCitiesInPlan(CreateOrChangeCitiesInPlan command);

        [OperationContract]
        void CreateOrChangeSitesInPlan(CreateOrChangeSitesInPlan command);

        [OperationContract]
        void ChangeSiteDisplayInfo(ChangeDisplayInfoForSite command);

        [OperationContract]
        void ChangeSiteMountingInfo(ChangeMountingInfoForSite command);

        [OperationContract]
        void AssignVendorToSitesInfo(AssignVendorsToSite command);

        [OperationContract]
        void CreatePlanAlbum(CreatePlanAlbum command);

        [OperationContract]
        void AddOrRemovePhotosFromPlanAlbum(AddOrRemovePhotosFromPlanAlbum command);

        [OperationContract]
        void AddOrRemovePhotosFromCity(AddOrRemovePhotosFromPlanCity command);

        [OperationContract]
        void AddOrRemovePhotosFromPlanSite(AddOrRemovePhotosFromPlanSite command);

    }
}
