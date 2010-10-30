<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Website.ViewModel.Plan.PlanCityMainViewModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <link href="<%= Url.Content("~/Content/ui.jqgrid.css" )%> "rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Scripts/themes/grid.css" )%> " rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/jqModal.css" )%> "rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/jquery.autocomplete.css")%> " rel="stylesheet" type="text/css" />    

    <script src="<%= Url.Content("~/Scripts/js/i18n/grid.locale-en.js")%> " type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/js/jquery.jqGrid.min.js")%> " type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/js/grid.inlinedit.js")%> " type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.autocomplete.js")%> " type="text/javascript"></script>    
    <script src="<%= Url.Content("~/Scripts/jquery.validate.min.js")%> " type="text/javascript"></script>       
    <script src="<%= Url.Content("~/Scripts/PlanCities.js" )%> "type="text/javascript"></script>
    
    <script type ="text/javascript">
        
        function CopyBriefAllocationCities() {
            $.ajax({
                url: rootPath + "Plan.aspx/CopyBriefAllocationCities/?planId=" + PlanDetailsId + "&Region=" + $("#Region").val(),
                success: function (data, result) {
                    if (!result) alert('Error while copying.');
                    $.ajax({
                        url: rootPath + "Plan.aspx/GetCities/?sidx=1&sord=1&page=0&rows=10&PlanDetailsId=" + PlanDetailsId + "&region=" + $("#Region").val(),
                        success: function (data, result) {
                            if (!result) alert('Failure to retrieve the Cities.');
                            //var jsonObject = eval('(' + data + ')');
                            $("#Cities")[0].addJSONData(data);
                        }
                    });
                },
                error: function (data, result) {
                    alert('Error while copying');
                }
            });

        }
        $(document).ready(function () {
            $("#EditPlanLink").html("<li> <a href= " + rootPath + "Plan.aspx/Edit/?Id=" + PlanDetailsId + ">Edit Plan</a></li>");
            $("#PlanRegionsLink").html("<li> <a href=" + rootPath + "Plan.aspx/PlanRegionsMainView/?Id=" + PlanDetailsId + ">Add/Edit Region to Plan</a></li>");
            $("#PlanSitesLink").html("<li> <a href=" + rootPath +  "Site.aspx/PlanSite/?planDetailId=" + PlanDetailsId + ">Add/Edit Site to Plan</a></li>");
            $("#PlanSitePlanningAndBuyingLink").html("<li> <a href=" + rootPath + "SiteMisc.aspx/SiteVendorAssignment/?planDetailsId=" + PlanDetailsId + ">Planning & Buying Site</a></li>"); 
            $("#menu-plan").toggleClass("closed");
            $("#h-menu-plan").addClass("selected");

            $("#Region").change(function () {
                var region = $("#Region").val();
                $.ajax({
                    url: rootPath + "Shared.aspx/GetRegionInfo/?PlanDetailsId=" + PlanDetailsId + "&region=" + region,
                    success: function (data, result) {
                        if (!result) alert('Failure to retrieve the Cities.');
                        var jsonObject = data;
                        regionValues = jsonObject.split(";");
                        $("#RegionBudget").html(regionValues[0]);
                        $("#PlannerId").html(regionValues[1]);
                    }
                });
                $.ajax({
                    url: rootPath + "Plan.aspx/GetCities/?sidx=1&sord=1&page=0&rows=10&PlanDetailsId=" + PlanDetailsId + "&region=" + region,
                    success: function (data, result) {
                        if (!result) alert('Failure to retrieve the Cities.');
                        //var jsonObject = eval('(' + data + ')');
                        $("#Cities")[0].addJSONData(data);
                    }
                });
            });
        });   
    </script>

    <div class="box"> 
					<!-- box / title --> 
					<div class="title"> 
						<h5>Cities - Plan</h5> 						
					</div> 
					<!-- end box / title --> 
                   <div class="form" > 
    <% using (Html.BeginForm("PlanRegionCity", "Plan", FormMethod.Post, new { id = "CityPlanForm" }))
       {%>
        <%: Html.ValidationSummary(true) %>
        
        <fieldset>
            
              <%: Html.HiddenFor(model => model.PlanDetailsId) %>
             <%: Html.HiddenFor(model => model.RegionString) %>
            <table style ="width:85%;" class="contentTable">
            <tr>
                <td style="border: none; padding: 4px 4px 4px 4px;">Plan No</td>
                <td style="border: none; padding: 4px 4px 4px 4px;"> <%: Html.DisplayTextFor(model => model.PlanNo) %>
                     <%: Html.ValidationMessageFor(model => model.PlanNo) %></td>
                <td style="border: none; padding: 4px 4px 4px 4px;"></td>
                <td style="border: none; padding: 4px 4px 4px 4px;"></td> <td style="border: none;"></td>
            </tr>
            <tr>
                <td style="border: none; padding: 4px 4px 4px 4px;">BriefNo </td>
                <td style="border: none; padding: 4px 4px 4px 4px;"><%: Html.DisplayTextFor(model => model.BriefNo)%>
                    <%: Html.ValidationMessageFor(model => model.BriefNo) %> </td>
                <td style="border: none; padding: 4px 4px 4px 4px;"> CreatedOn </td>
                <td style="border: none; padding: 4px 4px 4px 4px;"> <%: Html.EditorFor(model => model.CreatedOn)%>
                     <%: Html.ValidationMessageFor(model => model.CreatedOn) %> </td><td style="border: none;"></td>
            </tr>
            <tr>
                <td style="border: none; padding: 4px 4px 4px 4px;">Head Planner</td>
                <td style="border: none; padding: 4px 4px 4px 4px;"> <%: Html.DisplayTextFor(model => model.HeadPlannerName)%>
                     <%: Html.ValidationMessageFor(model => model.HeadPlannerName) %></td>
                <td style="border: none; padding: 4px 4px 4px 4px;">Budget </td>
                <td style="border: none; padding: 4px 4px 4px 4px;"> <%: Html.DisplayTextFor(model => model.Budget)%>
                     <%: Html.ValidationMessageFor(model => model.Budget) %></td>
            </tr>
            <tr>
                <td style="border: none; padding: 4px 4px 4px 4px;">Start Date</td>
                <td style="border: none; padding: 4px 4px 4px 4px;"> <%: Html.EditorFor(model => model.StartDate)%>
                     <%: Html.ValidationMessageFor(model => model.StartDate) %></td>
                <td style="border: none; padding: 4px 4px 4px 4px;">End Date</td>
                <td style="border: none; padding: 4px 4px 4px 4px;"> <%: Html.EditorFor(model => model.EndDate)%>
                     <%: Html.ValidationMessageFor(model => model.EndDate) %>                     
                     </td>
            <td style="border: none;"></td>                   
            </tr>
            <tr>
                <td style="border: none; padding: 4px 4px 4px 4px;">Select Region</td>
                <td style="border: none; padding: 4px 4px 4px 4px;"> <%: Html.DropDownListFor(model => model.Region, Model.RegionsList,   "Select one..", new { Region = "Region" })%>
                     <%: Html.ValidationMessageFor(model => model.PlannerId) %></td>
                <td style="border: none; padding: 4px 4px 4px 4px;">Regional Planner</td>
                <td id="PlannerId" style="border: none; padding: 4px 4px 4px 4px;"> <%: Html.DisplayTextFor(model => model.PlannerId)%>
                     <%: Html.ValidationMessageFor(model => model.PlannerId) %></td>
                <td style="border: none; padding: 4px 4px 4px 4px;">Regional Budget </td>
                <td id="RegionBudget" style="border: none; padding: 4px 4px 4px 4px;"> <%: Html.DisplayTextFor(model => model.RegionBudget)%>
                     <%: Html.ValidationMessageFor(model => model.Budget) %></td>                       
                <td style="border: none;"></td>
                </tr>
            </table>          
        </fieldset>
        <div><a href="javascript:CopyBriefAllocationCities();" >Copy Brief Allocation</a></div>
        <br />
       <table id="Cities" style ="width:100%" ></table>
       <br />
          <a href="javascript:AddCity()" >Add New City</a>            
            
    <br />
      <br />  
       <div id="fogLoaderimg"></div>     
         <input type="submit" value="Save" onclick="javascript:SaveForm(); return false;"/>            
                              
    <br />
    <br />
    <div>        
         <%: Html.ActionLink("Plan/Region/Site Common Index", "PlanRegionIndex")%> &nbsp;        
        
    </div>

    <br />       
            
    <br />
    <% } %>
    </div>
    </div>
</asp:Content>
