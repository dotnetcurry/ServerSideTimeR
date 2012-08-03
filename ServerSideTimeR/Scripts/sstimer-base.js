/// <reference path="../knockout-2.0.0.debug.js" />
/// <reference path="../jquery-ui-1.8.11.js" />
/// <reference path="../jquery-1.6.4.js" />
/// <reference path="../ajax-util.js" />
/// <reference path="../ko-protected-observable.js" />

var page = 1;

$(function () {

    var hub = $.connection.serverSideTimerHub;
    $.connection.hub.start();

    hub.joined = function (userName, time) {
        $("#statusLabel").text("Joined");
        $("#connectionId").text(userName);
        enableForm();
    }

    hub.rejoined = function (userName, time) {
        $("#statusLabel").text("Rejoined");
        $("#connectionId").text(userName);
        enableForm();
    }

    hub.leave = function (userName, time) {
        $("#statusLabel").text("Left");
        $("#connectionId").text(userName);
        disableForm();
    }

    hub.tick = function (connectionId, userId, dateTime) {
        $("#userId").text(userId)
        $("#connectionId").text(connectionId);
        $("#onlineSince").text(dateTime);
    }

    $('#nextButton').on('click', function () {
        showNextPage();
        $("#pageNumber").text(page);
    });

    $('#previous').on('click', function () {
        showPreviousPage();
        $("#pageNumber").text(page);
    });
});

disableForm = function () {
    $("#nextButton").attr('disabled', 'disabled');
    $("#previous").attr('disabled', 'disabled');
    showPage(page);
}

enableForm = function () {
    $("#nextButton").removeAttr('disabled');
    $("#previous").removeAttr('disabled');
    showPage(page);
}

showNextPage = function () {
    if (page < 3) {
        page = page + 1;
    }
    showPage(page);
    manageButtons(page);
    return false;
}

showPreviousPage = function () {
    if (page > 1) {
        page = page - 1;
    }
    showPage(page);
    manageButtons(page);
}

successCallback = function (data) {
    $("#serverSideData").text(data.name + ": " + data.number);
}

showPage = function (pageNumber) {
    $("#page" + pageNumber).show();
    for (var i = 1; i < 4; i++) {
        if (i != pageNumber) {
            $("#page" + i).hide();
        }
    }
    ajaxAdd("/Home/GetPageInfo", '{ "page":' + pageNumber + "}", successCallback);
}

manageButtons = function (pageNumber) {
    if (pageNumber == 1) {
        $("#nextButton").removeAttr('disabled');
        $("#previous").attr('disabled', 'disabled'); ;
    }
    else if (pageNumber == 3) {
        $("#nextButton").attr('disabled', 'disabled');
        $("#previous").removeAttr('disabled');
    }
    else {
        $("#nextButton").removeAttr('disabled');
        $("#previous").removeAttr('disabled');
    }
}

