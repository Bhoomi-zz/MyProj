<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Website.ViewModel.PlanIndexViewModel>>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">    
<script type="text/javascript">
    $(document).ready(function () {
        $("#menu-plan").toggleClass("closed");
        $("#h-menu-plan").addClass("selected");
    });
</script>
<div class="box"> 
					<!-- box / title --> 
					<div class="title"> 
						<h5>Plan Index </h5> 						
					</div> 
					<!-- end box / title --> 
                   <div class="form" id="contentTable"> 
    <table>
        <tr>
            <th></th>
            <th>
                Plan No
            </th>
            <th>
                Brief No
            </th>
            <th>
                Client
            </th>
            <th>
                Created On
            </th>
            <th>
                HeadPlanner 
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
                <%: Html.ActionLink("Edit", "Edit", new {  id=item.PlanDetailsId  }) %>                
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
                <%: String.Format("{0:d}", item.CreatedOn) %>
            </td>
            <td>
                <%: item.HeadPlanner %>
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
<br /><br />

        <%: Html.ActionLink("Create New Plan", "Create") %>

</div> 
</div>
</asp:Content>

