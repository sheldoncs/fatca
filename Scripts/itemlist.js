$(document).ready(function () {

    $("#btnCloseItemList").on("click", function () {

        $("#BrowseList").slideUp(1000);
        $(".cover").hide();

    });
    $("#btnCloseItemList").on("click", function () {

    });
    $("#btnOpenList").on("click", function () {

        $("#popupDriver").hide();
        $("#BrowseList").slideDown(1000);
        $(".cover").show();
        LocatorAdmin.LocatorAdmin.ListItems(localStorage["personId"], function (result) {

            $(".browseItemList").empty();
            var li = "";

            for (var i = 0; i < result.length; i++) {
                li += "<li style='background:#454444;color:#fff;width:600px;'><div class='ordList menu'><table style='position:relative;left:20px;width:580px;'><tr><td width='200px'>" + result[i].item + "<td width='150px'><select id=" + result[i].id + " class='selectAmount' " + "><option value='0'>0</option><option value='1'>1</option><option value='2'>2</option><option value='3'>3</option></select></td><td width='150px'><div class='toggleItemList' style='positon:relative;top:0px;'><div id =" + result[i].id + "  class='togcls circleInActive " + result[i].id + "'></div></div></td></tr></table></div></li>";
            }
            $(".browseItemList").html(li);
            $(".browseItemList").listview('refresh');

            for (var j = 0; j < result.length; j++) {
                $("#" + result[j].id).val(result[j].amount).change();

                if (result[j].enable == true) {
                    $("." + result[j].id).removeClass("circleInActive");
                    $("." + result[j].id).addClass("circleActive");
                    $("." + result[j].id).parent().css("background", "#2ECC71");
                }
            }

            $(".togcls").on("click", function () {
                var $_id = "";
                var on = false;
                if ($(this).attr("class").indexOf("circleInActive") > 0) {
                    $(this).removeClass("circleInActive");
                    $(this).addClass("circleActive");
                    $(this).parent().css("background", "#2ECC71");
                    on = true;
                } else if ($(this).attr("class").indexOf("circleActive") > 0) {
                    $(this).removeClass("circleActive");
                    $(this).addClass("circleInActive");
                    $(this).parent().css("background", "#E74C3C");
                    on = false;
                }

                $_id = $(this).attr("id")
                LocatorAdmin.LocatorAdmin.getItemCheckList(localStorage["personId"], $(this).attr("id"), function (result) {

                    if (result == false) {
                        if ($(".selectAmount").val() > 0) {
                            LocatorAdmin.LocatorAdmin.InsertCheckList(localStorage["personId"], $_id, $(".selectAmount").val(), on);
                        } else {
                            swal("Oops..", "Please Select Amount First!", "error");
                        }
                    } else {

                        if ($(".togcls").attr("class").indexOf("circleActive") > 0) {


                            LocatorAdmin.LocatorAdmin.UpdateItemInList(localStorage["personId"], $_id, true);
                        } else if ($(".togcls").attr("class").indexOf("circleInActive") > 0) {

                            LocatorAdmin.LocatorAdmin.UpdateItemInList(localStorage["personId"], $_id, false);
                        }

                    }
                });

            });

            $(".selectAmount").on("change", function () {
                var id = $(this).attr("id");
                var amt = $(this).val();
                
                LocatorAdmin.LocatorAdmin.getItemCheckList(localStorage["personId"], id, function (result) {

                    if (result == true) {

                        LocatorAdmin.LocatorAdmin.UpdateItemAmountInList(localStorage["personId"], id, amt);

                    } else {

                        LocatorAdmin.LocatorAdmin.InsertCheckList(localStorage["personId"], id, amt);

                    }

                });


            });



        });


    });


});