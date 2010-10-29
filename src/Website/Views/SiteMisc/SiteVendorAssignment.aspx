<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Website.ViewModel.Plan.SiteVendorAssignmentViewModel>"
 UICulture="en" Culture="en-GB"   %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


<link href="<%= Url.Content("~/Content/ui.jqgrid.css" )%> " rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Scripts/themes/grid.css")%> " rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/jqModal.css" )%> " rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/jquery.autocomplete.css" )%> " rel="stylesheet" type="text/css" />    

    <script src="<%= Url.Content("~/Scripts/js/i18n/grid.locale-en.js")%> " type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/js/jquery.jqGrid.min.js")%> " type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/js/grid.inlinedit.js")%> " type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.autocomplete.js" )%> "type="text/javascript"></script>    
    <script src="<%= Url.Content("~/Scripts/jquery.validate.min.js" )%> "type="text/javascript"></script>       
    <script src="<%= Url.Content("~/Scripts/ChangeVendorAssignment.js")%> " type="text/javascript"></script>


     <script type ="text/javascript">
         $(document).ready(function () {
             $("#EditPlanLink").html("<li> <a href=" + rootPath + "Plan.aspx/Edit/?Id=" + PlanDetailsId + ">Edit Plan</a></li>");
             $("#PlanRegionsLink").html("<li> <a href=" + rootPath + "Plan.aspx/PlanRegionsMainView/?Id=" + PlanDetailsId + ">Add/Edit Region to Plan</a></li>");
             $("#PlanCitiesLink").html("<li> <a href=" + rootPath + "Plan.aspx/PlanRegionCity/?Id=" + PlanDetailsId + ">Add/Edit City to Plan</a></li>");
             $("#PlanSitesLink").html("<li> <a href=" + rootPath + "Site.aspx/PlanSite/?PlanDetailId=" + PlanDetailsId + ">Add/Edit Site to Plan</a></li>");
             $("#menu-plan").toggleClass("closed");
             $("#h-menu-plan").addClass("selected");

             $('.checkbox').click( function() {alert('here')});
         });
    </script>
    <style>
        #VendorName 
        {
            width:85%;
        }
    </style>
    
     <div class="box"> 
					<!-- box / title --> 
					<div class="title"> 
						<h5>Planning & Buying</h5> 						
					</div> 
					<!-- end box / title --> 
                   <div class="form" >   
    <% using (Html.BeginForm("SiteVendorAssignment", "SiteMisc", FormMethod.Post, new { id = "AssignVendorsForm" }))
       {%>
        <%: Html.ValidationSummary(true) %>
        
                
           <%: Html.HiddenFor(model => model.PlanDetailsId) %>                          
            <table style ="width:90%;">
            <tr>
                <td style="border: none; padding: 3px 3px 3px 3px;">Client</td>
                <td style="border: none; padding: 3px 3px 3px 3px;"> <%: Html.DisplayTextFor(model => model.ClientName) %> </td>
                <td></td><td></td><td></td>
                </tr>
            <tr>
                <td style="border: none; padding: 3px 3px 3px 3px;">Plan No</td>
                <td style="border: none; padding: 3px 3px 3px 3px;"> <%: Html.DisplayTextFor(model => model.PlanNo) %> </td>
                <td style="border: none; padding: 3px 3px 3px 3px;">BriefNo </td>
                <td style="border: none; padding: 3px 3px 3px 3px;"><%: Html.DisplayTextFor(model => model.BriefNo)%> </td>                     
                <td></td>
            </tr>                     
            <tr>
                <td style="border: none; padding: 3px 3px 3px 3px;">Start Date</td>
                <td style="border: none; padding: 3px 3px 3px 3px;"> <%: String.Format("{0:d}", Model.StartDate)%></td>
                <td style="border: none; padding: 3px 3px 3px 3px;">End Date</td>
                <td style="border: none; padding: 3px 3px 3px 3px;"> <%:  String.Format("{0:d}", Model.EndDate)%></td>                     
                <td style="border: none; padding: 3px 3px 3px 3px;">Budget</td>
                <td style="border: none; padding: 3px 3px 3px 3px;"> <%: Html.DisplayTextFor(model => model.PlanBudget)%></td>                     
                <td style="border: none;"></td>                   
            </tr>
            </table>
            <br />
            <div><b>Total Sites :<label id="TotalSiteCount"></label></b></div>            
            <table style ="text-align:right" >            
            <tr>
                <th>Financials</th>
                <th style="text-align:right">Total</th>
                <th style="text-align:right"><b>Display</b></th>
                <th style="text-align:right"><b>Mounting</b></th>
                <th style="text-align:right"><b>Printing</b></th>
                <th style="text-align:right"><b>Fabrication</b></th>            
             </tr>
            <tbody>
                <tr>
                    <td>Total Buying Cost</td> 
                    <td  style="padding: 3px 3px 3px 5px;"><label id="TotalCost">0</label> </td>
                    <td  style="padding: 3px 3px 3px 5px;"><label id="TotalDisplayCost">0</label> </td>
                    <td  style="padding: 3px 3px 3px 5px;"><label id="TotalMountingCost">0</label> </td>
                    <td  style="padding: 3px 3px 3px 5px;"><label id="TotalPrintingCost">0</label> </td>
                    <td  style="padding: 3px 3px 3px 5px;"><label id="TotalFabricationCost">0</label> </td>
                </tr >
                <tr>
                    <td>Total Client Cost</td> 
                    <td  style="padding: 3px 3px 3px 5px;"><label id="TotalClientCost">0</label> </td>
                    <td  style="padding: 3px 3px 3px 5px;"><label id="TotalDisplayClientCost">0</label> </td>
                    <td  style="padding: 3px 3px 3px 5px;"><label id="TotalMountingClientCost">0</label> </td>
                    <td  style="padding: 3px 3px 3px 5px;"><label id="TotalPrintingClientCost">0</label> </td>
                    <td  style="padding: 3px 3px 3px 5px;"><label id="TotalFabricationClientCost">0</label> </td>
                </tr >
                <tr>
                    <td>Difference</td> 
                    <td  style="padding: 3px 3px 3px 5px;"><label id="TotalDiffInCost">0</label> </td>
                    <td  style="padding: 3px 3px 3px 5px;"><label id="DisplayCostDiff">0</label> </td>
                    <td  style="padding: 3px 3px 3px 5px;"><label id="MountingCostDiff">0</label> </td>
                    <td  style="padding: 3px 3px 3px 5px;"><label id="PrintingCostDiff">0</label> </td>
                    <td  style="padding: 3px 3px 3px 5px;"><label id="FabricationCostDiff">0</label> </td>
                </tr >
                 </tbody>
                                       
                <tr>
                    <th  style="text-align:left" colspan = "6"><b>Statuses</b></th>
                </tr>
                <tr style ="color:red">
                    <td>Pending</td>
                    <td></td>
                    <td  style="padding: 3px 3px 3px 5px;"><label id="TotalDisplayPending">0</label> </td>
                    <td  style="padding: 3px 3px 3px 5px;"><label id="TotalMountingPending">0</label> </td>
                    <td  style="padding: 3px 3px 3px 5px;"><label id="TotalPrintingPending">0</label> </td>
                    <td  style="padding: 3px 3px 3px 5px;"><label id="TotalFabricationPending">0</label> </td>
                    </tr>
                 <tr style ="color:Teal">
                    <td>Proposed</td>
                    <td></td>
                    <td  style="padding: 3px 3px 3px 5px;"><label id="TotalDisplayProposed">0</label> </td>
                    <td  style="padding: 3px 3px 3px 5px;"><label id="TotalMountingProposed">0</label> </td>
                    <td  style="padding: 3px 3px 3px 5px;"><label id="TotalPrintingProposed">0</label> </td>
                    <td  style="padding: 3px 3px 3px 5px;"><label id="TotalFabricationProposed">0</label> </td>
                    </tr>
                 <tr style ="color:Purple">
                    <td>Under Negotiation</td>
                    <td></td>
                    <td  style="padding: 3px 3px 3px 5px;"><label id="TotalDisplayUN">0</label> </td>
                    <td  style="padding: 3px 3px 3px 5px;"><label id="TotalMountingUN">0</label> </td>
                    <td  style="padding: 3px 3px 3px 5px;"><label id="TotalPrintingUN">0</label> </td>
                    <td  style="padding: 3px 3px 3px 5px;"><label id="TotalFabricationUN">0</label> </td>
                    </tr>
                <tr style ="color:green">
                    <td>Booked</td>
                    <td></td>
                    <td  style="padding: 3px 3px 3px 5px;"><label id="TotalDisplayBooked">0</label> </td>
                    <td  style="padding: 3px 3px 3px 5px;"><label id="TotalMountingBooked">0</label> </td>
                    <td  style="padding: 3px 3px 3px 5px;"><label id="TotalPrintingBooked">0</label> </td>
                    <td  style="padding: 3px 3px 3px 5px;"><label id="TotalFabricationBooked">0</label> </td>
                </tr >                
            </table>           
        </div>
         </div>   
          
             <div id="box-tabs" class="box"> 
					<!-- box / title --> 
					<div class="title"> 
						<h5>Buying Tasks</h5> 
						<ul class="links"> 
							<li><a href="#box-vendorInfo">Display</a></li> 
							<li><a href="#box-vendorInfo">Mounting</a></li> 
                            <li><a href="#box-vendorInfo">Printing</a></li> 
                            <li><a href="#box-vendorInfo">Fabrication</a></li> 
						</ul> 
					</div>  
					<!-- end box / title --> 
                    <div class="form" >   
					<div id="box-vendorInfo"> 
						<table style="border:1px solid lightgray; padding:2px 2px 2px 2px ">                            
                            <tr>
                            <td style="border: none; padding: 3px 3px 3px 3px;">Vendor</td>
                                <td style="border: none; padding: 3px 3px 3px 3px;width:24%"> <%: Html.TextBoxFor(model => model.VendorName)%>
                                <input type="image" style="padding:3px 0 0 0" src="<%= Url.Content("~/Resources/Images/icons/autocomplete.gif") %>" /> 
                                <%:Html.HiddenFor(model => model.VendorId) %> </td>
                                </tr>
                                <tr>
                                 <td style="border: none; padding: 5px 3px 3px 3px;">Rate</td>
                                <td style="border: none; padding: 5px 3px 3px 3px;"> <%: Html.TextBoxFor(model => model.Rate)%></td>  
                                <td style="border: none; padding: 5px 3px 3px 3px;">Client Rate</td>
                                <td style="border: none; padding: 5px 3px 3px 3px;"> <%: Html.TextBoxFor(model => model.ClientRate)%></td>  
                                <td>Charging Basis</td>
                                <td class="select"><%: Html.DropDownListFor(model => model.ChargingBasis, Model.ChargingBasisList, "Select one..", new { cb = "ChargingBasis" })%> </td>
                                </tr>                
                            <tr>
                            <td style="padding: 0px 3px 7px 3px;">Status</td>
                            <td style="padding: 0px 3px 7px 3px;"><%: Html.DropDownListFor(model => model.Status, Model.StatusList, "Select one..", new { Status = "Status" })%> </td>
                            <td style="padding: 0px 3px 7px 3px;"><label id="lblStartDate"  >Start Date</label></td>
                            <td style="padding: 0px 3px 7px 3px;"> <%: Html.EditorFor(model => model.StartDate, new Dictionary<string, object>() { { "Title", "DateTimePicker" } })%>            </td>
                            <td style="padding: 0px 3px 7px 3px;"><label id="lblEndDate">End Date</label></td>
                            <td style="padding: 0px 3px 7px 3px;"> <%: Html.EditorFor(model => model.EndDate, new Dictionary<string, object>() { { "Title", "DateTimePicker" } })%>            </td>
                            <td  style="border: none; padding: 0px 3px 3px 3px;"><input type="button" onclick="javascript:AssignVendorInfo()" value ="Assign" /></td>
                            </tr>
                            </table>   <br /><br />                                              
  
                    <a href ="javascript:SelectAll()" >All</a> /
                    <a href ="javascript:SelectNone()" >None</a> &nbsp;&nbsp;&nbsp;
                                Select Region
                                <%: Html.DropDownListFor(model => model.Region, Model.RegionsList,   "Select one..", new { Region = "Region" })%> 
                
                                &nbsp; &nbsp;&nbsp; Select City
                                <%: Html.DropDownListFor(model => model.CityName, Model.CitiesList,   "Select one..", new { CityId = "LocationId" })%> 
                                         <%: Html.HiddenFor(model => model.CityId)%>
                                        <input type="button" id="CityFilter" value="Filter" onclick="javascript:GetsitesByCityOrRegion()" /> 
                                        <span style ="padding:0 0 0 35%"></span>
                                        <input type="button" id="submit" value="Save" onclick="javascript:SubmitVendorInfo(); return false;" />                     
                                        <br />
                                         
                         <br />
                        <div class="DivOverflow" >
                            <table id="SiteList" style ="width:100%"></table>
                       </div>                                                               
					</div> 	
                    </div>				
				
                
				<!-- end box / left --> 
             <div id="fogLoaderimg"></div>                       
         <input type="button" style="margin-left:25px" id="Save" value="Save" onclick="javascript:SubmitVendorInfo(); return false;" />                
         <%-- <span style ="padding:0 0 0 80%"><a id ="AddNewSite" href= "<%=URL.Content("~/Site.aspx/AddNewSite/?planDetailsId=" + Model.PlanDetailsId +"&region=&locationId=")%>">Add New Site</a> </span>                          --%>

          
         </div> 
         <div>        
        <%: Html.ActionLink("Back to List", "IndexForVendorInfoChange")%>
        </div>
    <br />    
    <% } %>
   
    
</asp:Content>

