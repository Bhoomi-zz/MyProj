<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Website.ViewModel.BriefViewModel>" %>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   
    <script src="<%= Url.Content("~/Scripts/jquery.autocomplete.js" ) %> " type="text/javascript"></script>
    
    <link href="<%= Url.Content("~/Content/jquery.autocomplete.css" )%> " rel="stylesheet" type="text/css" />    
    <link href="<%= Url.Content("~/Content/Brief.css") %> " rel="stylesheet" type="text/css" />

    <script src="../../Scripts/date.js" type="text/javascript"></script>
<script type="text/javascript">

    $(document).ready(function () {
        $("#menu-brief").toggleClass("closed");
        $("#h-menu-brief").addClass("selected");

        contactId = $("#sContactId").val();
        $("#ContactPerson").autocomplete(rootPath + 'Shared.aspx/FindContactPerson/',
          {
              minChars: 1,
              delay: 400,
              cacheLength: 100,
              max: 10,
              contentType: "application/json; charset=utf-8",
              dataType: "json",
              formatItem: function (item, index, total, query) {
                  return item.value;
              },
              formatMatch: function (item) {
                  return item.Key;
              },
              formatResult: function (item) {
                  return item.value;
              },
              parse: function (data) {
                  return $.map(data, function (item) {
                      return {
                          data: item,
                          value: item.Key,
                          result: item.value
                      }
                  });
              }
          }).result(function (event, row) {
              $("#ContactPersonId").val(row.Key);
          });
          $("#TentativeStartDate").change(function () {
              var startdate = $("#TentativeStartDate").val();
              var enddate = $("#TentativeEndDate").val();
              if (startdate != "" && enddate != "") {
                  var minutes = 1000 * 60;
                  var hours = minutes * 60;
                  var day = hours * 24;

                  var startdate1 = getDateFromFormat(startdate, "dd/mm/yyyy");
                  var enddate1 = getDateFromFormat(enddate, "dd/mm/yyyy");

                  var days = 1 + Math.round((enddate1 - startdate1) / day);

                  if (days > 0)
                  { $("#NoOfDays").val(days); }
                  else
                  { $("#NoOfDays").val(0); }                  
              }
          });
        $("#sContactId").change(function () {
            $("#ContactPerson").setOptions({
                extraParams: { 'contactId' : $("#sContactId").val() }
            });           
        });
    }); 

</script>
    
    <div class="box"> 
					<!-- box / title --> 
					<div class="title"> 
						<h5>Create Brief</h5> 						
					</div> 
					<!-- end box / title --> 
                   <div class="form"> 	

    <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>

        <fieldset>
            
            
            <div class="editor-label" id ="contentPanel">
            <div>
            <table class="TablePanel" >
            <tr>
                <td>Brief# </td>
                <td>  <%: Html.TextBoxFor(model => model.BriefNo, new Dictionary<string, object>() { { "readonly", "true" } })%></td>
                <td>Brief Date </td>
                <td>  <%: Html.EditorFor(model => model.BriefDate, new Dictionary<string, object>(){{"Title","DateTimePicker"}}) %>
                      <%: Html.ValidationMessageFor(model => model.BriefDate) %></td>
            </tr>
            <tr>
                <td>
              <%: Html.LabelFor(model => model.Customer) %>
             </td>
             <td>
                <%: Html.DropDownListFor(model => model.Customer, Model.Customers, "Select one..", new { id = "sContactId" })%>
                <%: Html.ValidationMessageFor(model => model.Customer) %>
                </td>
                <td rowspan="2" class="BlockTd" >
                        <%: Html.LabelFor(model => model.ClientBusinessProfile) %>            
                 </td>
                 <td rowspan="2"  class="MultiLineTextBox">
                        <%: Html.TextBoxFor(model => model.ClientBusinessProfile) %>
                        <%: Html.ValidationMessageFor(model => model.ClientBusinessProfile)%>            
                 </td>
                </tr>
                <tr>
                <td>
                    <%: Html.LabelFor(model => model.ContactPerson) %>            
                    </td>
                    <td>
                    <%: Html.TextBoxFor(model => model.ContactPerson) %>
                    <%: Html.ValidationMessageFor(model => model.ContactPerson) %>    
                     <%: Html.HiddenFor(model => model.ContactPersonId) %>            
                    </td>                    
                </tr>
            </table> 
            </div>   
            <table class="TablePanel" style="width:70%">
            <tr>               
                <td> <%: Html.LabelFor(model => model.TentativeStartDate) %> </td>
                <td><%: Html.EditorFor(model => model.TentativeStartDate, new Dictionary<string, object>() { { "Title", "DateTimePicker" } })%>
                      <%: Html.ValidationMessageFor(model => model.TentativeStartDate) %></td>
                <td> <%: Html.LabelFor(model => model.TentativeEndDate) %> </td>
                <td><%: Html.EditorFor(model => model.TentativeEndDate, new Dictionary<string, object>() { { "Title", "DateTimePicker" } })%>
                      <%: Html.ValidationMessageFor(model => model.TentativeEndDate) %></td>
                       <td></td>
                <td></td>
            </tr>
            <tr>
                <td><%: Html.LabelFor(model => model.DeadLineDate) %> </td>
                <td><%: Html.EditorFor(model => model.DeadLineDate, new Dictionary<string, object>() { { "Title", "DateTimePicker" } })%>
                      <%: Html.ValidationMessageFor(model => model.DeadLineDate) %></td>
                <td><%: Html.LabelFor(model => model.RequestSentToHO) %></td>
                <td><%: Html.EditorFor(model => model.RequestSentToHO, new Dictionary<string, object>() { { "Title", "DateTimePicker" } })%>
                      <%: Html.ValidationMessageFor(model => model.RequestSentToHO)%></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
            <td>Campaign Period</td>
            <td>&nbsp;Year (0-9) 
            <%: Html.TextBoxFor(model => model.NoOfYears)%>
                      <%: Html.ValidationMessageFor(model => model.NoOfYears)%></td>
            <td > &nbsp; Month(0-12) 
            <%: Html.TextBoxFor(model => model.NoOfMonths)%>
                      <%: Html.ValidationMessageFor(model => model.NoOfMonths)%>
            </td>    
            <td> &nbsp;Days(0-365)
            <%: Html.TextBoxFor(model => model.NoOfDays)%>
                      <%: Html.ValidationMessageFor(model => model.NoOfDays)%>
            </td>
            </tr>
            <tr>
                <td> Budget </td>
                <td> <%: Html.TextBoxFor(model => model.BudgetValue) %>
                     <%: Html.ValidationMessageFor(model => model.BudgetValue) %></td>
                <td> Budget Type </td>
                  <td><%: Html.DropDownListFor(model => model.BudgetTypes, Model.BudgetTypesList, "Select one..")%> 
                     <%: Html.ValidationMessageFor(model => model.BudgetTypes) %></td>
            </tr>
            </table>
            <table  class="TablePanel">
                <tr>
                    <td > Brands: </td>
                    
                    <td>Brand 1</td>
                    <td>Brand 2</td>
                    <td>Brand 3</td>
                    <td>Brand 4</td>
                </tr>
                <tr>
                    <td>Brand Name</td>                    
                    <td><%: Html.TextBoxFor(model => model.Brand1) %>
                        <%: Html.ValidationMessageFor(model => model.Brand1)%></td>
                    <td><%: Html.TextBoxFor(model => model.Brand2) %>
                        <%: Html.ValidationMessageFor(model => model.Brand2)%></td>
                    <td><%: Html.TextBoxFor(model => model.Brand3) %>
                        <%: Html.ValidationMessageFor(model => model.Brand3)%></td>
                    <td><%: Html.TextBoxFor(model => model.Brand4) %>
                        <%: Html.ValidationMessageFor(model => model.Brand4)%></td>
                </tr>
                <tr>
                    <td>Target Group</td>
                    <td><%: Html.TextBoxFor(model => model.TargetGroupBrand1) %>
                        <%: Html.ValidationMessageFor(model => model.Brand1)%></td>
                    <td><%: Html.TextBoxFor(model => model.TargetGroupBrand2) %>
                        <%: Html.ValidationMessageFor(model => model.Brand2)%></td>
                    <td><%: Html.TextBoxFor(model => model.TargetGroupBrand3) %>
                        <%: Html.ValidationMessageFor(model => model.Brand3)%></td>
                    <td><%: Html.TextBoxFor(model => model.TargetGroupBrand4) %>
                        <%: Html.ValidationMessageFor(model => model.Brand4)%></td>
                    </tr>
                <tr>
                    <td>TG Specific Media (Roadside, Proximity, or Area)</td>
                    
                    <td><%: Html.TextBoxFor(model => model.TgsMedia1) %>
                        <%: Html.ValidationMessageFor(model => model.TgsMedia1)%></td>
                    <td><%: Html.TextBoxFor(model => model.TgsMedia1) %>
                        <%: Html.ValidationMessageFor(model => model.TgsMedia2)%></td>
                    <td><%: Html.TextBoxFor(model => model.TgsMedia1) %>
                        <%: Html.ValidationMessageFor(model => model.TgsMedia3)%></td>
                    <td><%: Html.TextBoxFor(model => model.TgsMedia1) %>
                        <%: Html.ValidationMessageFor(model => model.TgsMedia4)%></td>
                    </tr>
                <tr>                   
                    <td>Competition</td>
                   
                    <td><%: Html.TextBoxFor(model => model.CompetitionBrand1) %>
                        <%: Html.ValidationMessageFor(model => model.CompetitionBrand1)%></td>
                    <td><%: Html.TextBoxFor(model => model.CompetitionBrand2) %>
                        <%: Html.ValidationMessageFor(model => model.CompetitionBrand2)%></td>
                    <td><%: Html.TextBoxFor(model => model.CompetitionBrand3) %>
                        <%: Html.ValidationMessageFor(model => model.CompetitionBrand3)%></td>
                    <td><%: Html.TextBoxFor(model => model.CompetitionBrand4) %>
                        <%: Html.ValidationMessageFor(model => model.CompetitionBrand4)%></td>
                </tr>
                <tr>
                     <td>Markets</td>
                   
                    <td><%: Html.TextBoxFor(model => model.Market1) %>
                        <%: Html.ValidationMessageFor(model => model.Market1)%></td>
                    <td><%: Html.TextBoxFor(model => model.Market2) %>
                        <%: Html.ValidationMessageFor(model => model.Market2)%></td>
                    <td><%: Html.TextBoxFor(model => model.Market3) %>
                        <%: Html.ValidationMessageFor(model => model.Market3)%></td>
                    <td><%: Html.TextBoxFor(model => model.Market4) %>
                        <%: Html.ValidationMessageFor(model => model.Market4)%></td>
                </tr>
            </table>
            <table class="TablePanel">
                <tr>
                    <td class="Width50px"><span> Marketing Objective</span></td>
                    <td></td>
                </tr>
                <tr>    
                    <td class="Width50px">Ad Objective</td>
                    <td class="MultiLineTextBox"><%: Html.TextBoxFor(model => model.AdObjective) %>
                        <%: Html.ValidationMessageFor(model => model.AdObjective)%></td>
                </tr>
                <tr>
                     <td>Outdoor Objective</td>
                    <td class="MultiLineTextBox"><%: Html.TextBoxFor(model => model.OutDoorObjective) %>
                        <%: Html.ValidationMessageFor(model => model.OutDoorObjective)%></td>
                </tr>
                <tr>
                     <td>Ad Mix</td>
                    <td class="MultiLineTextBox"><%: Html.TextBoxFor(model => model.AdMix) %>
                        <%: Html.ValidationMessageFor(model => model.AdMix)%></td>
                </tr>
            </table>
            <table class ="TablePanel">
                <tr>
                    <td>Layout Adaptation</td>
                    <td><%: Html.DropDownListFor(model => model.LayoutAdapLimitations, Model.YesNoList, "Select one..")%> 
                        <%: Html.ValidationMessageFor(model => model.LayoutAdapLimitations)%></td>
                    <td></td>
                    <td></td>
                    <td></td>                    
                    <td>Display Types</td>
                    <td></td>                    
                    <td  > <%: Html.CheckBoxFor(model => model.InVinyl) %>
                        <%: Html.ValidationMessageFor(model => model.InVinyl)%>Vinyl</td>                    
                    <td  > <%: Html.CheckBoxFor(model => model.InPrintBl) %>
                        <%: Html.ValidationMessageFor(model => model.InPrintBl)%>Print BL</td>
                    <td  ><%: Html.CheckBoxFor(model => model.InPrintFl) %>
                        <%: Html.ValidationMessageFor(model => model.InPrintFl)%>Print FL
                    </td>
                </tr>
                <tr>
                    <td>Preferred Ratio</td>
                    <td> <%: Html.CheckBoxFor(model => model.Ratio11) %>
                        <%: Html.ValidationMessageFor(model => model.Ratio11)%>1:1</td>
                    <td><%: Html.CheckBoxFor(model => model.Ratio12) %>
                        <%: Html.ValidationMessageFor(model => model.Ratio12)%>1:2</td>
                    <td> <%: Html.CheckBoxFor(model => model.Ratio13)%>
                        <%: Html.ValidationMessageFor(model => model.Ratio13)%>1:3</td>
                    <td>Preferred size</td>
                    <td></td>
                    <td></td>
                    <td ><%: Html.CheckBoxFor(model => model.InPainting) %>
                        <%: Html.ValidationMessageFor(model => model.InPainting)%>In Painting</td>
                    <td ><%: Html.CheckBoxFor(model => model.Complete) %>
                        <%: Html.ValidationMessageFor(model => model.Complete)%>Complete</td>
                    <td ><%: Html.CheckBoxFor(model => model.PartVinyl) %>
                        <%: Html.ValidationMessageFor(model => model.PartVinyl)%>Part Vinyl</td>
                </tr>
                <tr>
                    <td></td>
                    <td><%: Html.CheckBoxFor(model => model.Ratio14) %>
                        <%: Html.ValidationMessageFor(model => model.Ratio14)%>1:4</td>
                    <td> <%: Html.CheckBoxFor(model => model.Ratio34) %>
                        <%: Html.ValidationMessageFor(model => model.Ratio34)%>3:4</td>
                    <td> <%: Html.CheckBoxFor(model => model.RatioAll) %>
                        <%: Html.ValidationMessageFor(model => model.RatioAll)%>All</td>
                    <td><%: Html.TextBoxFor(model => model.PreferredSize) %>
                        <%: Html.ValidationMessageFor(model => model.PreferredSize)%></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
            </table>
            <table class="TablePanel">
                <tr>
                    <td colspan="2">Preferred Media Vehicle</td>
                    
                    <td></td>
                    <td>Additional Information Discussed</td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td><%: Html.CheckBoxFor(model => model.BillBoard)  %>
                        <%: Html.ValidationMessageFor(model => model.BillBoard)%>Bill Board</td>
                    <td><%: Html.CheckBoxFor(model => model.Buses) %>
                        <%: Html.ValidationMessageFor(model => model.Buses)%>Buses</td>
                    <td><%: Html.CheckBoxFor(model => model.PoleKiosks) %>
                        <%: Html.ValidationMessageFor(model => model.PoleKiosks)%>Pole Kiosks</td>
                    <td rowspan="3" valign="top"><%: Html.TextBoxFor(model => model.AdditionalInfo) %>
                        <%: Html.ValidationMessageFor(model => model.AdditionalInfo)%> </td>                    
                </tr>
                <tr>
                    <td><%: Html.CheckBoxFor(model => model.BusQShelter)  %>
                        <%: Html.ValidationMessageFor(model => model.BusQShelter)%>Bus Q shelter</td>
                    <td><%: Html.CheckBoxFor(model => model.TransitMedia)  %>
                        <%: Html.ValidationMessageFor(model => model.TransitMedia)%>Transit Media</td>
                    <td><%: Html.CheckBoxFor(model => model.PublicUtitities)  %>
                        <%: Html.ValidationMessageFor(model => model.PublicUtitities)%>Public Utilities</td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td><%: Html.CheckBoxFor(model => model.OtherMediaVehicle)%>
                        <%: Html.ValidationMessageFor(model => model.OtherMediaVehicle)%>Others</td>
                    <td colspan="2"><%: Html.TextBoxFor(model => model.Others)%>
                        <%: Html.ValidationMessageFor(model => model.OtherMediaVehicle)%></td>                    
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
                
            </table>
            <table class="TablePanel">
             <tr>
                    <td colspan="8"><%: Html.CheckBoxFor(model => model.IsInActive)%>
                        <%: Html.ValidationMessageFor(model => model.IsInActive)%>Deactivate</td>                        
                </tr>
            </table>
            </div> 
            <div id="fogLoaderimg"></div>              
                <input type="submit" value="Save" onclick="javascript:LoadSpinnerImg('fogLoaderimg');"  />
            
        </fieldset>

    <% } %>

    <div>
        <%: Html.ActionLink("Back to List", "Index") %>
    </div>
</div>
</div>
</asp:Content>

