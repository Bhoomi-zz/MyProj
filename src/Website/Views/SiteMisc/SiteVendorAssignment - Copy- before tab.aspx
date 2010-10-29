<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Website.ViewModel.Plan.SiteVendorAssignmentViewModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


<link href="../../Content/ui.jqgrid.css" rel="stylesheet" type="text/css" />
    <link href="../../Scripts/themes/grid.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/jqModal.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/jquery.autocomplete.css" rel="stylesheet" type="text/css" />    

    <script src="../../Scripts/js/i18n/grid.locale-en.js" type="text/javascript"></script>
    <script src="../../Scripts/js/jquery.jqGrid.min.js" type="text/javascript"></script>
    <script src="../../Scripts/js/grid.inlinedit.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.autocomplete.js" type="text/javascript"></script>    
    <script src="../../Scripts/jquery.validate.min.js" type="text/javascript"></script>       
    <script src="../../Scripts/ChangeVendorAssignment.js" type="text/javascript"></script>
    <style>
        #VendorName 
        {
            width:90%;
        }
    </style>

     <div class="box"> 
					<!-- box / title --> 
					<div class="title"> 
						<h5>Assign Vendors</h5> 						
					</div> 
					<!-- end box / title --> 
                   <div class="form" >   
    <% using (Html.BeginForm("SiteVendorAssignment", "SiteMisc", FormMethod.Post, new { id = "AssignVendorsForm" }))
       {%>
        <%: Html.ValidationSummary(true) %>
        
        <fieldset>          
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
                <td style="border: none; padding: 3px 3px 3px 3px;"> <%: Html.DisplayTextFor(model => model.StartDate)%></td>
                <td style="border: none; padding: 3px 3px 3px 3px;">End Date</td>
                <td style="border: none; padding: 3px 3px 3px 3px;"> <%: Html.DisplayTextFor(model => model.EndDate)%></td>                     
                <td style="border: none; padding: 3px 3px 3px 3px;">Budget</td>
                <td style="border: none; padding: 3px 3px 3px 3px;"> <%: Html.DisplayTextFor(model => model.PlanBudget)%></td>                     
                <td style="border: none;"> <a href="/Plan/Edit/?id=<%: Model.PlanDetailsId %>">Edit Plan</a></td>                   
            </tr>
               
            </table>
            <br />
            
            <table style="border:1px solid lightgray; padding:2px 2px 2px 2px">
            <tr>
            <td style="padding: 7px 3px 0px 3px;">Activity</td>
            <td style="padding: 7px 3px 0px 3px;"><%: Html.DropDownListFor(model => model.Activity, Model.ActivityList, "Select one..", new { Activity = "Activity" })%> 
              &nbsp;&nbsp;&nbsp;Pending : <label id ="PendingActivities">0</label> </td>
            <td>  Booked : <label id="BookedActivities">0</label></td>
            </tr>
            <tr>
            <td style="border: none; padding: 3px 3px 3px 3px;">Vendor</td>
                <td style="border: none; padding: 3px 3px 3px 3px;width:30%"> <%: Html.TextBoxFor(model => model.VendorName)%>
                <%:Html.HiddenFor(model => model.VendorId) %> </td>
                 <td style="border: none; padding: 5px 3px 3px 3px;">Rate</td>
                <td style="border: none; padding: 5px 3px 3px 3px;"> <%: Html.TextBoxFor(model => model.Rate)%></td>  
                <td>Charging Basis</td>
                <td><%: Html.DropDownListFor(model => model.ChargingBasis, Model.ChargingBasisList, "Select one..", new { cb = "ChargingBasis" })%> </td>
                </tr>                
            <tr>
            <td style="padding: 0px 3px 7px 3px;">State</td>
            <td style="padding: 0px 3px 7px 3px;"><%: Html.DropDownListFor(model => model.State, Model.StateList, "Select one..", new { State = "State" })%> </td>
            <td style="padding: 0px 3px 7px 3px;">Start Date</td>
            <td style="padding: 0px 3px 7px 3px;"> <%: Html.TextBoxFor(model => model.StartDate, new Dictionary<string, object>() { { "Title", "DateTimePicker" } })%>            </td>
            <td style="padding: 0px 3px 7px 3px;">End Date</td>
            <td style="padding: 0px 3px 7px 3px;"> <%: Html.TextBoxFor(model => model.EndDate, new Dictionary<string, object>() { { "Title", "DateTimePicker" } })%>            </td>
            <td  style="border: none; padding: 0px 3px 3px 3px;"><input type="button" onclick="javascript:AssignVendorInfo()" value ="Assign" /></td>
            </tr>
            </table>   <br /><br />
                  
                    </fieldset>            
  
    <a href ="javascript:SelectAll()" >All</a> /
    <a href ="javascript:SelectNone()" >None</a> &nbsp;&nbsp;&nbsp;
                Select Region
                <%: Html.DropDownListFor(model => model.Region, Model.RegionsList,   "Select one..", new { Region = "Region" })%> 
                
                &nbsp; &nbsp;&nbsp; Select City
                <%: Html.TextBoxFor(model => model.CityName)%>
                         <%: Html.HiddenFor(model => model.CityId)%>
                        <input type="button" id="CityFilter" value="Filter" onclick="javascript:GetsitesByCityOrRegion()" /> 
                    &nbsp; &nbsp; &nbsp;    Total Display Cost : <label id="TotalDisplayCost">0</label> &nbsp; &nbsp; &nbsp;Total Mounting Cost <label id="TotalMountingCost">0</label>
             <div style="float:left;padding:0 0 0 60%">Total Printing Cost : <label id ="TotalPrintingCost">0</label>&nbsp; &nbsp; &nbsp; Total Fabrication Cost : <label id="TotalFabricationCost">0</label></div>
    <br />     <br />
    <b>Site details for <i><label id="activityName">Display</label></i></b>
    <br /><br />
        <div class="DivOverflow" >
            <table id="SiteList" style ="width:100%"></table>
       </div> 
      <br />           
         <input type="submit" value="Save" />                     
         <input type="submit" value="Cancel" />                            
    <br />
    <br />
    <div>        
        <%: Html.ActionLink("Back to List", "Index") %>
    </div>
    <% } %>
   </div>
    </div>
</asp:Content>
