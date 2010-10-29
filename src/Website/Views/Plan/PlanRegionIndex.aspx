<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Website.ViewModel.Plan.PlanEditViewModel>>" %>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<script type="text/javascript">
    $("#menu-plan").toggleClass("closed");
    $("#h-menu-plan").addClass("selected");
</script>
    <div class="box"> 
					<!-- box / title --> 
					<div class="title"> 
						<h5>Plan - Region - City - Site</h5> 						
					</div> 
					<!-- end box / title --> 
                   <div class="form" id="contentTable"> 
         <table>
        <tr>
            <th style="width:20%">Add/Edit</th>
             <th>
                PlanNo
            </th>           
            <th>
                BriefNo
            </th>
            <th>
                Client
            </th>
            <th>
                Head Planner
            </th>
            
            <th>
                Budget
            </th>
            <th>
                Start Date
            </th>
            <th>
                End Date
            </th>
          
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: Html.ActionLink("Region", "PlanRegionsMainView", new { Id = item.PlanDetailsId })%> |
                <%: Html.ActionLink("City", "PlanRegionCity", new { Id = item.PlanDetailsId })%> | 
                <%: Html.ActionLink("Site", "PlanSite", "Site", new { planDetailId = item.PlanDetailsId }, null)%>                
            </td>
            <td>
                <%: item.PlanNo %>
            </td>            
            <td>
                <%: item.BriefNo %>
            </td>
            <td>
                <%: item.ClientName%>
            </td>
            <td>
                <%: item.HeadPlannerName %>
            </td>            
            <td>
                <%: String.Format("{0:F}", item.Budget) %>
            </td>
            <td>
                <%: String.Format("{0:d}", item.StartDate) %>
            </td>
            <td>
                <%: String.Format("{0:d}", item.EndDate) %>
            </td>          
        </tr>    
    <% } %>
    </table>
</div>
</div>
</asp:Content>

