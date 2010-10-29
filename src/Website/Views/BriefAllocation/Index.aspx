<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Website.ViewModel.BriefAllocationViewModelForIndex>>" %>



<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

   
   <div class="box"> 
					<!-- box / title --> 
					<div class="title"> 
						<h5>Brief Allocation Index</h5> 						
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
                HeadPlanner
            </th>
            <th>
                CreatedOn
            </th>
            <th></th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: Html.ActionLink("Edit", "Edit", new {id = new Guid( item.BriefAllocationId.ToString()) })%>               
            </td>
            
            <td>
                <%: item.BriefNo %>
            </td>
            <td>
                <%: item.HeadPlanner %>
            </td>
            <td>
                <%:   String.Format("{0:d}", item.CreatedOn.Value.ToShortDateString()) %>
            </td>
            <td><%: Html.ActionLink("Create Plan", "CreatePlanWithBriefNo", "Plan", new { briefNo = item.BriefNo, contactId = item.contactId, contactName = item.ClientName }, null)%>        </td>
        </tr>
    
    <% } %>

    </table>
   <br />
        <%: Html.ActionLink("Create New Brief Allocatioin", "Create") %>   
</div>
</div>
</asp:Content>

