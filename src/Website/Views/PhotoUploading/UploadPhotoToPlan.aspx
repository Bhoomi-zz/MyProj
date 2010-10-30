<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Website.ViewModel.Photo.PlanPhotoUploadingViewModel>" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <link href="<%= Url.Content("~/Content/ui.theme.css")%>" rel="stylesheet" type="text/css" />
    	<style>
	        #PhotosAttached {  min-height: 12em; } * html #PhotosAttached { height: 12em; } /* IE6 */
	        .PhotosAttached.custom-state-active { background: #eee; }
	        .PhotosAttached li { float: left; width: 120px; padding: 0.4em; margin: 0 0.4em 0.4em 0; text-align: center; }
	        .PhotosAttached li h5 { margin: 0 0 0.4em;  margin:3px 3px; padding:0 0 3px}
	        .PhotosAttached li a { float: right; }
	        .PhotosAttached li a.ui-icon-zoomin { float: left; }	        
            .siteImagesli { margin: 0 0 0.4em; font-size:smaller; margin:3px 3px; padding:0 0 3px; width:inherit; width:50px; background:orange; color:white}	       
	  </style>
    
    <%--<script src="<%= Url.Content("~/Scripts/fileuploader.js")%>" type="text/javascript"></script>    
    <link href="../../Scripts/fileuploader.css" rel="stylesheet" type="text/css" />--%>

    <script src="<%= Url.Content("~/Scripts/Uploadify/swfobject.js")%>" type="text/javascript"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/Uploadify/jquery.uploadify.v2.1.0.js")%>"></script>
    <link href="<%= Url.Content("~/Scripts/Uploadify/uploadify.css")%>" rel="stylesheet" type="text/css" />

<script type ="text/javascript">      

    $(document).ready(function () {
        
        GetUnattachedPhotosForPlan();
        var planId = $("#planDetailsId").val();
        $('#fileInput').uploadify({
            'uploader': rootPath + 'Scripts/Uploadify/uploadify.swf',
            'script': rootPath + 'ImageUploader.ashx',
            'scriptData': { 'PlanId': planId },
            'cancelImg': 'Scripts/Uploadify/cancel.png',
            'auto': true,
            'multi': true,
            'fileDesc': 'Image Files',
            'fileExt': '*.jpg;*.png;*.gif;*.bmp;*.jpeg',
            'queueSizeLimit': 90,
            'sizeLimit': 4000000,
            'buttonText': 'Browse Images',
            'folder': rootPath + 'Uploads',
            'onAllComplete': function (event, queueID, fileObj, response, data) { GetUnattachedPhotosForPlan(); }
        });        
    });    
    function GetUnattachedPhotosForPlan() {
        var planDetailsId = $("#planDetailsId").val();

        $.ajax({ url: rootPath + "PhotoUploading.aspx/GetAllPhotosforPlan/?planDetailId=" + planDetailsId,
            success: function (data, result) {
                if (!result) alert('Failure to retrieve the Photos.');
                $('#PhotosAttached').html(data);                                
            },
            error: function (data) {
                alert("Failure retrieving images");
            }
        });
    }
    function UploadFile() {
        alert('here');
    }
    function SaveForm() {
        LoadSpinnerImg('fogLoaderimg');
        planDetailsId = $("#planDetailsId").val();
        planAlbumId = $("#PlanAlbumId").val();
        $.ajax({
            url: rootPath + "PhotoUploading.aspx/SaveUploadedPhotosToPlan/?planDetailsId=" + planDetailsId + "&planAlbumId=" + planAlbumId,
            success: function (data, result) {
                UnLoadSpinnerImg('fogLoaderimg');
                if (!result) alert('Failed to Save.');
                else alert('Saved Successfully');
            },
            error: function (data, result) {
                UnLoadSpinnerImg('fogLoaderimg');
                alert('Error while saving..');
            }
        });
    }
    function removeImage(img) {
        photoId = $(img).nextAll().eq(0).attr("id");

        $.ajax({
            url: rootPath + "PhotoUploading.aspx/RemovePhoto/?photoId=" + photoId,
            success: function (data, result) {
                if (!result) alert('Failed to Save.');
                    $(img).parent().remove()
            },
            error: function (data, result) {
                alert('Error while deleting..');
            }
        });
    }
</script>

<% using (Html.BeginForm())
           {%>
    <div class="box"> 
	    <!-- box / title --> 
	    <div class="title"> 
		    <h5>Plan Album</h5> 						
	    </div> 
	    <!-- end box / title --> 
        
        <div class="form" > 
            <table style="width:50%;">
            <tr>
                <td style="padding: 3px 3px 3px 3px;">Plan No </td>
                <td style="padding: 3px 3px 3px 3px;"><%:Html.DisplayTextFor(model => model.PlanNo)%>
                    <%:Html.HiddenFor(model => model.planDetailsId)%>
                    <%:Html.HiddenFor(model => model.PlanAlbumId)%>
                </td>                
            </tr>
           <tr><td style="padding: 3px 3px 3px 3px;">Client</td>
                <td style="padding: 3px 3px 3px 3px;"><%:Html.DisplayTextFor(model => model.ClientName)%></td>                </tr>
            </table>
            <div style="width:100%"> <div style="margin:0 0 0 87%"><div id="fileInput" ></div>                 </div></div>
          
                 
           <%-- <div class="yoxview">
                <a href="../../Resources/Images/SitePhotos/CityId_E9E3B5A5-C29D-4621-BEC4-D56B1375164D/18A539B7-8F92-49FA-84EA-CCF701274136_pepsi.JPG"><img src="../../Resources/Images/SitePhotos/CityId_E9E3B5A5-C29D-4621-BEC4-D56B1375164D/18A539B7-8F92-49FA-84EA-CCF701274136_pepsi.JPG" alt="First" title="First image" /></a>
                <a href="../../Resources/Images/SitePhotos/CityId_E9E3B5A5-C29D-4621-BEC4-D56B1375164D/607B5F20-B84F-4F52-A206-D1DD67678B02_krish.jpg"><img src="../../Resources/Images/SitePhotos/CityId_E9E3B5A5-C29D-4621-BEC4-D56B1375164D/607B5F20-B84F-4F52-A206-D1DD67678B02_krish.jpg" alt="Second" title="Second image" /></a>
            </div>--%>            
             <div style="width:100%;border:1px solid lightgray; margin:10px 0" id ="PhotosAttached1">                            
                 <div class="title"> 
		            <h5>Photos</h5> 						
	            </div> 
                <div style="padding:3px 3px 3px 3px">
                <ul id="PhotosAttached" class="PhotosAttached ui-helper-reset ui-helper-clearfix" >                  
                </ul></div>
                
            </div>              
           <%-- <div id="DivUploadPhotos" name="DivUploadPhotos"  />--%>
            
            <div style="vertical-align:bottom">
               <div id="fogLoaderimg"></div>           
                <input  value="Save" type="submit" onclick="javascript:SaveForm(); return false;"/> <span style="padding:0 0 0 50%;">
                </span> </div>                      
            <br /><br />        
          <%: Html.ActionLink("Back to list", "UploadIndex") %> |           
          <%: Html.ActionLink("Attach to City", "UploadPhotoToCity", new { planDetailId = Model.planDetailsId,  planAlbumId  = Model.PlanAlbumId})%>                
       </div> 
    </div>
         <%} %>
</asp:Content>