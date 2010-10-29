<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Website.ViewModel.Plan.PlanEditViewModel>" %>
<%@ Import Namespace="Microsoft.Web.Mvc" %>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <link href="<%= Url.Content("~/Content/ui.jqgrid.css")%> " rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Scripts/themes/grid.css" )%> "rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/jqModal.css" )%> "rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/jquery.autocomplete.css")%> " rel="stylesheet" type="text/css" />    

    <script src="<%= Url.Content("~/Scripts/js/i18n/grid.locale-en.js" )%> "type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/js/jquery.jqGrid.min.js" )%> "type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/js/grid.inlinedit.js" )%> "type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.autocomplete.js" )%> "type="text/javascript"></script>    
    <script src="<%= Url.Content("~/Scripts/jquery.validate.min.js" )%> "type="text/javascript"></script>       
    <script src="<%= Url.Content("~/Scripts/PlanRegions.js" )%> "type="text/javascript"></script>
    
    <script type ="text/javascript">
        function CopyBriefAllocation() {
            $.ajax({
                url: rootPath + "Plan.aspx/CopyBriefAllocation/?planId=" + PlanDetailsId,
                success: function (data, result) {
                    if (!result) alert('Error while copying.');
                    $.ajax({
                        url: rootPath + "Plan.aspx/GetRegions/?sidx=1&sord=1&page=0&rows=10&PlanDetailsId=" + PlanDetailsId,
                        success: function (data, result) {
                            if (!result) alert('Failure to retrieve the Cities.');
                            //var jsonObject = eval('(' + data + ')');
                            $("#Regions")[0].addJSONData(data);
                        }
                    });
                },
                error: function(data, result) {
                    alert('Error while copying');
                }
            });
           
        }
     $(document).ready(function () {
         $("#EditPlanLink").html("<li> <a href=" + rootPath + "Plan.aspx/Edit/?Id=" + PlanDetailsId + ">Edit Plan</a></li>");
         $("#PlanCitiesLink ").html("<li> <a href= " + rootPath + "Plan.aspx/PlanRegionCity/?Id=" + PlanDetailsId + ">Add/Edit City to Plan</a></li>");
         $("#PlanSitePlanningAndBuyingLink").html("<li> <a href=" + rootPath + "SiteMisc.aspx/SiteVendorAssignment/?planDetailsId=" + PlanDetailsId + ">Planning & Buying Site</a></li>"); 
         $("#menu-plan").toggleClass("closed");
         $("#h-menu-plan").addClass("selected");
     });
     </script>
     <div class="box"> 
					<!-- box / title --> 
					<div class="title"> 
						<h5>Regions - Plan</h5> 						
					</div> 
					<!-- end box / title --> 
                   <div class="form" id="contentTable"> 

    <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>
        
        <fieldset>
           
              <%: Html.HiddenFor(model => model.PlanDetailsId) %>
            
            <table style ="width:70%;">
            <tr>
                <td style="border: none; padding: 4px 4px 4px 4px;">Plan No</td>
                <td style="border: none; padding: 4px 4px 4px 4px;"> <%: Html.DisplayTextFor(model => model.PlanNo) %>
                <%:Html.HiddenFor(model=> model.PlanNo) %>
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
                     <%: Html.ValidationMessageFor(model => model.HeadPlannerName)%></td>
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
            </table>          
        </fieldset>
        <br />
         </div>
         <div style="margin:0 0 0 20px" >
          <a href="javascript:AddRegion()" >Add New Region</a>
           
    <br />
         <br />
         <div><a href="javascript:CopyBriefAllocation();" >Copy Brief Allocation</a></div>

            <table id="Regions" style ="width:100%;"></table>
         
       <br />
          <a href="javascript:AddRegion()" >Add New Region</a>
           
    <br />   
      <br />      
      
       <div id="fogLoaderimg"></div>     
         <input type="submit" value="Save" onclick="javascript:SaveForm(); return false;"/>                     
                          
    <br />
    <br />
    <div>        
        <%: Html.ActionLink("Plan/Region/Site Common Index", "PlanRegionIndex")%> &nbsp;
        <%: Html.ActionLink("Plan Index", "Index") %>
    </div>
    <% } %>
    </div>

</div>     
</asp:Content>
