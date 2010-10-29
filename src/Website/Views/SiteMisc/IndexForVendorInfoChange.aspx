<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Website.ViewModel.PlanIndexViewModel>>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">  
<script type ="text/javascript">
    $(document).ready(function () {
        $("#menu-plan").toggleClass("closed");
        $("#h-menu-plan").addClass("selected");
    });
   </script>

<div class="box"> 
					<!-- box / title --> 
					<div class="title"> 
						<h5>Planning & Buying Site - Plan Index </h5> 						
					</div> 
					<!-- end box / title --> 
                   <div class="form" id="contentTable"> 
    <table>
        <tr>
            <th></th>   
            <th>
                PlanNo
            </th>          
            <th>
                ClientName
            </th>             
            <th>
                BriefNo
            </th>
            <th>
                HeadPlanner
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: Html.ActionLink("Edit", "SiteVendorAssignment",  new { planDetailsId = item.PlanDetailsId }) %>                
            </td>            
            <td>
                <%: item.PlanNo %>
            </td>
            <td>
                <%: item.ClientName %>
            </td>
             
              <td>
                <%: item.BriefNo%>
            </td>
            <td>
                <%: item.HeadPlanner %>
            </td>            
        </tr>    
    <% } %>
    </table> 
</div>
</div>
</asp:Content>

