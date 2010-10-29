<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Website.ViewModel.Plan.PlanEditViewModel>" %>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<style>
    #ClientName
    {
        width:93%;
    }
</style>
    

 <script type ="text/javascript">
 
     $(document).ready(function () {
         $("#PlanRegionsLink").html("<li> <a href=" + rootPath + "Plan.aspx/PlanRegionsMainView/?Id=" + $("#PlanDetailsId").val() + ">Add/Edit Region to Plan</a></li>");
         $("#menu-plan").toggleClass("closed");
         $("#h-menu-plan").addClass("selected");                  
       });
 </script>  

 <script src="<%= Url.Content("~/Scripts/Plan.js")%>" type="text/javascript"></script>
  <div class="box"> 
					<!-- box / title --> 
					<div class="title"> 
						<h5>Edit Plan</h5> 						
					</div> 
					<!-- end box / title --> 
                   <div class="form" > 

    <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>          
           
              <%: Html.HiddenFor(model => model.PlanDetailsId) %>
            
            <table style="width:80%">
            <tr>
                <td style="border: none; padding: 2px 2px 2px 2px;">Plan No</td>
                <td style="border: none; padding: 2px 2px 2px 2px;"> <%: Html.TextBoxFor(model => model.PlanNo, new Dictionary<string, object>() { { "readonly", "true" } })%>
                     <%: Html.ValidationMessageFor(model => model.PlanNo) %></td>
                <td style="border: none; padding: 2px 2px 2px 2px;"> CreatedOn </td>
                <td style="border: none; padding: 2px 2px 2px 2px;"> <%: Html.EditorFor(model => model.CreatedOn, new Dictionary<string, object>(){{"Title","DateTimePicker"}}) %>
                     <%: Html.ValidationMessageFor(model => model.CreatedOn) %> </td>
            </tr>
            <tr>
                <td style="border: none; padding: 2px 2px 2px 2px;">BriefNo </td>
                <td style="border: none; padding: 2px 2px 2px 2px;"><%: Html.TextBoxFor(model => model.BriefNo) %>
                    <%: Html.ValidationMessageFor(model => model.BriefNo) %> <em>*</em> </td>
                <td style="border: none; padding: 2px 2px 2px 2px;"> Client</td>
                <td style="border: none; padding: 2px 2px 2px 2px;"> <%: Html.TextBoxFor(model => model.ClientName) %>
                     <%: Html.HiddenFor(model => model.ClientId) %>
                     <%: Html.ValidationMessageFor(model => model.ClientName) %> <em>*</em></td>
            </tr>
            <tr>
                <td style="border: none; padding: 2px 2px 2px 2px;">Head Planner</td>
                <td style="border: none; padding: 2px 2px 2px 2px;"> <%: Html.DropDownListFor(model => model.HeadPlannerId, Model.Users,"Select one..", new { id = "HeadPlannerId" }) %>
                     <%: Html.ValidationMessageFor(model => model.HeadPlannerId) %></td>
                <td style="border: none; padding: 2px 2px 2px 2px;">Budget </td>
                <td style="border: none; padding: 2px 2px 2px 2px;"> <%: Html.TextBoxFor(model => model.Budget) %>
                     <%: Html.ValidationMessageFor(model => model.Budget) %></td>
            </tr>
            <tr>
                <td style="border: none; padding: 2px 2px 2px 2px;">Start Date</td>
                <td style="border: none; padding: 2px 2px 2px 2px;"> <%: Html.EditorFor(model => model.StartDate, new Dictionary<string, object>(){{"Title","DateTimePicker"}}) %>
                     <%: Html.ValidationMessageFor(model => model.StartDate) %> <em>*</em></td>
                <td style="border: none; padding: 2px 2px 2px 2px;">End Date</td>
                <td style="border: none; padding: 2px 2px 2px 2px;"> <%: Html.EditorFor(model => model.EndDate, new Dictionary<string, object>(){{"Title","DateTimePicker"}}) %>
                     <%: Html.ValidationMessageFor(model => model.EndDate) %> <em>*</em></td>
            </tr>
            </table>
             <br />
             <div id="fogLoaderimg"></div>
                <input type="submit" value="Save" onclick ="javascript:LoadSpinnerImg('fogLoaderimg');" />                       
    <% } %>
     <br />
    <div>
        <br /> 
        <%: Html.ActionLink("Back to List", "Index") %>
    </div>
</div>
</div>
</asp:Content>

