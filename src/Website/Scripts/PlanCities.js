var PlanDetailsId;
var selectedRegion;
var selectedLocationIdvar = "NotAssigned";
var regions;
function AddCity() {
    jQuery("#Cities").jqGrid('editGridRow', "new",
                { height: 280, reloadAfterSubmit: false,
                    onclickSubmit: function (params) {
                        var id = GenerateGUID();
                        $("#PlanCitiesId").val(id);
                        return { PlanCitiesId: id,  PlanDetailsId: $("#PlanDetailsId").val(),
                            PlannerName: $("#PlannerId :selected").text(),
                            selectedLocationId: selectedLocationIdvar
                        };
                    }
                }
           );
            }
            function SaveForm() {
                LoadSpinnerImg('fogLoaderimg');
                $.ajax({
                    url: rootPath + "Plan.aspx/SaveCities/?planDetailsId=" + PlanDetailsId,
                    success: function (data, result) {
                        UnLoadSpinnerImg('fogLoaderimg');
                        if (!result) alert('Failure to retrieve the Cities.');                        
                        else alert('Saved Successfully');
                    },
                    error: function (data, result) {
                        UnLoadSpinnerImg('fogLoaderimg');
                        alert('Error while saving');
                    }
                });
            }
$(document).ready(function () {
    PlanDetailsId = $("#PlanDetailsId").val();
    selectedRegion = $("#Region").val();     
    regions = $("#RegionString").val();
    var planners = $.ajax({ url: rootPath + 'Shared.aspx/GetAllUsers/',
        async: false,
        success: function (data, result) {
            if (!result) alert('Failure to retrieve the Planners.');
        }
    }).responseText;


    var updateDialog = {
        url: '<%= Url.Action("UpdateCity", "Plan") %>'
                , closeAfterAdd: true
                , closeAfterEdit: true
                , recreateForm: true
                , modal: false
    };

    //For passing id of the selected row.
    myextraparam = { selectedLocationId: "NotAssigned" };
    myediturl = rootPath + 'Plan.aspx/UpdateCity/';

    $("#Cities").jqGrid({
        url: rootPath + 'Plan.aspx/GetCities/?PlanDetailsId=' + PlanDetailsId + "&region=" + selectedRegion,
        datatype: 'json',
        colNames: ["Actions", 'CitiesId', 'Region', 'City', 'Planner', 'Budget'],
        colModel: [
                                  { name: 'act', index: 'act', width: 75, sortable: false },
                                  { name: 'PlanCitiesId', index: 'PlanCitiesId', width: 100, align: 'left', /* key: true,*/editable: true, editrules: { edithidden: false }, hidedlg: true, hidden: true },
                                  { name: 'Region', index: 'Region', width: 100, align: 'left', editable: true, edittype: 'select', editoptions: { value: regions }, editrules: { required: true }, formoptions: { elmsuffix: ' *'} },
                                  { name: 'LocationName', index: 'LocationName', width: 300, align: 'left', editable: true, edittype: 'text',
                                      editoptions: { dataInit: function (elem) {
                                          setTimeout(function () {
                                              $(elem).autocomplete(rootPath + 'Shared.aspx/FindLocation/', {
                                                  dataType: "json",
                                                  multiple: false,
                                                  formatItem: function (item, index, total, query) {
                                                      return item.value;
                                                  },
                                                  parse: function (data) {
                                                      return $.map(data, function (item) {
                                                          return {
                                                              data: item,
                                                              value: item.Key,
                                                              result: item.value
                                                          };
                                                      });
                                                  }
                                              }).result(function (event, row) {
                                                  selectedLocationIdvar = row.Key;
                                                  myextraparam = { selectedLocationId: row.Key };
                                              });
                                          }, 100);
                                      }
                                  }, editrules: { required: true }, formoptions: { elmsuffix: ' *' }
                                  },
                                  { name: 'PlannerId', index: 'PlannerId', width: 150, align: 'left', editable: true, edittype: 'select', editoptions: { value: planners }, editrules: { required: false} },
                                  { name: 'Budget', index: 'Budget', width: 100, align: 'left', editable: true, edittype: 'text', editrules: { required: false }},
                                ],
        pager: $('#listPager'),
        rowNum: 100,
        autowidth: true,
        height: '300px',
        rowList: [5, 10, 20, 50],
        sortname: 'Region',
        sortorder: "asc",
        viewrecords: true,
        scrollOffset:0,
        prmNames: {
            PlanDetailsId: "ab"
        },
        imgpath: '/scripts/themes/steel/images',
        caption: '<b>Regions And Cities</b>',
        editurl: rootPath + 'Plan.aspx/UpdateCity/',
        gridComplete: function () {
            var ids = jQuery("#Cities").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                be = "<input style='padding:2px 2px 0px 2px;' type='image'  title='Edit' src= '" + rootPath + "Resources/Images/edit_icon.png' value='edit' onclick=\"jQuery('#Cities').editRow('" + cl + "'); return false; \" />";
                se = "<input style='padding:2px 2px 0px 2px' type='image' title=\"Save\" src= '" + rootPath + "Resources/Images/save_icon.gif' value='Save1' onclick=\"$('#Cities').saveRow('" + cl + "', false, myediturl, myextraparam); return false; \" />";
                ce = "<input  style='padding:2px 2px 0px 2px' type='image' title=\"Cancel\" src= '" + rootPath + "Resources/Images/revert.gif' value='Cancel' onclick=\"jQuery('#Cities').restoreRow('" + cl + "'); return false; \" />";
                jQuery("#Cities").jqGrid('setRowData', ids[i], { act: be + se + ce });
            }
        }
    });

    $("Cities").jqGrid('navGrid', '#listPager', { edit: true, add: true, del: false, refresh: false },
            { onclickSubmit: function (params) { alert('updating'); } }, updateDialog, null);


    $("#AddCity").click(function () {
        jQuery("#RegionAndCity").jqGrid('editGridRow', "new",
                { height: 280, reloadAfterSubmit: false,
                    onclickSubmit: function (params) {
                        return { PlanDetailsId: $("#PlanDetailsId").val(),
                            PlannerName: $("#PlannerId :selected").text(),
                            selectedLocationId: selectedLocationIdvar
                        };
                    }
                }
           );
    });
}); 
         