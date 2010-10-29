<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Website.ViewModel.Plan.PlanAddDisplayInfoViewModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server"> 
 <link href="../../Content/ui.jqgrid.css" rel="stylesheet" type="text/css" />
    <link href="../../Scripts/themes/grid.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/jqModal.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/jquery.autocomplete.css" rel="stylesheet" type="text/css" />    

    <script src="../../Scripts/js/i18n/grid.locale-en.js" type="text/javascript"></script>
    <script src="../../Scripts/js/jquery.jqGrid.min.js" type="text/javascript"></script>
    <script src="../../Scripts/js/grid.inlinedit.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.autocomplete.js" type="text/javascript"></script>    
    <script src="../../Scripts/jquery.validate.min.js" type="text/javascript"></script>       
     
    <script src="../../Scripts/ChangeDisplayInfo.js" type="text/javascript"></script>
     <div class="box"> 
					<!-- box / title --> 
					<div class="title"> 
						<h5>Change Display Info</h5> 						
					</div> 
					<!-- end box / title --> 
                   <div class="form" >   
    <% using (Html.BeginForm("ChangeDisplayInfo", "Site", FormMethod.Post, new { id = "ChangeDisplayInfoForm" }))
       {%>
        <%: Html.ValidationSummary(true) %>
        
        <fieldset>          
           <%: Html.HiddenFor(model => model.PlanDetailsId) %>                          
            <table style ="width:100%;">
            <tr>
                <td style="border: none; padding: 3px 3px 3px 3px;">Plan No</td>
                <td style="border: none; padding: 3px 3px 3px 3px;"> <%: Html.DisplayTextFor(model => model.PlanNo) %>
                     <%: Html.ValidationMessageFor(model => model.PlanNo) %></td>
                <td style="border: none; padding: 3px 3px 3px 3px;">BriefNo </td>
                <td style="border: none; padding: 3px 3px 3px 3px;"><%: Html.DisplayTextFor(model => model.BriefNo)%>
                    <%: Html.ValidationMessageFor(model => model.BriefNo) %> </td> 
                    <td style="border: none; padding: 3px 3px 3px 3px;"> </td>                                   
            </tr>                     
            <tr>
                <td style="border: none; padding: 3px 3px 3px 3px;">Start Date</td>
                <td style="border: none; padding: 3px 3px 3px 3px;"> <%: Html.DisplayTextFor(model => model.StartDate)%>
                     <%: Html.ValidationMessageFor(model => model.StartDate) %></td>
                <td style="border: none; padding: 3px 3px 3px 3px;">End Date</td>
                <td style="border: none; padding: 3px 3px 3px 3px;"> <%: Html.DisplayTextFor(model => model.EndDate)%>
                     <%: Html.ValidationMessageFor(model => model.EndDate) %>                     
                     </td>
                     <td style="border: none; padding: 3px 3px 3px 3px;"></td>      
                     <td style="border: none; padding: 3px 3px 3px 3px;"></td>      
            <td style="border: none;"> <a href="/Plan.aspx/Edit/?id=<%: Model.PlanDetailsId %>">Edit Plan</a></td>                   
            </tr>
            <tr>
                <td style="border: none; padding: 3px 3px 3px 3px;">Select Region</td>
                <td style="border: none; padding: 3px 3px 3px 3px;"> <%: Html.DropDownListFor(model => model.Region, Model.RegionsList,   "Select one..", new { Region = "Region" })%> </td>
                <td style="border: none; padding: 3px 3px 3px 3px;"></td>   
                <td style="border: none; padding: 3px 3px 3px 3px;"></td>             
                <td style="border: none; padding: 3px 3px 3px 3px;"> </td>                
                <td style="border: none; padding: 3px 3px 3px 3px;"> </td>    
                <td style="border: none;"> <a href="/Plan.aspx/PlanRegionsMainView/?id=<%: Model.PlanDetailsId %>">Edit Plan Regions</a></td>
            </tr>
            <tr>
                <td style="border: none; padding: 3px 3px 3px 3px;">Select Location</td>
                <td style="border: none; padding: 3px 3px 3px 3px;"> <%: Html.TextBoxFor(model => model.LocationName)%>
                <%: Html.HiddenFor(model => model.LocationId)%>                     
                     <%: Html.HiddenFor(model => model.PlanCityId)%></td>
                <td style="border: none; padding: 3px 3px 3px 3px;">Budget</td>
                <td id="CityBudget" style="border: none; padding: 3px 3px 3px 3px;"> <%: Html.DisplayTextFor(model => model.CityBudget)%>  </td>
                <td style="border: none; padding: 3px 3px 3px 3px;"></td>                
                <td style="border: none; padding: 3px 3px 3px 3px;"></td>      
                <td style="border: none;"> <a href="/Plan.aspx/PlanRegionCity/?id=<%: Model.PlanDetailsId  %>">Edit Plan Cities</a></td>
            </tr>
            </table>
            <br />
            <table style="border:1px solid gray; padding:2px 2px 2px 2px">
            <tr>
            <td style="border: none; padding: 3px 3px 3px 3px;">Display Vendor</td>
                <td style="border: none; padding: 3px 3px 3px 3px;"> <%: Html.TextBoxFor(model => model.DisplayVendorName)%>
                <%:Html.HiddenFor(model => model.DisplayVendorId) %> </td>
                 <td style="border: none; padding: 5px 3px 3px 3px;">Vendor Rate</td>
                <td style="border: none; padding: 5px 3px 3px 3px;"> <%: Html.TextBoxFor(model => model.DisplayRate)%></td>
                <td style="border: none; padding: 5px 3px 3px 3px;">Client Rate</td>
                <td style="border: none; padding: 5px 3px 3px 3px;"> <%: Html.TextBoxFor(model => model.ClientRate)%></td>
                <td  style="border: none; padding: 5px 3px 3px 3px;"><input type="button" onclick="javascript:UpdateDisplayInfo()" value ="Update" /></td>
            </tr>
            <tr>
            <td style="border: none; padding: 3px 3px 5px 3px;">Display From Date</td>
                <td style="border: none; padding: 3px 3px 5px 3px;"> <%: Html.TextBoxFor(model => model.DisplayFromDate, new Dictionary<string, object>() { { "Title", "DateTimePicker" } })%> </td>
                <td style="border: none; padding: 3px 3px 5px 3px;">Display To Date</td>
                <td style="border: none; padding: 3px 3px 5px 3px;"> <%: Html.TextBoxFor(model => model.DisplayToDate, new Dictionary<string, object>() { { "Title", "DateTimePicker" } })%> </td>
            </tr>

            </table>          

        </fieldset>                            
         
    <br />
    <a href ="javascript:SelectAll()" >All</a> /
    <a href ="javascript:SelectNone()" >None</a>
    <br />     <br />
        <div class="DivOverflow" >
       <table id="DisplaySites" style ="width:100%"></table>
       </div>        
            
 
      <br />           
         <input type="submit" value="Save" />                     
         <input type="submit" value="Cancel" />                            
    <br />
    <br />
    <div>        
        <%: Html.ActionLink("Back to List", "Index") %>
    </div>
    <% } %>
   </div>
    </div>
</asp:Content>
