using System;
using System.Linq;
using Events;
using Ncqrs.Eventing.ServiceModel.Bus;

namespace ReadModel.Denormalizers
{
   public class BriefDenormalizer : IEventHandler<NewBriefAdded>, IEventHandler<BriefChanged>
    {

        public void Handle(NewBriefAdded evnt)
        {
            using (var context = new MyNotesReadModelEntities())
            {
                var newItem = new ptmPlanVersionsC();
                

                newItem.sPlanVersionsCSID = evnt.BriefId.ToString();
                if (evnt.BriefDate !=  null)
                    newItem.dtVoucher = (DateTime) evnt.BriefDate;
                else
                {
                    newItem.dtVoucher = System.DateTime.UtcNow;
                }
                newItem.sAdditionalInfo = evnt.AdditionalInfo;
                newItem.sContactIDFK = evnt.Customer;
                newItem.sCPersonIDFK = evnt.ContactPerson;
                newItem.nBudgetValue = evnt.BudgetValue;
                newItem.sClientBusiProfile = evnt.ClientBusinessProfile;
                newItem.sBudgetTypes = evnt.BudgetTypes;
                newItem.sApprovedPlan = evnt.IsApproved.ToString();
                newItem.sApprovalNo = evnt.ApprovalNo.ToString();
                if (evnt.ApprovalDate!= null)
                    newItem.dApproval = evnt.ApprovalDate;
                if (evnt.DeadLineDate != null)
                    newItem.dDeadlineDate = evnt.DeadLineDate;
                newItem.sBrand1 = evnt.Brand1;
                if (evnt.TentativeStartDate!= null)
                    newItem.dTentativeStartDate = evnt.TentativeStartDate;
                if (evnt.TentativeEndDate != null)
                    newItem.dTentativeEndDate = evnt.TentativeEndDate;
                newItem.nNoOfYears = evnt.NoOfYears;
                newItem.nOtherSTAgencyComm = evnt.OtherStCommision;
                newItem.sMarketingObjective = evnt.MarketingObjective;
                if(evnt.RequestSentToHO != null)
                    newItem.dRequestSentTOHo = evnt.RequestSentToHO;
                newItem.sCompetitionBrand1 = evnt.CompetitionBrand1;
                newItem.sCompetitionBrand2 = evnt.CompetitionBrand2;
                newItem.sBriefNo = Series.AutoGenerateNumber(context, "ptmPlanVersionsC", "sBriefNo").ToString(); //evnt.BriefNo.ToString();
                newItem.sBrand2 = evnt.Brand2;
                newItem.nNoOfMonths = evnt.NoOfMonths;
                newItem.nNoOfDays = evnt.NoOfDays;
                newItem.nBriefStatus = evnt.BriefStatus;
                newItem.nOSDays = evnt.OsDays;
                if (evnt.DateCreated != null)
                    newItem.dtSave = evnt.DateCreated;
                newItem.sBrand3 = evnt.Brand3;
                newItem.sCompetitionBrand3 = evnt.CompetitionBrand3;
                if (evnt.SendDate != null)
                    newItem.dtSend = evnt.SendDate;
                newItem.sHeadPlannerUserIDFK = evnt.HeadPlannerUserId.ToString();
                newItem.nInPrintFL = evnt.InPrintFl;
                newItem.sAdObjective = evnt.AdObjective;
                newItem.sBrand4 = evnt.Brand4;
                newItem.sLayOutAdapLimitations = evnt.LayoutAdapLimitations;
                newItem.nInPrintBL = evnt.InPrintBl;
                newItem.sCompetitionBrand4 = evnt.CompetitionBrand4;
                newItem.nInPainting = evnt.InPainting;
                newItem.sTargetGroupBrand1 = evnt.TargetGroupBrand1;
                newItem.sTypofVehicles = evnt.TypeOfVehicles;
                newItem.sTGSMediaB1 = evnt.TgsMedia1;
                newItem.PreferredSize = evnt.PreferredSize;
                newItem.bOthers = evnt.OtherMediaVehicle;
                newItem.sAdMIX = evnt.AdMix;
                newItem.sDisplayType = evnt.DisplayType;
                newItem.sTargetGroupBrand2 = evnt.TargetGroupBrand2;
                newItem.sTGSMediaB2 = evnt.TgsMedia2;
                newItem.sDisplayTypesSel = evnt.DisplayTypeSel;
                newItem.sTGSMediaB3 = evnt.TgsMedia3;
                newItem.sTargetGroupBrand3 = evnt.TargetGroupBrand3;
                newItem.sTGSMediaB4 = evnt.TgsMedia4;
                newItem.sTargetGroupBrand4 = evnt.TargetGroupBrand4;
                newItem.sOutdoorObjective = evnt.OutDoorObjective;
                newItem.sMarket1 = evnt.Market1;
                newItem.sMarket2 = evnt.Market2;
                newItem.sMarket3 = evnt.Market3;
                newItem.sMarket4 = evnt.Market4;
                newItem.InVinyl = evnt.InVinyl;
                newItem.Complete = evnt.Complete;
                newItem.PartVinyl = evnt.PartVinyl;
                newItem.Ratio11 = evnt.Ratio11;
                newItem.Ratio12 = evnt.Ratio12;
                newItem.Ratio13 = evnt.Ratio13;
                newItem.Ratio14 = evnt.Ratio14;
                newItem.Ratio34 = evnt.Ratio34;
                newItem.RatioAll = evnt.RatioAll;
                newItem.BillBoard = evnt.BillBoard;
                newItem.Buses = evnt.Buses;
                newItem.PoleKiosks = evnt.PoleKiosks;
                newItem.BusQShelter = evnt.BusQShelter;
                newItem.TransitMedia = evnt.TransitMedia;
                newItem.PublicUtitities = evnt.PublicUtitities;
                newItem.sOthers = evnt.Others;
                newItem.IsInActive = evnt.IsInActive;

                context.Briefs.AddObject(newItem);
                context.SaveChanges();
            }
        }

        public void Handle(BriefChanged evnt)
        {
            string id = evnt.BriefId.ToString();
            using (var context = new MyNotesReadModelEntities())
            {
                var itemToUpdate = context.Briefs.Single(item => item.sPlanVersionsCSID == id);
                if (evnt.BriefDate != null)
                    itemToUpdate.dtVoucher = (DateTime) evnt.BriefDate;
                itemToUpdate.sAdditionalInfo = evnt.AdditionalInfo;
                itemToUpdate.sContactIDFK = evnt.Customer;
                itemToUpdate.sCPersonIDFK = evnt.ContactPerson;
                itemToUpdate.nBudgetValue = evnt.BudgetValue;
                itemToUpdate.sClientBusiProfile = evnt.ClientBusinessProfile;
                itemToUpdate.sBudgetTypes = evnt.BudgetTypes;
                itemToUpdate.sApprovedPlan = evnt.IsApproved.ToString();
                itemToUpdate.sApprovalNo = evnt.ApprovalNo.ToString();
                if (evnt.ApprovalDate != null)
                    itemToUpdate.dApproval = evnt.ApprovalDate;
                if (evnt.DeadLineDate != null)
                    itemToUpdate.dDeadlineDate = evnt.DeadLineDate;
                itemToUpdate.sBrand1 = evnt.Brand1;
                if (evnt.TentativeStartDate != null)
                    itemToUpdate.dTentativeStartDate = evnt.TentativeStartDate;
                if (evnt.TentativeEndDate != null)
                    itemToUpdate.dTentativeEndDate = evnt.TentativeEndDate;
                itemToUpdate.nNoOfYears = evnt.NoOfYears;
                itemToUpdate.nOtherSTAgencyComm = evnt.OtherStCommision;
                itemToUpdate.sMarketingObjective = evnt.MarketingObjective;
                if (evnt.RequestSentToHO != null)
                    itemToUpdate.dRequestSentTOHo = evnt.RequestSentToHO;
                itemToUpdate.sCompetitionBrand1 = evnt.CompetitionBrand1;
                itemToUpdate.sCompetitionBrand2 = evnt.CompetitionBrand2;
                itemToUpdate.sBriefNo = evnt.BriefNo.ToString();
                itemToUpdate.sBrand2 = evnt.Brand2;
                itemToUpdate.nNoOfMonths = evnt.NoOfMonths;
                itemToUpdate.nNoOfDays = evnt.NoOfDays;
                itemToUpdate.nBriefStatus = evnt.BriefStatus;
                itemToUpdate.nOSDays = evnt.OsDays;
                if (evnt.DateCreated != null)
                    itemToUpdate.dtSave = evnt.DateCreated;
                itemToUpdate.sBrand3 = evnt.Brand3;
                itemToUpdate.sCompetitionBrand3 = evnt.CompetitionBrand3;
                if (evnt.SendDate != null)
                    itemToUpdate.dtSend = evnt.SendDate;
                itemToUpdate.sHeadPlannerUserIDFK = evnt.HeadPlannerUserId.ToString();
                itemToUpdate.nInPrintFL = evnt.InPrintFl;
                itemToUpdate.sAdObjective = evnt.AdObjective;
                itemToUpdate.sBrand4 = evnt.Brand4;
                itemToUpdate.sLayOutAdapLimitations = evnt.LayoutAdapLimitations;
                itemToUpdate.nInPrintBL = evnt.InPrintBl;
                itemToUpdate.sCompetitionBrand4 = evnt.CompetitionBrand4;
                itemToUpdate.nInPainting = evnt.InPainting;
                itemToUpdate.sTargetGroupBrand1 = evnt.TargetGroupBrand1;
                itemToUpdate.sTypofVehicles = evnt.TypeOfVehicles;
                itemToUpdate.sTGSMediaB1 = evnt.TgsMedia1;
                itemToUpdate.PreferredSize = evnt.PreferredSize;
                itemToUpdate.bOthers = evnt.OtherMediaVehicle;
                itemToUpdate.sAdMIX = evnt.AdMix;
                itemToUpdate.sDisplayType = evnt.DisplayType;
                itemToUpdate.sTargetGroupBrand2 = evnt.TargetGroupBrand2;
                itemToUpdate.sTGSMediaB2 = evnt.TgsMedia2;
                itemToUpdate.sDisplayTypesSel = evnt.DisplayTypeSel;
                itemToUpdate.sTGSMediaB3 = evnt.TgsMedia3;
                itemToUpdate.sTargetGroupBrand3 = evnt.TargetGroupBrand3;
                itemToUpdate.sTGSMediaB4 = evnt.TgsMedia4;
                itemToUpdate.sTargetGroupBrand4 = evnt.TargetGroupBrand4;
                itemToUpdate.sOutdoorObjective = evnt.OutDoorObjective;
                itemToUpdate.sMarket1 = evnt.Market1;
                itemToUpdate.sMarket2 = evnt.Market2;
                itemToUpdate.sMarket3 = evnt.Market3;
                itemToUpdate.sMarket4 = evnt.Market4;
                itemToUpdate.InVinyl = evnt.InVinyl;
                itemToUpdate.Complete = evnt.Complete;
                itemToUpdate.PartVinyl = evnt.PartVinyl;
                itemToUpdate.Ratio11 = evnt.Ratio11;
                itemToUpdate.Ratio12 = evnt.Ratio12;
                itemToUpdate.Ratio13 = evnt.Ratio13;
                itemToUpdate.Ratio14 = evnt.Ratio14;
                itemToUpdate.Ratio34 = evnt.Ratio34;
                itemToUpdate.RatioAll = evnt.RatioAll;
                itemToUpdate.BillBoard = evnt.BillBoard;
                itemToUpdate.Buses = evnt.Buses;
                itemToUpdate.PoleKiosks = evnt.PoleKiosks;
                itemToUpdate.BusQShelter = evnt.BusQShelter;
                itemToUpdate.TransitMedia = evnt.TransitMedia;
                itemToUpdate.PublicUtitities = evnt.PublicUtitities;
                itemToUpdate.sOthers = evnt.Others;
                itemToUpdate.IsInActive = evnt.IsInActive;
                context.SaveChanges();
            }
        }
    }
}
