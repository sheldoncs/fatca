$(document).ready(function () {


    var delivery = 0;
    var carryOut = 0;
    var status = 0;

    $(".menuClick").on("click", function () {
        $(".cover").show();
        $(".menupopup").show();

        $.ajax({
            type: "POST",
            url: "../RealEstate.asmx/getCategoryList",
            contentType: "application/json",
            datatype: "json",
            success: function (result) {
                listCategories(result);
            },
            error: function (xhr) {
                alert(xhr.responseText);
            }
        });

    });

    $("#delivery").on("click", function () {

        if (delivery === 0) {

            $("#delivery").removeClass("delivery");
            $("#delivery").addClass("deliveryClick");
            $("#popupDelivery").removeClass("delivery");
            $("#popupDelivery").addClass("deliveryClick");

            $("#carryOut").removeClass("carryOutClick");
            $("#carryOut").addClass("carryOut");
            $("#popupCarryOut").removeClass("carryOutClick");
            $("#popupCarryOut").addClass("carryOut");

            $("#adrDelivery").removeClass("adrDelivery");
            $("#adrDelivery").addClass("adrDeliveryClick");

            $("#adrCarryOut").removeClass("carryOutClick");
            $("#adrCarryOut").addClass("carryOut");
            status = 1;
            delivery = 1;
            carryOut = 0;
            carrentals.CarRental1.setCollectStatus(delivery, carryOut);

        }
    });
    $("#popupDelivery").on("click", function () {

        if (delivery === 0) {

            $("#delivery").removeClass("delivery");
            $("#delivery").addClass("deliveryClick");
            $("#popupDelivery").removeClass("delivery");
            $("#popupDelivery").addClass("deliveryClick");

            $("#carryOut").removeClass("carryOutClick");
            $("#carryOut").addClass("carryOut");
            $("#popupCarryOut").removeClass("carryOutClick");
            $("#popupCarryOut").addClass("carryOut");

            status = 1;
            delivery = 1;
            carryOut = 0;
            carrentals.CarRental1.setCollectStatus(delivery, carryOut);

            
        }
    });


    $("#carryOut").on("click", function () {

        if (carryOut === 0) {

            $("#carryOut").addClass("carryOutClick");
            $("#carryOut").removeClass("carryOut");
            $("#popupCarryOut").addClass("carryOutClick");
            $("#popupCarryOut").removeClass("carryOut");
            status = 1;

            $("#delivery").addClass("delivery");
            $("#delivery").removeClass("deliveryClick");
            $("#popupDelivery").addClass("delivery");
            $("#popupDelivery").removeClass("deliveryClick");

            
            carryOut = 1;
            delivery = 0;
            carrentals.CarRental1.setCollectStatus(delivery, carryOut);

            
        }
    });
    $("#popupCarryOut").on("click", function () {

        if (carryOut === 0) {

            $("#carryOut").addClass("carryOutClick");
            $("#carryOut").removeClass("carryOut");
            $("#popupCarryOut").addClass("carryOutClick");
            $("#popupCarryOut").removeClass("carryOut");
            status = 1;

            $("#delivery").addClass("delivery");
            $("#delivery").removeClass("deliveryClick");
            $("#popupDelivery").addClass("delivery");
            $("#popupDelivery").removeClass("deliveryClick");

            carryOut = 1;
            delivery = 0;
            carrentals.CarRental1.setCollectStatus(delivery, carryOut);
        }
    });

    function listCategories(result) {

        $('#listCategories').empty();
        var li = "";
        var div = "";
     

        for (var i = 0; i < result.length; i++) {


            div = "<div style='position:relative;float:left;margin:10px' ><img style='width:150px;height:100px;' src='../" + result[i].websitepath + "'/></div>" +
                    "<div class='textFont' style='position:relative;float:left;margin:10px;font-size:25px;' >" + result[i].categories + "</div>";
            
            li += "<li class='liClass' id=" + result[i].id +"><a href='#'>" + div + "</a></li>";
            
        }


        $('#listCategories').append(li);
        $('#listCategories').listview('refresh');

        $(".liClass").on("click", function () {
            if (status === 1) {
                
                carrentals.CarRental1.setLastIdentity();
                carrentals.CarRental1.setCategoryId($(this).attr('id'));
                
                if ($(this).attr('id') == 19) {
                   
                    window.location.replace("../toppings/");
                }
            } else {
                alert("Please Select Delivery or Pickup");
            }
        });

      
    }

});