<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Website.ViewModel.Photo.PlanSiteAlbumViewModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link href="<%= Url.Content("~/Content/ui.theme.css")%>" rel="stylesheet" type="text/css" />
    	<style>	        
	        #siteImages h5 { line-height: 16px; margin: 0 0 0.4em; }
	        #siteImages h5 .ui-icon { float: left; }        
	        
	        #siteImages { float: left; min-height: 12em; width:100% } * html #imagesUnUsed { height: 12em; } /* IE6 */
	        .siteImages.custom-state-active { background: #eee; }
	        .siteImages li { float: left; width: 126px; padding: 0.4em; margin: 0 0.4em 0.4em 0; text-align: center; }
	        .siteImagesli { margin: 0 0 0.4em; font-size:smaller; margin:3px 3px; padding:0 0 3px; width:inherit; width:50px; background:orange; color:white}
	        .siteImages li a { float: right; }
	        .siteImages li a.ui-icon-zoomin { float: left; }
	        .siteImages li img { width: 100%;  }
	  </style>
      
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/PhotoViewer.js")%>"></script>
    <script type="text/javascript"  src="<%= Url.Content("~/Scripts/yox.js")%>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/yoxview-init.js")%>"></script>    

<script type ="text/javascript">
    var selectedSiteId;
    var selectedCityId;

    function GetSitesforCity(planCityId) {
        $.ajax({ url: rootPath + "Photo.aspx/GetSiteListForCity/?planCityId=" + planCityId ,
            success: function (data, result) {
                if (!result) alert('Failure to retrieve the Sites.');
                $('#SiteList').html(data);                
            },
            error: function (data) {
                alert("Failure retrieving Sites");
            }
        });
    }

    function GetPhotosForSite(siteId) {
        var planCityId = selectedCityId;
        if (planCityId == "")
            return;
        $("#PlanCityId").val(planCityId);
        var planDetailId = $("#PlanId").val();
        selectedSiteId = siteId;
        if (siteId == "")
            return;
        $("#SelectedSiteId").val(selectedSiteId);
        $("div[id^='div_']").css("background", "none")
        $("#div_" + siteId).css("background", "orange")
        plancityId = $("#PlanCityId").val();
        $.ajax({ url: rootPath + "Photo.aspx/GetSitePhotosBySiteIdforViewer/?siteId=" + siteId + "&planCityId=" + plancityId + "&planDetailsId=" + planDetailId,
            success: function (data, result) {
                if (!result) alert('Failure to retrieve the Sites.');
                $('#siteImages').html(data);
                $(".yoxview").yoxview({
                    videoSize: { maxwidth: 720, maxheight: 560 }
                }); 
            },
            error: function (data) {
                alert("Failure retrieving images");
            }
        });
    }

    $(document).ready(function () {

        $("#Region").change(function () {
            var region = $("#Region").val();
            selectedRegion = region;
            var locations = $("#Cities")[0];
            $("#Cities").val("");

            for (var i = 0; i < locations.length; i++) {
                if (locations[i].value.split(";")[1] != region) {
                    locations[i].disabled = true;
                }
                else {
                    locations[i].disabled = false;
                }
            }
        });

        $("#Cities").change(function (data) {
            if ($("#Cities").val() != "") {
                selectedCityId = $("#Cities").val().split(";")[0];
            }
            else
                selectedCityId = "";
            GetSitesforCity(selectedCityId);
        });
        $(".yoxview").yoxview({
            videoSize: { maxwidth: 720, maxheight: 560 }
        });

    });
    
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
                    <%:Html.HiddenFor(model => model.PlanAlbumId)%>
                </td>
                <td style="padding: 3px 3px 3px 3px;">Client</td>
                <td style="padding: 3px 3px 3px 3px;"><%:Html.DisplayTextFor(model => model.ClientName)%></td>                
            </tr>
            <tr>
                <td style="padding: 3px 3px 3px 3px;">Region</td>
                <td style="padding: 3px 3px 3px 3px;"> <%: Html.DropDownListFor(model => model.Region, Model.RegionsList, "Select one..", new { Region = "Region" })%> </td>
                <td style="padding: 3px 3px 3px 3px;">Cities</td>
                <td style="padding: 3px 3px 3px 3px;"> <%: Html.DropDownListFor(model => model.Cities, Model.CitiesList, "Select one..", new { PlanCityId = "LocationIdWithRegion" })%></td>
                        <%: Html.HiddenFor(model => model.PlanCityId) %>
                        
                        <%: Html.HiddenFor(model=> model.SelectedSiteId) %>
            </tr>
            </table>
            <br />
            <br />
            <div style="float:left; width:20%; border: 1px solid lightgray" >
            <div class="title"> 
		            <h5>Sites</h5> 						
	              </div> 
                  <div id ="SiteList"></div>
                <%--<%foreach (var site in Model.Sites)
                  {%>                  
                    <div style="vertical-align:baseline; height:2em; width:100%; margin:0 0 0 5px" id="div_<%:site.PlanCityId%>_<%:site.SiteId%>">  <a href="javascript:GetPhotosForSite('<%:site.SiteId%>','<%:site.PlanCityId%>')"> <%: site.SiteName%></a>  </div>
                  <br />
                <%} %>--%>
            </div>  
           <%-- <div class="yoxview">
                <a href="../../Resources/Images/SitePhotos/CityId_E9E3B5A5-C29D-4621-BEC4-D56B1375164D/18A539B7-8F92-49FA-84EA-CCF701274136_pepsi.JPG"><img src="../../Resources/Images/SitePhotos/CityId_E9E3B5A5-C29D-4621-BEC4-D56B1375164D/18A539B7-8F92-49FA-84EA-CCF701274136_pepsi.JPG" alt="First" title="First image" /></a>
                <a href="../../Resources/Images/SitePhotos/CityId_E9E3B5A5-C29D-4621-BEC4-D56B1375164D/607B5F20-B84F-4F52-A206-D1DD67678B02_krish.jpg"><img src="../../Resources/Images/SitePhotos/CityId_E9E3B5A5-C29D-4621-BEC4-D56B1375164D/607B5F20-B84F-4F52-A206-D1DD67678B02_krish.jpg" alt="Second" title="Second image" /></a>
            </div>--%>

           
           <%--div  class="thumbnails yoxview ">
                <a href="<%= Url.Content("~/Resources/Images/imperialwharf.jpg")%>"><img src= "<%= Url.Content("~/Resources/Images/imperialwharf.jpg")%>" alt="First" title="First image" />
                        </a>
                <a href="<%= Url.Content("~/Resources/Images/krish.jpg")%>"><img src="<%= Url.Content("~/Resources/Images/krish.jpg")%>" alt="Second" title="Second image" /></a>
           </div--%>
           
            <div style="float:right; width:78%; border:1px solid lightgray; " class="demo ui-widget ui-helper-clearfix thumbnails yoxview ">
             <div class="title"> 
		            <h5>Attached Photos</h5> 						
	            </div> 
                 <ul id="siteImages" class="siteImages ui-helper-reset ui-helper-clearfix" >                  
                 </ul>
                     </div>   
                 <br /><br /><br />
                 <div style="float:right; width:78%;" >
                    <%: Html.ActionLink("Back to list", "PhotoViewerIndex", "Photo") %>  
                 </div>       
                  
            
             </div>
        </div>
    
         <%} %>
</asp:Content>
