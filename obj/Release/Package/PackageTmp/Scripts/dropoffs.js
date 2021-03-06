﻿$(document).ready(function () {

    $("#btnDropOffs").on("click", function () {

        $("#newCustomer").show();
        $("#toolbox").hide();
        $("#title").hide();
        $('#listCustomer').empty();

        dropOffs();
    });

    function dropOffs() {

        carrentals.CarRental1.getDropOffs(localStorage['companyCode'], function (result) {

            var li = "";
            for (var i = 0; i < result.length; i++) {

                var descriptionDiv = "<div class='listFont' style='font-size:18px;font-weight:bold;color:#3B6078;position:relative;float:left;margin:10px;top:35px;'><span>" + result[i].description + "</span></div>";

                var photoDiv = "<div style='position:relative;float:left;margin:10px;'>";
                photoDiv = photoDiv + "<div style='position:relative;float:left;clear:both;'>" + "<img src=" + result[i].vehiclePhoto + " width='160px' height='80px' alt='' /></div>";
                photoDiv = photoDiv + "<div class='listFont' style='position:relative;margin:auto;width:150px;left:18px;clear:both;font-size:10px;'><span style='color:#000'>" + result[i].vehiclename + "</span></div>";
                photoDiv = photoDiv + "</div>";

                var coverDivOptions = "<div style='position:relative;float:left;margin:10px;'>";
                var divOptions = "<div style='position:relative;float:left;margin:10px;clear:both'>";

                /*Passengers*/
                divOptions = divOptions + "<div style='position:relative;float:left;margin:2px;'>"
                divOptions = divOptions + "<div style='position:relative;float:left;margin:2px;clear:both'>" + "<img src='../images/small/passenger.png' width='40px' height='40px' alt='' /></div>";
                divOptions = divOptions + "<div class='listFont' style='position:relative;margin:auto;clear:both;width:10px;font-size:11px'>" + result[i].passengers + "</div>";
                divOptions = divOptions + "</div>"

                /*Luggage*/
                divOptions = divOptions + "<div style='position:relative;float:left;margin:2px;'>"
                divOptions = divOptions + "<div style='position:relative;float:left;margin:2px;clear:both'>" + "<img src='../images/small/luggage.png' width='40px' height='40px' alt='' /></div>";
                divOptions = divOptions + "<div class='listFont' style='position:relative;margin:auto;clear:both;width:10px;font-size:11px'>" + result[i].luggage + "</div>";
                divOptions = divOptions + "</div>"

                /*doors*/
                divOptions = divOptions + "<div style='position:relative;float:left;margin:2px;'>"
                divOptions = divOptions + "<div style='position:relative;float:left;margin:2px;clear:both'>" + "<img src='../images/small/doors.png' width='40px' height='40px' alt='' /></div>";
                divOptions = divOptions + "<div class='listFont' style='position:relative;margin:auto;clear:both;width:10px;font-size:11px'>" + result[i].doors + "</div>";
                divOptions = divOptions + "</div>"

                /*transmission*/
                divOptions = divOptions + "<div style='position:relative;float:left;margin:2px;'>"
                divOptions = divOptions + "<div style='position:relative;float:left;margin:2px;clear:both'>" + "<img src='../images/small/transmission.png' width='40px' height='40px' alt='' /></div>";
                divOptions = divOptions + "<div class='listFont' style='position:relative;margin:auto;clear:both;width:10px;font-size:11px'>" + result[i].transmission + "</div>";
                divOptions = divOptions + "</div>"

                divOptions = divOptions + "</div>";

                var divOptions2 = "<div style='position:relative;float:left;margin:10px;clear:both'>";

                /*gas*/
                divOptions2 = divOptions2 + "<div style='position:relative;float:left;margin:2px;'>"
                divOptions2 = divOptions2 + "<div style='position:relative;float:left;margin:2px;clear:both'>" + "<img src='../images/small/gas.png' width='40px' height='40px' alt='' /></div>";
                divOptions2 = divOptions2 + "<div class='listFont' style='position:relative;margin:auto;clear:both;width:10px;font-size:11px'>" + result[i].gastype + "</div>";
                divOptions2 = divOptions2 + "</div>"

                /*gas*/
                divOptions2 = divOptions2 + "<div style='position:relative;float:left;margin:2px;'>"
                divOptions2 = divOptions2 + "<div style='position:relative;float:left;margin:2px;clear:both'>" + "<img src='../images/small/garmin.png' width='40px' height='40px' alt='' /></div>";
                divOptions2 = divOptions2 + "<div class='listFont' style='position:relative;margin:auto;clear:both;width:10px;font-size:11px'>" + result[i].garmin + "</div>";
                divOptions2 = divOptions2 + "</div>"

                /*babyseat*/
                divOptions2 = divOptions2 + "<div style='position:relative;float:left;margin:2px;'>"
                divOptions2 = divOptions2 + "<div style='position:relative;float:left;margin:2px;clear:both'>" + "<img src='../images/small/babyseat.png' width='40px' height='40px' alt='' /></div>";
                divOptions2 = divOptions2 + "<div class='listFont' style='position:relative;margin:auto;clear:both;width:10px;font-size:11px'>" + result[i].garmin + "</div>";
                divOptions2 = divOptions2 + "</div>"

                divOptions2 = divOptions2 + "</div>";

                coverDivOptions = coverDivOptions + divOptions + divOptions2 + "</div>";

                var otherStuff = "<div style='position:relative;float:left;margin:10px;'>";
                otherStuff = otherStuff + "<div class='listFont' style='top:12px;font-size:11px;position:relative;float:left;margin:2px;clear:both;color:red;font-weight:bold'>" + result[i].name.toUpperCase() + "</div>";
                otherStuff = otherStuff + "<div class='listFont' style='top:12px;font-size:11px;position:relative;float:left;margin:2px;clear:both;'>" + result[i].days + " days" + "</div>";
                otherStuff = otherStuff + "<div class='listFont' style='top:12px;font-size:11px;position:relative;float:left;margin:2px;clear:both;'>" + result[i].price + " USD" + "</div>";
                otherStuff = otherStuff + "</div>";

                var cancel = "<div style='position:relative;float:left;margin:10px;'>";
                cancel = cancel + "<div id='" + result[i].bookingId + "&" + result[i].vehicleId + "' class='returned emptycheckbox' style='width:60px;height:60px;top:12px;font-size:11px;position:relative;float:left;clear:both;margin:auto;cursor:pointer'></div>";
                cancel = cancel + "<div style='width:45px;height:60px;top:14px;font-size:11px;position:relative;clear:both;margin:auto;cursor:pointer'>CANCEL</div>";
                cancel = cancel + "</div>";

                var contactinfo = "<div class='phone' style='position:relative;float:left;margin:10px;'>";
                contactinfo = contactinfo + "<div style='width:45px;top:14px;font-size:11px;position:relative;clear:both;margin:auto;cursor:pointer'><b>" + "CONTACT INFO" + "</b></div>";
                contactinfo = contactinfo + "<div style='width:45px;top:14px;font-size:11px;position:relative;clear:both;margin:auto;cursor:pointer'><b>DROP OFF LOCATION:</b>" + result[i].dropofflocation + "</div>";
                contactinfo = contactinfo + "<div style='width:45px;top:14px;font-size:11px;position:relative;clear:both;margin:auto;cursor:pointer'><b>DROP OFF DATE:</b>" + result[i].pickupdate + "</div>";
                contactinfo = contactinfo + "<div style='width:45px;top:14px;font-size:11px;position:relative;clear:both;margin:auto;cursor:pointer'><b>HOME:</b>" + result[i].homePhone + "</div>";
                contactinfo = contactinfo + "<div style='width:45px;top:14px;font-size:11px;position:relative;clear:both;margin:auto;cursor:pointer'><b>MOBILE:</b>" + result[i].mobilePhone + "</div>";
                contactinfo = contactinfo + "</div>";

                li += "<li style='height:160px'>" + descriptionDiv + photoDiv + coverDivOptions + otherStuff + contactinfo + "</li>";
                //divOptions

            }

            $('#listCustomer').html(li);
            $('#listCustomer').listview('refresh');

        });
    }
});