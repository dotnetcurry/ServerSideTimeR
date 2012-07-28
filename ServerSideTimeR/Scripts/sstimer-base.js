/// <reference path="../knockout-2.0.0.debug.js" />
/// <reference path="../jquery-ui-1.8.11.js" />
/// <reference path="../jquery-1.6.4.js" />
/// <reference path="../ajax-util.js" />
/// <reference path="../ko-protected-observable.js" />

$(function () {
    var element = $(".username");
    var elementText = "";
    if (element && element.length != 0) {
        elementText = element[0].innerHTML;
    }
    if (elementText) {
        alert("You Logged in as " + elementText);
    }
    else {
        alert("Please Login First");
    }

});