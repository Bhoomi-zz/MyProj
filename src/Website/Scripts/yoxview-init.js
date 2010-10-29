var _yoxviewPath = getYoxviewPath();
var cssLink = top.document.createElement("link");
cssLink.setAttribute("rel", "Stylesheet");
cssLink.setAttribute("type", "text/css");
cssLink.setAttribute("href", _yoxviewPath + "yoxview.css");
top.document.getElementsByTagName("head")[0].appendChild(cssLink);

function LoadScript(url)
{
	document.write( '<scr' + 'ipt type="text/javascript" src="' + url + '"><\/scr' + 'ipt>' ) ;
}

if (typeof jQuery == "undefined")
    LoadScript("http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js");
    
LoadScript(_yoxviewPath + "jquery.yoxview-2.09.js");

function getYoxviewPath()
{
    var scripts = document.getElementsByTagName("script");
    var regex = /(.*\/)yoxview-init\.js/i;
    for(var i=0; i<scripts.length; i++)
    {
        var currentScriptSrc = scripts[i].src;
        if (currentScriptSrc.match(regex))
            return currentScriptSrc.match(regex)[1];
    }
    
    return null;
}
// Remove the next line's comment to apply yoxview without knowing jQuery to all containers with class 'yoxview':
//LoadScript(_yoxviewPath + "yoxview-nojquery.js"); 