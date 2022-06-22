// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

"use strict";


/************************************************************** */
var _url = "/";          //if your app upload outside Default Web site - for my pc
 _url = "/tmsystem/";  //if your app upload under Default Web site - for company
/********** AND Dont forget to share your DOC folder in the server for the file uploading  ******************** */



var ci, ri, drag_ci, drag_ri, final_process;
var dragname, dragid, dragdate, dropid, dropdate;

var SharedAddProcess = 'Shared/AddProcess';
var SharedEditRemark = 'Shared/EditRemark';
var SharedCheckProcess = 'Shared/CheckProcess';
var SharedDeleteProcess = 'Shared/DeleteProcess';
var DetailIndex = "Detail/Index";
var MasterProcess = 'Master/Process';
var Reload = "Home/ReloadTable";
var scrollxlocation = 0;


jQuery(document).ready(function () {
    //var $j = jQuery.noConflict();
    StartEndDate();
    DataTableOnClick();
    ProcessDrag();
    window.onresize = setWindow;
    setWindow();
    try {
        final_process = document.getElementById("finalProcess").innerText;
    } catch (err) { }

    for (var s = document.getElementsByClassName("remark"), t = 0; t < s.length; t++) {

        s[t].addEventListener("keyup", remarkKeyup);
        //s[t].obj = "afasfe";//use this.obj to assecc it
       
    }


    jQuery('#searchpic')
        .click(function (event) {//for multiple select on process
            jQuery("#div_searchstatus").hide();

            jQuery.ajax({
                type: 'GET',
                url: _url + 'Shared/SearchPic',
                success: function (result) {

                    var t = "<table id='table_searchpic'>";
                    t += "<tr>";
                    t += "<td>All_PIC</td>";
                    t += "</tr>";
                    for (var j = 0; j < result.length; j++) {
                        t += "<tr>";
                        t += "<td>" + result[j]["name"] + "</td>";
                        t += "</tr>";
                    }
                    t += "</table>";

                    jQuery("#div_searchpic")
                        .show()
                        .css({
                            "position": "absolute",
                            "top": event.clientY +20 + "px",
                            "left": event.clientX -40+ "px"

                        })
                        .html(t)

                    jQuery('#table_searchpic td')
                        .click(function (event) {//for multiple select on process
                            var RI = jQuery(this).parent().parent().children().index(this.parentNode);

                            var table = document.getElementById("table_searchpic");
                            document.getElementById("searchpic").value =
                                table.rows[RI].cells[0].innerText;
                            loadHomePage(
                                document.getElementById("startdate").value,
                                document.getElementById("enddate").value,
                                "1",
                                document.getElementById("searchpic").value,
                                document.getElementById("searchstatus").value,
                                document.getElementById("searchproject").value
                            );

                            jQuery("#div_searchpic").hide();
                                    
                        })
                }
            });
        })


    jQuery('#searchstatus')
        .click(function (event) {//for multiple select on process

            jQuery("#div_searchpic").hide();

            var t = "<table id='table_searchstatus'>";
            t += "<tr>";
            t += "<td>All_Status</td>";
            t += "</tr>";
            t += "<tr>";
            t += "<td>In_Progress</td>";
            t += "</tr>";
            t += "<tr>";
            t += "<td>Over_Due</td>";
            t += "</tr>";
            t += "<tr>";
            t += "<td>Completed</td>";
            t += "</tr>";
            t += "</table>";

            jQuery("#div_searchstatus")
                .show()
                .css({
                    "position": "absolute",
                    "top": event.clientY + 20 + "px",
                    "left": event.clientX - 30 + "px"

                })
                .html(t)

            jQuery('#table_searchstatus td')
                .click(function (event) {//for multiple select on process
                    var RI = jQuery(this).parent().parent().children().index(this.parentNode);

                    var table = document.getElementById("table_searchstatus");
                    document.getElementById("searchstatus").value =
                        table.rows[RI].cells[0].innerText;
                    loadHomePage(
                        document.getElementById("startdate").value,
                        document.getElementById("enddate").value,
                        "1",
                        document.getElementById("searchpic").value,
                        document.getElementById("searchstatus").value,
                        document.getElementById("searchproject").value
                    );

                    jQuery("#div_searchstatus").hide();

                })

            
               
        })

    jQuery("#searchproject").click(function () {
        this.focus();
        document.getElementById("searchproject").select();
    })
    jQuery("#btn_searchproject").click(function () {
        loadHomePage(
            document.getElementById("startdate").value,
            document.getElementById("enddate").value,
            "1",
            document.getElementById("searchpic").value,
            document.getElementById("searchstatus").value,
            document.getElementById("searchproject").value
        );
    })

    
});

function remarkKeyup(e) {
    
    if (e.keyCode == 13) {        

        var table = document.getElementById("table_data");
        var Controller = "Home";
        var url = String(window.location.href).indexOf("Detail");
        if (url != -1) {
            Controller = "Detail";
        }
        jQuery.ajax({
            type: "POST",
            url: _url + SharedEditRemark,
            data: jQuery.param({
                id: parseInt(table.rows[ri].cells[5].innerText),
                remark: this.value, 
                Controllerx: Controller
            }),
            success: function (res) {
                alert("Remark updated");
            }
        });
        
    }
}

function scrollx() {
    for (var s = document.getElementsByClassName("locked_left"), t = 0; t < s.length; t++) {
        parseFloat(document.getElementById("div_data").scrollLeft) > 5 ?
            -5 ?
                s[t].style.left = parseFloat(document.getElementById("div_data").scrollLeft) + -5 + "px" :
                s[t].style.left = document.getElementById("div_data").scrollLeft + "px" :
            s[t].style.left = "0px"
        s[t].style.border = "1px solid black";
    }
    scrollxlocation = parseFloat(document.getElementById("div_data").scrollLeft);

}

function setWindow() {

    var x = document.getElementsByClassName('div_data');
    for (var i = 0; i < x.length; i++) {
        x[i].style.width = (window.innerWidth - 30) + 'px';
        x[i].style.height = (window.innerHeight - 150) + 'px';
    }

}

function StartEndDate() {
    jQuery(".startdate,.enddate")
        .css({ "cursor": "pointer" })
        .change(function () {
            //createTable('PROJECT/READ','AddProcess');
            document.getElementById("DataForm").submit();
        })
        .mouseover(function () {
            this.style.background = "lightyellow";
        })
        .mouseleave(function () {
            this.style.background = "white";
        })
        .datepicker({
            dateFormat: "dd-mm-yy",
            changeMonth: true,
            changeYear: true,
            showAnim: "show",
            beforeShow: function () {

            }
        })
}

function loadHomePage(startdate, enddate, page, searchpic, searchstatus, searchproject) {
    //when you get the viewbag from the view to js you have to use 
    //"@ViewBag.StartDate"
    window.location.href = _url + 
        Reload +
        "?StartDate=" + startdate +
        "&EndDate=" + enddate +
        "&searchpic=" + searchpic +
        "&searchstatus=" + searchstatus +
        "&searchproject=" + searchproject +
        "&page="+page;
}

var urlx;

function showInPopup(url, title) {
    urlx = url;
    jQuery.ajax({
        type: "GET",
        url: url,
        success: function (res) {
            jQuery("#form-modal .modal-body").html(res);
            jQuery("#form-modal .modal-title").html(title);
            jQuery("#form-modal").modal('show');
            jQuery("#duedate,#startdate")
                .css({ "cursor": "pointer" })
                .datepicker({
                    dateFormat: "dd-mm-yy",
                    changeMonth: true,
                    changeYear: true,
                    showAnim: "show",
                    beforeShow: function () {
                    }
                })
        }
    })
}
function showUpload() {
    jQuery.ajax({
        type: "GET",
        url: urlx,
        success: function (res) {
            jQuery("#form-modal .modal-body").html(res);
            jQuery("#form-modal").modal('show');
            jQuery("#duedate,#startdate")
                .css({ "cursor": "pointer" })
                .datepicker({
                    dateFormat: "dd-mm-yy",
                    changeMonth: true,
                    changeYear: true,
                    showAnim: "show",
                    beforeShow: function () {
                    }
                })
        }
    })
}

function jQueryAjaxPost(form) {
    try {
        jQuery.ajax({
            type: "POST",
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    jQuery("#form-modal .modal-body").html("");
                    jQuery("#form-modal .modal-title").html("");
                    jQuery("#form-modal").modal('hide');
                    
                    if (res.controllerx == "Detail")
                        window.location.href = _url + res.controllerx +
                            "/ReloadTable?StartDate=" + res.startdate +
                            "&EndDate=" + res.enddate +
                            "&td_tms_id=" + res.td_tms_id +
                            "&taskname=" + res.taskname +
                            "&page=" + res.page +
                            "&email=" + res.email +
                            "&searchpic=" + res.searchpic +
                            "&searchstatus=" + res.searchstatus +
                            "&searchproject=" + res.searchproject +
                            "&duedate=" + document.getElementById("duedate2").value;
                    else if (res.controllerx == "Master")
                        window.location.href = _url + res.controllerx +
                            "/Index?StartDate=" + res.startdate +
                            "&EndDate=" + res.enddate +
                            "&searchpic=" + res.searchpic +
                            "&searchstatus=" + res.searchstatus +
                            "&searchproject=" + res.searchproject +
                            "&page=" + res.page;
                    else
                        window.location.href = _url + res.controllerx +
                            "/ReloadTable?StartDate=" + res.startdate +
                            "&EndDate=" + res.enddate +
                            "&page=" + res.page +
                            "&searchpic=" + res.searchpic +
                            "&searchproject=" + res.searchproject +
                            "&searchstatus=" + res.searchstatus; //in future need to set condition for the delay on and off

                } else {
                    jQuery("#form-modal .modal-body").html(res.html);
                }
                
            },
            error: function (err) {
                console.log(err)
            }
        })
            
        
    } catch (e) {
        console.log(e);
    }

    // to prevent default form submit event
    return false;
}

function jQueryAjaxPostUpload(form) {

    try {
        jQuery.ajax({
            type: "POST",
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    /*
                    jQuery("#form-modal .modal-body").html("");
                    jQuery("#form-modal .modal-title").html("");
                    jQuery("#form-modal").modal('hide');

                    window.location.href =
                        _url + res.controllerx +
                        "/ReloadTable?StartDate=" + res.startdate +
                        "&EndDate=" + res.enddate +
                        "&page=" + res.page; //in future need to set condition for the delay on and off
                        */
                    showInPopup(urlx,'Upload & Download Document');
                } else {
                    alert("Exiting File");
                    jQuery("#form-modal .modal-body").html(res.html);
                }

            },
            error: function (err) {
                console.log(err);
            }
        })


    } catch (e) {
        console.log(e);
    }

    // to prevent default form submit event
    return false;
}

function ProcessDrag(content,tmsid,ri) {

    if (
        document.getElementById("myemail")
    ) {
        jQuery(".process")
            .draggable({
                revert: true,
                start: function () {
                    dragname = jQuery(this).html();
                    this.style.zIndex = 100;//make sure the drag item can go over other item
                }
            })

        //reload the page after drag the process of "Due Date"
        if (content != undefined && (content == final_process || content == "Due Date")) {
            var table = document.getElementById("table_data");
            var Index = "Home/ReloadTable";
            var taskname = table.rows[ri].cells[1].innerText;

            var detailpage = String(window.location.href).indexOf("Detail");
            if (detailpage != -1) {
                Index = "Detail/ReloadTable";
                taskname = document.getElementById("taskname").value;
            }

            window.location.href = _url +
                Index +
                "?id=" + parseInt(table.rows[ri].cells[5].innerText) +
                "&td_tms_id=" + tmsid +
                "&taskname=" + taskname +
                "&page=" + document.getElementById("page").value +
                "&searchpic=" + document.getElementById("searchpic").value +
                "&searchstatus=" + document.getElementById("searchstatus").value +
                "&searchproject=" + document.getElementById("searchproject").value +
                "&email=" + table.rows[ri].cells[7].innerText +
                "&StartDate=" + document.getElementById("startdate").value +
                "&EndDate=" + document.getElementById("enddate").value;
        }


    }
    
}

function DataTableOnClick() {
    jQuery('#table_data td')
        .mouseover(function () {
            drag_ci = jQuery(this).parent().children().index(this);
            drag_ri = jQuery(this).parent().parent().children().index(this.parentNode);
        })
        .click(function (event) {//for multiple select on process
            var table = document.getElementById("table_data");
            ci = jQuery(this).parent().children().index(this);
            ri = jQuery(this).parent().parent().children().index(this.parentNode);

            if (
                ci == 1 &&
                String(window.location.href).indexOf("Detail") == -1 &&
                String(window.location.href).indexOf("Master") == -1
             ) {
                window.location.href = _url + DetailIndex +
                    "?id=" + table.rows[ri].cells[5].innerText +
                    "&taskname=" + table.rows[ri].cells[1].innerText +
                    "&page=" + document.getElementById("page").value +
                    "&StartDate=" + document.getElementById("startdate").value +
                    "&EndDate=" + document.getElementById("enddate").value +
                    "&searchpic=" + document.getElementById("searchpic").value +
                    "&searchstatus=" + document.getElementById("searchstatus").value +
                    "&searchproject=" + document.getElementById("searchproject").value +
                    "&email=" + String(table.rows[ri].cells[7].innerText).trim() +
                    "&duedate=" + String(table.rows[ri].cells[3].innerText).trim();
            }
            if (ci > 5 && ri > 1) {
                //set the permission
                if (
                    document.getElementById("myemail").value ==
                    String(table.rows[ri].cells[6].innerText).trim() ||
                    document.getElementById("myemail").value ==
                    String(table.rows[ri].cells[7].innerText).trim() ||
                    document.getElementById("myemail").value == "superuser@nok.com"
                ) {
                    ShowProcessDeleteItem(table);
                    //to prevent loading the data everytime onclick on the table
                    //if (document.getElementById("div_process").innerHTML == "") {
                        jQuery.ajax({
                            type: 'GET',
                            url: _url + MasterProcess,
                            data: jQuery.param({
                                spec: String(table.rows[ri].cells[6].innerText).trim()
                            }),
                            success: function (result) {

                                var t = "<table id='table_process'>";
                                t += "<tr><th style='display:none'></th><th>ADD</th><th style='display:none'></th><th>X</th></tr>";
                                t += "<tr><th style='display:none'></th><th colspan=3><input id='processInput' style='background:#ffffb3;width:120px' value='' /></th></tr>";
                                for (var j = 0; j < result.length; j++) {
                                    t += "<tr>";
                                    t += "<td style='display:none'>" + (j + 1) + "</td>";
                                    t += "<td style='background-color:" + result[j]["colour"] + "'>" + result[j]["name"] + "</td>";
                                    t += "<td style='display:none'>" + result[j]["colour"] + "</td>";
                                    t += "<td style='background-color:" + result[j]["colour"] + "'></td>";
                                    t += "</tr>";
                                }
                                t += "</table>";

                                jQuery("#div_process")
                                    .show()
                                    .css({
                                        "position": "absolute",
                                        "top": event.clientY - 30 + "px",
                                        "left": event.clientX - 130 + "px"
                                        
                                    })
                                    .html(t)

                                ProcessTableOnClick();
                            }
                        });
                        /*
                    } else {

                        jQuery("#div_process")
                            .show()
                            .css({
                                "position": "absolute",
                                "top": event.clientY - 30 + "px",
                                "left": event.clientX - 130 + "px"
                            })

                    }
                    */
                }

                
            } else {
                jQuery("#div_process").hide();
                jQuery("#div_process_delete").hide();
            }
        })
        .droppable({
            accept: '.process',
            drop: function (event, ui) {
                var table = document.getElementById('table_data');
                ci = jQuery(this).parent().children().index(this);
                ri = jQuery(this).parent().parent().children().index(this.parentNode);
                //set the permission
                if (
                    document.getElementById("myemail").value ==
                    String(table.rows[ri].cells[6].innerText).trim() ||
                    document.getElementById("myemail").value ==
                    String(table.rows[ri].cells[7].innerText).trim() ||
                    document.getElementById("myemail").value == "superuser@nok.com"
                ) {
                    jQuery(ui.draggable).remove();
                    

                    table.rows[ri].cells[ci].innerHTML += "<div class='process'>" + dragname + "</div>";

                    var a = String(dragname).split("background-color:");
                    var b = String(a[1]).split('"');

                    DeleteProcess(ui.draggable.text(), table, b[0]);
                }
                

            }
        });
}

function ShowProcessDeleteItem(table) {
    jQuery("#div_process_delete").html(table.rows[ri].cells[ci].innerHTML);
    var a = document.getElementById("div_process_delete");
    var b = a.getElementsByTagName("div");
    if (b.length > 0) {
        var html = "<table id='table_process_delete'>";;
        html += "<tr>";
        html += "<th>DELETE</th><th style='width:10px'>X</th>";
        html += "</tr>"
        for (var i = 0; i < b.length; i++) {
            html += "<tr>";
            html += "<td><div class='process'>"+b[i].innerHTML+"</div></td><td style='width:10px'></td>";
            html += "</tr>";
        }
        html += "</table>";
        jQuery("#div_process_delete")
            .css({
                "position": "absolute",
                "top": event.clientY - 30 + "px",
                "left": event.clientX - 270 + "px"
            })
            .show()
            .html(html);


        jQuery('#table_process_delete th').click(function () {
            jQuery("#div_process_delete").hide();
        });
        jQuery('#table_process_delete td')
            .click(function (event) {//for multiple select on process
                var ci_process = jQuery(this).parent().children().index(this);
                var ri_process = jQuery(this).parent().parent().children().index(this.parentNode);

                var table_process_delete = document.getElementById("table_process_delete");
                var table = document.getElementById("table_data");

                var detailpage = String(window.location.href).indexOf("Detail");
                var tmsid = "0";
                if (detailpage != -1) tmsid = document.getElementById("td_tms_id").value;

                jQuery.ajax({
                    type: "POST",
                    url: _url + SharedDeleteProcess,
                    data: jQuery.param({
                        id: parseInt(table.rows[ri].cells[5].innerText),
                        date: table.rows[1].cells[ci].innerText,
                        td_tms_id:tmsid,
                        content: table_process_delete.rows[ri_process].cells[0].innerText
                    }),
                    success: function (res) {
                        var content = table_process_delete.rows[ri_process].cells[0].innerText;
                        var table = document.getElementById("table_data");
                        table.rows[ri].cells[ci].innerHTML = "";
                        var a = document.getElementById("div_process_delete");
                        var b = a.getElementsByTagName("div");
                        var c = 0;
                        for (var i = 0; i < b.length; i++) {
                            if (
                                b[i].innerText !="" &&
                                b[i].innerText != table_process_delete.rows[ri_process].cells[0].innerText
                            ) {
                                table.rows[ri].cells[ci].innerHTML += "<div class='process' >" + b[i].innerHTML + "</div>";
                                
                            }
                            if (b[i].innerText == table_process_delete.rows[ri_process].cells[0].innerText){
                                b[i].innerText = "";

                                //table_process_delete.rows[ri_process].cells[0].innerHTML = "";
                                
                            }
                            if (b[i].innerText != "") {
                                c = 1;
                            }
                        }

                        if(c!=1)
                            jQuery("#div_process_delete").hide();

                        ProcessDrag();

                        if (content == final_process) {
                            var Index = "Home/ReloadTable";
                            var taskname = table.rows[ri].cells[1].innerText;
                            var detailpage = String(window.location.href).indexOf("Detail");
                            if (detailpage != -1) {
                                Index = "Detail/ReloadTable";
                                taskname = document.getElementById("taskname").value;
                            }
                            window.location.href = _url +
                                Index +
                                "?id=" + table.rows[ri].cells[5].innerText +
                                "&td_tms_id=" + tmsid +
                                "&taskname=" + taskname +
                                "&email=" + table.rows[ri].cells[7].innerText +
                                "&page=" + document.getElementById("page").value +
                                "&searchpic=" + document.getElementById("searchpic").value +
                                "&searchstatus=" + document.getElementById("searchstatus").value +
                                "&searchproject=" + document.getElementById("searchproject").value +
                                "&StartDate=" + document.getElementById("startdate").value +
                                "&EndDate=" + document.getElementById("enddate").value;
                        }
                    }
                });
            })
    } else {
        jQuery("#div_process_delete").hide();
    }
}

//the following Delete and Add are for the process moving by dragging
function DeleteProcess(content, table, colour) {
    var detailpage = String(window.location.href).indexOf("Detail");
    var tmsid = "0";
    if (detailpage != -1) tmsid = document.getElementById("td_tms_id").value;
    if (content != "Due Date") {
        jQuery.ajax({
            type: "POST",
            url: _url + SharedDeleteProcess,
            data: jQuery.param({
                id: parseInt(table.rows[drag_ri].cells[5].innerText),
                date: table.rows[1].cells[drag_ci].innerText,
                content: content,
                td_tms_id: tmsid
            }),
            success: function (res) {
                AddProcess(content, table, colour, tmsid);
            }
        });
    } else {
        AddProcess(content, table, colour, tmsid);
    }
}
function AddProcess(content, table, colour, tmsid) {
    jQuery.ajax({
        type: "POST",
        url: _url + SharedAddProcess,
        data: jQuery.param({
            id: parseInt(table.rows[ri].cells[5].innerText),
            date: String(table.rows[1].cells[ci].innerText).trim(),
            spec: String(table.rows[ri].cells[6].innerText).trim(),
            taskname: String(table.rows[ri].cells[1].innerText).trim(),
            tdtmsid: tmsid,
            colour:colour,
            content: content.trim()
        }),
        success: function (res) {
            ProcessDrag(
                content,
                tmsid,
                ri);
        }
    });
}

function ProcessTableOnClick() {


    jQuery('#processInput').keyup(function (e) {
        if (e.keyCode == 13 && jQuery(this).val()!="Due Date") {
            var detailpage = String(window.location.href).indexOf("Detail");
            var tmsid = "0";
            if (detailpage != -1) tmsid = document.getElementById("td_tms_id").value;
            var content = jQuery(this).val();
            PostProcess(content, "#ffffff",tmsid);
        }
        if (jQuery(this).val()=="Due Date") {
            alert("The process was not allowed");
        }
            
    });

    jQuery('#table_process th').click(function () {
        var r = jQuery(this).parent().parent().children().index(this.parentNode);
        if (r == 0) {
            jQuery("#div_process").hide();
            jQuery("#div_process_delete").hide();
        }
    });
    jQuery('#table_process td')
        .click(function (event) {//for multiple select on process
            var ci_process = jQuery(this).parent().children().index(this);
            var ri_process = jQuery(this).parent().parent().children().index(this.parentNode);

            var table_process = document.getElementById("table_process");
            var table = document.getElementById("table_data");

            var detailpage = String(window.location.href).indexOf("Detail");
            var tmsid = "0";
            if (detailpage != -1) tmsid = document.getElementById("td_tms_id").value;

            jQuery.ajax({
                type: "POST",
                url: _url + SharedCheckProcess,
                data: jQuery.param({
                    id: parseInt(table.rows[ri].cells[5].innerText),
                    date: table.rows[1].cells[ci].innerText,
                    content: table_process.rows[ri_process].cells[1].innerText,
                    td_tms_id: tmsid  
                }),
                success: function (res) {
                    if (res == "0") {
                        var colour = table_process.rows[ri_process].cells[2].innerText; 
                        var content = table_process.rows[ri_process].cells[1].innerText;
                        PostProcess(content,colour,tmsid);
                    } else {
                        alert("Selected process already exist");
                    }
                }
            });
 
        })
}

function PostProcess(content,colour,tmsid) {
    var table = document.getElementById("table_data");
    jQuery.ajax({
        type: "POST",
        url: _url + SharedAddProcess,
        data: jQuery.param({
            id: parseInt(table.rows[ri].cells[5].innerText),
            spec: String(table.rows[ri].cells[6].innerText).trim(),
            date: table.rows[1].cells[ci].innerText,
            colour: colour,
            content:content,
            email: table.rows[ri].cells[7].innerText,
            tdtmsid: tmsid
        }),
        success: function (res) {

            var table = document.getElementById("table_data");

            table.rows[ri].cells[ci].innerHTML +=
                "<div class='process'><li style='background-color:" + colour + "'>" + content + "</li></div>";
            ShowProcessDeleteItem(table);
            ProcessDrag();

            if (content == final_process) {

                var Index = "Home/ReloadTable";
                var taskname = table.rows[ri].cells[1].innerText;

                var detailpage = String(window.location.href).indexOf("Detail");
                if (detailpage != -1) {
                    Index = "Detail/ReloadTable";
                    taskname = document.getElementById("taskname").value;
                }

                window.location.href = _url +
                    Index +
                    "?id=" + table.rows[ri].cells[5].innerText +
                    "&td_tms_id=" + tmsid +
                    "&taskname=" + taskname +
                    "&page=" + document.getElementById("page").value +
                    "&searchpic=" + document.getElementById("searchpic").value +
                    "&searchstatus=" + document.getElementById("searchstatus").value +
                    "&searchproject=" + document.getElementById("searchproject").value +
                    "&email=" + table.rows[ri].cells[7].innerText +
                    "&StartDate=" + document.getElementById("startdate").value +
                    "&EndDate=" + document.getElementById("enddate").value;
            }

            document.getElementById("processInput").value = "";
            document.getElementById("processInput").focus();

            //jQuery("#div_process").hide();
            //jQuery("#div_process_delete").hide();
        }
    });
}


