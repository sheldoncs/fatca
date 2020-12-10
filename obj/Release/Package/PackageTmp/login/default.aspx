<%@ Page Title="" Language="C#" MasterPageFile="~/Fatca.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="FATCA_2._0.login._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <link href="../css/fatca.css" rel="stylesheet" />
 <link href="../css/signin.css" rel="stylesheet" />
    <script type="text/javascript">

        $(function () {

            $(".logout").hide();

            $(".formats").on("click", function () {

                $(".cover").show();
                $(".formatpopup").show();

            });

            $(".close").on("click", function () {

                $(".cover").hide();
                $(".formatpopup").hide();

            });
            $(".login").on("click", function () {

                fatca.fatca1.confirmLogin($(".txtuser").val(), $(".txtpass").val(), function (result) {

                   
                    if (result == true) {
                        window.location.replace("../fatca/");
                    }

                });
                

            });
        });

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

        <div class="outerElement">
            <div class="innerElement">
                <div class="loginlabel">
                       <span class="textFont" style="font-size:20px;color:#fff">
                           LOGIN HERE
                       </span>

                </div>
            </div>
        </div>
        <div class="signin">
            
            <div class="user">
                <input type="text" id="" class="textFont txtuser" placeholder="username" style="font-size:20px" />
            </div><br/>
            <div class="pass">
               <input type="password" id="" placeholder="password" class="textFont txtpass" style="font-size:20px"/>
            </div><br/>
            <div class="login">
             
             <div class="outerElement">
              <div class="innerElement" style="padding-top:2px">
               <span class="textFont logsignin" style="font-size:20px;color:#fff">
                   LOGIN
               </span>
              </div>
             </div>

            </div>
            

        </div>

        <div class="cover"></div>
        <div class="formatpopup">
               <div class="close textFont" style="color:#000;cursor:pointer">X</div>
                <div class="repeatformats textfont">
                    <span class="textFont" style="font-size:20px;font-weight:bold">Excel file formats should be created as
                        specified below. Any Variation from these formats will result in the software
                        not processing the Excel document.
                    </span>
                 
                </div>
                <div class="repeatformats">
                    <span class="textFont" style="font-size:20px;font-weight:bold">BPWCCU CRS FILE FORMAT
                    <img src="../images/formats/bpw_crs.PNG" />
                    </span>
                </div>
                <div class="repeatformats">
                    <span class="textFont" style="font-size:20px;font-weight:bold">BPWCCU FATCA FILE FORMAT
                    <img src="../images/formats/bpw_fatca.PNG" />
                    </span>
                </div>
              <div class="repeatformats">
                    <span class="textFont" style="font-size:20px;font-weight:bold">CAPITA CRS FILE FORMAT
                    <img src="../images/formats/capita_crs.PNG" width="950px" height="30px"/>
                    </span>
                </div>
                <div class="repeatformats">
                    <span class="textFont" style="font-size:20px;font-weight:bold">CAPITA FATCA FILE FORMAT
                    <img src="../images/formats/capita_fatca.PNG" />
                    </span>
                </div>
          </div>

   

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
</asp:Content>

