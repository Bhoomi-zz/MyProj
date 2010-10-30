<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Website.ViewModel.Photo.PlanPhotoUploadingViewModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link href="<%= Url.Content("~/Content/ui.theme.css")%>" rel="stylesheet" type="text/css" />
    	<style>
	        #imagesUnUsed {  min-height: 12em; } * html #imagesUnUsed { height: 12em; } /* IE6 */
	        .imagesUnUsed.custom-state-active { background: #eee; }
	        .imagesUnUsed li { float: left; width: 96px; padding: 0.4em; margin: 0 0.4em 0.4em 0; text-align: center; }
	        .imagesUnUsed li h5 { margin: 0 0 0.4em; cursor: move; margin:3px 3px; padding:0 0 3px}
	        .imagesUnUsed li a { float: right; }
	        .imagesUnUsed li a.ui-icon-zoomin { float: left; }
	        .imagesUnUsed li img { width: 100%; cursor: move; }
	        
	        #cityImages h5 { line-height: 16px; margin: 0 0 0.4em; }
	        #cityImages h5 .ui-icon { float: left; }        
	        
	        #cityImages { float: left; min-height: 12em; width:100% } * html #imagesUnUsed { height: 12em; } /* IE6 */
	        .cityImages.custom-state-active { background: #eee; }
	        .cityImages li { float: left; width: 96px; padding: 0.4em; margin: 0 0.4em 0.4em 0; text-align: center; }
	        .cityImagesli { margin: 0 0 0.4em; cursor: move;  font-size:smaller; margin:3px 3px; padding:0 0 3px; width:inherit; width:50px; background:orange; color:white}
	        .cityImages li a { float: right; }
	        .cityImages li a.ui-icon-zoomin { float: left; }
	        .cityImages li img { width: 100%; cursor: move; }	        
	        
	  </style>

    <script type="text/javascript" src="<%= Url.Content("~/Scripts/UploadPhotosToCity.js")%>"></script>

    <%--<script src="<%= Url.Content("~/Scripts/fileuploader.js")%>" type="text/javascript"></script>    
    <link href="../../Scripts/fileuploader.css" rel="stylesheet" type="text/css" />--%>

    <script src="<%= Url.Content("~/Scripts/Uploadify/swfobject.js")%>" type="text/javascript"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/Uploadify/jquery.uploadify.v2.1.0.js")%>"></script>
    <link href="<%= Url.Content("~/Scripts/Uploadify/uploadify.css")%>" rel="stylesheet" type="text/css" />

    <%--<script type ="text/javascript">
        function startUpload(id) {
            $('#UploadPix').uploadifyUpload("UploadPixQueue");
        }
    </script>--%>
<script type ="text/javascript">
    var selectedCityId;
    var sessionId;
    function GetPhotosForCity(planCityId) {
        selectedCityId = planCityId;
        var planDetailsId = $("#planDetailsId").val();
        if (planCityId == "" || planCityId==undefined)
            return;
        $("#SelectedCityId").val(selectedCityId);
        $("div[id^='div_']").css("background", "none")
        $("#div_" + planCityId).css("background", "orange")

//        $('#UploadPix').uploadifySettings('scriptData', "&PlanCityID=" + selectedCityId);

        $.ajax({ url: rootPath + "PhotoUploading.aspx/GetCityPhotosByCityId/?planDetailsId=" + planDetailsId + "&planCityId=" + planCityId + "&sessionId="+ sessionId,
            success: function (data, result) {
                if (!result) alert('Failure to retrieve the images.');
                $('#cityImages').html(data);                
                attachDraggabletoAttached();
            },
            error: function (data) {
                alert("Failure retrieving images");
            }
        });
    }

    function SaveForm() {
        LoadSpinnerImg('fogLoaderimg');
        var planDetailsId = $("#planDetailsId").val();
        var planAlbumId = $("#PlanAlbumId").val();
        $.ajax({ url: rootPath + "PhotoUploading.aspx/SaveForm/?planAlbumId=" + planAlbumId + "&planDetailsId=" + planDetailsId + "&sessionId=" + sessionId,
            success: function (data, result) {
                UnLoadSpinnerImg('fogLoaderimg');
                if (!result) alert('Error while saving...');
                else alert('Saved Successfully');
            },
            error: function (data) {
                UnLoadSpinnerImg('fogLoaderimg');
                alert("Error while saving");
            }
        });
    }

    $(document).ready(function () {

        sessionId = $("#sessionId").val();
        $("#Cities").change(function () { GetUnattachedPhotosForCity(); })
        GetUnattachedPhotosForPlan();
        //        if (selectedCityId == undefined) {
        //            alert('Please select a city from left pane');
        //            return;
        //        }
        var planId = $("#planDetailsId").val();
        $('#UploadPix').uploadify({
            'uploader': rootPath + 'Scripts/Uploadify/uploadify.swf',
            'script': rootPath + 'ImageUploader.ashx',
            'scriptData': { 'PlanId': planId, 'sessionId': sessionId },
            'cancelImg': 'Scripts/Uploadify/cancel.png',
            'auto': true,
            'multi': true,
            'fileDesc': 'Image Files',
            'fileExt': '*.jpg;*.png;*.gif;*.bmp;*.jpeg',
            'queueSizeLimit': 90,
            'sizeLimit': 4000000,
            'buttonText': 'Browse Photos',
            'folder': rootPath + 'Uploads',
            'onAllComplete': function (event, queueID, fileObj, response, data) { GetPhotosForCity(selectedCityId); }
        });


        //         createUploader();
    });
//    function createUploader() {
//        var uploader = new qq.FileUploader({
//            element: document.getElementById('DivUploadPhotos'),
//            //            onSubmit: function (id, fileName) { alert('here') },            
//            action: rootPath + 'ImageUploader.ashx',
//        });
//    }
    function GetUnattachedPhotosForPlan() {
        var planDetailsId = $("#planDetailsId").val();
        var planAlbumid = $("#PlanAlbumId").val();
        $.ajax({ url: rootPath + "PhotoUploading.aspx/GetUnAttachedPhotosForCity/?planAlbumId=" + planAlbumid + "&planDetailId=" +planDetailsId,
            success: function (data, result) {
                if (!result) alert('Failure to retrieve the Cities Photos.');

                $('#imagesUnUsed').html(data);
                //$("#PhotoAgId").val($("#PhotoAgIdForSelectedPlan").val());
                attachDraggabletoUnattached();                
            },
            error: function (data) {
                alert("Failure retrieving images");
            }
        });
    }
    function UploadFile() {
        alert('here');
    }
</script>

<% using (Html.BeginForm())
           {%>
    <div class="box"> 
	    <!-- box / title --> 
	    <div class="title"> 
		    <h5>City Album</h5> 						
	    </div> 
	    <!-- end box / title --> 
        
        <div class="form" > 
            <table style="width:50%;">
            <tr>
                <td style="padding: 3px 3px 3px 3px;">Plan No </td>
                <td style="padding: 3px 3px 3px 3px;"><%:Html.DisplayTextFor(model => model.PlanNo)%>
                    <%:Html.HiddenFor(model => model.planDetailsId)%>
                    <%:Html.HiddenFor(model => model.PlanAlbumId)%>
                    <%:Html.HiddenFor(model => model.sessionId)%>
                </td>
                <td style="padding: 3px 3px 3px 3px;">Client</td>
                <td style="padding: 3px 3px 3px 3px;"><%:Html.DisplayTextFor(model => model.ClientName)%></td>                
            </tr>
            <tr>
                <%--<td style="padding: 3px 3px 3px 3px;">Region</td>
                <td style="padding: 3px 3px 3px 3px;"> <%: Html.DropDownListFor(model => model.Region, Model.RegionsList, "Select one..", new { Region = "Region" })%> </td>--%>
                <td>
                        <%: Html.HiddenFor(model => model.PhotoAgId) %>
                        <%: Html.HiddenFor(model=> model.SelectedCityId) %>
                        </td>
            </tr>
            </table>
          
          <div style="float:right;margin:0 0 0 80%">  <div id="UploadPix"></div></div>
          <%-- <a href="javascript:startUpload('UploadPix')">Start Upload</a>--%>

            <div style="float:left; width:20%; border: 1px solid lightgray">
            <div class="title"> 
		            <h5>Cities</h5> 						
	              </div> 
                <%foreach (var city in Model.Cities)
                  {%>                  
                    <div style="vertical-align:baseline; height:2em; width:95%; margin:0 5px 0 5px" id="div_<%:city.PlanCitiesId%>">  <a href="javascript:GetPhotosForCity('<%:city.PlanCitiesId%>')"> <%: city.LocationName%></a>  </div>
                  <br />
                <%} %>
            </div>  
           <%-- <div class="yoxview">
                <a href="../../Resources/Images/SitePhotos/CityId_E9E3B5A5-C29D-4621-BEC4-D56B1375164D/18A539B7-8F92-49FA-84EA-CCF701274136_pepsi.JPG"><img src="../../Resources/Images/SitePhotos/CityId_E9E3B5A5-C29D-4621-BEC4-D56B1375164D/18A539B7-8F92-49FA-84EA-CCF701274136_pepsi.JPG" alt="First" title="First image" /></a>
                <a href="../../Resources/Images/SitePhotos/CityId_E9E3B5A5-C29D-4621-BEC4-D56B1375164D/607B5F20-B84F-4F52-A206-D1DD67678B02_krish.jpg"><img src="../../Resources/Images/SitePhotos/CityId_E9E3B5A5-C29D-4621-BEC4-D56B1375164D/607B5F20-B84F-4F52-A206-D1DD67678B02_krish.jpg" alt="Second" title="Second image" /></a>
            </div>--%>
            <div style="float:right; width:78%;border:1px solid lightgray; " class="demo ui-widget ui-helper-clearfix">
             <div class="title"> 
		            <h5>Attached Photos</h5> 						
	            </div> 
                 <ul id="cityImages" class="cityImages ui-helper-reset ui-helper-clearfix" >                  
                 </ul>
            </div>
             <div style="float:right; width:78%;border:1px solid lightgray; margin:10px 0" id ="imagesUnUsed1">                            
                 <div class="title"> 
		            <h5>UnAttached Photos</h5> 						
	            </div> <div style="padding:3px 3px 3px 3px">
                <ul id="imagesUnUsed" class="imagesUnUsed ui-helper-reset ui-helper-clearfix" >                  
                </ul></div>
                
            </div>              
           <%-- <div id="DivUploadPhotos" name="DivUploadPhotos"  />--%>
            
             
            <div style="float:right; width:78%;">
              <div id="fogLoaderimg"></div>      
            <input type="submit" value="Save" onclick="javascript:SaveForm(); return false;"/> &nbsp;                
            <br /><br />        
            <%: Html.ActionLink("Back to list", "UploadIndex") %> | 
            <%: Html.ActionLink("Attach to Site", "SiteAlbum", "Photo", new { planAlbumId = Model.PlanAlbumId }, null)%>                

                 </div>
            
             </div>
        </div>
    
         <%} %>
</asp:Content>