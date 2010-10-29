
var PlanDetailsId;
var selectedActivity ='Display';


function GetSitesForSelectedCity() {
    var region = $("#Region").val();
    var selectedCityId = $("#CityId").val();

    var activity = selectedActivity;
    $.ajax({
        url: rootPath + "SiteMisc.aspx/GetSites/?sidx=1&sord=1&page=0&rows=10&PlanDetailsId=" + PlanDetailsId + "&region=" + region + "&CityId=" + selectedCityId + '&activity=' + activity,
        success: function (data, result) {
            if (!result) alert('Failure to retrieve the Sites.');
            //var jsonObject = eval('(' + data + ')');
            $("#SiteList")[0].addJSONData(data);
        },
        error: function (data, result) {
            alert('Failed to retrive sites');
        }
    });
}

function SelectSpecific(siteId, value) {    
    url1 = rootPath + 'SiteMisc.aspx/SelectSpecificSite/?siteId=' + siteId + "&isSelected=" + value;
    $.ajax({
        url : url1,
        success: function (data, result) {
            if (!result) alert('Failure to retrieve the Sites.');          
        },
        error: function(date, result)
        {
            alert("Failed to select the site");
        }
    });
}
function SelectAll() {
    var activity = selectedActivity;
    var region = $("#Region").val();
    var selectedCityId = $("#CityId").val();
    $.ajax({
        url: rootPath + "SiteMisc.aspx/SelectAllSites/?PlanDetailsId=" + PlanDetailsId + "&region=" + region + "&CityId=" + selectedCityId + "&activity=" + activity,
        success: function (data, result) {
            if (!result) alert('Failure to select all the Sites.');
            //var jsonObject = eval('(' + data + ')');
            $("#SiteList")[0].addJSONData(data);
        },
        error: function (data, result) {
            alert('Failed to select all sites');
        }
    });
}

function SelectNone() {
    var activity = selectedActivity;
    var region = $("#Region").val();
    var selectedCityId = $("#CityId").val();
    $.ajax({
        url: rootPath + "SiteMisc.aspx/SelectNone/?PlanDetailsId=" + PlanDetailsId + "&region=" + region + "&CityId=" + selectedCityId + "&activity=" + activity,
        success: function (data, result) {
            if (!result) alert('Failure to retrieve the Sites.');
            //var jsonObject = eval('(' + data + ')');
            $("#SiteList")[0].addJSONData(data);
        },
        error: function (data, result) {
            alert('Failed to deselect site');
        }
    });
}

function AssignVendorInfo() {
    var selectedRegion = $("#Region").val();
    var selectedCityId = $("#CityId").val();
    var vendorName = $("#VendorName").val();
    var vendorId = $("#VendorId").val();

    var clientRate = $("#ClientRate").val();
    if (clientRate == "") {
        clientRate = 0;
    }
    var fromdate = $("#StartDate").val();
    var todate = $("#EndDate").val();
    var activity = selectedActivity;
    var status = $("#Status").val();
    var chargingBasis = $("#ChargingBasis").val();
    var Rate = $("#Rate").val();    
    if (Rate == "")
        Rate = 0;
    $.ajax({
        url: rootPath + "SiteMisc.aspx/AssignVendorInfo/?PlanDetailsId=" + PlanDetailsId + "&activity=" + selectedActivity + "&chargeBasis=" + chargingBasis + "&status=" + status + "&region=" + selectedRegion + "&cityId=" + selectedCityId + "&VendorName=" + vendorName + "&VendorId=" + vendorId + "&Rate=" + Rate + "&fromDate=" + fromdate + "&toDate=" + todate + "&clientRate=" + clientRate,
        success: function (data, result) {
                if (!result) alert('Failure to retrieve the Sites.');
                //var jsonObject = eval('(' + data + ')');
                $("#SiteList")[0].addJSONData(data);


//                $("#Region").val("");
//                $("#CityId").val("");
                $("#VendorName").val("");
                $("#VendorId").val("");

                $("#ClientRate").val("");
                $("#StartDate").val("");
                $("#EndDate").val("");
                $("#Status").val("");
                $("#ChargingBasis").val("");
                $("#Rate").val("");
            },
            error: function (data, result) {
                alert('Failed to Assign Info to site');
            }    
    });
}

function GetsitesByCityOrRegion() {
    var activity = selectedActivity;
    var region = $("#Region").val();
    var selectedCityId = $("#CityName").val().split(';')[0];
    $.ajax({
        url: rootPath + 'SiteMisc.aspx/GetSites/?PlanDetailsId=' + PlanDetailsId + '&region=' + region + '&CityId=' + selectedCityId + "&activity=" + activity,
        success: function (data, result) {
            if (!result) alert('Failure to retrieve the Sites.');
            //var jsonObject = eval('(' + data + ')');
            $("#SiteList")[0].addJSONData(data);

        },
        error: function (data, result) {
            alert('Failed to retrive sites');
        } 
    });
}
function CalculateCosts() {
    var activity = selectedActivity;
    $.ajax({
        url: rootPath + 'SiteMisc.aspx/CalculateCosts/?activity=' + activity,
        success: function (data, result) {
            if (!result) alert('Failure to retrieve the Sites.');
            var costValues = data.split(";");

            $("#TotalDisplayCost").text(costValues[0]);
            $("#TotalMountingCost").text(costValues[1]);
            $("#TotalPrintingCost").text(costValues[2]);
            $("#TotalFabricationCost").text(costValues[3]);

            $("#TotalDisplayPending").text(costValues[4]);
            $("#TotalDisplayBooked").text(costValues[5]);
            $("#TotalDisplayProposed").text(costValues[6]);
            $("#TotalDisplayUN").text(costValues[7]);

            $("#TotalMountingPending").text(costValues[8]);
            $("#TotalMountingBooked").text(costValues[9]);
            $("#TotalMountingProposed").text(costValues[10]);
            $("#TotalMountingUN").text(costValues[11]);

            $("#TotalPrintingPending").text(costValues[12]);
            $("#TotalPrintingBooked").text(costValues[13]);
            $("#TotalPrintingProposed").text(costValues[14]);
            $("#TotalPrintingUN").text(costValues[15]);

            $("#TotalFabricationPending").text(costValues[16]);
            $("#TotalFabricationBooked").text(costValues[17]);
            $("#TotalFabricationProposed").text(costValues[18]);
            $("#TotalFabricationUN").text(costValues[19]);

            $("#TotalDisplayClientCost").text(costValues[20]);
            $("#TotalMountingClientCost").text(costValues[21]);
            $("#TotalPrintingClientCost").text(costValues[22]);
            $("#TotalFabricationClientCost").text(costValues[23]);

            $("#TotalSiteCount").text(costValues[24]);

            $("#DisplayCostDiff").text($("#TotalDisplayClientCost").text() - $("#TotalDisplayCost").text());
            $("#MountingCostDiff").text($("#TotalMountingClientCost").text() - $("#TotalMountingCost").text());
            $("#PrintingCostDiff").text($("#TotalPrintingClientCost").text() - $("#TotalPrintingCost").text());
            $("#FabricationCostDiff").text($("#TotalFabricationClientCost").text() - $("#TotalFabricationCost").text());

            $("#TotalCost").text(eval($("#TotalDisplayCost").text()) + eval($("#TotalMountingCost").text()) + eval($("#TotalPrintingCost").text()) + eval($("#TotalFabricationCost").text()));
            $("#TotalClientCost").text(eval($("#TotalDisplayClientCost").text()) + eval($("#TotalMountingClientCost").text()) + eval($("#TotalPrintingClientCost").text()) + eval($("#TotalFabricationClientCost").text()));
            $("#TotalDiffInCost").text(eval($("#TotalClientCost").text()) - eval($("#TotalCost").text()));
        },
        error: function (data, result) {
            alert('Failed to calculate costs');
        }
    });
}
function pickValues(id) {
    jQuery("#" + id + "_StartDate").datepicker({ dateFormat: "yy-mm-dd" });
    jQuery("#" + id + "_EndDate").datepicker({ dateFormat: "yy-mm-dd" });
    if ($("#SiteList img").length <= 0) {        
        $("#SiteList tr:nth-child("+ id +") td:nth-child(2) input").css("visibility", "visible")        
    }
}

function hideIcon(id) {
    if ($("#SiteList img").length <= 0) {
        $("#SiteList tr:nth-child(" + id + ") td:nth-child(2) input").css("visibility", "hidden")
    }
}

function SubmitVendorInfo() {
    LoadSpinnerImg('fogLoaderimg');
    $.ajax({
        url: rootPath + 'SiteMisc.aspx/SaveVendorInfo/?planDetailsId=' + PlanDetailsId,
        success: function (data, result) {
            UnLoadSpinnerImg('fogLoaderimg');
            if (!result) alert('Failed to save.');
            else alert('Saved Successfully');
        },
        error: function (data, result) {
            UnLoadSpinnerImg('fogLoaderimg');
            alert('Failed to save');
        }
    });
}

$(document).ready(function () {
    PlanDetailsId = $("#PlanDetailsId").val();
    selectedRegion = $("#PlanRegionInHeader").val();
    selectedActivity = 'Display';
    $('a[href=#box-vendorInfo]').click(function () {
        selectedActivity = this.text;
        $("#VendorName").val("");
        $("#VendorId").val("");
        $("#Rate").val("");
        $("#ClientRate").val("");
        $("#ChargingBasis").val("");
        $("#Status").val("");
        $("#StartDate").val("");
        $("#EndDate").val("");
        selectedActivity = this.text; 
        if (selectedActivity == 'Display') {
            $("#StartDate").css("visibility", "visible");
            $("#EndDate").css("visibility", "visible");
            $("#lblStartDate").css("visibility", "visible");
            $("#lblEndDate").css("visibility", "visible");
        }
        else {
            $("#StartDate").css("visibility", "hidden");
            $("#EndDate").css("visibility", "hidden");
            $("#lblStartDate").css("visibility", "hidden");
            $("#lblEndDate").css("visibility", "hidden");
        }
        myextraparam = { selectedCityId: "NotAssigned", activity: selectedActivity };
        GetsitesByCityOrRegion();
        $("#jqgh_VendorName").html(selectedActivity + " Vendor");
        return false;
    });

    $("#CityName").autocomplete(rootPath + 'Shared.aspx/FindLocationFilterdWithRegion/?region=' + selectedRegion, {
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
        $("#CityId").val(row.Key);
        selectedCityId = row.Key.split(';')[0];
        GetSitesForSelectedCity();
    });

    $("#VendorName").autocomplete(rootPath + 'Shared.aspx/FindVendor/', {
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
        $("#VendorId").val(row.Key);
    });
    $("#CityName").change(function () {
        selectedCityId = $("#CityName").val().split(';')[0];
        $("#CityId").val(selectedCityId);
        $("#AddNewSite").attr("href", rootPath + "Site.aspx/AddNewSite/?planDetailsId=" + PlanDetailsId + "&region=" + $("#Region").val() + "&locationId=" + selectedCityId);

    });
    $("#Region").change(function () {
        var region = $("#Region").val();
        var locations = $("#CityName")[0];

        $("#CityName").val("");
        $("#CityId").val("");
        for (var i = 0; i < locations.length; i++) {
            if (locations[i].value.split(";")[1] != region) {
                locations[i].disabled = true;
            }
            else {
                locations[i].disabled = false;
            }
        }
        $("#AddNewSite").attr("href", rootPath + "Site.aspx/AddNewSite/?planDetailsId=" + PlanDetailsId + "&region=" + $("#Region").val() + "&locationId=");
    });

    myextraparam = { selectedCityId: "NotAssigned", activity: selectedActivity };



    myediturl = rootPath + 'SiteMisc.aspx/UpdateSiteInfoOnEdit/';
    var lastsel2;
    $("#SiteList").jqGrid({
        url: rootPath + 'SiteMisc.aspx/GetSites/?PlanDetailsId=' + PlanDetailsId + '&region=&CityId=&activity=' + selectedActivity,
        datatype: 'json',
        colNames: ["Mark/UnMark", "", 'SitesId', 'Region', 'City', 'Media Type', 'Site', 'Illumination', 'H', 'V', 'Size Ratio', 'Days', 'Qty', 'Sq. Ft', 'VendorId', 'Display Vendor', 'Rate', 'Cost', 'Client Rate', 'Client Cost', 'Start Date', 'End Date', 'Status'],
        colModel: [
                                  { name: 'IsSelected', index: 'IsSelected', width: '20', edittype: 'checkbox', formatter: "checkbox", editoptions: { value: "True:False" }, formatoptions: { disabled: false }, sortable: true, editable: true },
                                  { name: 'act', index: 'act', width: '22', sortable: false },
                                  { name: 'planSiteId', index: 'PlanSiteId', width: '100', align: 'left', /* key: true,*/editable: true, hidedlg: true, hidden: true },
                                  { name: 'Region', index: 'Region', width: '50', align: 'left', editable: false, edittype: 'text' },
                                  { name: 'City', index: 'City', width: '80', align: 'left', editable: false, edittype: 'text' },
                                  { name: 'MediaType', index: 'MediaType', width: '80', align: 'left', editable: false, edittype: 'text' },
                                  { name: 'siteName', index: 'Site Name', width: '200', align: 'left', editable: false, edittype: 'text' },
                                  { name: 'Illumination', index: 'Illumination', width: '80', align: 'left', editable: false, edittype: 'text' },
                                  { name: 'H', index: 'H', width: '30', align: 'left', editable: false, edittype: 'text' },
                                  { name: 'V', index: 'V', width: '30', align: 'left', editable: false, edittype: 'text' },
                                  { name: 'SizeRatio', index: 'SizeRatio', width: '62', align: 'left', editable: false, edittype: 'text' },
                                  { name: 'Days', index: 'SizeRatio', width: '30', align: 'left', editable: false, edittype: 'text' },
                                  { name: 'Qty', index: 'SizeRatio', width: '30', align: 'left', editable: false, edittype: 'text' },
                                  { name: 'SiteSizeInSqFt', index: 'SiteSizeInSqFt', width: '50', align: 'left', editable: false, edittype: 'text' },
                                  { name: 'VendorId', index: 'VendorId', width: '100', align: 'left', hidden: true, editrules: { required: false }, editable: true },
                                  { name: 'VendorName', index: 'VendorName', width: '200', align: 'left', editable: true, edittype: 'text',
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
                                                  var ret = jQuery("#SiteList").jqGrid('getRowData', 1);
                                                  selectedVendorvar = row.Key;
                                                  myextraparam = { selectedVendor: row.Key, activity: selectedActivity };
                                              });
                                          }, 100);
                                      }
                                      }, editrules: { required: false }
                                  },
                                { name: 'Rate', index: 'Rate', width: '80', align: 'left', editable: true, edittype: 'text', editrules: { required: false} },
                                { name: 'Cost', index: 'Cost', width: '80', align: 'left', editable: true, edittype: 'text', editrules: { required: false} },
                                { name: 'ClientRate', index: 'Cost', width: '80', align: 'left', editable: true, edittype: 'text', editrules: { required: false} },
                                { name: 'ClientCost', index: 'Cost', width: '80', align: 'left', editable: true, edittype: 'text', editrules: { required: false} },
                                { name: 'StartDate', index: 'StartDate', width: '100', align: 'left', editable: true },
                                  { name: 'EndDate', index: 'EndDate', width: '100', align: 'left', editable: true },

                                  { name: 'Status', index: 'Status', width: '100', align: 'left', edittype: 'select', editable: true, editoptions: { value: "Proposed:Proposed;Under Negotiation:Under Negotiation;Booked:Booked"} },
                                ],
        onSelectRow: function (id) {
            if (id && id !== lastsel2) {
                jQuery('#SiteList').jqGrid('saveRow', lastsel2, true, hideIcon(lastsel2));
                jQuery('#SiteList').jqGrid('editRow', id, true, pickValues(id));
                lastsel2 = id;
            }
        },
        pager: $('#listPager'),
        rowNum: 100,
        autowidth: 'true',
        height: '450px',
        rowList: [5, 10, 20, 50],
        sortname: 'SiteName',
        sortorder: "asc",
        viewrecords: true,
        prmNames: {
            PlanDetailsId: "ab"
        },
        afterShowForm: function (formid) { alert(formid); },
        imgpath: '/scripts/themes/steel/images',
        caption: '',
        editurl: rootPath + 'SiteMisc.aspx/UpdateSiteInfoOnEdit/',
        gridComplete: function () {
            var ids = jQuery("#SiteList").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                se = "<input style='visibility:hidden' type='image' src= '" + rootPath + "Resources/Images/save_icon.gif' onclick=\"$('#SiteList').saveRow('" + cl + "', hideIcon('" + cl + "'), myediturl, myextraparam);\" />";

                jQuery("#SiteList").jqGrid('setRowData', ids[i], { act: se });
            }
            $('input:checkbox').click(function () { SelectSpecific($(this).parent().next().next().attr("title"), $(this).attr('checked')); });
            CalculateCosts();
        }
    });

    $("#SiteList").jqGrid('navGrid', '#listPager', { edit: true, add: true, del: true, refresh: false },
            { onclickSubmit: function (params) { alert('updating'); } }, null, null);

}); 
         
         

         