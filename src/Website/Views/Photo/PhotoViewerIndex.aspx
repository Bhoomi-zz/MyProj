<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Website.ViewModel.Photo.PlanAlbumIndexViewModel>>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box"> 
	    <!-- box / title --> 
	    <div class="title"> 
		    <h5>Site Album Index</h5> 						
	    </div> 
	    <!-- end box / title --> 
        
        <div class="form" id="contentTable"> 
    <table >
        <tr>
            <th></th>            
            <th>
                PlanNo
            </th>
            <th>
                ClientName
            </th>            
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: Html.ActionLink("View", "PlanPhotoViewer", new { planAlbumId = item.PlanAlbumId })%>                 
            </td>            
            <td>
                <%: item.PlanNo %>
            </td>
            <td>
                <%: item.ClientName %>
            </td>            
        </tr>
    
    <% } %>
    
    </table>

    </div>
    </div>

</asp:Content>
