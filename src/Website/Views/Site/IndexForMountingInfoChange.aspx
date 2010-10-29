<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Website.ViewModel.Plan.PlanAddMountingInfoViewModel>>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">  
  <div class="box"> 
					<!-- box / title --> 
					<div class="title"> 
						<h5>Change Mounting Info</h5> 						
					</div> 
					<!-- end box / title --> 
                   <div class="form" id="contentTable"> 
<table>
        <tr>
            <th></th>
           
            <th>
                BriefNo
            </th>
            <th>
                PlanNo
            </th>
            <th>
                StartDate
            </th>
            <th>
                EndDate
            </th>                     
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: Html.ActionLink("Edit", "ChangeMountingInfo", new { id = item.PlanDetailsId, })%>              
            </td>
            
            <td>
                <%: item.BriefNo %>
            </td>
            <td>
                <%: item.PlanNo %>
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

