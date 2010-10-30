var PlanDetailsId;
var selectedRegion;
var selectedLocationId = "NotAssigned";
var regions;

function GetAdditionalInfoForLocation() {
    var region = $("#Region").val();
    selectedRegion = region;
    selectedLocationId = $("#LocationId").val();
    $.ajax({
        url: rootPath + "Shared.aspx/GetLocationInfo/?PlanDetailsId=" + PlanDetailsId + "&region=" + region + "&locationId=" + selectedLocationId,
        success: function (data, result) {
            if (!result) alert('Failure to retrieve the Sites.');
            cityValues = data.split(";");
            $("#PlanCityId").val(cityValues[0]);
            $("#CityBudget").html(cityValues[1]);
        }
    });
}
function GetSitesForSelectedCity() {
    var region = $("#Region").val();
    selectedRegion = region;
    selectedLocationId = $("#LocationId").val();
    $.ajax({
        url: rootPath + "Site.aspx/GetSitesForMounting/?sidx=1&sord=1&page=0&rows=10&PlanDetailsId=" + PlanDetailsId + "&region=" + region + "&locationId=" + selectedLocationId,
        success: function (data, result) {
            if (!result) alert('Failure to retrieve the Sites.');
            //var jsonObject = eval('(' + data + ')');
            $("#MountingSites")[0].addJSONData(data);
        }
    });
}

function SelectAll() {
    $.ajax({
        url: rootPath + "Site.aspx/SelectAllSites/?PlanDetailsId=" + PlanDetailsId + "&region=" + regions + "&locationId=" + selectedLocationId,
        success: function (data, result) {
            if (!result) alert('Failure to retrieve the Sites.');
            //var jsonObject = eval('(' + data + ')');
            $("#MountingSites")[0].addJSONData(data);
        }
    });
}

function SelectNone() {
    $.ajax({
        url: rootPath + "Site.aspx/SelectNone/?PlanDetailsId=" + PlanDetailsId + "&region=" + regions + "&locationId=" + selectedLocationId,
        success: function (data, result) {
            if (!result) alert('Failure to retrieve the Sites.');
            //var jsonObject = eval('(' + data + ')');
            $("#MountingSites")[0].addJSONData(data);
        }
    });
}

function UpdateMountingInfo() {
    var vendorName = $("#MountingVendorName").val();
    var vendorId = $("#MountingVendorId").val();
    var MountingRate = $("#MountingRate").val();
    var clientRate = $("#MountingClientRate").val();
    
    $.ajax({
        url: rootPath + "Site.aspx/UpdateMountingInfo/?PlanDetailsId=" + PlanDetailsId + "&region=" + regions + "&locationId=" + selectedLocationId + "&VendorName=" + vendorName + "&VendorId=" + vendorId + "&MountingRate=" + MountingRate + "&ClientRate=" + clientRate,
        success: function (data, result) {
            if (!result) alert('Failure to retrieve the Sites.');
            //var jsonObject = eval('(' + data + ')');
            $("#MountingSites")[0].addJSONData(data);
        }
    });
}
$(document).ready(function () {
    PlanDetailsId = $("#PlanDetailsId").val();
    selectedRegion = $("#PlanRegionInHeader").val();
    regions = $("#RegionString").val();

    $("#LocationName").autocomplete(rootPath + 'Shared.aspx/FindLocationFilterdWithRegion/?region=' + selectedRegion, {
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
        $("#LocationId").val(row.Key);
        GetAdditionalInfoForLocation();
        GetSitesForSelectedCity();
    });


    $("#MountingVendorName").autocomplete(rootPath + 'Shared.aspx/FindVendor/', {
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
        $("#MountingVendorId").val(row.Key);

    });
    //For passing id of the selected row.
    myextraparam = { selectedLocationId: "NotAssigned" };
    myediturl = '/Site.aspx/UpdateMountingInfoForSite/';
    var lastsel2;
    $("#MountingSites").jqGrid({
        url: rootPath + 'Site.aspx/GetSitesForMounting/?PlanDetailsId=' + PlanDetailsId + '&region=' + selectedRegion + '&locationId=' + selectedLocationId,
        datatype: 'json',
        colNames: [ "", "Select", 'SitesId', 'Site#', 'Site Name', 'SqFt', 'MountingVendor', 'Mounting Vendor', 'Mounting Rate', 'Mounting Cost', 'Client Rate', 'Client Cost', 'Address line 1', 'Address line 2', 'Address line 3'],
        colModel: [
                                  { name: 'act', index: 'act', width: 22, sortable: false },
                                  { name: 'IsSelected', index: 'IsSelected', width: 75, edittype: 'checkbox', sortable: true, editable: true },                                  
                                  { name: 'planSiteId', index: 'PlanSiteId', width: 100, align: 'left', /* key: true,*/editable: true, editrules: { edithidden: false }, hidedlg: true, hidden: true },
                                  { name: 'SiteNo', index: 'SiteNo', width: 80, align: 'left', editable: false, edittype: 'text', editrules: { required: true} },
                                  { name: 'siteName', index: 'Site Name', width: 100, align: 'left', editable: false, edittype: 'text', editrules: { required: true }, formoptions: { elmsuffix: ' *' }, editrules: { required: true} },
                                  { name: 'SiteSizeInSqFt', index: 'SiteSizeInSqFt', width: 100, align: 'left', editable: false, edittype: 'text', editrules: { required: true }, formoptions: { elmsuffix: ' *' }, editrules: { required: true} },
                                  { name: 'MountingVendor', index: 'MountingVendor', width: 100, align: 'left', /* key: true,*/editable: true, editrules: { edithidden: false }, hidedlg: true, hidden: true },
                                  { name: 'MountingVendorName', index: 'MountingVendorName', width: 200, align: 'left', editable: true, edittype: 'text',
                                      editoptions: { dataInit: function (elem) {
                                          setTimeout(function () {
                                              $(elem).autocomplete(rootPath + 'Shared.aspx/FindVendor/', {
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
                                                  var ret = jQuery("#MountingSites").jqGrid('getRowData', 1);
                                                  selectedLocationVendorvar = row.Key;
                                                  myextraparam = { selectedLocationVendor: row.Key };
                                              });
                                          }, 100);
                                      }
                                      }, editrules: { required: true }
                                  },
                                { name: 'MountingRate', index: 'MountingRate', width: 150, align: 'left', editable: true, edittype: 'text', editrules: { required: false} },
                                { name: 'MountingCost', index: 'MountingCost', width: 150, align: 'left', editable: true, edittype: 'text', editrules: { required: false} },
                                { name: 'ClientMountingRate', index: 'ClientMountingRate', width: 150, align: 'left', editable: true, edittype: 'text', editrules: { required: false} },
                                { name: 'ClientMountingCost', index: 'ClientMountingCost', width: 150, align: 'left', editable: true, edittype: 'text', editrules: { required: false} },
                                { name: 'addressline1', index: 'addressline1', width: 150, align: 'left', editable: false, edittype: 'text', editrules: { required: false} },
                                { name: 'addressline2', index: 'addressline2', width: 150, align: 'left', editable: false, edittype: 'text', editrules: { required: false} },
                                { name: 'addressline3', index: 'addressline3', width: 150, align: 'left', editable: false, edittype: 'text', editrules: { required: false} }
                                ],
        onSelectRow: function (id) {
            if (id && id !== lastsel2) {
                jQuery('#MountingSites').jqGrid('saveRow', lastsel2);
                jQuery('#MountingSites').jqGrid('editRow', id, true);
                lastsel2 = id;
            }
        },
        pager: $('#listPager'),
        rowNum: 100,
        width: '100%',
        height: '300px',
        rowList: [5, 10, 20, 50],
        sortname: 'SiteName',
        sortorder: "asc",
        viewrecords: true,
        prmNames: {
            PlanDetailsId: "ab"
        },
        afterShowForm: function (formid) { alert(formid); },
        imgpath: '/scripts/themes/steel/images',
        caption: '<b>Sites</b>',
        editurl: rootPath + 'Site.aspx/UpdateMountingInfoForSite/',
        gridComplete: function () {
            var ids = jQuery("#MountingSites").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                se = "<input style='height:22px;width:22px;' type='button' value='S' onclick=\"$('#MountingSites').saveRow('" + cl + "', false, myediturl, myextraparam);\" />";
                jQuery("#MountingSites").jqGrid('setRowData', ids[i], { act: se });
            }
        }
    });

    $("#MountingSites").jqGrid('navGrid', '#listPager', { edit: true, add: true, del: true, refresh: false },
            { onclickSubmit: function (params) { alert('updating'); } }, null, null);

}); 
         