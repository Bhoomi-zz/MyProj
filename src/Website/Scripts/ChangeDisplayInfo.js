
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
        url: rootPath + "Site.aspx/GetSitesForDisplay/?sidx=1&sord=1&page=0&rows=10&PlanDetailsId=" + PlanDetailsId + "&region=" + region + "&locationId=" + selectedLocationId,
        success: function (data, result) {
            if (!result) alert('Failure to retrieve the Sites.');
            //var jsonObject = eval('(' + data + ')');
            $("#DisplaySites")[0].addJSONData(data);
        }
    });
}

function SelectAll() {
    $.ajax({
        url: rootPath + "Site.aspx/SelectAllSites/?PlanDetailsId=" + PlanDetailsId + "&region=" + regions + "&locationId=" + selectedLocationId,
        success: function (data, result) {
            if (!result) alert('Failure to retrieve the Sites.');
            //var jsonObject = eval('(' + data + ')');
            $("#DisplaySites")[0].addJSONData(data);
        }
    });
}

function SelectNone() {
    $.ajax({
        url: rootPath + "Site.aspx/SelectNone/?PlanDetailsId=" + PlanDetailsId + "&region=" + regions + "&locationId=" + selectedLocationId,
        success: function (data, result) {
            if (!result) alert('Failure to retrieve the Sites.');
            //var jsonObject = eval('(' + data + ')');
            $("#DisplaySites")[0].addJSONData(data);
        }
    });
}

function UpdateDisplayInfo() {
    var vendorName = $("#DisplayVendorName").val();
    var vendorId = $("#DisplayVendorId").val();
    var displayRate = $("#DisplayRate").val();
    var clientRate = $("#ClientRate").val();
    var fromdate = $("#DisplayFromDate").val();
    var todate = $("#DisplayToDate").val();
    $.ajax({
        url: rootPath + "Site.aspx/UpdateDisplayInfo/?PlanDetailsId=" + PlanDetailsId + "&region=" + regions + "&locationId=" + selectedLocationId + "&VendorName=" + vendorName + "&VendorId=" + vendorId + "&DisplayRate=" + displayRate + "&fromDate=" + fromdate + "&toDate=" + todate + "&ClientRate=" + clientRate,
        success: function (data, result) {
            if (!result) alert('Failure to retrieve the Sites.');
            //var jsonObject = eval('(' + data + ')');
            $("#DisplaySites")[0].addJSONData(data);
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


    $("#DisplayVendorName").autocomplete(rootPath + 'Shared.aspx/FindVendor/', {
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
        $("#DisplayVendorId").val(row.Key);    
        
    });
    //For passing id of the selected row.
    myextraparam = { selectedLocationId: "NotAssigned" };
    myediturl = '/Site.aspx/UpdateDisplayInfoForSite/';
    var lastsel2;
    $("#DisplaySites").jqGrid({
        url: rootPath + 'Site.aspx/GetSitesForDisplay/?PlanDetailsId=' + PlanDetailsId + '&region=' + selectedRegion + '&locationId=' + selectedLocationId,
        datatype: 'json',
        colNames: ["S", "Select", 'SitesId', 'Site#', 'Site Name', 'SqFt', 'DisplayVendor', 'Vendor', 'Display Rate', 'Display Cost', 'Display From Date', 'Display To Date', 'Client Rate', 'Client Cost', 'Address line 1', 'Address line 2', 'Address line 3'],
        colModel: [
                                  { name: 'act', index: 'act', width: '22', sortable: false },
                                  { name: 'IsSelected', index: 'IsSelected', width: '75', edittype: 'checkbox', sortable: true, editable: true },                                  
                                  { name: 'planSiteId', index: 'PlanSiteId', width: '100', align: 'left', /* key: true,*/editable: true, editrules: { edithidden: false }, hidedlg: true, hidden: true },
                                  { name: 'SiteNo', index: 'SiteNo', width: '80', align: 'left', editable: false, edittype: 'text', editrules: { required: true} },
                                  { name: 'siteName', index: 'Site Name', width: '100', align: 'left', editable: false, edittype: 'text', editrules: { required: true }, formoptions: { elmsuffix: ' *' }, editrules: { required: true} },
                                  { name: 'SiteSizeInSqFt', index: 'SiteSizeInSqFt', width: '100', align: 'left', editable: false, edittype: 'text', editrules: { required: true }, formoptions: { elmsuffix: ' *' }, editrules: { required: true} },
                                  { name: 'DisplayVendor', index: 'DisplayVendor', width: '100', align: 'left', /* key: true,*/editable: true, editrules: { edithidden: false }, hidedlg: true, hidden: true },
                                  { name: 'DisplayVendorName', index: 'DisplayVendorName', width: '200', align: 'left', editable: true, edittype: 'text',
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
                                                  var ret = jQuery("#DisplaySites").jqGrid('getRowData', 1);
                                                  selectedLocationVendorvar = row.Key;
                                                  myextraparam = { selectedLocationVendor: row.Key };
                                              });
                                          }, 100);
                                      }
                                      }, editrules: { required: true }
                                  },
                                { name: 'DisplayRate', index: 'DisplayRate', width: '150', align: 'left', editable: true, edittype: 'text', editrules: { required: false} },
                                { name: 'DisplayCost', index: 'DisplayCost', width: '150', align: 'left', editable: true, edittype: 'text', editrules: { required: false} },
                                { name: 'DisplayFromDate', index: 'DisplayFromDate', width: '150', align: 'left', editable: true, edittype: 'text', editrules: { required: false} },
                                { name: 'DisplayToDate', index: 'DisplayToDate', width: '150', align: 'left', editable: true, edittype: 'text', editrules: { required: false} },
                                { name: 'ClientDisplayRate', index: 'ClientDisplayRate', width: '150', align: 'left', editable: true, edittype: 'text', editrules: { required: false} },
                                { name: 'ClientDisplayCost', index: 'ClientDisplayCost', width: '150', align: 'left', editable: true, edittype: 'text', editrules: { required: false} },
                                { name: 'addressline1', index: 'addressline1', width: '150', align: 'left', editable: false, edittype: 'text', editrules: { required: false} },
                                { name: 'addressline2', index: 'addressline2', width: '150', align: 'left', editable: false, edittype: 'text', editrules: { required: false} },
                                { name: 'addressline3', index: 'addressline3', width: '150', align: 'left', editable: false, edittype: 'text', editrules: { required: false} }
                                ],
        onSelectRow: function (id) {
            if (id && id !== lastsel2) {
                jQuery('#DisplaySites').jqGrid('saveRow', lastsel2);
                jQuery('#DisplaySites').jqGrid('editRow', id, true);
                lastsel2 = id;
            }
        },
        pager: $('#listPager'),
        rowNum: 100,
        autowidth: 'true',
       
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
        editurl: rootPath + 'Site.aspx/UpdateDisplayInfoForSite/',
        gridComplete: function () {
            var ids = jQuery("#DisplaySites").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];                
                se = "<input style='height:22px;width:22px;' type='button' value='S' onclick=\"$('#DisplaySites').saveRow('" + cl + "', false, myediturl, myextraparam);\" />";
                jQuery("#DisplaySites").jqGrid('setRowData', ids[i], { act: se });
            }
        }
    });

    $("#DisplaySites").jqGrid('navGrid', '#listPager', { edit: true, add: true, del: true, refresh: false },
            { onclickSubmit: function (params) { alert('updating'); } }, null, null);
    
}); 
         