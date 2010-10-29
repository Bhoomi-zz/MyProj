<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Website.ViewModel.Photo.PlanSiteAlbumViewModel>" %>

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
	        
	        #siteImages h5 { line-height: 16px; margin: 0 0 0.4em; }
	        #siteImages h5 .ui-icon { float: left; }        
	        
	        #siteImages { float: left; min-height: 12em; width:100% } * html #imagesUnUsed { height: 12em; } /* IE6 */
	        .siteImages.custom-state-active { background: #eee; }
	        .siteImages li { float: left; width: 96px; padding: 0.4em; margin: 0 0.4em 0.4em 0; text-align: center; }
	        .siteImagesli { margin: 0 0 0.4em; cursor: move;  font-size:smaller; margin:3px 3px; padding:0 0 3px; width:inherit; width:50px; background:orange; color:white}
	        .siteImages li a { float: right; }
	        .siteImages li a.ui-icon-zoomin { float: left; }
	        .siteImages li img { width: 100%; cursor: move; }
	  </style>

    <script type="text/javascript" src="<%= Url.Content("~/Scripts/SiteAlbum.js")%>"></script>
    <%--<script src="../../Scripts/yoxview-init.js" type="text/javascript"></script>--%>
<script type ="text/javascript">
    var selectedSiteId;

    function GetPhotosForSite(siteId) {
        selectedSiteId = siteId;
        var planDetailsId = $("#PlanId").val();
        if(siteId=="")
            return;
        $("#SelectedSiteId").val(selectedSiteId);
        $("div[id^='div_']").css("background", "none")
        $("#div_" + siteId).css("background", "orange")
        plancityId = $("#PlanCityId").val();
        $.ajax({ url: rootPath + "Photo.aspx/GetSitePhotosBySiteId/?siteId=" + siteId + "&planCityId=" + plancityId + "&planDetailsId=" + planDetailsId,
            success: function (data, result) {
                if (!result) alert('Failure to retrieve the Sites.');
                $('#siteImages').html(data);
                attachDraggabletoAttached();
            },
            error: function (data) {
                alert("Failure retrieving images");
            }
        });
    }

    function SaveForm() {
        LoadSpinnerImg('fogLoaderimg');
        var planDetailsId = $("#PlanId").val();
        var planAlbumId = $("#PlanAlbumId").val();
        var planCityId = $("#PlanCityId").val();
        $.ajax({ url: rootPath + "Photo.aspx/SaveForm/?planAlbumId=" + planAlbumId + "&planDetailsId=" + planDetailsId + "&planCityId=" + planCityId,
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

    function GetSitesforCity(planCityId) {
        $.ajax({ url: rootPath + "Photo.aspx/GetSiteListForCity/?planCityId=" + planCityId,
            success: function (data, result) {
                if (!result) alert('Failure to retrieve the Sites.');
                $('#SiteList').html(data);
                $('#siteImages').html("");
            },
            error: function (data) {
                alert("Failure retrieving Sites");
            }
        });
    }


    $(document).ready(function () {
        $("#Region").change(function () {
            var region = $("#Region").val();
            var locations = $("#Cities")[0];
            $("#Cities").val("");
            $("#PlanCityId").val("");
            $("#SiteList").html("");
            for (var i = 0; i < locations.length; i++) {
                if (locations[i].value.split(";")[1] != region) {
                    locations[i].disabled = true;
                }
                else {
                    locations[i].disabled = false;
                }
            }
        });
        $("#Cities").change(function () {
            GetSitesforCity($("#Cities").val().split(';')[0]);
            GetUnattachedPhotosForCity();
        })
        GetUnattachedPhotosForCity();
        GetPhotosForSite($("#SelectedSiteId").val())
    });

    function GetUnattachedPhotosForCity() {        
        var planCityId = $("#Cities").val().split(';')[0];
        var planDetailsId = $("#PlanId").val();
        if (planCityId == "")
            return;
        $("#PlanCityId").val(planCityId);
        $.ajax({ url: rootPath + "Photo.aspx/GetUnattachedSitePhotosByCityId/?planCityId=" + planCityId + "&planDetailsId=" + planDetailsId,
            success: function (data, result) {
                if (!result) alert('Failure to retrieve the Sites.');
                $('#imagesUnUsed').html(data);                
                attachDraggabletoUnattached();                
            },
            error: function (data) {
                alert("Failure retrieving images");
            }
        });
    }
</script>

<% using (Html.BeginForm())
           {%>
    <div class="box"> 
	    <!-- box / title --> 
	    <div class="title"> 
		    <h5>Site Album</h5> 						
	    </div> 
	    <!-- end box / title --> 
        
        <div class="form" > 
            <table style="width:50%;">
            <tr>
                <td style="padding: 3px 3px 3px 3px;">Plan No </td>
                <td style="padding: 3px 3px 3px 3px;"><%:Html.DisplayTextFor(model => model.PlanNo)%>
                    <%:Html.HiddenFor(model => model.PlanId)%>
                    <%: Html.HiddenFor(model => model.PlanAlbumId) %>
                </td>
                <td style="padding: 3px 3px 3px 3px;">Client</td>
                <td style="padding: 3px 3px 3px 3px;"><%:Html.DisplayTextFor(model => model.ClientName)%></td>                
            </tr>
            <tr>
                <td style="padding: 3px 3px 3px 3px;">Region</td>
                <td style="padding: 3px 3px 3px 3px;"> <%: Html.DropDownListFor(model => model.Region, Model.RegionsList, "Select one..", new { Region = "Region" })%> </td>
                <td style="padding: 3px 3px 3px 3px;">Cities</td>
                <td style="padding: 3px 3px 3px 3px;"> <%: Html.DropDownListFor(model => model.Cities, Model.CitiesList, "Select one..", new { PlanCityId = "PlanCitiesId" })%></td>
                        <%: Html.HiddenFor(model => model.PlanCityId) %>                        
                        <%: Html.HiddenFor(model=> model.SelectedSiteId) %>
            </tr>
            </table>
           <div style=" width:100%; margin:0 0 0 93%" >
                 <input type="submit" value="Save" onclick="javascript:SaveForm(); return false;"/> &nbsp;
                 </div>
            <div style="float:left; width:20%; border: 1px solid lightgray">
            <div class="title"> 
		            <h5>Sites</h5> 						
	              </div> 
               <%-- <%foreach (var site in Model.Sites)
                  {%>                  
                    <div style="vertical-align:baseline; height:2em; width:95%; margin:0 5px 0 5px" id="div_<%:site.SiteId%>">  <a href="javascript:GetPhotosForSite('<%:site.SiteId%>')"> <%: site.SiteName%></a>  </div>
                  <br />
                <%} %>--%>
                <div id="SiteList"></div>
            </div>  
           <%-- <div class="yoxview">
                <a href="../../Resources/Images/SitePhotos/CityId_E9E3B5A5-C29D-4621-BEC4-D56B1375164D/18A539B7-8F92-49FA-84EA-CCF701274136_pepsi.JPG"><img src="../../Resources/Images/SitePhotos/CityId_E9E3B5A5-C29D-4621-BEC4-D56B1375164D/18A539B7-8F92-49FA-84EA-CCF701274136_pepsi.JPG" alt="First" title="First image" /></a>
                <a href="../../Resources/Images/SitePhotos/CityId_E9E3B5A5-C29D-4621-BEC4-D56B1375164D/607B5F20-B84F-4F52-A206-D1DD67678B02_krish.jpg"><img src="../../Resources/Images/SitePhotos/CityId_E9E3B5A5-C29D-4621-BEC4-D56B1375164D/607B5F20-B84F-4F52-A206-D1DD67678B02_krish.jpg" alt="Second" title="Second image" /></a>
            </div>--%>
            <div style="float:right; width:78%;border:1px solid lightgray; " class="demo ui-widget ui-helper-clearfix">
             <div class="title"> 
		            <h5>Attached Photos</h5> 						
	            </div> 
                 <ul id="siteImages" class="siteImages ui-helper-reset ui-helper-clearfix" >                  
                 </ul>
            </div>
             <div style="float:right; width:78%;border:1px solid lightgray; margin:10px 0" id ="imagesUnUsed1">                            
                 <div class="title"> 
		            <h5>UnAttached Photos</h5> 						
	            </div> <div style="padding:3px 3px 3px 3px">
                <ul id="imagesUnUsed" class="imagesUnUsed ui-helper-reset ui-helper-clearfix" >                  
                </ul></div>
                
            </div>  
            <div style="float:right; width:78%;">
             <div id="fogLoaderimg"></div>     
            <input type="submit" value="Save" onclick="javascript:SaveForm(); return false;"/> &nbsp;                 
            <br /><br />        
            <%: Html.ActionLink("Back to list", "UploadIndex", "PhotoUploading") %> | 
            <%: Html.ActionLink("City Album", "UploadPhotoToCity", "PhotoUploading",  new { planDetailId = Model.PlanId, planAlbumId = Model.PlanAlbumId }, null)%>   

                 </div>
            
             </div>
        </div>
    
         <%} %>
</asp:Content>
