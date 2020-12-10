$(document).ready(function () {
    var location = "";
    var markers = [];
    var map = null;


    $("#btnDistress").on('click', function () {

        //i = 8;
        $("#distressListView").empty();
        $("#distress").show();
        $(".cover").show();


        distressCall();

    });
    $("#distress").width(screen.width - 600).height(screen.height - 450);


    $(".distresslist").css({
        'width': '90%',
        'height': '54%',
        'left': '5%'
    });
    $("#distressCover").css({
        'width': '95%',
        'height': '95%',
        'left': '1%'
    });
    function setMapOnAll(map) {
        for (var i = 0; i < markers.length; i++) {
            markers[i].setMap(map);
        }
    }

    // Removes the markers from the map, but keeps them in the array.
    function clearMarkers() {
        setMapOnAll(null);
    }
    function setAddress(result, i) {
        var geocoder = new google.maps.Geocoder;


        var latlng = { lat: parseFloat(result[i].lat), lng: parseFloat(result[i].lng) };
        geocoder.geocode({ 'location': latlng }, function (results, status) {

            $("#distressLocation" + i).css({
                'width': '200px',
                'height': '33px',
                'color': '#fff'
            });
            if (status === 'OK') {
                if (results[0]) {

                    $(".distressLocation" + i).text("Current Location: " + results[0].formatted_address);


                } else {
                    swal("Oops!!", "No results found", "error");

                }
            } else {
                swal("Oops!!", 'Geocoder failed due to: ' + status, "error");

            }

        });
    }


    $(".stopDistress").on("click", function () {
        $("#distress").hide();
        $(".cover").hide();
    });

    function distressCall() {
        var li = "";
        LocatorAdmin.LocatorAdmin.executeDistressCalls(function (result) {

            for (var i = 0; i < result.length; i++) {


                li += "<li style='background:#292929'><table><tr><td><div style='position:relative;float:left;margin:10px;'><img width='180px' height='180px' style='-webkit-filter: blur(10px);filter: blur(10px);border-radius:50%' src='data:image/png;base64, " + result[i].photo + "' /></div><div style='position:relative;float:left;margin:10px;top:-13px;'><div class='menu' style='positon:relative;float:left;margin:10px;clear:both;'><span class='menu' style='color:#fff'>Active</span></div><div class='toggleItemList' style='positon:relative;float:left;margin:10px;clear:both;top:-17px;'><div id =" + result[i].id + "  class='togcls circleInActive indexActive" + result[i].id + "'></div></div>" +
                "<div style='position:relative;float:left;clear:both;left:9px;top:-10px;color:#fff'><span class='menu'>Driver In</span></div><div class='toggleItemList' style='positon:relative;float:left;margin:10px;clear:both;top:-15px;'><div id =" + result[i].id + "  class='togcls driverIn indexPosition" + result[i].id + "'></div></div>" +
                "<div style='positon:relative;float:left;margin:10px;clear:both;top:-3px;' id =" + result[i].id + "  class='togcls mapSelect'><span class='menu' id ='" + result[i].id + "' style='position:relative;font-weight:normal;color:#fff;left:22px;top:8px;'>COORDINATES</span></div>" +

                "</div></td><td><div class='stopDistress menu' id=" + result[i].id + " style='position:relative;top:-85px;left:100px'><span style='position:relative;color:#fff;top:9px;left:40px;'>STOP DISTRESS</span></div></td></tr><tr><td><div style='font-size:20px;position:relative;float:left;clear:both;color:#fff;left:32px;top:-38px;'><span class='menu'  style='font-size:20px;'>" + result[i].name + "</span></div></td></tr>" +
                "<tr><td style='height:37px;'></td></tr>" +
                "</table>" +
                "<div style='position:relative;float:left;margin:10px;'><span class='menu' style='border:0px solid #fff;border-radius:5px;width:150px;height:33px;font-size:14px;color:#fff;margin:10px;'>Contact:" + result[i].contactNumber + "</span></div><div style='position:relative;float:left;margin:10px;'><span class='menu distressLocation" + i + "' style='border:0px solid #fff;border-radius:5px;width:200px;height:33px;font-size:14px;margin:5px;color:#fff'></span></div>" +


            "</li>";


            }

            $(".distresslist").html(li);
            $(".distresslist").listview('refresh');

            $(".stopDistress").on("click", function () {

                LocatorAdmin.LocatorAdmin.stopDistress($(this).attr("id"));
                distressCall();

            });

            $(".mapSelect").on("click", function () {


                LocatorAdmin.LocatorAdmin.SelectDriverByID($(this).attr("id"), function (result) {

                    setDistressAddress(result[0].lat, result[0].lng);
 

                    swal("Coordinates", "Latitude: " + result[0].lat + " Longitude: " + result[0].lng, "success");
                    $("#distress").hide();
                    $(".cover").hide();
                });

            });
            for (var k = 0; k < result.length; k++) {

                setAddress(result, k);

                $(".distressLocation" + k).on('click', function () {

                });

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
    }

    function setDistressAddress(lat, lng) {
        var geocoder = new google.maps.Geocoder;


        var latlng = { lat: lat, lng: lng };
        geocoder.geocode({ 'location': latlng }, function (results, status) {

            if (status === 'OK') {
                if (results[0]) {

                    localStorage["location"] = results[0].formatted_address;
                    return results[0].formatted_address;
                } else {
                    swal("Oops!!", "No results found", "error");

                }
            } else {
                swal("Oops!!", 'Geocoder failed due to: ' + status, "error");

            }

        });
    }
    function processDistressLocationOnMap(location, lat, lng) {
        var addresses = [];
        var coor = [];
        var address = "";

        function clearMarkers() {
            setMapOnAll(null);
        }
        var locations = [
                  [location, lat, lng]
              ];



        var infowindow = new google.maps.InfoWindow();
        var geocoder = new google.maps.Geocoder();
        var marker, i;

        for (i = 0; i < locations.length; i++) {
            marker = new google.maps.Marker({
                position: new google.maps.LatLng(locations[i][1], locations[i][2]),
                map: map
            });

            markers.push(marker);

            google.maps.event.addListener(marker, 'click', (function (marker, i) {
                return function () {
                    alert(locations[i][0]);
                    infowindow.setContent(locations[i][0]);
                    infowindow.open(map, marker);
                }
            })(marker, i));
        }
    }
});