<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Website.ViewModel.BriefAllocationViewModelForCreate>" %>
<%@ Import Namespace="MvcGridView.Code" %>
<%@ Import Namespace="Website.Services" %>
<%@ Import Namespace="Website.ViewModel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <link href="../../Content/BRIEFALLOCATION.css" rel="stylesheet" type="text/css" /> 
      <link href="../../Content/ui.jqgrid.css" rel="stylesheet" type="text/css" />
     <link href="../../Scripts/themes/grid.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/jqModal.css" rel="stylesheet" type="text/css" />

    
    <script src="../../Scripts/jquery.autocomplete.js" type="text/javascript"></script>    
    <script src="../../Scripts/jquery.validate.min.js" type="text/javascript"></script>        
    <script src="../../Scripts/jquery.jqGrid.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.maskedinput-1.2.2.min.js" type="text/javascript"></script>
        
<link href="../../Content/jquery.autocomplete.css" rel="stylesheet" type="text/css" />    

     <% using (Html.BeginForm())
        {%>         
         <fieldset>
            <legend>Fields</legend>
            
            <%:Html.HiddenFor(model => model.PlanId) %>
            
            <div class="editor-field">
                <%:Html.TextBoxFor(model => model.BriefNo)%>
                <%:Html.ValidationMessageFor(model => model.BriefNo)%>
            </div>
            
            <div class="editor-label">
                <%:Html.LabelFor(model => model.CreatedOn)%>
            </div>
            <div class="editor-field">                
                <%: Html.TextBoxFor(model => model.CreatedOn, new Dictionary<string, object>(){{"Title","DateTimePicker"}}) %>
                <%:Html.ValidationMessageFor(model => model.CreatedOn)%>
            </div>
            
            <div class="editor-label">
                <%:Html.LabelFor(model => model.HeadPlannerId)%>
            </div>
            <div class="editor-field">
                <%:Html.DropDownListFor(model => model.HeadPlannerId, (Model == null ? new SelectList(SharedDataService.GetAllUsers()): Model.Users), "Select one..", new { id = "HeadPlannerId" })%>
                <%:Html.ValidationMessageFor(model => model.HeadPlannerId)%>
            </div>             
            </fieldset>           
     <div>
     <table id ="RegionAndCity" ></table>             
    
             <div id="listPager" ></div>                                                            
                <div id="listPsetcols" ></div>  
                </div>
                 <p>
                <input type="submit" value="Create" />
            </p>
    <div>
        <%:Html.ActionLink("Back to List", "Index")%>
    </div>
  <% }%>

<script type="text/javascript">
    $(document).ready(function () {
        var enterdValue = $("LocationName").val();

        var countries = $.ajax({ url: '/Shared/FindLocation/',
            async: false,
            success: function (data, result) {
                if (!result) alert('Failure to retrieve the Countries.');
            }
        }).responseText;

        var planners = $.ajax({ url: '/Shared/GetAllUsers/',
            async: false,
            success: function (data, result) {
                if (!result) alert('Failure to retrieve the Planners.');
            }
        }).responseText;

        var PlanId = $("#PlanId").val();
        var updateDialog = {
            url: '<%= Url.Action("UpdateRegionAndCity", "BriefAllocation") %>'
                , closeAfterAdd: true
                , closeAfterEdit: true
                , recreateForm: true
                , modal: false,
            onclickSubmit: function (params) {
                var ajaxData = {};
                var list = $("#RegionAndCity");
                var selectedRow = list.getGridParam("selrow");
                rowData = list.getRowData(selectedRow);
                var plannerName = $("#PlannerId :selected").text();
                ajaxData = { PlanId: PlanId, PlannerName: plannerName, LocationName: $("#LocationId :selected").text() };

                return ajaxData;
            }
        };
        $.jgrid.nav.addtext = "Add";
        $.jgrid.nav.edittext = "Edit";
        $.jgrid.nav.deltext = "Delete";
        $.jgrid.edit.addCaption = "Add City";
        $.jgrid.edit.editCaption = "Edit City";
        $.jgrid.del.caption = "Delete City";
        $.jgrid.del.msg = "Delete selected City?";

        $("#RegionAndCity").jqGrid({
            url: '/BriefAllocation/GetRegionAndCities/?PlanId=' + PlanId,
            datatype: 'json',
            colNames: ['RegionsAndCitiesId', 'LocationId', 'PlannerId', 'Region', 'Budget'],
            colModel: [
                                  { name: 'RegionsAndCitiesId', index: 'RegionsAndCitiesId', width: 100, align: 'left', /* key: true,*/editable: true, editrules: { edithidden: false }, hidedlg: true, hidden: true },
                                  { name: 'LocationId', index: 'LocationId', width: 150, align: 'left', editable: true, edittype: 'select', editoptions: { value: countries }, editrules: { required: true }, formoptions: { elmsuffix: ' *'} },
                                 
                                  { name: 'PlannerId', index: 'PlannerId', width: 150, align: 'left', editable: true, edittype: 'select', editoptions: { value: planners }, editrules: { required: false} },
                                  { name: 'Region', index: 'Region', width: 100, align: 'left', editable: true, edittype: 'select', editoptions: { value: "North:North;South:South;East:East;West:West" }, editrules: { required: true }, formoptions: { elmsuffix: ' *'} },
                                  { name: 'Budget', index: 'Budget', width: 100, align: 'left', editable: true, edittype: 'text', editrules: { required: true }, formoptions: { elmsuffix: ' *'} },
                                ],
            pager: $('#listPager'),
            rowNum: 10,
            rowList: [5, 10, 20, 50],
            sortname: 'RegionsAndCitiesId',
            sortorder: "asc",
            viewrecords: true,
            imgpath: '/scripts/themes/steel/images',
            caption: '<b>Regions And Cities</b>'            
        }).navGrid('#listPager', { edit: true, add: true, del: true, refresh: true },
                updateDialog,
                updateDialog,
                updateDialog);
    });
         </script>
   
</asp:Content>

