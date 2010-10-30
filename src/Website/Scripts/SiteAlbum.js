var $imagesUnUsed = $("#imagesUnUsed"),
			$siteImages = $("#siteImages");

function attachDraggabletoUnattached() {
    $("#imagesUnUsed li").draggable({
        cancel: "a.ui-icon", // clicking an icon won't initiate dragging
        revert: "invalid", // when not dropped, the item will revert back to its initial position
        containment: $("#demo-frame").length ? "#demo-frame" : "document", // stick to demo-frame if present
        helper: "clone",
        cursor: "move"
    });

        // let the imagesUnUsed be droppable as well, accepting items from the siteImages
     $("#imagesUnUsed").droppable({        
        activeClass: "custom-state-active",
        drop: function (event, ui) {
            unAttachImage(ui.draggable);
        }
    });
    }
function attachDraggabletoAttached()
{
   $("#siteImages li").draggable({
        cancel: "a.ui-icon", // clicking an icon won't initiate dragging
        revert: "invalid", // when not dropped, the item will revert back to its initial position
        containment: $("#demo-frame").length ? "#demo-frame" : "document", // stick to demo-frame if present
        helper: "clone",
        cursor: "move"
    });
    // let the siteImages be droppable, accepting the imagesUnUsed items
    $("#siteImages").droppable({
        activeClass: "ui-state-highlight",
        drop: function (event, ui) {
            attachImage(ui.draggable);
        }
    });
}


    // image deletion function
    //var recycle_icon = "<a href='link/to/recycle/script/when/we/have/js/off' title='Recycle this image' class='ui-icon ui-icon-refresh'>Recycle image</a>";
function attachImage($item) {    
	    $siteImages = $("#siteImages");
        $item.fadeOut(function () {
            var $list = $("ul", $siteImages).length ?
					$("ul", $siteImages) :
					$("<ul class='imagesUnUsed ui-helper-reset'/>").appendTo($siteImages);

            //$item.find("a.ui-icon-siteImages").remove();
            $item.appendTo($list).fadeIn(function () {
                $item                
						.animate({ width: "96px" })
						.find("img")
							.animate({ height: "72px" });
            });
        });
        var splittedstring = $item.children("img").attr("src").split("/");
        var title = $item.children("img").attr("alt");
        var photoid = $item.children("img").attr("id");
        var len = splittedstring.length;
        var planCityId=  $("#PlanCityId").val();
        if (len == 0)
            return;
        $.ajax({ url: rootPath + "Photo.aspx/AttachSite/?siteId=" + selectedSiteId + "&filePath=" + splittedstring[len - 1] + "&title=" + title + "&tags=" + "&photoId=" + photoid + "&planCityId=" + planCityId,
            success: function (data, result) {
                if (!result) alert('Failure to retrieve the Sites.');
            },
            error: function (data) {
                alert("Failure attacing photo to site.");
            }
        });
    }

    // image recycle function
    //var siteImages_icon = "<a href='link/to/siteImages/script/when/we/have/js/off' title='Delete this image' class='ui-icon ui-icon-siteImages'>Delete image</a>";
    function unAttachImage($item) {
        $imagesUnUsed = $("#imagesUnUsed");
        $item.fadeOut(function () {
            var $list = $("ul", $imagesUnUsed).length ?
					$("ul", $imagesUnUsed) :
					$("<ul class='siteImages ui-helper-reset'/>").appendTo($imagesUnUsed);

            //$item.find("a.ui-icon-siteImages").remove();
            $item.appendTo($list).fadeIn(function () {
                $item
						.animate({ width: "106px" })
						.find("img")
							.animate({ height: "82px" });
            });
        });
        var splittedstring = $item.children("img").attr("src").split("/");
        var title = $item.children("img").attr("alt");
        var photoid = $item.children("img").attr("id");
        var planCityId = $("#PlanCityId").val();
        var len = splittedstring.length;
        if (len == 0)
            return;
        $.ajax({ url: rootPath + "Photo.aspx/UnAttachSite/?siteId=" + selectedSiteId + "&filePath=" + splittedstring[len - 1] + "&title=" + title + "&tags=" + "&photoId=" + photoid + "&planCityId=" + planCityId,
            success: function (data, result) {
                if (!result) alert('Failure to retrieve the Sites.');
            },
            error: function (data) {
                alert("Failure attacing photo to site.");
            }
        });
    }

    // image preview function, demonstrating the ui.dialog used as a modal window
    function viewLargerImage($link) {
        var src = $link.attr("href"),
				title = $link.siblings("img").attr("alt"),
				$modal = $("img[src$='" + src + "']");

        if ($modal.length) {
            $modal.dialog("open");
        } else {
            var img = $("<img alt='" + title + "' width='384' height='288' style='display: none; padding: 8px;' />")
					.attr("src", src).appendTo("body");
            setTimeout(function () {
                img.dialog({
                    title: title,
                    width: 400,
                    modal: true
                });
            }, 1);
        }
    }

    
	
