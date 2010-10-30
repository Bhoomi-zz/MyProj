<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Website.ViewModel.BriefAllocationViewModelForCreate>" %>
<%@ Import Namespace="MvcGridView.Code" %>
<%@ Import Namespace="Website.Services" %>
<%@ Import Namespace="Website.ViewModel" %>



<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     
      <link href="<%= Url.Content("~/Content/ui.jqgrid.css")%> " rel="stylesheet" type="text/css" />
     <link href="<%= Url.Content("~/Scripts/themes/grid.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/jqModal.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/jquery.autocomplete.css")%>" rel="stylesheet" type="text/css" />    

    <script src="<%= Url.Content("~/Scripts/js/i18n/grid.locale-en.js")%>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/js/jquery.jqGrid.min.js" )%>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/js/grid.inlinedit.js" )%>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.autocomplete.js" )%>" type="text/javascript"></script>    
    <script src="<%= Url.Content("~/Scripts/jquery.validate.min.js"  )%>" type="text/javascript"></script>       
    <script src="<%= Url.Content("~/Scripts/BriefAllocation.js" )%>" type="text/javascript"></script>
      <script src="<%= Url.Content("~/Scripts/common.js") %>" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#menu-brief").toggleClass("closed");
            $("#h-menu-brief").addClass("selected");
           
        });
    </script>
   <div class="box"> 
					<!-- box / title --> 
					<div class="title"> 
						<h5>Create Brief Allocation </h5> 						
					</div> 
					<!-- end box / title --> 
                   <div class="form" > 
     <% using (Html.BeginForm("Create","BriefAllocation"))
        {%>           
            <div class="form"> 
						<div > 
            <%:Html.HiddenFor(model => model.PlanId) %>
            
            <div >
                <label id="lblbriefno">Brief #</label>
                <%:Html.TextBoxFor(model => model.BriefNo)%>
                <%:Html.ValidationMessageFor(model => model.BriefNo)%>
           &nbsp; &nbsp;&nbsp;
               <label id="Label1">Created On</label>     
                <%: Html.EditorFor(model => model.CreatedOn, new Dictionary<string, object>(){{"Title","DateTimePicker"}}) %>
                <%:Html.ValidationMessageFor(model => model.CreatedOn)%>
            </div>
            <br />
            <div>
               <label id="Label2" class ="label">Head Planner</label>                           
                <%:Html.DropDownListFor(model => model.HeadPlannerId, (Model.Users), "Select one..", new { id = "HeadPlannerId" })%>
                <%:Html.ValidationMessageFor(model => model.HeadPlannerId)%>
                  </div>
              </div> 
            </div>     
     <a href="javascript:AddRegionAndCity()">Add New Region And City</a>
     <br />  <br />
     <div  >
     <table id ="RegionAndCity" ></table>                 
     <div id="listPager" ></div>                                                            
     <div id="listPsetcols" ></div>  
     </div>
     <br />
     <a href="javascript:AddRegionAndCity()">Add New Region And City</a>
      <br />  <br />
        
         <div class="buttons">    <div id="fogLoaderimg"></div>     
                <input type="submit" value="Save" onclick ="javascript:LoadSpinnerImg('fogLoaderimg');"  />
         </div>
     <div>  <br />
        <%:Html.ActionLink("Back to List", "Index")%>
    </div>
  <% }%>
    </div>
  </div>

   
</asp:Content>

