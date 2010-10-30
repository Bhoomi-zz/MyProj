
var PlanDetailsId;
var selectedRegion;
var selectedLocationId = "NotAssigned";
var selectedLocationVendorvar = "";
var regions;
var deletedRowId;


function AddSite() {
    jQuery("#Sites").jqGrid('editGridRow', "new",
                { height: "800px", width: "500px", reloadAfterSubmit: false,
                    afterShowForm: function (form) {
                        $("#StartDate").datepicker({ dateFormat: "yy-mm-dd" });
                        $("#EndDate").datepicker({ dateFormat: "yy-mm-dd" });
                        $("#siteName").css("width", "300px");
                        $("#DisplayVendorName").css("width", "300px");
                        $("#addressline1").css("width", "300px");
                        $("#addressline2").css("width", "300px");
                        $("#addressline3").css("width", "300px");
                        $("#Days").format({ precision: 0, autofix: true });
                        $("#Qty").format({ precision: 0, autofix: true });
                        $("#H").format({ precision: 0, autofix: true });
                        $("#V").format({ precision: 0, autofix: true });
                        $("#NoOfFaces").format({ precision: 0, autofix: true });
                        $("#SiteSizeInSqFt").attr("readonly", "true");
                        if ($("#TblGrid_Sites img").length <= 0) {
                            $("<img src='" + rootPath + "Resources/Images/icons/autocomplete.gif />").insertAfter("#DisplayVendorName");
                            $("<img src='" + rootPath + "Resources/Images/icons/autocomplete.gif />").insertAfter("#siteName");
                            $("<img src='" + rootPath + "Resources/Images/icons/autocomplete.gif />").insertAfter("#SiteNo");
                        }

                        //                        $("#DisplayVendorName").change(function (data) {
                        //                            if (selectedLocationVendorvar.length < 36) {
                        //                                alert('Please select a vendor from the list');
                        //                            }
                        //                        });
                    },

                    onclickSubmit: function (params) {
                        IncreaseOrDecreaseCityCount("increase");
                        var siteid = GenerateGUID();
                        $("#planSiteId").val(siteid);
                        return { planSiteId: siteid, PlanDetailsId: $("#PlanDetailsId").val(),
                            VendorName: $("#DisplayVendor :selected").text(),
                            selectedLocationVendor: selectedLocationVendorvar
                        };
                    }
                }
           );
  }

  function IncreaseOrDecreaseCityCount(action) {
      var locationName = $("#LocationName :selected").text();
      splitText1 = locationName.split("(");
      if (splitText1 != null && splitText1.length > 1) {
          splitText2 = splitText1[1].split(")");
          if (splitText2 != null && splitText2.length > 1) {
              no = splitText2[0];
              if (no == "")
                  no = 0;
              if (action == "increase") {
                  no = eval(no) + 1;
              }
              if (action == "decrease") {
                  no = eval(no) - 1;
              }
              locationName = splitText1[0] + "(" + no + ")";
              $("#LocationName :selected").text(locationName);
          }
      }
  }

function pickdates(id) {
    jQuery("#" + id + "_StartDate").datepicker({ dateFormat: "yy-mm-dd" });
    jQuery("#" + id + "_EndDate").datepicker({ dateFormat: "yy-mm-dd" });
    $("#" + id + "_siteName").css("width", "300px");
    $("#" + id + "_DisplayVendorName").css("width", "300px");
    $("#" + id + "_addressline1").css("width", "300px");
    $("#" + id + "_addressline2").css("width", "300px");
    $("#" + id + "_addressline3").css("width", "300px");
    $("#" + id + "_Days").format({ precision: 0, autofix: true });
    $("#" + id + "_Qty").format({ precision: 0, autofix: true });
    $("#" + id + "_H").format({ precision: 0, autofix: true });
    $("#" + id + "_V").format({ precision: 0, autofix: true });
    $("#" + id + "_NoOfFaces").format({ precision: 0, autofix: true });
    if ($("#Sites img").length <= 0) {
        $("<img src='" + rootPath + "Resources/Images/icons/autocomplete.gif />").insertBefore("#" + id + "_DisplayVendorName");
        $("<img src='" + rootPath + "Resources/Images/icons/autocomplete.gif />").insertBefore("#" + id + "_siteName");
        $("<img src='" + rootPath + "Resources/Images/icons/autocomplete.gif />").insertBefore("#" + id + "_SiteNo");
    }
}
function SaveForm() {
    planCityId = $("#PlanCityId").val();
    LoadSpinnerImg('fogLoaderimg');
    $.ajax({
        url: rootPath + "Site.aspx/SaveForm/?planDetailsId=" + PlanDetailsId + "&planCityId=" + planCityId,
        success: function (data, result) {
            if (!result) alert('Failed to Save.');            
            UnLoadSpinnerImg('fogLoaderimg');
            alert('Saved Successfully');
        },
        error: function (data, result) {
            UnLoadSpinnerImg('fogLoaderimg');
            alert('Error while saving..');
        }
    }); 
}
function GetAdditionalInfoForLocation()
{
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
function GetSitesForSelectedCity()
{
    var region = $("#Region").val();
                selectedRegion = region;
                selectedLocationId = $("#LocationId").val();                
                $.ajax({
                    url: rootPath + "Site.aspx/GetSites/?sidx=1&sord=1&page=0&rows=10&PlanDetailsId=" + PlanDetailsId + "&region=" + region + "&locationId=" + selectedLocationId,
                    success: function (data, result) {
                        if (!result) alert('Failure to retrieve the Sites.');
                        //var jsonObject = eval('(' + data + ')');
                        $("#Sites")[0].addJSONData(data);
                    }
                });
}

function GetSiteDetailsByName(selectedSiteName) {
    var arr = selectedSiteName.split("_");
    var rowid;
    if (arr.length > 1) {
        var rowid = arr[0];
    }
    var enteredValue = null;
    if ($("#" + rowid + "_siteName") != null)
            enteredValue = $("#" + rowid + "_siteName").val();
    if (enteredValue == null)
        enteredValue = $("#siteName").val();
    SetValues(rowid, enteredValue, "SiteName");
}
function SetValues(rowid, enteredValue, forWhat) {
    $.ajax({
        url: rootPath + "Site.aspx/GetSiteDetails/?SiteNameOrNo=" + enteredValue + "&type=" + forWhat,
        success: function (data, result) {
            if (!result) alert('Failure to retrieve the Sites.');
            var siteValues = data.split(";");
            var IsEdit = $("#" + rowid + "_siteName").val();
            if (IsEdit != null) {
                $("#" + rowid + "_SiteNo").val(siteValues[0]);
                $("#" + rowid + "_siteName").val(siteValues[1]);
                $("#" + rowid + "_DisplayVendor").val(siteValues[2]);
                $("#" + rowid + "_DisplayVendorName").val(siteValues[3]);
                $("#" + rowid + "_addressline1").val(siteValues[4]);
                $("#" + rowid + "_addressline2").val(siteValues[5]);
                $("#" + rowid + "_addressline3").val(siteValues[6]);
                $("#" + rowid + "_siteSize").val(siteValues[7]);
                $("#" + rowid + "_illumination").val(siteValues[8]);
                $("#" + rowid + "_H").val(siteValues[9]);
                $("#" + rowid + "_V").val(siteValues[10]);
                $("#" + rowid + "_SizeRatio").val(siteValues[11]);
                $("#" + rowid + "_MediaType").val(siteValues[12]);
                $("#" + rowid + "_NoOfFaces").val(siteValues[13]);
            }
            else {
                $("#SiteNo").val(siteValues[0]);
                $("#siteName").val(siteValues[1]);
                $("#DisplayVendor").val(siteValues[2]);
                $("#DisplayVendorName").val(siteValues[3]);
                $("#addressline1").val(siteValues[4]);
                $("#addressline2").val(siteValues[5]);
                $("#addressline3").val(siteValues[6]);
                $("#siteSize").val(siteValues[7]);
                $("#illumination").val(siteValues[8]);
                $("#H").val(siteValues[9]);
                $("#V").val(siteValues[10]);
                $("#SizeRatio").val(siteValues[11]);
                $("#MediaType").val(siteValues[12]);
                $("#NoOfFaces").val(siteValues[13]);
            }
        }
    });
}
function GetSiteDetailsByNo(selectedid) {
    var arr = selectedid.split("_");
    var rowid = arr[0];
    var enteredValue = $("#" + rowid + "_SiteNo").val();
    if (enteredValue == null)
        enteredValue = $("#SiteNo").val();
    SetValues(rowid, enteredValue, "SiteNo");
}
function DeleteGridRow(selectedId, plansiteid)
{
    toDelete = selectedId;
    deletedRowId = plansiteid
    var id = jQuery("#Sites").jqGrid('getRowData', selectedId)["planSiteId"];

        // You'll get a pop-up confirmation dialog, and if you say yes,
    // it will call "delete.php" on your server.    
        $("#Sites").jqGrid(
        'delGridRow',
        toDelete,
          { url: rootPath + 'Site.aspx/DeleteSite/',
              reloadAfterSubmit: false,
              onclickSubmit: function (params) {
                  IncreaseOrDecreaseCityCount("decrease");
                  return { planSiteId: deletedRowId };                  
              }
          }
        );
}

$(document).ready(function () {


    PlanDetailsId = $("#PlanDetailsId").val();
    selectedRegion = $("#PlanRegionInHeader").val();
    selectedRegion = $("#Region").val();
    regions = $("#RegionString").val();
    selectedLocationId = $("#LocationName").val();
    var planners = $.ajax({ url: rootPath + 'Shared.aspx/GetAllUsers/',
        async: false,
        success: function (data, result) {
            if (!result) alert('Failure to retrieve the Planners.');
        }
    }).responseText;


    //    $("#LocationName").autocomplete('/Shared/FindLocationFilterdWithRegion/?region=' + selectedRegion, {
    //        dataType: "json",
    //        multiple: false,
    //        formatItem: function (item, index, total, query) {
    //            return item.value;
    //        },
    //        parse: function (data) {
    //            return $.map(data, function (item) {
    //                return {
    //                    data: item,
    //                    value: item.Key,
    //                    result: item.value
    //                };
    //            });
    //        }
    //    }).result(function (event, row) {
    //        $("#LocationId").val(row.Key);
    //        GetAdditionalInfoForLocation();
    //        GetSitesForSelectedCity();
    //    });

    $("#LocationName").change(function () {
        var locationId = $("#LocationName").val().split(";")[0];
        $("#LocationId").val(locationId);
        GetAdditionalInfoForLocation();
        GetSitesForSelectedCity();
    })

    

    var updateDialog = {
        url: '<%= Url.Action("UpdateSite", "Site") %>'
                , closeAfterAdd: true
                , closeAfterEdit: true
                , recreateForm: true
                , modal: false,
        afterShowForm: function (form) { form.css("height", "800px"); },
        height: '800px'
    };

    //For passing id of the selected row.
    myextraparam = { selectedLocationId: "NotAssigned" };
    myediturl = rootPath + 'Site.aspx/UpdateSite/';

    $("#Sites").jqGrid({
        url: rootPath + 'Site.aspx/GetSites/?PlanDetailsId=' + PlanDetailsId + '&region=' + selectedRegion + '&locationId=' + selectedLocationId,
        datatype: 'json',
        colNames: ["Actions", 'SitesId', 'Site#', 'Site Name', 'DisplayVendor', 'Vendor', 'Address line 1', 'Address line 2', 'Address line 3', 'Size', 'Illumination', 'H', 'V', 'Site Size In SqFt', 'Size Ratio', 'Media Type', 'No Of Faces', 'Start Date', 'End Date', 'Days', 'Qty'],
        colModel: [
                                  { name: 'act', index: 'act', width: 75, sortable: false },
                                  { name: 'planSiteId', index: 'PlanSiteId', width: 100, align: 'left', /* key: true,*/editable: true, editrules: { edithidden: false }, hidedlg: true, hidden: true },
                                  { name: 'SiteNo', index: 'SiteNo', width: 80, align: 'left', editable: true, edittype: 'text',
                                      editoptions: { dataInit: function (elem) {
                                          setTimeout(function () {
                                              $(elem).autocomplete(rootPath + 'Shared.aspx/FindSites/', {
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
                                                  var ret = jQuery("#Sites").jqGrid('getRowData', 1);
                                                  GetSiteDetailsByNo(this.id);
                                                  myextraparam = { selectedLocationVendor: row.Key };
                                              });
                                          }, 100);
                                      }
                                      }, editrules: { required: false }
                                  },
                                  { name: 'siteName', index: 'Site Name', width: 200, align: 'left', editable: true, edittype: 'text', editrules: { required: true }, formoptions: { elmsuffix: ' *' },
                                      editoptions: { dataInit: function (elem) {
                                          setTimeout(function () {
                                              $(elem).autocomplete(rootPath + 'Shared.aspx/FindSitesByName/', {
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
                                                  //var ret = jQuery("#Sites").jqGrid('getRowData', 1);
                                                  GetSiteDetailsByName(event.currentTarget.id);
                                                  myextraparam = { selectedLocationVendor: row.Key };
                                              });
                                          }, 100);
                                      }
                                      }, editrules: { required: true }
                                  },
                                  { name: 'DisplayVendor', index: 'DisplayVendor', width: 100, align: 'left', /* key: true,*/editable: true, editrules: { edithidden: false }, hidedlg: true, hidden: true },
                                  { name: 'DisplayVendorName', index: 'DisplayVendorName', width: 200, align: 'left', editable: true, edittype: 'text',
                                      editoptions: { dataInit: function (elem) {
                                          setTimeout(function () {
                                              $(elem).autocomplete(rootPath + 'Shared.aspx/FindVendor/', {
                                                  dataType: "json",
                                                  multiple: false,
                                                  mustMatch: true,
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
                                                  var ret = jQuery("#Sites").jqGrid('getRowData', 1);
                                                  selectedLocationVendorvar = row.Key;
                                                  myextraparam = { selectedLocationVendor: row.Key };
                                              });

                                          }, 0);
                                      }
                                      }, editrules: { required: true }
                                  },
                                  { name: 'addressline1', index: 'addressline1', width: 150, align: 'left', editable: true, edittype: 'text', editrules: { required: false} },
                                  { name: 'addressline2', index: 'addressline2', width: 150, align: 'left', editable: true, edittype: 'text', editrules: { required: false} },
                                  { name: 'addressline3', index: 'addressline3', width: 150, align: 'left', editable: true, edittype: 'text', editrules: { required: false} },
                                  { name: 'siteSize', index: 'siteSize', width: 50, align: 'left', editable: true, edittype: 'text', editrules: { required: false} },
                                  { name: 'illumination', index: 'illumination', width: 80, align: 'left', editable: true, edittype: 'text', editrules: { required: false} },
                                  { name: 'H', index: 'H', width: 50, align: 'left', editable: true, edittype: 'text', editrules: { required: false} },
                                  { name: 'V', index: 'V', width: 50, align: 'left', editable: true, edittype: 'text', editrules: { required: false} },
                                  { name: 'SiteSizeInSqFt', index: 'SiteSizeInSqFt', width: 50, align: 'left', editable: true, edittype: 'text', editrules: { required: false} },
                                  { name: 'SizeRatio', index: 'SizeRatio', width: 50, align: 'left', editable: true, edittype: 'text', formoptions: { elmsuffix: ' *'} },
                                  { name: 'MediaType', index: 'MediaType', width: 80, align: 'left', editable: true, edittype: 'select', editoptions: { value: "BillBoard:BillBoard;Buses:Buses;BusQShelter:BusQShelter;PoleKiosks:Pole Kiosks;TransitMedia:Transit Media;PublicUtilities:Public Utilities;Others:Others," }, formoptions: { elmsuffix: ' *'} },
                                  { name: 'NoOfFaces', index: 'NoOfFaces', width: 50, align: 'left', editable: true, edittype: 'text', formoptions: { elmsuffix: ' *'} },
                                  { name: 'StartDate', index: 'StartDate', width: 80, align: 'left', editable: true, edittype: 'text', editrules: { required: false} },
                                  { name: 'EndDate', index: 'EndDate', width: 80, align: 'left', editable: true, edittype: 'text', editrules: { required: false} },
                                  { name: 'Days', index: 'Days', width: 50, align: 'left', editable: true, edittype: 'text', formoptions: { elmsuffix: ' *' }, editrules: { required: true} },
                                  { name: 'Qty', index: 'Qty', width: 50, align: 'left', editable: true, edittype: 'text', formoptions: { elmsuffix: ' *' }, editrules: { required: true} }
                                ],
        pager: $('#listPager'),
        rowNum: 100,
        width: '100%',
        height: '500px',
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
        editurl: rootPath + 'Site.aspx/UpdateSite/',
        gridComplete: function () {
            var ids = jQuery("#Sites").jqGrid('getDataIDs');
            
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                var plansiteid = jQuery("#Sites").jqGrid('getRowData', cl)["planSiteId"];
                be = "<input style='padding:2px 2px 0px 2px;' type='image'  title='Edit' src= '" + rootPath + "Resources/Images/edit_icon.png' value='edit'  onclick=\"jQuery('#Sites').editRow('" + cl + "',true, pickdates); return false;\" />";
                se = "<input style='padding:2px 2px 0px 2px' type='image' title=\"Save\" src= '" + rootPath + "Resources/Images/save_icon.gif' value='Save1' onclick=\"$('#Sites').saveRow('" + cl + "', false, myediturl, myextraparam); return false; \" />";
                ce = "<input style='padding:2px 2px 0px 2px' type='image' title=\"Cancel\" src= '" + rootPath + "Resources/Images/revert.gif' value='Cancel' onclick=\"jQuery('#Sites').restoreRow('" + cl + "'); return false;\" />";
                de = "<input style='padding:2px 2px 0px 2px' type='image' title=\"Deactivate\" src= '" + rootPath + "Resources/Images/delete.jpg' value='Delete' id='de' onclick=\"javascript:DeleteGridRow(" + cl + ",'" + plansiteid + "'); return false;\" />";
                jQuery("#Sites").jqGrid('setRowData', ids[i], { act: be + se + ce + de });
            }
        }
    });



    $("Sites").jqGrid('navGrid', '#listPager', { edit: true, add: true, del: true, refresh: false },
            { onclickSubmit: function (params) { alert('updating'); } }, updateDialog, null);


    //    $("#AddSite").click(function () {
    //        jQuery("#Sites").jqGrid('editGridRow', "new",
    //                { height: '800px', reloadAfterSubmit: false,
    //                    afterShowForm: function (form) {
    //                        alert('here');
    //                        form.css("height", "800px");
    //                    },
    //                    onclickSubmit: function (params) {
    //                        return { PlanDetailsId: $("#PlanDetailsId").val(),
    //                            VendorName: $("#DisplayVendor :selected").text(),
    //                            selectedLocationVendor: selectedLocationVendorvar
    //                        };
    //                    }
    //                }, pickdates
    //           );
    //    });

}); 
         