using System;
using Ncqrs;
using Events;
using Ncqrs.Domain;


namespace Domain
{
    public class Brief : AggregateRootMappedByConvention
    {
        public Guid Id
        {
            get { return EventSourceId; }
            set { EventSourceId = value; }
        }

        private DateTime? _briefDate;
        private string _customer;
        private string _additionalInfo;
        private decimal _budgetValue;
        private string _contactPerson;
       
        private string _clientBusinessProfile ;
        private string _budgetTypes ;
        private bool _isApproved ;
        private int _approvalNo ;
        private DateTime? _approvalDate ;
        private DateTime? _deadLineDate ;
        private string _brand1 ;
        private DateTime? _tentativeStartDate ;
        private DateTime? _tentativeEndDate ;
        private int _noOfYears ;
        private int _otherStCommision ;
        private string _marketingObjective ;
        private DateTime? _requestSentToHO ;
        private string _competitionBrand1 ;
        private string _competitionBrand2 ;
        private string _preparedBy ;
        private int _briefNo ;
        private string _brand2 ;
        private int _noOfMonths ;
        private int _noOfDays ;
        private int _briefStatus ;
        private int _osDays ;
        private DateTime? _dateCreated ;
        private string _brand3 ;
        private string _competitionBrand3 ;
        private DateTime? _sendDate ;
        private Guid _headPlannerUserId ;
        private bool _inPrintFl ;
        private string _adObjective ;
        private string _brand4 ;
        private string _layoutAdapLimitations ;
        private bool _inPrintBl ;
        private string _competitionBrand4 ;
        
        private bool _inPainting ;
        private string _targetGroupBrand1 ;
        private string _typeOfVehicles ;
        private string _tgsMedia1 ;
        
        private string _preferredSize ;
        private string _others ;
        private string _adMix ;
        private string _displayType ;
        private string _targetGroupBrand2 ;
        private string _tgsMedia2 ;
        private string _displayTypeSel ;
        private string _tgsMedia3 ;
        private string _targetGroupBrand3 ;
        private string _tgsMedia4 ;
        private string _targetGroupBrand4 ;
        private string _outDoorObjective ;
        private string _market1 ;
        private string _market2 ;
        private string _market3 ;
        private string _market4 ;
        private bool _inVinyl ;
        private bool _complete ;
        private bool _partVinyl ;
        private bool _ratio11 ;
        private bool _ratio12 ;
        private bool _ratio13 ;
        private bool _ratio14 ;
        private bool _ratio34 ;
        private bool _ratioAll ;
        private bool _billBoard ;
        private bool _buses ;
        private bool _poleKiosks ;
        private bool _busQShelter ;
        private bool _transitMedia ;
        private bool _publicUtitities ;
        private bool _otherMediaVehicle ;
        private bool _IsInActive;
        private Brief()
        {
        }

        public Brief(DateTime? briefdate, string customer, string additionalInfo, decimal budgetValue, string contactPerson, string contactPersonId ,
              string clientBusinessProfile, string budgetTypes, bool isApproved, int approvalNo, DateTime? approvalDate, DateTime? deadLineDate, string brand1,
              DateTime? tentativeStartDate, DateTime? tentativeEndDate, int noOfYears, int otherStCommision, string marketingObjective, DateTime? requestSentToHO,
              string competitionBrand1, string competitionBrand2, string preparedBy, int briefNo, string brand2, int noOfMonths, int noOfDays, int briefStatus,
              int osDays, DateTime? dateCreated, string brand3, string competitionBrand3, DateTime? sendDate, Guid headPlannerUserId, bool inPrintFl, string adObjective,
              string brand4, string layoutAdapLimitations, bool inPrintBl, string competitionBrand4,  bool inPainting, string targetGroupBrand1,
              string typeOfVehicles, string tgsMedia1, string preferredSize, string others, string adMix, string displayType, string targetGroupBrand2,
              string tgsMedia2, string displayTypeSel, string tgsMedia3, string targetGroupBrand3, string tgsMedia4, string targetGroupBrand4, string outDoorObjective,
              string market1, string market2, string market3, string market4, bool inVinyl, bool complete, bool partVinyl, bool ratio11, bool ratio12, bool ratio13,
              bool ratio14, bool ratio34, bool ratioAll, bool billBoard, bool buses, bool poleKiosks, bool busQShelter, bool transitMedia, bool publicUtitities, bool otherMediaVehicle, bool isInActive)
        {

            ApplyEvent(new NewBriefAdded
                           {
                               BriefId = Id,
                               BriefDate = briefdate,
                               AdditionalInfo = additionalInfo,
                               BudgetValue = budgetValue,
                               Customer = customer,
                               ContactPerson = contactPerson,
                               ClientBusinessProfile = clientBusinessProfile,
                               BudgetTypes = budgetTypes,
                               IsApproved = isApproved,
                               ApprovalNo = approvalNo,
                               ApprovalDate = approvalDate,
                               DeadLineDate = deadLineDate,
                               Brand1 = brand1,
                               TentativeStartDate = tentativeStartDate,
                               TentativeEndDate = tentativeEndDate,
                               NoOfYears = noOfYears,
                               OtherStCommision = otherStCommision,
                               MarketingObjective = marketingObjective,
                               RequestSentToHO = requestSentToHO,
                               CompetitionBrand1 = competitionBrand1,
                               CompetitionBrand2 = competitionBrand2,
                               PreparedBy = preparedBy,
                               BriefNo = briefNo,
                               Brand2 = brand2,
                               NoOfMonths = noOfMonths,
                               NoOfDays = noOfDays,
                               BriefStatus = briefStatus,
                               OsDays = osDays,
                               DateCreated = dateCreated,
                               Brand3 = brand3,
                               CompetitionBrand3 = competitionBrand3,
                               SendDate = sendDate,
                               HeadPlannerUserId = headPlannerUserId,
                               InPrintFl = inPrintFl,
                               AdObjective = adObjective,
                               Brand4 = brand4,
                               LayoutAdapLimitations = layoutAdapLimitations,
                               InPrintBl = inPrintBl,
                               CompetitionBrand4 = competitionBrand4,
                              
                               InPainting = inPainting,
                               TargetGroupBrand1 = targetGroupBrand1,
                               TypeOfVehicles = typeOfVehicles,
                               TgsMedia1 = tgsMedia1,
                               
                               PreferredSize = preferredSize,
                               Others = others,
                               AdMix = adMix,
                               DisplayType = displayType,
                               TargetGroupBrand2 = targetGroupBrand2,
                               TgsMedia2 = tgsMedia2,
                               DisplayTypeSel = displayTypeSel,
                               TgsMedia3 = tgsMedia3,
                               TargetGroupBrand3 = targetGroupBrand3,
                               TgsMedia4 = tgsMedia4,
                               TargetGroupBrand4 = targetGroupBrand4,
                               OutDoorObjective = outDoorObjective,
                               Market1 = market1,
                               Market2 = market2,
                               Market3 = market3,
                               Market4 = market4,
                               InVinyl = inVinyl,
                               Complete = complete,
                               PartVinyl = partVinyl,
                               Ratio11 = ratio11,
                               Ratio12 = ratio12,
                               Ratio13 = ratio13,
                               Ratio14 = ratio14,
                               Ratio34 = ratio34,
                               RatioAll = ratioAll,
                               BillBoard = billBoard,
                               Buses = buses,
                               PoleKiosks = poleKiosks,
                               BusQShelter = busQShelter,
                               TransitMedia = transitMedia,
                               PublicUtitities = publicUtitities,
                               OtherMediaVehicle = otherMediaVehicle,
                               IsInActive =  isInActive
                           });
        }

        public void OnNewBriefAdded(NewBriefAdded e)
        {
            Id = e.BriefId;
            _briefDate = e.BriefDate;
            _budgetValue = e.BudgetValue;
            _additionalInfo = e.AdditionalInfo;
            _contactPerson = e.ContactPerson;
            _customer = e.Customer;
            _clientBusinessProfile = e.ClientBusinessProfile;
            _budgetTypes = e.BudgetTypes;
            _isApproved = e.IsApproved;
            _approvalNo = e.ApprovalNo;
            _approvalDate = e.ApprovalDate;
            _deadLineDate = e.DeadLineDate;
            _brand1 = e.Brand1;
            _tentativeStartDate = e.TentativeStartDate;
            _tentativeEndDate = e.TentativeEndDate;
            _noOfYears = e.NoOfYears;
            _otherStCommision = e.OtherStCommision;
            _marketingObjective = e.MarketingObjective;
            _requestSentToHO = e.RequestSentToHO;
            _competitionBrand1 = e.CompetitionBrand1;
            _competitionBrand2 = e.CompetitionBrand2;
            _preparedBy = e.PreparedBy;
            _briefNo = e.BriefNo;
            _brand2 = e.Brand2;
            _noOfMonths = e.NoOfMonths;
            _noOfDays = e.NoOfDays;
            _briefStatus = e.BriefStatus;
            _osDays = e.OsDays;
            _dateCreated = e.DateCreated;
            _brand3 = e.Brand3;
            _competitionBrand3 = e.CompetitionBrand3;
            _sendDate = e.SendDate;
            _headPlannerUserId = e.HeadPlannerUserId;
            _inPrintFl = e.InPrintFl;
            _adObjective = e.AdObjective;
            _brand4 = e.Brand4;
            _layoutAdapLimitations = e.LayoutAdapLimitations;
            _inPrintBl = e.InPrintBl;
            _competitionBrand4 = e.CompetitionBrand4;
            _inPainting = e.InPainting;
            _targetGroupBrand1 = e.TargetGroupBrand1;
            _typeOfVehicles = e.TypeOfVehicles;
            _tgsMedia1 = e.TgsMedia1;
            _preferredSize = e.PreferredSize;
            _others = e.Others;
            _adMix = e.AdMix;
            _displayType = e.DisplayType;
            _targetGroupBrand2 = e.TargetGroupBrand2;
            _tgsMedia2 = e.TgsMedia2;
            _displayTypeSel = e.DisplayTypeSel;
            _tgsMedia3 = e.TgsMedia3;
            _targetGroupBrand3 = e.TargetGroupBrand3;
            _tgsMedia4 = e.TgsMedia4;
            _targetGroupBrand4 = e.TargetGroupBrand4;
            _outDoorObjective = e.OutDoorObjective;
            _market1 = e.Market1;
            _market2 = e.Market2;
            _market3 = e.Market3;
            _market4 = e.Market4;
            _inVinyl = e.InVinyl;
            _complete = e.Complete;
            _partVinyl = e.PartVinyl;
            _ratio11 = e.Ratio11;
            _ratio12 = e.Ratio12;
            _ratio13 = e.Ratio13;
            _ratio14 = e.Ratio14;
            _ratio34 = e.Ratio34;
            _ratioAll = e.RatioAll;
            _billBoard = e.BillBoard;
            _buses = e.Buses;
            _poleKiosks = e.PoleKiosks;
            _busQShelter = e.BusQShelter;
            _transitMedia = e.TransitMedia;
            _publicUtitities = e.PublicUtitities;
            _otherMediaVehicle = e.OtherMediaVehicle;
            _IsInActive = e.IsInActive;
        }

        public void ChangeBrief(DateTime? briefdate, string customer, string additionalInfo, decimal budgetValue, string contactPerson, string contactPersonId ,
              string clientBusinessProfile, string budgetTypes, bool isApproved, int approvalNo, DateTime? approvalDate, DateTime? deadLineDate, string brand1,
              DateTime? tentativeStartDate, DateTime? tentativeEndDate, int noOfYears, int otherStCommision, string marketingObjective, DateTime? requestSentToHO,
              string competitionBrand1, string competitionBrand2, string preparedBy, int briefNo, string brand2, int noOfMonths, int noOfDays, int briefStatus,
              int osDays, DateTime? dateCreated, string brand3, string competitionBrand3, DateTime? sendDate, Guid headPlannerUserId, bool inPrintFl, string adObjective,
              string brand4, string layoutAdapLimitations, bool inPrintBl, string competitionBrand4,  bool inPainting, string targetGroupBrand1,
              string typeOfVehicles, string tgsMedia1, string preferredSize, string others, string adMix, string displayType, string targetGroupBrand2,
              string tgsMedia2, string displayTypeSel, string tgsMedia3, string targetGroupBrand3, string tgsMedia4, string targetGroupBrand4, string outDoorObjective,
              string market1, string market2, string market3, string market4, bool inVinyl, bool complete, bool partVinyl, bool ratio11, bool ratio12, bool ratio13,
              bool ratio14, bool ratio34, bool ratioAll, bool billBoard, bool buses, bool poleKiosks, bool busQShelter, bool transitMedia, bool publicUtitities, bool otherMediaVehicle, bool isInActive)
        {
            ApplyEvent(new BriefChanged()
            {
                BriefId = Id,
                BriefDate = briefdate,
                AdditionalInfo = additionalInfo,
                BudgetValue = budgetValue,
                Customer = customer,
                ContactPerson = contactPerson,
                ClientBusinessProfile = clientBusinessProfile,
                BudgetTypes = budgetTypes,
                IsApproved = isApproved,
                ApprovalNo = approvalNo,
                ApprovalDate = approvalDate,
                DeadLineDate = deadLineDate,
                Brand1 = brand1,
                TentativeStartDate = tentativeStartDate,
                TentativeEndDate = tentativeEndDate,
                NoOfYears = noOfYears,
                OtherStCommision = otherStCommision,
                MarketingObjective = marketingObjective,
                RequestSentToHO = requestSentToHO,
                CompetitionBrand1 = competitionBrand1,
                CompetitionBrand2 = competitionBrand2,
                PreparedBy = preparedBy,
                BriefNo = briefNo,
                Brand2 = brand2,
                NoOfMonths = noOfMonths,
                NoOfDays = noOfDays,
                BriefStatus = briefStatus,
                OsDays = osDays,
                DateCreated = dateCreated,
                Brand3 = brand3,
                CompetitionBrand3 = competitionBrand3,
                SendDate = sendDate,
                HeadPlannerUserId = headPlannerUserId,
                InPrintFl = inPrintFl,
                AdObjective = adObjective,
                Brand4 = brand4,
                LayoutAdapLimitations = layoutAdapLimitations,
                InPrintBl = inPrintBl,
                CompetitionBrand4 = competitionBrand4,

                InPainting = inPainting,
                TargetGroupBrand1 = targetGroupBrand1,
                TypeOfVehicles = typeOfVehicles,
                TgsMedia1 = tgsMedia1,

                PreferredSize = preferredSize,
                Others = others,
                AdMix = adMix,
                DisplayType = displayType,
                TargetGroupBrand2 = targetGroupBrand2,
                TgsMedia2 = tgsMedia2,
                DisplayTypeSel = displayTypeSel,
                TgsMedia3 = tgsMedia3,
                TargetGroupBrand3 = targetGroupBrand3,
                TgsMedia4 = tgsMedia4,
                TargetGroupBrand4 = targetGroupBrand4,
                OutDoorObjective = outDoorObjective,
                Market1 = market1,
                Market2 = market2,
                Market3 = market3,
                Market4 = market4,
                InVinyl = inVinyl,
                Complete = complete,
                PartVinyl = partVinyl,
                Ratio11 = ratio11,
                Ratio12 = ratio12,
                Ratio13 = ratio13,
                Ratio14 = ratio14,
                Ratio34 = ratio34,
                RatioAll = ratioAll,
                BillBoard = billBoard,
                Buses = buses,
                PoleKiosks = poleKiosks,
                BusQShelter = busQShelter,
                TransitMedia = transitMedia,
                PublicUtitities = publicUtitities,
                OtherMediaVehicle = otherMediaVehicle,
                IsInActive = isInActive
            });
        }

        public void OnBriefChanged(BriefChanged e)
        {
            
            _briefDate = e.BriefDate;
            _budgetValue = e.BudgetValue;
            _additionalInfo = e.AdditionalInfo;
            _contactPerson = e.ContactPerson;
            _customer = e.Customer;
            _clientBusinessProfile = e.ClientBusinessProfile;
            _budgetTypes = e.BudgetTypes;
            _isApproved = e.IsApproved;
            _approvalNo = e.ApprovalNo;
            _approvalDate = e.ApprovalDate;
            _deadLineDate = e.DeadLineDate;
            _brand1 = e.Brand1;
            _tentativeStartDate = e.TentativeStartDate;
            _tentativeEndDate = e.TentativeEndDate;
            _noOfYears = e.NoOfYears;
            _otherStCommision = e.OtherStCommision;
            _marketingObjective = e.MarketingObjective;
            _requestSentToHO = e.RequestSentToHO;
            _competitionBrand1 = e.CompetitionBrand1;
            _competitionBrand2 = e.CompetitionBrand2;
            _preparedBy = e.PreparedBy;
            _briefNo = e.BriefNo;
            _brand2 = e.Brand2;
            _noOfMonths = e.NoOfMonths;
            _noOfDays = e.NoOfDays;
            _briefStatus = e.BriefStatus;
            _osDays = e.OsDays;
            _dateCreated = e.DateCreated;
            _brand3 = e.Brand3;
            _competitionBrand3 = e.CompetitionBrand3;
            _sendDate = e.SendDate;
            _headPlannerUserId = e.HeadPlannerUserId;
            _inPrintFl = e.InPrintFl;
            _adObjective = e.AdObjective;
            _brand4 = e.Brand4;
            _layoutAdapLimitations = e.LayoutAdapLimitations;
            _inPrintBl = e.InPrintBl;
            _competitionBrand4 = e.CompetitionBrand4;
            _inPainting = e.InPainting;
            _targetGroupBrand1 = e.TargetGroupBrand1;
            _typeOfVehicles = e.TypeOfVehicles;
            _tgsMedia1 = e.TgsMedia1;
            _preferredSize = e.PreferredSize;
            _others = e.Others;
            _adMix = e.AdMix;
            _displayType = e.DisplayType;
            _targetGroupBrand2 = e.TargetGroupBrand2;
            _tgsMedia2 = e.TgsMedia2;
            _displayTypeSel = e.DisplayTypeSel;
            _tgsMedia3 = e.TgsMedia3;
            _targetGroupBrand3 = e.TargetGroupBrand3;
            _tgsMedia4 = e.TgsMedia4;
            _targetGroupBrand4 = e.TargetGroupBrand4;
            _outDoorObjective = e.OutDoorObjective;
            _market1 = e.Market1;
            _market2 = e.Market2;
            _market3 = e.Market3;
            _market4 = e.Market4;
            _inVinyl = e.InVinyl;
            _complete = e.Complete;
            _partVinyl = e.PartVinyl;
            _ratio11 = e.Ratio11;
            _ratio12 = e.Ratio12;
            _ratio13 = e.Ratio13;
            _ratio14 = e.Ratio14;
            _ratio34 = e.Ratio34;
            _ratioAll = e.RatioAll;
            _billBoard = e.BillBoard;
            _buses = e.Buses;
            _poleKiosks = e.PoleKiosks;
            _busQShelter = e.BusQShelter;
            _transitMedia = e.TransitMedia;
            _publicUtitities = e.PublicUtitities;
            _otherMediaVehicle = e.OtherMediaVehicle;
            _IsInActive = e.IsInActive;
        }
    }
}
