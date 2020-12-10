$(document).ready(function () {

    localStorage["storeId"] = 0;

    $("#closeDash").on("click", function () {

        $("#dashBoard").hide();
        $(".cover").hide();

    });

    $(".trackLocation").on("click", function () {

        if ($(this).attr("class").indexOf("circleInActive") >= 0) {

            $(this).removeClass("circleInActive");
            $(this).addClass("circleActive");
            $(this).parent().css("background", "#2ECC71");
            StartInterval();

        } else if ($(this).attr("class").indexOf("circleActive") >= 0) {

            $(this).removeClass("circleActive");
            $(this).addClass("circleInActive");
            $(this).parent().css("background", "#E74C3C");
            StopInterval();
        }

    });

    var myVar;

    function StartInterval() {
        myVar = setInterval(RefreshDriverLocation, 3000);
    }

    function StopInterval() {
        clearInterval(myVar);
    }
    function RefreshDriverLocation() {
        LocatorAdmin.LocatorAdmin.GetDriverRoute(localStorage["storeId"], function (result) {

            delivered(result);

        });
    }


    function setAddress(result) {
        var geocoder = new google.maps.Geocoder;

        var latlng = { lat: parseFloat(result[0].lat), lng: parseFloat(result[0].lng) };
        geocoder.geocode({ 'location': latlng }, function (results, status) {
            if (status === 'OK') {
                if (results[0]) {

                    $("#txtLocation").val(results[0].formatted_address);

                    $('#txtLocation').attr("disabled", "disabled");

                } else {
                    swal("Oops!!", "No results found", "error");

                }
            } else {
                swal("Oops!!", 'Geocoder failed due to: ' + status, "error");

            }
        });
    }

    $("#btnBrowseUnDelivered").on("click", function () {


        $("#dashBoard").show();

        $("#totalUnDelivered").val("");
        $("#totalUnDelivered").attr("disabled", "disabled");
        $("#total").val("");
        $("#total").attr("disabled", "disabled");
        $("#totalDelivered").val("");
        $("#totalDelivered").attr("disabled", "disabled");
        $("#txtLocation").val("");
        $("#txtLocation").attr("disabled", "disabled");
        $(".undeliveredlist").empty();


        $("#dashBoard").width(screen.width - 90).height(screen.height - 180);
        $(".undeliveredlist").width(screen.width - 720).height(screen.height - 650);
        $(".driverList").width(450).height(screen.height - 240);


        $("#totalDelivered").css({
            'width': '70px',
            'color': '#000'
        });
        $("#totalUnDelivered").css({
            'width': '50px'
        });
        $("#dvul").css({
            'width': '100%',
            'height': '16%',
            'top': '-12px'
        });

        $(".cover").show();

        LocatorAdmin.LocatorAdmin.listDrivers(function (result) {

            var li = "";
            for (var i = 0; i < result.length; i++) {

                li += "<li style='background:#292929'><table><tr><td><div style='position:relative;float:left;margin:10px;'><img width='180px' height='180px' style='-webkit-filter: blur(10px);filter: blur(10px);border-radius:50%' src='data:image/png;base64, " + result[i].photo + "' /></div><div style='position:relative;float:left;margin:10px;top:-13px;'><div class='menu' style='positon:relative;float:left;margin:10px;clear:both;'><span class='menu' style='color:#fff'>Active</span></div><div class='toggleItemList' style='positon:relative;float:left;margin:10px;clear:both;top:-17px;'><div id =" + result[i].id + "  class='togcls circleInActive indexActive" + result[i].id + "'></div></div>" +
                "<div style='position:relative;float:left;clear:both;left:9px;top:-10px;color:#fff'><span class='menu'>Driver In</span></div><div class='toggleItemList' style='positon:relative;float:left;margin:10px;clear:both;top:-15px;'><div id =" + result[i].id + "  class='togcls driverIn indexPosition" + result[i].id + "'></div></div>" +
                "<div style='positon:relative;float:left;margin:10px;clear:both;top:-3px;' id =" + result[i].id + "  class='togcls dashboardSelect'><span class='menu' style='position:relative;font-weight:normal;color:#fff;left:18px;top:8px;'>DETAILS</span></div>" +

                "</div></td></tr><tr><td><div style='font-size:20px;position:relative;float:left;clear:both;color:#fff;left:12px;top:-38px;'><span class='menu'  style='font-size:20px;'>" + result[i].name + "</span></div></td></tr></table>" +
            "</li>";
            }

            $(".driverList").html(li);
            $(".driverList").listview('refresh');

            for (var k = 0; k < result.length; k++) {


                if (result[k].driverInOut == 1) {

                    $(".indexPosition" + result[k].id).removeClass("driverIn");
                    $(".indexPosition" + result[k].id).addClass("driverOut");
                    $(".indexPosition" + result[k].id).parent().css("background", "#2ECC71");

                } else if (result[k].driverInOut == 0) {

                    $(".indexPosition" + result[k].id).removeClass("driverOut");
                    $(".indexPosition" + result[k].id).addClass("driverIn");
                    $(".indexPosition" + result[k].id).parent().css("background", "#E74C3C");
                }


            }

            $(".dashboardSelect").on("click", function () {


                var id = $(this).attr("id");
                localStorage["storeId"] = id;
                LocatorAdmin.LocatorAdmin.GetDriverRoute(id, function (result) {

                    delivered(result);

                });

                LocatorAdmin.LocatorAdmin.SelectDriverByID(id, function (result) {

                    if (result.length != 0) {
                        setAddress(result);

                        LocatorAdmin.LocatorAdmin.ItemsDeliveredByDriver(id, true, function (result) {

                            $("#totalDelivered").val(result);

                            $("#totalDelivered").attr("disabled", "disabled");
                            LocatorAdmin.LocatorAdmin.ItemsDeliveredByDriver(id, false, function (iresult) {
                                $("#totalUnDelivered").val(iresult);
                                $("#totalUnDelivered").attr("disabled", "disabled");
                                $("#total").val(Number($("#totalUnDelivered").val()) + Number($("#totalDelivered").val()));
                                $("#total").attr("disabled", "disabled");
                            });

                        });

                    } else {

                        $("#totalUnDelivered").val("");
                        $("#totalUnDelivered").attr("disabled", "disabled");
                        $("#total").val("");
                        $("#total").attr("disabled", "disabled");
                        $("#totalDelivered").val("");
                        $("#totalDelivered").attr("disabled", "disabled");
                        $("#txtLocation").val("");
                        $("#txtLocation").attr("disabled", "disabled");
                        $(".undeliveredlist").empty();
                        swal("Oops!!", "Location Not Detected for Driver", "error");
                    }

                });


            });

            for (var j = 0; j < result.length; j++) {

                if (result[j].enable == true) {
                    $(".indexActive" + result[j].id).removeClass("circleInActive");
                    $(".indexActive" + result[j].id).addClass("circleActive");
                    $(".indexActive" + result[j].id).parent().css("background", "#2ECC71");
                } else {
                    $(".indexActive" + result[j].id).removeClass("circleActive");
                    $(".indexActive" + result[j].id).addClass("circleInActive");
                    $(".indexActive" + result[j].id).parent().css("background", "#E74C3C");
                }

                $(".indexActive" + result[j].id).on("click", function () {

                    if ($(this).attr("class").indexOf("circleInActive") > 0) {
                        $(this).removeClass("circleInActive");
                        $(this).addClass("circleActive");
                        $(this).parent().css("background", "#2ECC71");
                    } else if ($(this).attr("class").indexOf("circleActive") > 0) {
                        $(this).removeClass("circleActive");
                        $(this).addClass("circleInActive");
                        $(this).parent().css("background", "#E74C3C");
                    }



                });
            }
        });

        function delivered(route) {

            LocatorAdmin.LocatorAdmin.GatherDeliveredItems(route, function (result) {
                listOfDeliveryItems(result);
            });

        }

        function listOfDeliveryItems(result) {



            $(".undeliveredlist").empty();
            var li = "";
            if (result != 0) {
                for (var i = 0; i < result.length; i++) {
                    li += "<li  style='background:#454444;color:#fff'><div class='undel menu' style='width:510px'><table style='width:40%;'><tr><td style='width:150px;'>" + result[i].name.toUpperCase() + "</td><td style='position:absolute;left:160px;width:60px;'>" + result[i].icase + "</td><td style='position:absolute;left:190px;width:300px;'>" + result[i].address.toUpperCase() + "</td><td style='position:absolute;left:500px !important;top:3px;width:150px;'><div class='toggleItemList' style='position:relative;top:0px;'><div id =" + result[i].id + "  class='circleInActive togcls" + result[i].id + "'></div></div></td></tr></table></div></li>";

                }
            } else {
                li += "<li  style='background:#454444;color:#fff'><div class='undel menu' style='width:510px'></div></li>";

            }

            $(".undeliveredlist").html(li);
            $(".undeliveredlist").listview('refresh');

            for (var j = 0; j < result.length; j++) {

                if (result[j].delivered == 1) {

                    $(".togcls" + result[j].id).removeClass("circleInActive");
                    $(".togcls" + result[j].id).addClass("circleActive");
                    $(".togcls" + result[j].id).parent().css("background", "#2ECC71");

                } else {

                    $(".togcls" + result[j].id).removeClass("circleActive");
                    $(".togcls" + result[j].id).addClass("circleInActive");
                    $(".togcls" + result[j].id).parent().css("background", "#E74C3C");

                }

                $(".togcls" + result[j].id).on('click', function () {

                    var activity = $(this).attr("class");

                    if (activity.indexOf("circleInActive") >= 0) {
                        $(this).removeClass("circleInActive");
                        $(this).addClass("circleActive");
                        $(this).parent().css("background", "#2ECC71");

                        

                        LocatorAdmin.LocatorAdmin.updateDelivery($(this).attr("id"), 1, function () {
                            $("#totalUnDelivered").val(Number($("#totalUnDelivered").val()) - 1);
                            $("#totalDelivered").val(Number($("#totalDelivered").val()) + 1);
                            $("#totalUnDelivered").attr("disabled", "disabled");
                            $("#total").val(Number($("#totalUnDelivered").val()) + Number($("#totalDelivered").val()));
                            $("#total").attr("disabled", "disabled");
                        });
                    } if (activity.indexOf("circleActive") >= 0) {
                        $(this).removeClass("circleActive");
                        $(this).addClass("circleInActive");
                        $(this).parent().css("background", "#E74C3C");
                        LocatorAdmin.LocatorAdmin.updateDelivery($(this).attr("id"), 0, function () {
                            $("#totalUnDelivered").val(Number($("#totalUnDelivered").val()) + 1);
                            $("#totalDelivered").val(Number($("#totalDelivered").val()) - 1);
                            $("#totalUnDelivered").attr("disabled", "disabled");
                            $("#total").val(Number($("#totalUnDelivered").val()) + Number($("#totalDelivered").val()));
                            $("#total").attr("disabled", "disabled");
                        });
                    }


                });

            }
        }
    });



});