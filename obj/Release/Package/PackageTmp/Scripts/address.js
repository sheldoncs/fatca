$(document).ready(function () {

    var popupAddress = ($(window).width() - 485) / 2;
    $("#popupAddress").css({
        'left': popupAddress + 'px'
    }); 

    var addressResults = null;
    $("#adrNew").on("click", function () {

        $("#txtAddress1").val("");
        $("#txtAddress2").val("");
        $("#txtAdrZip").val("");
        $("#txtCity").val("");

    });
    $("#search").keyup(function () {

        var SEARCHWORD = this.value;
        $(".browselist li").each(function () {


            if ($(this).
              find(".addr").
              text().toUpperCase().
              indexOf(SEARCHWORD.toUpperCase()) >= 0)

                $(this).show();
            else
                $(this).hide();

        });


    });
    $("#adrClose").on("click", function () {
        $("#popupAddress").slideUp(2000);
        $(".cover").hide();
    });
    $(".btnBrowseAddress").on("click", function () {

        $(".cover").show();
        $("#browseAddresses").slideDown(1000);

        LocatorAdmin.LocatorAdmin.GatherAllAddresses(function (result) {

            addressResults = result;


            listOfAddresses(result);
        });

    });

    $(".btnCloseAddress").on("click", function () {
        $("#browseAddresses").slideUp(2000);
        $(".cover").hide();
    });
    function listOfAddresses(result) {

        $(".browselist").empty();
        var li = "";
        for (var i = 0; i < result.length; i++) {
            li += "<li class='menu' style='background:#454444;color:#fff'><div class='addr menu'><table  style='width:600px;'><tr><td style='width:400px;'>" + result[i].address.toUpperCase() + "</td><td  style='width:200px;'><div id=" + result[i].id + " class='buttonAddrSelect' style='background:#F5F262'><span class='menu' style='position:relative;padding-left:13px;top:3px;color:#000'>SELECT</span></div></td></tr></table></div></li>";
        }
        $(".browselist").html(li);
        $(".browselist").listview('refresh');

        $(".buttonAddrSelect").on("click", function () {



            $("#browseAddresses").hide();
            $(".cover").hide();

            $("#popupAddress").slideDown(1000);
            $(".cover").show();
            
            localStorage["addressId"] = $(this).attr("id");
            
            LocatorAdmin.LocatorAdmin.GatherSpecificAddress($(this).attr("id"), function (result) {


                
                localStorage["addressUpdate"] = true;

                $("#txtAddress1").val(result[0].addr1);
                $("#txtAddress2").val(result[0].addr2);
                $("#txtAdrZip").val(result[0].zip);
                $("#txtCity").val(result[0].city);
                $("#txtLatitude").val(result[0].lat);
                $("#txtLongitude").val(result[0].lng);

            });


        });


        $("#adrSave").on("click", function () {

            
            if (localStorage["addressUpdate"] == false) {

                LocatorAdmin.LocatorAdmin.InsertAddress($("#txtAddress1").val(), $("#txtAddress2").val(),
                                                              $("#txtAdrZip").val(), $("#txtCity").val(),
                                                                     $("#txtLatitude").val(), $("#txtLongitude").val(), function (result) {
                                                                         swal("Success", "Address Added Successfully", "success");
                                                                     });

            } else {

                LocatorAdmin.LocatorAdmin.updateAddress($("#txtAddress1").val(), $("#txtAddress2").val(),
                                                              $("#txtAdrZip").val(), $("#txtCity").val(),
                                                               $("#txtLatitude").val(), $("#txtLongitude").val(),
                                                               localStorage["addressId"], function (result) {

                                                                   swal("Success", "Address Updated Successfully", result);

                                                               });
            }
        });

        
    }





});