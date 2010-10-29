var PlanId;
var selectedLocationIdvar = "NotAssigned";

function AddRegionAndCity() {
    jQuery("#RegionAndCity").jqGrid('editGridRow', "new",
                { height: 280, reloadAfterSubmit: false,
                    onclickSubmit: function (params) {
                        var id = GenerateGUID();
                        $("#RegionsAndCitiesId").val(id);
                        return { RegionsAndCitiesId : id, PlanId: $("#PlanId").val(),
                            PlannerName: $("#PlannerId :selected").text(),
                            selectedLocationId: selectedLocationIdvar
                        };
                    }
                }
           );
}


$(document).ready(function () {
    PlanId = $("#PlanId").val();
    $("input[Title=DateTimePicker]").datepicker({ dateFormat: 'yy/mm/dd' });

    var planners = $.ajax({ url: rootPath + 'Shared.aspx/GetAllUsers/',
        async: false,
        success: function (data, result) {
            if (!result) alert('Failure to retrieve the Planners.');
        }
    }).responseText;

    
    var updateDialog = {
        url: '<%= Url.Action("UpdateRegionAndCity", "BriefAllocation") %>'
                , closeAfterAdd: true
                , closeAfterEdit: true
                , recreateForm: true
                , modal: false
    };

    //For passing id of the selected row.
    myextraparam = { selectedLocationId: "NotAssigned" };
    myediturl = rootPath + 'BriefAllocation.aspx/UpdateRegionAndCity/';

    $("#RegionAndCity").jqGrid({
        url: rootPath + 'BriefAllocation.aspx/GetRegionAndCities/?PlanId=' + PlanId,
        datatype: 'json',
        colNames: ["Actions", 'RegionsAndCitiesId', 'Region', 'City', 'Planner', 'Budget'],
        colModel: [
                                  { name: 'act', index: 'act', width: 75, sortable: false },
                                  { name: 'RegionsAndCitiesId', index: 'RegionsAndCitiesId', width: 100, align: 'left', /* key: true,*/editable: true, editrules: { edithidden: false }, hidedlg: true, hidden: true },
                                  { name: 'Region', index: 'Region', width: 100, align: 'left', editable: true, edittype: 'select', editoptions: { value: "North:North;South:South;East:East;West:West" }, editrules: { required: true }, formoptions: { elmsuffix: ' *'} },
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
                                  { name: 'PlannerId', index: 'PlannerId', width: 200, align: 'left', editable: true, edittype: 'select', editoptions: { value: planners }, editrules: { required: false} },
                                  { name: 'Budget', index: 'Budget', width: 100, align: 'left', editable: true, edittype: 'text', editrules: { required: false } },
                                ],
        pager: $('#listPager'),
        rowNum: 100,        
        rowList: [5, 10, 20, 50],
        sortname: 'RegionsAndCitiesId',
        sortorder: "asc",
        autowidth:'true',
        height:'350px',
        viewrecords: true,
        prmNames: {
            PlanId: "ab"
        },
        imgpath: '/scripts/themes/steel/images',
        caption: '<b>Regions And Cities</b>',
        editurl: rootPath + 'BriefAllocation.aspx/UpdateRegionAndCity/',
        gridComplete: function () {
            var ids = jQuery("#RegionAndCity").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                be = "<input style='padding:2px 2px 0px 2px;' type='image'  title='Edit' src= '" + rootPath + "Resources/Images/edit_icon.png' value='Edit' onclick=\"jQuery('#RegionAndCity').editRow('" + cl + "'); return false; \" />";
                se = "<input  style='padding:2px 2px 0px 2px' type='image' title=\"Save\" src= '" + rootPath + "Resources/Images/save_icon.gif' value='Save1' onclick=\"$('#RegionAndCity').saveRow('" + cl + "', false, myediturl, myextraparam); return false;\" />";
                ce = "<input  style='padding:2px 2px 0px 2px' type='image' title=\"Cancel\" src= '" + rootPath + "Resources/Images/revert.gif' value='Cancel' onclick=\"jQuery('#RegionAndCity').restoreRow('" + cl + "'); return false; \" />";
                jQuery("#RegionAndCity").jqGrid('setRowData', ids[i], { act: be + se + ce });
            }
        }
    });

    $("RegionAndCity").jqGrid('navGrid', '#listPager', { edit: true, add: true, del: false, refresh: false },
            { onclickSubmit: function (params) { alert('updating'); } }, updateDialog, null);

    

    $("#AddRegionAndCity").click(function () {
        jQuery("#RegionAndCity").jqGrid('editGridRow', "new",
                { height: 280, reloadAfterSubmit: false,
                    onclickSubmit: function (params) {
                        var id = GenerateGUID();
                        $("#RegionsAndCitiesId").val(id);
                        return { RegionsAndCitiesId: id, PlanId: $("#PlanId").val(),
                            PlannerName: $("#PlannerId :selected").text(),
                            selectedLocationId: selectedLocationIdvar
                        };
                    }
                }
           );
    });
}); 
         