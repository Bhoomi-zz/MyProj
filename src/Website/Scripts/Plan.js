/// <reference path="jquery-1.4.2.js" />

/// <reference path="jquery.validate.js" />
/// <reference path="jquery.fogLoader.0.9.1/jquery.fogLoader.0.9.1.js" />




$(document).ready(function () {
    $("#menu-plan").toggleClass("closed");
    $("#h-menu-plan").addClass("selected");

    $("#BriefNo").change(function () {
        var briefid = $("#BriefNo").val();
                

        $.ajax({
            url: rootPath + 'Shared.aspx/ValidateBriefAndGetClientInfoByBriefId?briefNo=' + briefid + "&planDetailsId=" + $("#PlanDetailsId").val(),
            success: function (data, result) {
                if (!result) alert('Failure to retrieve the Sites.');
                if (data == "") {
                    $("#ClientId").val("");
                    $("#ClientName").val("");
                }
                else {
                    var clientValues = data.split(";");
                    if (clientValues[0] == "1") {
                        alert("Plan is already created for this BriefNo for  Client : " + clientValues[2]);
                        $("#BriefNo").val("");
                    }
                    else {
                        $("#ClientId").val(clientValues[1]);
                        $("#ClientName").val(clientValues[2]);
                    }
                }
            },
            error: function (data, result) {
                alert('Failure to retrive data');
            }
        });
    });   
});
