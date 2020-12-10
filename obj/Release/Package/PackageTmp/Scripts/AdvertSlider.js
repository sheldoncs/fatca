$(document).ready(function () {

    carrentals.CarRental1.getAllVehicles(function (result) {
        var div = "";

        $(".vehicleSingleSlider").empty();
        for (var i = 0; i < result.length; i++) {

            div += "<div class='slide' style='position:relative;margin:10px;float:left;'><img  width='250px' height='220px' src='" + result[i].vehiclePhoto + "'></div>";
           
        }
        $(".vehicleSingleSlider").html(div);
        $(".vehicleSingleSlider").slick({
            dots: false,
            infinite: true,
            centerMode: true,
            autoplay: true,
            slidesToShow: 1
        });

    });


});