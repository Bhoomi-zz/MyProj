<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage< Website.ViewModel.Photo.PlanAlbumViewModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
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
                <td style="padding: 3px 3px 3px 3px;">Plan #</td>
                <td style="padding: 3px 3px 3px 3px;"><%:Html.DisplayTextFor(model => model.PlanNo)%>
                    <%:Html.HiddenFor(model => model.PlanDetailsId)%>
                    <%:Html.HiddenFor(model => model.PlanAlbumId)%>
                </td>
                <td>Album # &nbsp;&nbsp;&nbsp;&nbsp; <%: Html.DisplayFor(x=> x.AlbumNo) %></td>                
            </tr>
           <tr><td style="padding: 3px 3px 3px 3px;">Client</td>
                <td style="padding: 3px 3px 3px 3px;"><%:Html.DisplayTextFor(model => model.ClientName)%></td>               
           </tr>
            </table>
                                    
           <%-- <div class="yoxview">
                <a href="../../Resources/Images/SitePhotos/CityId_E9E3B5A5-C29D-4621-BEC4-D56B1375164D/18A539B7-8F92-49FA-84EA-CCF701274136_pepsi.JPG"><img src="../../Resources/Images/SitePhotos/CityId_E9E3B5A5-C29D-4621-BEC4-D56B1375164D/18A539B7-8F92-49FA-84EA-CCF701274136_pepsi.JPG" alt="First" title="First image" /></a>
                <a href="../../Resources/Images/SitePhotos/CityId_E9E3B5A5-C29D-4621-BEC4-D56B1375164D/607B5F20-B84F-4F52-A206-D1DD67678B02_krish.jpg"><img src="../../Resources/Images/SitePhotos/CityId_E9E3B5A5-C29D-4621-BEC4-D56B1375164D/607B5F20-B84F-4F52-A206-D1DD67678B02_krish.jpg" alt="Second" title="Second image" /></a>
            </div>--%>            
                     
           <%-- <div id="DivUploadPhotos" name="DivUploadPhotos"  />--%>
            <br /><br />
            <div style="vertical-align:bottom">
                 <div id="fogLoaderimg"></div>    
                <input  value="Create Plan Album" type="submit" onclick="javascript:LoadSpinnerImg('fogLoaderimg');"/>
              </div>                      
            <br /><br />        
             
        </div>
    </div>
         <%} %>
</asp:Content>
