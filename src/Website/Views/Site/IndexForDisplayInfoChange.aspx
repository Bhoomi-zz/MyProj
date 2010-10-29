<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Website.ViewModel.Plan.PlanAddDisplayInfoViewModel>>" %>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="box"> 
					<!-- box / title --> 
					<div class="title"> 
						<h5>Change Display Info</h5> 						
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
                <%: Html.ActionLink("Edit", "ChangeDisplayInfo", new { id = item.PlanDetailsId, })%>                
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

<br />
<br />
        <%: Html.ActionLink("Plan Index", "Index", "Plan") %>

</div>
</div>
</asp:Content>

