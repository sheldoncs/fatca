<%@ Page Title="" Language="C#" MasterPageFile="~/Fatca.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="FATCA_2._0.fatca._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../css/fatca.css" rel="stylesheet" />

    <script type="text/javascript">

        $(function () {
            
           
            $(".options").prop("checked", false).checkboxradio('refresh');

            var selectprocess = "";
            var fileName = "";
            var dispFileName = "";
            var whichfile = "";

            $("#xml").val("");
            
            $("#periodending").datepicker();
            $('.prev i').removeClass();
            $('.prev i').addClass("fa fa-chevron-left");

            $('.next i').removeClass();
            $('.next i').addClass("fa fa-chevron-right");

            $('body').on('dragover drop', function (e) { e.preventDefault(); });
            $(document).on('draginit dragstart dragover dragend drag drop', function (e) {
                e.stopPropagation();
                e.preventDefault();
            });
            
            function makeDroppable(element, callback, docType) {

                var input = document.createElement('input');
                input.setAttribute('type', 'file');
                input.setAttribute('id', docType);
                input.setAttribute('multiple', true);
                input.style.display = 'none';

                //input.addEventListener('change', triggerCallback);
                input.addEventListener('change', function (e) {

                    triggerCallback(e, docType);
                });

                element.appendChild(input);

                element.addEventListener('dragover', function (e) {
                    e.preventDefault();
                    e.stopPropagation();

                    element.classList.add('dragover');
                });

                element.addEventListener('dragleave', function (e) {

                    e.preventDefault();
                    e.stopPropagation();
                    element.classList.remove('dragover');
                });

                element.addEventListener('drop', function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    element.classList.remove('dragover');
                    triggerCallback(e, docType);
                });

                element.addEventListener('click', function (e) {
                    input.value = null;
                    input.click();
                    console.log("input = " + input.value);

                });

                function triggerCallback(e, docType) {

                    var files;

                    if (e.dataTransfer) {

                        files = e.dataTransfer.files;

                    } else if (e.target) {

                        files = e.target.files;
                        console.log("Files = " + files);

                    }

                    if (e.target.files == undefined) {
                        dispFileName = e.dataTransfer.files[0].name;
                    } else {
                        dispFileName = e.target.files[0].name;
                    }

                    

                    callback.call(null, files, docType);

                }
            }

            function displayFileName(docType,filName) {
                fileName = docType;
                
                if (docType == "bpw_crs") {
                   
                    $("#lblbpwcrs").text(dispFileName + " Uploaded Successfully.");

                } else if (docType == "capita_crs") {

                    $("#lblcapitacrs").text(dispFileName + " Uploaded Successfully.");

                } else if (docType == "capita_fatca") {

                    $("#lblcapitafatca").text(dispFileName + " Uploaded Successfully.");

                } else if (docType == "bpw_fatca") {

                    $("#lblbpwfatca").text(dispFileName + " Uploaded Successfully.");

                } 

            }
            function waitFunction(docType) {
                if (docType == "bpw_crs") {

                    $("#lblbpwcrs").text("Waiting...");

                } else if (docType == "capita_crs") {

                    $("#lblcapitacrs").text("Waiting...");

                } else if (docType == "bpw_fatca") {

                    $("#lblbpwfatca").text("Waiting...");

                } else if (docType == "capita_fatca") {

                    $("#lblcapitafatca").text("Waiting...");

                } 
            }
            function callback(files, docType) {
                var formData = new FormData();

               
                waitFunction(docType);
                $.each(files, function (i, file) {

                    formData.append('drpfile', file);
                       
                });

                
                $.ajax({
                    url: '../services/ExcelHandler.ashx?docType=' + docType,
                    type: 'POST',
                    data: formData,
                    processData: false,
                    contentType: false,
                    success: function (response) {

                        if (response.indexOf('error') == -1) {

                            displayFileName(response,docType);
                            alert("Success", "Files uploaded successfully.", "success");

                        } else {

                            swal("Error", response, "error");

                        }
                        if (docType == 'photo') {

                            
                        }
                    }
                });

            }
            var elementbpwcrs = document.querySelector('.bpwcrs');
            var elementcapitacrs = document.querySelector('.capitacrs');
            var elementbpwfatca = document.querySelector('.bpwfatca');
            var elementcapitafatca = document.querySelector('.capitafatca');

            makeDroppable(elementbpwcrs, callback, "bpwcrs");
            makeDroppable(elementcapitacrs, callback, "capitacrs");
            makeDroppable(elementbpwfatca, callback, "bpwfatca");
            makeDroppable(elementcapitafatca, callback, "capitafatca");
            
            $(".uploadProcess").on("click", function () {

                var companycode = "";
                var filetype = "";

                if ($(this).hasClass("uploaddbpwcrs") == true) {

                    selectprocess = "bpw_crs";
                    companycode = "BPW";
                    filetype = "CRS";

                } else if ($(this).hasClass("uploadcapitawcrs") == true) {

                    selectprocess = "capita_crs";
                    companycode = "CAPITA";
                    filetype = "CRS";

                } else if ($(this).hasClass("uploadbpwfatca") == true) {

                    selectprocess = "bpw_fatca";
                    companycode = "BPW";
                    filetype = "FATCA";

                } else if ($(this).hasClass("uploadcapitafatca") == true) {

                    selectprocess = "capita_fatca";
                    companycode = "CAPITA";
                    filetype = "FATCA";

                }

                fatca.fatca1.getCompanyDetails(companycode, filetype, function (result) {

                    $("#SendingCompanyIN").val(result.sendingcompany);
                    $("#name").val(result.name);
                    $("#address").val(result.shortaddress);
                    $("#messagerefid").val(result.messageref);
                    $("#docrefid").val(result.docref);

                    $(".cover").show();
                    $(".popup").show();

                });
                

            });
            $(".close").on("click", function () {

                $(".cover").hide();
                $(".popup").hide();
                $(".formatpopup").hide();

            });

            function fieldValid(field) {
                var valid = true;
                if ($(field).val() == "")
                {
                    valid = false;
                }
                return valid;
            }
            $(".reset").on("click", function () {
                /*
                $(".SendingCompanyIN").val("");
                $(".name").val("");
                $(".address").val("");
                $(".messagerefid").val("");
                $(".periodending").val("");
                $(".docrefid").val("");
                */
            });
 
            $(".update").on("click", function () {

               
                $(".popup").hide();

                $(".gearloadingpopup").show();
                if ((selectprocess == "bpw_crs") || (selectprocess == "capita_crs")) {
                    
                    fatca.fatca1.readInCRSExcelFile(fileName, $("#SendingCompanyIN").val(), $("#docrefid").val(),$("#name").val(),
                                        $("#periodending").val(), $("#messagerefid").val(), $("#address").val(), selectprocess, function (result) {
                                            
                                            if (result.indexOf("Error") >= 0) {
                                                alert(result);
                                            } else if (result.indexOf("Not" >= 0)){
                                                alert(result);
                                            }
                                            else {
                                                alert("Successfully Loaded.");
                                            }
                                            $(".gearloadingpopup").hide();
                                            $(".cover").hide();
                                        });
                } else if ((selectprocess == "bpw_fatca") || (selectprocess == "capita_fatca")) {

                    fatca.fatca1.readInFatcaExcelFile(fileName, $("#SendingCompanyIN").val(), $("#docrefid").val(),
                        $("#name").val(), $("#periodending").val(), $("#messagerefid").val(), $("#address").val(), selectprocess, function (result) {

                            if (result.indexOf("Error") >= 0) {
                                alert(result);
                            } else {
                                alert("Successfully Loaded.");
                            }
                            $(".gearloadingpopup").hide();
                            $(".cover").hide();
                        });

                }
                  
               
            });

            $(".options").on("change", function () {

                var id = $(this).attr("id");
                if (id == "radiobpwcrs") {
                    fatca.fatca1.GetXMLString("bpw_crs", function (result) {
                       
                        whichfile = "bpw_crs";
                        $("#xml").val(formatXml(result));

                    });
                } else if (id == "radiocapitacrs") {

                    fatca.fatca1.GetXMLString("capita_crs", function (result) {

                        whichfile = "capita_crs";
                        $("#xml").val(formatXml(result));

                    });

                } else if (id == "radiobpwfatca") {

                    fatca.fatca1.GetXMLString("bpw_fatca", function (result) {

                        whichfile = "bpw_fatca";
                        $("#xml").val(formatXml(result));

                    });

                } else if (id == "radiocapitafatca") {

                    fatca.fatca1.GetXMLString("capita_fatca", function (result) {

                        whichfile = "capita_fatca";
                        $("#xml").val(formatXml(result));

                    });

                }
            });

            function formatXml(xml) {
                var formatted = '';
                var reg = /(>)(<)(\/*)/g;
                xml = xml.replace(reg, '$1\r\n$2$3');
                var pad = 0;
                jQuery.each(xml.split('\r\n'), function (index, node) {
                    var indent = 0;
                    if (node.match(/.+<\/\w[^>]*>$/)) {
                        indent = 0;
                    } else if (node.match(/^<\/\w/)) {
                        if (pad != 0) {
                            pad -= 1;
                        }
                    } else if (node.match(/^<\w[^>]*[^\/]>.*$/)) {
                        indent = 1;
                    } else {
                        indent = 0;
                    }

                    var padding = '';
                    for (var i = 0; i < pad; i++) {
                        padding += '  ';
                    }

                    formatted += padding + node + '\r\n';
                    pad += indent;
                });

                return formatted;
            }
            $(".save").on("click", function () {
               
                fatca.fatca1.UpdateXmlDoc($("#xml").val(), whichfile, function (result) {

                    alert(result);

                });

            });
            $(".logout").on("click", function () {
                fatca.fatca1.clearSession();
                window.location.replace("../login/");
            });
            $(".formats").on("click", function () {

                $(".cover").show();
                $(".formatpopup").show();

            });

            var success = '<%=Session["LoginSuccess"]%>';

            if (success == "True"){

            } else {
                alert("Login Unsuccessful");
                window.location.replace("../login/");
            }
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    

        <div class="left">
            <div class="leftHeader textFont" style="font-size:30px">Upload CRS/FATCA EXCEL Documents</div>
            
           <div class="fileProcess">
            <div class="filetitle textFont">
                <span style="font-size:22px">
                    BPW CRS
                </span>
            </div>
            <div class="fileBox">
                <div class="fileNameBox textFont"><span id="lblbpwcrs" style="font-size:18px"></span></div>

                <div class="uploadProcess uploaddbpwcrs">
                    <div class="outerElement">
                        <div class="innerElement" style="top:5px">
                          <span class="textFont" style="font-size:15px;color:#fff">PROCESS</span>
                        </div>
                    </div>
                </div>
                <div class="uploadExcel bpwcrs">
                    <div class="outerElement">
                        <div class="innerElement" style="top:5px">
                          <span class="textFont" style="font-size:15px;color:#fff">UPLOAD EXCEL</span>
                        </div>
                    </div>
                </div>
        
                
            </div>
            <div class="filetitle textFont">
            <span style="font-size:22px">
                CAPITA CRS
            </span>
        </div>
        <div class="fileBox">
            <div class="fileNameBox textFont"><span id="lblcapitacrs" style="font-size:18px"></span></div>

            <div class="uploadProcess uploadcapitawcrs">
                <div class="outerElement">
                    <div class="innerElement" style="top:5px">
                      <span class="textFont" style="font-size:15px;color:#fff">PROCESS</span>
                    </div>
                </div>
            </div>
            <div class="uploadExcel capitacrs">
                 <div class="outerElement">
                    <div class="innerElement" style="top:5px">
                      <span class="textFont" style="font-size:15px;color:#fff">UPLOAD EXCEL</span>
                    </div>
                </div>
            </div>
            
          </div>
          <div class="filetitle textFont">
            <span style="font-size:22px">
                BPW FATCA
            </span>
        </div>
        <div class="fileBox">
            <div class="fileNameBox textFont"><span id="lblbpwfatca" style="font-size:18px"></span></div>

            <div class="uploadProcess uploadbpwfatca">
                <div class="outerElement">
                    <div class="innerElement" style="top:5px">
                      <span class="textFont" style="font-size:15px;color:#fff">PROCESS</span>
                    </div>
                </div>
            </div>
            

            <div class="uploadExcel bpwfatca">
                <div class="outerElement">
                    <div class="innerElement" style="top:5px">
                      <span class="textFont" style="font-size:15px;color:#fff">UPLOAD EXCEL</span>
                    </div>
                 </div>
            </div>
            
            
           </div>
           
           <div class="filetitle textFont">
                <span style="font-size:22px">
                    CAPITA FATCA
                </span>
           </div>
           <div class="fileBox">
                  <div class="fileNameBox textFont"><span id="lblcapitafatca" style="font-size:18px"></span></div>
                  <div class="uploadProcess uploadcapitafatca">
                        <div class="outerElement">
                            <div class="innerElement" style="top:5px">
                              <span class="textFont" style="font-size:15px;color:#fff">PROCESS</span>
                            </div>
                        </div>
                  </div> 
                  
                  <div class="uploadExcel capitafatca">
                        <div class="outerElement">
                            <div class="innerElement" style="top:5px">
                              <span class="textFont" style="font-size:15px;color:#fff">UPLOAD EXCEL</span>
                            </div>
                         </div>
                  </div>
                    
               </div>
            </div>
           </div>
           
        
          <div class="right">
            <div class="textFont" style="position:relative;float:left;font-size:30px;width:300px;left:50px;">
                <div class="outerElement"> 
                    <div class="innerElement">
                      Display XML Documents
                    </div>
                </div>

            </div>
            <div class="fileOptions">
                
                 <div class="outerElement"> 
                    <div class="innerElement">
                    <div style="position:relative;float:left;margin:5px;width:120px">
                        <label for="radiobpwcrs" class="textFont" style="font-size:15px;color:#000">BPW CRS</label>
                        <input type="radio" class="options" data-theme="c" name="portion" id="radiobpwcrs" value="BPWCRS"/>
                    </div>
                    <div style="position:relative;float:left;margin:5px;width:120px">
                        <label for="radiocapitacrs" class="textFont" style="font-size:15px;color:#000">CAPITA CRS</label>
                        <input type="radio" class="options" data-theme="c" name="portion" id="radiocapitacrs" value="CAPCRS"/>
                    </div>
                    <div style="position:relative;float:left;margin:5px;width:120px">
                        <label for="radiobpwfatca" class="textFont" style="font-size:15px;color:#000">BPW FATCA</label>
                        <input type="radio" class="options" data-theme="c" name="portion" id="radiobpwfatca" value="BPWFATCA"/>
                    </div>
                    <div style="position:relative;float:left;margin:5px;width:120px">
                        <label for="radiocapitafatca" class="textFont" style="font-size:15px;color:#000">CAPITA CRS</label>
                        <input type="radio" class="options" data-theme="c" name="portion" id="radiocapitafatca" value="CAPFATCA"/>
                    </div>
                  </div>
                 </div>

            </div>
            
            <div class="editXML">
                <textarea placeholder="XML Document"  id="xml" rows="300" style="min-width:500px; max-width:100%;min-height:50px;height:500px;width:100%;">

               </textarea>
            </div>
            <div class="save">
                <div class="outerElement">
                    <div class="innerElement" style="top:4px">
                      <span class="textFont" style="color:#fff">UPDATE</span>
                    </div>
                </div>
            </div>
          </div>
          <div class="cover"></div>
          <div class="popup">
              <div class="close textFont" style="color:#000;cursor:pointer">X</div>

              <div class="fields">
                <div class="reset">
                  <div class="outerElement">
                     <div class="innerElement" style="top:4px">
                        <span class="textFont" style="font-size:20px;color:#fff">RESET</span>
                     </div>
                  </div> 
                </div>
              </div>
              
              <div class="fields">
                 <input type="text" placeholder="SendingCompanyIN" id="SendingCompanyIN" class="textFont" style="font-size:20px;"/>
              </div>
              <div class="fields">
                 <input type="text" placeholder="company name" id="name" class="textFont" style="font-size:20px;"/>
              </div>
              <div class="fields">
                 <input type="text" placeholder="short address" id="address" class="textFont" style="font-size:20px;"/>
              </div>   
              <div class="fields">
                 <input type="text" placeholder="messagerefid" id="messagerefid" class="textFont" style="font-size:20px;"/>
              </div>
              <div class="fields">
                 <input type="text" placeholder="period ending" id="periodending" class="textFont" style="font-size:20px;"/>
              </div>
              <div class="fields">
                 <input type="text" placeholder="docrefid" id="docrefid" class="textFont" style="font-size:20px;"/>
              </div>
              <div class="fields">
                <div class="update">
                  <div class="outerElement">
                     <div class="innerElement" style="top:4px">
                        <span class="textFont" style="font-size:20px;color:#fff">UPDATE</span>
                     </div>
                  </div> 
                </div>
              </div>

          </div>
          <div class="gearloadingpopup">
              <div class="outerElement">
                  <div class="innerElement">
                    <img src="../images/gear_loading.gif" />
                  </div>
              </div>
          </div> 
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
