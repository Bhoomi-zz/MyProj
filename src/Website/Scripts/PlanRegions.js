var PlanDetailsId;

function AddRegion() {
    jQuery("#Regions").jqGrid('editGridRow', "new",
                { height: 280, reloadAfterSubmit: false,
                    onclickSubmit: function (params) {
                        var id = GenerateGUID();
                        $("#planRegionId").val(id);
                        return { planRegionId : id, PlanDetailsId: $("#PlanDetailsId").val(),
                            PlannerName: $("#PlannerId :selected").text()
                        };
                    }
                }
           );
    }
    function SaveForm() {
        LoadSpinnerImg('fogLoaderimg');
        $.ajax({
            url: rootPath + "Plan.aspx/SaveRegions/?planDetailsId=" + PlanDetailsId,
            success: function (data, result) {
                if (!result) alert('Failure to retrieve the Cities.');
                UnLoadSpinnerImg('fogLoaderimg');
                alert('Saved Successfully');

            },
            error: function (data, result) {
                UnLoadSpinnerImg('fogLoaderimg');
                alert('Error while saving');
            }
        });
    }
$(document).ready(function () {
    PlanDetailsId = $("#PlanDetailsId").val();
    var planners = $.ajax({ url: rootPath + 'Shared.aspx/GetAllUsers/',
        async: false,
        success: function (data, result) {
            if (!result) alert('Failure to retrieve the Planners.');
        }
    }).responseText;

    var updateDialog = {
        url: '<%= Url.Action("UpdateRegions", "Plan") %>',
        closeAfterAdd: true,
        closeAfterEdit: true,
        recreateForm: true,
        modal: false
    };

    //For passing id of the selected row.
    myediturl = rootPath + '/Plan.aspx/UpdateRegions/';

    $("#Regions").jqGrid({
        url: rootPath + 'Plan.aspx/GetRegions/?PlanDetailsId=' + PlanDetailsId,
        datatype: 'json',
        colNames: ["Actions", 'planRegionId', 'Region', 'Planner', 'Budget'],
        colModel: [
                                  { name: 'act', index: 'act', width: 35, sortable: false },
                                  { name: 'planRegionId', index: 'planRegionId', width: 75, align: 'left', /* key: true,*/editable: true, editrules: { edithidden: false }, hidedlg: true, hidden: true },
                                  { name: 'Region', index: 'Region', width: 100, align: 'left', editable: true, edittype: 'select', editoptions: { value: "North:North;South:South;East:East;West:West" }, editrules: { required: true }, formoptions: { elmsuffix: ' *'} },                                  
                                  { name: 'PlannerId', index: 'PlannerId', width: 100, align: 'left', editable: true, edittype: 'select', editoptions: { value: planners }, editrules: { required: false} },
                                  { name: 'Budget', index: 'Budget', width: 50, align: 'left', editable: true, edittype: 'text', editrules: { required: false }},
                                ],
        pager: $('#listPager'),
        rowNum: 10,
        autowidth: true,
        rowList: [5, 10, 20, 50],
        sortname: 'Region',
        sortorder: "asc",
        viewrecords: true,
        height:'300px',
        prmNames: {
            PlanDetailsId: "ab"
        },
        imgpath: '/scripts/themes/steel/images',
        caption: '<b>Regions</b>',
        editurl: rootPath + 'Plan.aspx/UpdateRegions/',
        gridComplete: function () {
            var ids = jQuery("#Regions").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                be = "<input style='padding:2px 2px 0px 2px;' type='image'  title='Edit' src= '" + rootPath + "Resources/Images/edit_icon.png' value='edit' onclick=\"jQuery('#Regions').editRow('" + cl + "'); return false; \" />";
                se = "<input style='padding:2px 2px 0px 2px' type='image' title=\"Save\" src= '" + rootPath + "Resources/Images/save_icon.gif' value='Save1' onclick=\"$('#Regions').saveRow('" + cl + "', false, myediturl); return false; \" />";
                ce = "<input style='padding:2px 2px 0px 2px' type='image' title=\"Cancel\" src= '" + rootPath + "Resources/Images/revert.gif' value='Cancel' onclick=\"jQuery('#Regions').restoreRow('" + cl + "'); return false;\" />";
                jQuery("#Regions").jqGrid('setRowData', ids[i], { act: be + se + ce });
            }
        }
    });

    $("Regions").jqGrid('navGrid', '#listPager', { edit: false, add: true, del: false, refresh: false },
            { onclickSubmit: function (params) { alert('updating'); } }, updateDialog, null);

//    $("#Regions").click(function () {
//        jQuery("#Regions").jqGrid('editGridRow', "new",
//                { height: 280, reloadAfterSubmit: false,
//                    onclickSubmit: function (params) {
//                        return { PlanDetailsId: $("#PlanDetailsId").val(),
//                            PlannerName: $("#PlannerId :selected").text()
//                        };
//                    }
//                }
//           );
//    });
}); 
         