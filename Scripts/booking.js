$(document).ready(function () {

      function format1(n, currency) {
        return currency + " " + n.toFixed(2).replace(/./g, function (c, i, a) {
            return i > 0 && c !== "." && (a.length - i) % 3 === 0 ? "," + c : c;
        });
    }

    $(window).scroll(function () {
        if ($(this).scrollTop()) {
            $('#toTop:hidden').stop(true, true).fadeIn();
        } else {
            $('#toTop').stop(true, true).fadeOut();
        }
    });



    $('#toTop').on('click', function () {

        document.body.scrollTop = 0; // For Safari
        document.documentElement.scrollTop = 0; // For Chrome, Firefox, IE and Opera
    });




    var qwidth = ($(window).width() - $(".query").width()) / 2;

    $('select').closest('.ui-select').addClass('selectClass');
    $("select").find("option").css("background", "#386592");
    $("select").find("option").css("color", "#fff");


    $(".home").on("click", function () {
        carrentals.CarRental1.clearSession(function () {

            window.location.replace("../");


        });
       

    });

    $(".bookText").on("click", function () {

        window.location.replace("../quotation/?ctype=n");
        //window.location.replace("../quotation/default.aspx?ctype=y&bookingcode=110004"); 
    });
    $(".book").on("click", function () {

        //window.location.replace("../quotation/default.aspx?ctype=y&bookingcode=110004"); 
        var currentDate = new Date();
        var day = currentDate.getDate();
        var month = currentDate.getMonth() + 1;
        var year = currentDate.getFullYear();

        if (String(month).length < 2) {
            month = '0' + month;
        }
        if (String(day).length < 2) {
            day = '0' + day;
        }
      
        var dte = year + "-" + month + "-" + day;

        carrentals.CarRental1.insertSearchResult(dte, dte, "", "", 0, function (result) {
            
            window.location.replace("../quotation/?ctype=n&bookingcode=" + result);
        });
    });
    $(".enquiry").on("click", function () {

        window.location.replace("../contact/");

    });
    $(".terms").on("click", function () {

        window.location.replace("../terms/");

    });
    $(".aboutus").on("click", function () {

        window.location.replace("../aboutus/");

    });

    $(".srh").on("click", function () {

        window.location.replace("../");

    });
    $(".contactus").on("click", function () {

        window.location.replace("../contact/");

    });

    var tempResult = "";
    var currentIndex = 0;
    var left = screen.width - $("#sliderPosition").width();
    var lft = ($(window).width() - $("#sliderPosition").width()) / 2;

    $("#sliderPosition").css({
        'left': lft + 'px',
        'top': '20%;'
    });

    $("#selectLocation").empty();
    $("#selectLocation").append("<option value='0'>Loading, Please Wait...</option>");
    $("#selectLocation").attr("disabled", "");

    carrentals.CarRental1.getLocations(function (result) {

        for (var i = 0; i < result.length; i++) {
            if (i == 0) {
                $("#selectLocation").empty();
            }
            $("#selectLocation").append("<option class='hdrText' style='font-weight:normal;font-size:18px' value=" + result[i].id + ">" + result[i].location + "</option>");
        }

        $("#selectLocation").removeAttr("disabled", "");

    });

    
    carrentals.CarRental1.getAllVehicles(function (result) {


        $(".vehicleSlider").empty();
        $("#indexButtons").empty();
        var div = "";
        var divDetails = "";
        var idxdiv = "";
        
        for (var i = 0; i < result.length; i++) {
            divDetails = "<div class='hdrText' style='position:relative;margin:auto;width:270px;font-size:33px;color:#3044C2;clear:both;'><span>" + format1(result[i].price, "$") + " USD/Day</span></div>";
            
            /*divDetails = divDetails + "<div class='hdrText' style='position:relative;margin:auto;width:200px;font-size:15px;color:#969696;clear:both;'><div style='position:relative;float:left;margin:2px;'><img src='../images/small/tick.png' style='width:20px;height:22px'/></div><div style='position:relative;float:left;margin:2px;'><span>Free Vehicle Delivery</span></div></div>";*/
            divDetails = divDetails + "<div class='line' style='position:relative;margin:auto;width:270px;clear:both;'></div>";
            divDetails = divDetails + "<div class='hdrText' style='position:relative;margin:auto;width:200px;font-size:15px;color:#969696;clear:both;'><div style='position:relative;float:left;margin:2px;'><img src='../images/small/tick.png' style='width:20px;height:22px'/></div><div style='position:relative;float:left;margin:2px;'><span>Full Tank Of Fuel</span></div></div>";
            divDetails = divDetails + "<div class='line' style='position:relative;margin:auto;width:270px;clear:both;'></div>";
            divDetails = divDetails + "<div class='hdrText' style='position:relative;margin:auto;width:200px;font-size:15px;color:#969696;clear:both;'><div style='position:relative;float:left;margin:2px;'><img src='../images/small/tick.png' style='width:20px;height:22px'/></div><div style='position:relative;float:left;margin:2px;'><span>Free Roadside Assistance</span></div></div>";

            div += "<div class='slide' style='position:relative;margin:10px;float:left;background:#fff;border-radius:3px;'><div style='position:relative;margin:auto;width:280px;'><img  width='250px' height='220px' src='" + result[i].vehiclePhoto + "'></div>" + divDetails + "</div>";
            idxdiv += "<div class='slide' style='position:relative;margin:2px;float:left;'><input type='button' class='idxBtnClass' id =" + i + " style='width:40px;height:10px;'/></div>";

            
        }
        $(".vehicleSlider").html(div);
        $("#indexButtons").html(idxdiv);

        $(".vehicleSlider").slick({
            dots: false,
            infinite: true,
            centerMode: true,
            autoplay: true,
            slidesToShow: 3,
            slidesToScroll: 3
        });

        

        $('.idxBtnClass').on('click', function () {

            $('.vehicleSlider').slick('slickGoTo', Number($(this).attr('id')));
        });

        for (var l = 0; l < result.length; l++) {
            $("#" + l).css({
                'background': '#fff'
            });
        }
        $("#" + 0).css({
            'background': '#ffcc00'
        });
        $('.vehicleSlider').on('afterChange', function (event, slick, currentSlide, nextSlide) {

            for (var i = 0; i < result.length; i++) {
                $("#" + i).css({
                    'background': '#fff'
                });
            }
            $("#" + currentSlide).css({
                'background': '#ffcc00'
            });
            var pad = "";
            var diff = 16 - result[currentSlide].vehiclename.length;
            for (var j = 0; j < diff - 1; j++) {
                pad += "&nbsp;";
            }
            $("#vehName").empty();
            $("#vehName").append(pad + result[currentSlide].vehiclename);


        });
        // Get the current slide
        var currentSlide = $('.vehicleSlider').slick('slickCurrentSlide');
        $("#buy").on("click", function () {

            var cslide = Number($('.vehicleSlider').slick('slickCurrentSlide'));
           // alert(result[cslide].vehicleId);
            var currentDate = new Date();
            var day = currentDate.getDate();
            var month = currentDate.getMonth() + 1;
            var year = currentDate.getFullYear();

            if (String(month).length < 2) {
                month = '0' + month;
            }
            if (String(day).length < 2) {
                day = '0' + day;
            }
           
            var dte = year + "-" + month + "-" + day;
            carrentals.CarRental1.storeVehicleId(result[cslide].vehicleId, function (res) {
                carrentals.CarRental1.storeDates(dte, dte, '6:00 AM', '6:00 AM', res, function () {
                    carrentals.CarRental1.insertSearchResult(dte, dte, "", "", 0, function (result) {

                        window.location.replace("../search/search.aspx");

                    });
                });
            });

        });
        

        $("#testSlider").on('click', function () {

            $('.vehicleSlider').slick('slickGoTo', 3);

        });
    });

    $("#vehicleClass").on("change", function () {

        localStorage["vehicleClass"] = $(this).val();
        carrentals.CarRental1.storeVehicleClass($(this).val());

    });

    $("#vehicleClass").empty();
    $("#vehicleClass").append("<option value='0'>Loading, Please Wait...</option>");
    $("#vehicleClass").attr("disabled", "");

    carrentals.CarRental1.getVehicleClass(function (result) {

        var vehtypebox = "";

        for (var i = 0; i < result.length; i++) {
            if (i == 0) {
                $("#vehicleClass").empty();
                $("#vehicleClass").append("<option class='hdrText' style='font-weight:normal;font-size:10px' value='AAA'><span style='font-size:8px;'></span></option>");
            }
            $("#vehicleClass").append("<option class='hdrText' style='font-weight:normal;font-size:10px' value=" + result[i].vehclass + ">" + result[i].description + "</option>");

            vehtypebox += '<a href="#" id="' + result[i].vehclass + '" class="vlist" name = '+result[i].description + '><span class="hdrText descr" style="font-size:12px" >' + result[i].description + '</span></a>';

        }

        $(".vehtypebox").html(vehtypebox);

        $("#vehicleClass").removeAttr("disabled", "");
        $("#vehicleClass").val("AAA");
        $("#vehicleClass").selectmenu("refresh");

        $('.vlist').on('click', function () {
           
            
            $("#vehtypeText").text($(this).attr('name') + " - " + $(this).attr('id'));
            $("#vehtypeCls").text("");
            //$(this).attr('id')
        });

    });


});