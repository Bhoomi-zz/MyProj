<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Website.ViewModel.BriefViewModelIndex>>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <script type="text/javascript">

     $(document).ready(function () {
         $("#menu-brief").toggleClass("closed");
         $("#h-menu-brief").addClass("selected");
     });
    </script>

    <div class="box"> 
					<!-- box / title --> 
					<div class="title"> 
						<h5>Brief Index</h5> 						
					</div> 
					<!-- end box / title --> 
                   <div class="form" id="contentTable">              
      <table>
        <tr>
            <th></th>
            <th>
                Date
            </th>
             <th>Brief No </th>
            <th>
                Customer    
            </th>           
            <th></th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: Html.ActionLink("Edit", "Edit", new { id = item.BriefId.Trim()}) %>                                
            </td>
            <td>
                <%: item.BriefDate.ToShortDateString()%>
            </td>
             <td>
                <%: item.BriefNo%>
            </td>
            <td>
                <%: item.Customer%>
            </td>
            <td> <%: Html.ActionLink("Allocate Brief", "AllocateBrief", "BriefAllocation", new { briefNo = item.BriefNo }, null)%></td>           
        </tr>
    
    <% } %>

    </table>

    <br/>
        <%: Html.ActionLink("Add New Brief", "Add")%>
    
        </div>                
     </div>    
</asp:Content>

