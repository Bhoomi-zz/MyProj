<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Website.ViewModel.Plan.PlanSiteViewModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <link href="<%= Url.Content("~/Content/ui.jqgrid.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Scripts/themes/grid.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/jqModal.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/jquery.autocomplete.css")%>" rel="stylesheet" type="text/css" />    

    <script src="<%= Url.Content("~/Scripts/js/i18n/grid.locale-en.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/js/jquery.jqGrid.min.js") %> " type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/js/grid.inlinedit.js") %>" type="text/javascript"></script>
    
    <script src="<%= Url.Content("~/Scripts/jquery.format.1.04.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.autocomplete.js") %>" type="text/javascript"></script>    
    <script src="<%= Url.Content("~/Scripts/jquery.validate.min.js")%>" type="text/javascript"></script>       
    <script src= "<%= Url.Content("~/Scripts/PlanSite.js") %>" type="text/javascript"></script>


    <script type ="text/javascript">
        $(document).ready(function () {
            PlanDetailsId = $("#PlanDetailsId").val();
            $("#EditPlanLink").html("<li> <a href=" + rootPath + "Plan.aspx/Edit/?Id=" + PlanDetailsId + ">Edit Plan</a></li>");
            $("#PlanRegionsLink").html("<li> <a href=" + rootPath + "Plan.aspx/PlanRegionsMainView/?Id=" + PlanDetailsId + ">Add/Edit Region to Plan</a></li>");
            $("#PlanCitiesLink").html("<li> <a href=" + rootPath + "Plan.aspx/PlanRegionCity/?Id=" + PlanDetailsId + ">Add/Edit City to Plan</a></li>");
            $("#PlanSitePlanningAndBuyingLink").html("<li> <a href=" + rootPath + "SiteMisc.aspx/SiteVendorAssignment/?planDetailsId=" + PlanDetailsId + ">Planning & Buying Site</a></li>");
            $("#menu-plan").toggleClass("closed");
            $("#h-menu-plan").addClass("selected");

            $("#Region").change(function () {
                var region = $("#Region").val();
                selectedRegion = region;
                var locations = $("#LocationName")[0];
                $("#LocationName").val("");
                $("#LocationId").val("");
                for (var i = 0; i < locations.length; i++) {
                    if (locations[i].value.split(";")[1] != region) {
                        locations[i].disabled = true;
                    }
                    else {
                        locations[i].disabled = false;
                    }
                }
                $.ajax({
                    url: rootPath + "Shared.aspx/GetRegionInfo/?PlanDetailsId=" + PlanDetailsId + "&region=" + region,
                    success: function (data, result) {
                        if (!result) alert('Failure to retrieve the Sites.');
                        var jsonObject = data;
                        regionValues = jsonObject.split(";");
                        $("#RegionBudget").html(regionValues[0]);
                        $("#PlannerId").html(regionValues[1]);
                    }
                });
            });
        });   
    </script>
    <div class="box"> 
					<!-- box / title --> 
					<div class="title"> 
						<h5>Sites - Plan</h5> 						
					</div> 
					<!-- end box / title --> 
                   <div class="form" > 
                   
    <% using (Html.BeginForm("PlanSite", "Site", FormMethod.Post, new { id = "PlanSiteForm" }))
       {%>
        <%: Html.ValidationSummary(true) %>
        
        <fieldset>            
              <%: Html.HiddenFor(model => model.PlanDetailsId) %>
             <%: Html.HiddenFor(model => model.RegionString) %>
             
            <table style ="width:100%;">
            <tr>
                <td style="border: none; padding: 4px 4px 4px 4px;">Plan No</td>
                <td style="border: none; padding: 4px 4px 4px 4px;"> <%: Html.DisplayTextFor(model => model.PlanNo) %>
                     <%: Html.ValidationMessageFor(model => model.PlanNo) %></td>
                <td style="border: none; padding: 4px 4px 4px 4px;">BriefNo </td>
                <td style="border: none; padding: 4px 4px 4px 4px;"><%: Html.DisplayTextFor(model => model.BriefNo)%>
                    <%: Html.ValidationMessageFor(model => model.BriefNo) %> </td> 
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
                     <td style="border: none; padding: 4px 4px 4px 4px;"></td>      
                     <td style="border: none; padding: 4px 4px 4px 4px;"></td>      
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
                <td style="border: none;"> </td>
            </tr>
            <tr>
                <td style="border: none; padding: 4px 4px 4px 4px;">Select City</td>
                <td style="border: none; padding: 4px 4px 4px 4px;"> <%: Html.DropDownListFor(model => model.LocationName, Model.CitiesList,   "Select one..", new { LocationId = "LocationId" })%>                
                                    <%: Html.HiddenFor(model => model.LocationId)%>
                                     <%: Html.ValidationMessageFor(model => model.LocationId) %>
                                      <%: Html.HiddenFor(model => model.PlanCityId)%></td>
                <td style="border: none; padding: 4px 4px 4px 4px;">Budget</td>
                <td id="CityBudget" style="border: none; padding: 4px 4px 4px 4px;"> <%: Html.DisplayTextFor(model => model.CityBudget)%>    
                <%: Html.HiddenFor(model => model.CityBudgetH) %></td>
                <td style="border: none; padding: 4px 4px 4px 4px;"></td>                
                <td style="border: none; padding: 4px 4px 4px 4px;"></td>      
                <td style="border: none;"> </td>
            </tr>
            </table>          
        </fieldset>
                
          <a href="javascript:AddSite()" >Add New Site</a>  <span style ="padding:0 0 0 80%"></span>
            <input  type="submit" onclick="javascript:SaveForm(); return false;" value="Save" />                        
    <br />
    <br />
        <div class="DivOverflow">
       <table id="Sites" style ="width:100%"></table>
       </div>
         <br />
          <a href="javascript:AddSite()" >Add New Site</a>               
    <br /> 
      <br />           
       <div id="fogLoaderimg"></div>     
         <input type="submit" onclick="javascript:SaveForm(); return false;" value="Save" />                              
    <br />
    <br />
    <div>        
         <%: Html.ActionLink("Plan/Region/Site Common Index", "PlanRegionIndex", "Plan")%> &nbsp; | &nbsp;
         <%: Html.ActionLink("Plan Index", "Index", "Plan") %>
    </div>
    <% } %>
    </div>
    </div>
</asp:Content>