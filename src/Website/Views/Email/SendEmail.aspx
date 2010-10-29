<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Website.ViewModel.Email.EmailViewModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../../Scripts/jquery.media.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.metadata.min.js" type="text/javascript"></script>
    <script type ="text/javascript">
    
        function SendEmail() {
            LoadSpinnerImg('fogLoaderimg');
            planDetailsId = $("#PlanDetailsId").val();
            fromaddress = $("#FromAddress").val();
            toaddress = $("#ToAddress").val();
            subject = $("#Subject").val();
            content1 = escape($("#Content").val());
            fileName = $("#FileName").val();
            $.ajax({
                url: rootPath + "Email.aspx/SendEmailWithAttachment/?planDetailsId=" + planDetailsId + "&fromAddress=" + fromaddress + "&toAddress=" + toaddress + "&subject=" + subject + "&content=" + content1 + "&fileName=" + fileName,
                success: function (data, result) {
                    if (!result) alert('Failure to retrieve the Cities.');
                    UnLoadSpinnerImg('fogLoaderimg');
                    alert('Email sent Successfully');

                },
                error: function (data, result) {
                    UnLoadSpinnerImg('fogLoaderimg');
                    alert('Error while sending Email');
                }
            });
        }

        $(document).ready(function () {
            $('a.media').media({ width: "100%", height: 700 });
        });
    </script>
    <style>
           #Content
           {
                width:100%;
                height: 100px;
           }
           #FromAddress, #Subject, #ToAddress
           {
               width:100%;
           }           
           
    </style>
<div class="box"> 
					<!-- box / title --> 
					<div class="title"> 
						<h5>Emails Index </h5>                        	
					</div> 
					<!-- end box / title --> 
                   <div class="form box" > 
    <table>
        <tr>          
            <td style="padding:3px 3px 3px 3px">Plan# </td>
            <td style="padding:3px 3px 3px 3px"><%: Html.DisplayFor(x=> x.PlanNo) %>
            <%:Html.HiddenFor(x=>x.PlanDetailsId) %></td>
            <%:Html.HiddenFor(x=>x.FileName) %>
            </tr>
            <tr>
            <td style="padding:3px 3px 3px 3px">Client</td>
            <td style="padding:3px 3px 3px 3px"><%: Html.DisplayFor(x => x.Client) %></td>
            </tr>
        <tr>     
            <td style="padding:3px 3px 3px 3px">From : </td>
            <td style="padding:3px 3px 3px 3px"><%:Html.TextBoxFor(x=>x.FromAddress) %></td>
        </tr>
        <tr>
            <td style="padding:3px 3px 3px 3px">To :</td>
            <td style="padding:3px 3px 3px 3px"><%: Html.TextBoxFor(x=>x.ToAddress) %></td>
        </tr>
        <tr>    
            <td style="padding:3px 3px 3px 3px">Subject :</td>
            <td style="padding:3px 3px 3px 3px"><%: Html.TextBoxFor(x=> x.Subject) %></td>
        </tr>
        <tr>
            <td></td>
            <td style="padding:3px 3px 3px 3px"><%: Html.TextAreaFor(x=> x.Content) %></td>
        </tr>
    </table>
    <div id="fogLoaderimg"></div>  
    Report : <span style="float:right"> <input type="button" value="Send Email" onclick="javascript:SendEmail(); return false;" /> </span>
    
    <div>
    <br /><br />
    <a class="media" href ="<%=Url.Content("~/Reports/") + Model.FileName%>">PDF File</a> 
    </div>
    </div>
    </div>
</asp:Content>
