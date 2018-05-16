// Write your JavaScript code.
var resizeCoef = 0.08;
var updateInterval = 6;

function resizeDown(object)
{
    if (object.upsizeID) clearInterval(object.upsizeID);

    var style = getComputedStyle(object);
    object.downsizeID = setInterval(function () {
        var width = fromPx(style.width);
        var minwidth = fromPx(style.minWidth);
        if (Math.abs(minwidth - width) <= 1)
        {
            object.style.width = toPx(minwidth);
            clearInterval(object.downsizeID);
            return;
        }
        object.style.width = toPx(width + resizeCoef * (minwidth - width));
    }, updateInterval);
}

function toPx(px)
{
    return px.toString() + "px";
}
function fromPx(px)
{
    return parseFloat(px);
}


function resizeUp(object)
{
    if (object.downsizeID) clearInterval(object.downsizeID);
    var style = getComputedStyle(object);
    object.upsizeID = setInterval(function () {
        var width = fromPx(style.width);
        var maxwidth = fromPx(style.maxWidth);
        if (Math.abs(maxwidth - width) <= 1) {
            object.style.width = toPx(maxwidth);
            clearInterval(object.upsizeID);
            return;
        }
        object.style.width = toPx(width + resizeCoef * (maxwidth - width));
    }, updateInterval);
}

