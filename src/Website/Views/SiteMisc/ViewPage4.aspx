<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Commands.CreatePlan>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	ViewPage4
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>ViewPage4</h2>

    <table>
        <tr>
            <th></th>
            <th>
                PlanDetailsId
            </th>
            <th>
                BriefNo
            </th>
            <th>
                CreatedOn
            </th>
            <th>
                HeadPlannerId
            </th>
            <th>
                PlanNo
            </th>
            <th>
                Budget
            </th>
            <th>
                StartDate
            </th>
            <th>
                EndDate
            </th>
            <th>
                ClientId
            </th>
            <th>
                CommandIdentifier
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: Html.ActionLink("Edit", "Edit", new { /* id=item.PrimaryKey */ }) %> |
                <%: Html.ActionLink("Details", "Details", new { /* id=item.PrimaryKey */ })%> |
                <%: Html.ActionLink("Delete", "Delete", new { /* id=item.PrimaryKey */ })%>
            </td>
            <td>
                <%: item.PlanDetailsId %>
            </td>
            <td>
                <%: item.BriefNo %>
            </td>
            <td>
                <%: String.Format("{0:g}", item.CreatedOn) %>
            </td>
            <td>
                <%: item.HeadPlannerId %>
            </td>
            <td>
                <%: item.PlanNo %>
            </td>
            <td>
                <%: String.Format("{0:F}", item.Budget) %>
            </td>
            <td>"{0:d}"
                <%: String.Format("{0:g}", item.StartDate) %>
            </td>
            <td>
                <%: String.Format("{0:g}", item.EndDate) %>
            </td>
            <td>
                <%: item.ClientId %>
            </td>
            <td>
                <%: item.CommandIdentifier %>
            </td>
        </tr>
    
    <% } %>

    </table>

    <p>
        <%: Html.ActionLink("Create New", "Create") %>
    </p>

</asp:Content>

