<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Website.ViewModel.BriefAllocationViewModelForEdit>" %>
<%@ Import Namespace="MvcGridView.Code" %>
<%@ Import Namespace="Website.Services" %>
<%@ Import Namespace="Website.ViewModel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

      
      <link href="<%= Url.Content("~/Content/ui.jqgrid.css" )%> " rel="stylesheet" type="text/css" />
     <link href="<%= Url.Content("~/Scripts/themes/grid.css")%> " rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/jqModal.css" )%> "rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/jquery.autocomplete.css" )%> " rel="stylesheet" type="text/css" />    
    
    <script src="<%= Url.Content("~/Scripts/js/i18n/grid.locale-en.js" )%> " type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/js/jquery.jqGrid.min.js" )%> " type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/js/grid.inlinedit.js" )%> "type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.autocomplete.js")%> " type="text/javascript"></script>    
    <script src="<%= Url.Content("~/Scripts/jquery.validate.min.js")%> " type="text/javascript"></script>        
    <script src="<%= Url.Content("~/Scripts/BriefAllocation.js")%> " type="text/javascript"></script>           

    <script type="text/javascript">

        $(document).ready(function () {
            $("#menu-brief").toggleClass("closed");
            $("#h-menu-brief").addClass("selected");
        });
            </script>
    <div class="box"> 
					<!-- box / title --> 
					<div class="title"> 
						<h5>Edit Brief Allocation </h5> 						
					</div> 
					<!-- end box / title --> 
                   <div class="form" > 
     <% using (Html.BeginForm())
        {%>         
          <%:Html.HiddenFor(model => model.PlanId) %>
           
            <table style="border: none;width:80%;">
            <tr >
            <td style="border: none; padding: 2px 2px 2px 2px;"> 
                <label id="lblbriefno" class="label">Brief #</label> </td>
                <td style="border: none; padding: 2px 2px 2px 2px;"><%:Html.TextBoxFor(model => model.BriefNo)%> 
                <%:Html.ValidationMessageFor(model => model.BriefNo)%> <em>*</em> </td>
           
           <td style="border: none; padding: 2px 2px 2px 2px;">    <label id="Label1">Created On</label>     </td>
                <td style="border: none;"> <%: Html.EditorFor(model => model.CreatedOn, new Dictionary<string, object>(){{"Title","DateTimePicker"}}) %>
                <%:Html.ValidationMessageFor(model => model.CreatedOn)%></td>
            
            </tr>
            <tr>
             <td style="border: none; padding: 2px 2px 2px 2px;">  Head Planner </td>
              <td style="border: none; padding: 2px 2px 2px 2px;"> <%: Html.DropDownListFor(model => model.HeadPlannerId, (Model.Users), "Select one..", new { id = "HeadPlannerId" })%>  
                <%: Html.ValidationMessageFor(model => model.HeadPlannerId)%></td>
                </tr>            
            </table>             
                  <br />
     <div>
      
                <a href="javascript:AddRegionAndCity()" >Add New Region And City</a>
                <br /><br />
     <table id ="RegionAndCity" style="width:800px"></table>             
    
             <div id="listPager" class="scroll" style="text-align:center;"></div>                                                            
                <div id="listPsetcols" class="scroll" style="text-align:center;"></div>  
                 <br />
                <a href="javascript:AddRegionAndCity()" >Add New Region And City</a>
                
                </div>
                
                 <br />
                <div id="fogLoaderimg"></div>     
                <input type="submit" value="Save" onclick ="javascript:LoadSpinnerImg('fogLoaderimg');"  />              
            <br />   <br />
    <div>
        <%:Html.ActionLink("Back to List", "Index")%>
    </div>
  <% }%>
  </div>
  </div>

</asp:Content>
