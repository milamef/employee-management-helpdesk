/**
 * File name:	    call.js
 * Purpose: 		Provides client-side functionality for managing Helpdesk calls, including retrieving, 
 *                          displaying, adding, updating, and deleting call data via API calls.
 * Author:			Milana Meftakhutdinova
 * Date: 			November 26, 2025
 */

$(() => {  // main jQuery routine

    const getAll = async (msg) => {
        try {
            $("#callList").text("Finding Call Information...");
            let response = await fetch(`/api/call`);
            if (response.ok) {
                let payload = await response.json(); // this returns a promise, so we await it 
                buildCallList(payload);
                msg === "" ? // are we appending to an existing message 
                    $("#status").text("Calls Loaded") : $("#status").text(`${msg} - Calls Loaded`);
            } else if (response.status !== 404) { // probably some other client side error 
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else { // else 404 not found 
                $("#status").text("no such path on server");
            } // else


            // get problem data
            response = await fetch(`/api/problem`);
            if (response.ok) {
                let probs = await response.json(); // this returns a promise, so we await it 
                sessionStorage.setItem("allproblems", JSON.stringify(probs));
            } else if (response.status !== 404) { // probably some other client side error 
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else { // else 404 not found 
                $("#status").text("no such path on server");
            } // else

            // get employee data
            response = await fetch(`/api/employee`);
            if (response.ok) {
                let emps = await response.json(); // this returns a promise, so we await it 
                sessionStorage.setItem("allemployees", JSON.stringify(emps));
            } else if (response.status !== 404) { // probably some other client side error 
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else { // else 404 not found 
                $("#status").text("no such path on server");
            } // else

        } catch (error) {
            $("#status").text(error.message);
        }
    }; // getAll    


    const buildCallList = (data, usealldata = true) => {
        $("#callList").empty();
        div = $(`<div class="list-group-item row d-flex" id="status">Call Info</div> 
                  <div class= "list-group-item row d-flex text-center" id="heading"> 
                  <div class="col-4 h4">Date</div> 
                  <div class="col-4 h4">For</div> 
                  <div class="col-4 h4">Problem</div> 
               </div>`);
        div.appendTo($("#callList"));
        usealldata ? sessionStorage.setItem("allcalls", JSON.stringify(data)) : null;
        btn = $(`<button class="list-group-item row d-flex" id="0">...click to add call</button>`);
        btn.appendTo($("#callList"));
        data.forEach(call => {
            let formattedDate = call.dateOpened ? call.dateOpened.substring(0, 16).replace('T', ' ') : '';

            btn = $(`<button class="list-group-item row d-flex" id="${call.id}">
                  <div class="col-4" id="calldate${call.id}">${formattedDate}</div> 
                  <div class="col-4" id="callemployeename${call.id}">${call.employeeName}</div> 
                  <div class="col-4" id="callproblemdescription${call.id}">${call.problemDescription}</div>
                </button>`);
            btn.appendTo($("#callList"));
        }); // forEach 
    }; // buildCallList 

    getAll(""); // first grab the data from the server


    $("#callList").on('click', (e) => {
        if (!e) e = window.event;
        let id = e.target.parentNode.id;
        if (id === "callList" || id === "") {
            id = e.target.id;
        } // clicked on row somewhere else  
        if (id !== "status" && id !== "heading") {
            let data = JSON.parse(sessionStorage.getItem("allcalls"));
            id === "0" ? setupForAdd() : setupForUpdate(id, data);
        } else {
            return false; // ignore if they clicked on heading or status 
        }
    }); // callListClick


    const add = async () => {
        try {
            call = new Object();
            call.problemId = parseInt($("#ddlProblems").val());
            call.employeeId = parseInt($("#ddlEmployees").val());
            call.techId = parseInt($("#ddlTechnicians").val());
            call.dateClosed = null;
            call.openStatus = true;
            call.notes = $("#notes").val();;
            call.id = -1;
            call.timer = null;

            call.dateOpened = formatDateForServer(new Date());

            // send the call info to the server asynchronously using POST 
            let response = await fetch("/api/call", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json; charset=utf-8"
                },
                body: JSON.stringify(call)
            });
            if (response.ok) // or check for response.status  
            {
                let data = await response.json();
                getAll(data.msg);
            } else if (response.status !== 404) { // probably some other client side error 
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else { // else 404 not found 
                $("#status").text("no such path on server");
            } // else 
        } catch (error) {
            $("#status").text(error.message);
        }  // try/catch 
        $("#theModal").modal("toggle");
    }; // add


    const update = async (e) => {
        // action button click event handler
        try {
            // set up a new client side instance of Call
            let call = JSON.parse(sessionStorage.getItem("call"));

            // pouplate the properties
            call.problemId = parseInt($("#ddlProblems").val());
            call.employeeId = parseInt($("#ddlEmployees").val());
            call.techId = parseInt($("#ddlTechnicians").val());
            call.notes = $("#notes").val();

            // handle the close checkbox
            if ($("#checkBoxClose").is(":checked")) {
                call.openStatus = false;
                // only set dateClosed if it wasn't already set
                if (!call.dateClosed) {
                    call.dateClosed = formatDateForServer(new Date());
                }
            } else {
                call.openStatus = true;
                call.dateClosed = null;
            }

            // send the updated back to the server asynchronously using Http PUT
            let response = await fetch("/api/call", {
                method: "PUT",
                headers: { "Content-Type": "application/json; charset=utf-8" },
                body: JSON.stringify(call),
            });
            if (response.ok) {
                // or check for response.status
                let payload = await response.json();
                getAll(payload.msg); // Pass the message for status

                $("#theModal").modal("toggle");
            } else if (response.status !== 404) {
                // probably some other client side error
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else {
                // else 404 not found
                $("#status").text("no such path on server");
            } // else
        } catch (error) {
            $("#status").text(error.message);
            console.table(error);
        } // try/catch
    }; // update


    const clearModalFields = () => {
        loadProblemDDL(-1);
        loadEmployeeDDL(-1);
        loadTechnicianDDL(-1);
        $("#dateOpened").text("");
        $("#dateClosed").text("");
        $("#notes").val("");
        $("#checkBoxClose").prop("checked", false);
        sessionStorage.removeItem("call");

        // re-enable all fields
        $("#ddlProblems").prop("disabled", false);
        $("#ddlEmployees").prop("disabled", false);
        $("#ddlTechnicians").prop("disabled", false);
        $("#notes").prop("disabled", false);
        $("#checkBoxClose").prop("disabled", false);

        $("#actionbutton").show();
        $("#actionbutton").prop("disabled", false);

        let validator = $("#CallModalForm").validate();
        validator.resetForm();

        $("#modalstatus").attr("class", "list-group-item row d-flex justify-content-center text-center");
    }; // clearModalFields


    const setupForAdd = () => {
        clearModalFields();
        $("#actionbutton").val("add");
        $("#actionbutton").show();
        $("#actionbutton").prop("disabled", true);
        $("#modaltitle").html("<h4>Call Information</h4>");
        $("#modalstatus").text("add new call");
        $("#theModalLabel").text("Add");
        $("#deletebutton").hide();
        $("#dialog").hide();
        $("#rowDateClosed").hide();
        $("#rowCheckBoxClose").hide();

        let formattedDate = formatDate(new Date());
        $("#dateOpened").text(formattedDate);
        $("#theModal").modal("toggle");
    }; // setupForAdd


    const setupForUpdate = (id, data) => {
        $("#actionbutton").val("update");
        $("#modaltitle").html("<h4>Call information</h4>");
        clearModalFields();

        data.forEach(call => {
            if (call.id === parseInt(id)) {
                sessionStorage.setItem("call", JSON.stringify(call));

                // display formatted dates for user
                let formattedDateOpened = formatDate(call.dateOpened);
                let formattedDateClosed = call.dateClosed ? formatDate(call.dateClosed) : "";

                $("#dateOpened").text(formattedDateOpened);
                $("#dateClosed").text(formattedDateClosed);
                $("#notes").val(call.notes);

                $("#checkBoxClose").prop("checked", !call.openStatus);

                $("#modalstatus").text("enter/update data");
                $("#theModal").modal("toggle");
                $("#theModalLabel").text("Update");
                $("#deletebutton").show();
                $("#dialog").hide();
                $("#rowDateClosed").show();
                $("#rowCheckBoxClose").show();

                loadProblemDDL(call.problemId);
                loadEmployeeDDL(call.employeeId);
                loadTechnicianDDL(call.techId);

                // if call is closed (openStatus is false), disable all fields
                if (!call.openStatus) {
                    $("#ddlProblems").prop("disabled", true);
                    $("#ddlEmployees").prop("disabled", true);
                    $("#ddlTechnicians").prop("disabled", true);
                    $("#notes").prop("disabled", true);
                    $("#checkBoxClose").prop("disabled", true);
                    $("#actionbutton").hide();
                    $("#modalstatus").text("view only - call is closed");
                } else {
                    // make fields enabled for open calls
                    $("#ddlProblems").prop("disabled", false);
                    $("#ddlEmployees").prop("disabled", false);
                    $("#ddlTechnicians").prop("disabled", false);
                    $("#notes").prop("disabled", false);
                    $("#checkBoxClose").prop("disabled", false);
                    $("#actionbutton").show();
                }
            } // if 
        }); // data.forEach 
    }; // setupForUpdate


    $("#actionbutton").on("click", () => {
        $("#actionbutton").val() === "update" ? update() : add();
    }); // actionbutton click


    const _delete = async () => {
        let call = JSON.parse(sessionStorage.getItem("call"));
        try {
            let response = await fetch(`/api/call/${call.id}`, {
                method: 'DELETE',
                headers: { 'Content-Type': 'application/json; charset=utf-8' }
            });
            if (response.ok) // or check for response.status  
            {
                let data = await response.json();
                getAll(data.msg);
            } else {
                $('#status').text(`Status - ${response.status}, Problem on delete server side, see server console`);
            } // else 
            $('#theModal').modal('toggle');
        } catch (error) {
            $('#status').text(error.message);
        }
    };  // _delete


    $("#deletebutton").on("click", () => {
        $("#modalstatus").text("");
        $("#dialog").show();
        $("#dialogbutton").hide();

        $("#nobutton").on("click", (e) => {
            $("#dialog").hide();
            $("#modalstatus").text("delete cancelled!");
            $("#dialogbutton").show();

            // show action button only if call is not closed
            let call = JSON.parse(sessionStorage.getItem("call"));
            if (call.openStatus) {
                $("#actionbutton").show();
            }
        });

        $("#yesbutton").on("click", () => {
            $("#dialog").hide();
            $("#modalstatus").text("delete successful!");
            $("#dialogbutton").show();
            _delete();
        });
    }); // deletebutton click


    const loadProblemDDL = (callprob) => {
        html = '';
        $('#ddlProblems').empty();
        let allproblems = JSON.parse(sessionStorage.getItem('allproblems'));
        allproblems.forEach((prob) => { html += `<option value="${prob.id}">${prob.description}</option>` });
        $('#ddlProblems').append(html);
        $('#ddlProblems').val(callprob);
    }; // loadDepartmentDDL


    const loadEmployeeDDL = (callemp) => {
        html = '';
        $('#ddlEmployees').empty();
        let allemployees = JSON.parse(sessionStorage.getItem('allemployees'));
        allemployees.forEach((emp) => { html += `<option value="${emp.id}">${emp.lastname}</option>` });
        $('#ddlEmployees').append(html);
        $('#ddlEmployees').val(callemp);
    }; // loadDepartmentDDL


    const loadTechnicianDDL = (calltech) => {
        html = '';
        $('#ddlTechnicians').empty();
        let allemployees = JSON.parse(sessionStorage.getItem('allemployees'));
        let technicians = allemployees.filter(emp => emp.isTech === true);

        technicians.forEach((tech) => { html += `<option value="${tech.id}">${tech.lastname}</option>` });
        $('#ddlTechnicians').append(html);
        $('#ddlTechnicians').val(calltech);
    }; // loadDepartmentDDL


    $("#checkBoxClose").on("click", () => {
        if ($("#checkBoxClose").is(":checked")) {
            $("#dateClosed").text(formatDate(new Date()));
        } else {
            $("#dateClosed").text("");
        }
    }); // checkBoxClose


    const formatDate = (date) => {
        if (!date) return "";
        let d = new Date(date);
        let _day = d.getDate();
        if (_day < 10) { _day = "0" + _day; }
        let _month = d.getMonth() + 1;
        if (_month < 10) { _month = "0" + _month; }
        let _year = d.getFullYear();
        let _hour = d.getHours();
        if (_hour < 10) { _hour = "0" + _hour; }
        let _min = d.getMinutes();
        if (_min < 10) { _min = "0" + _min; }

        return _year + "-" + _month + "-" + _day + " " + _hour + ":" + _min;
    }; // formatDate


    const formatDateForServer = (date) => {
        let d = new Date(date);
        let _day = d.getDate();
        if (_day < 10) { _day = "0" + _day; }
        let _month = d.getMonth() + 1;
        if (_month < 10) { _month = "0" + _month; }
        let _year = d.getFullYear();
        let _hour = d.getHours();
        if (_hour < 10) { _hour = "0" + _hour; }
        let _min = d.getMinutes();
        if (_min < 10) { _min = "0" + _min; }

        return _year + "-" + _month + "-" + _day + "T" + _hour + ":" + _min + ":00";
    }; // formatDateForServer


    $("#srch").on("keyup", () => {
        let alldata = JSON.parse(sessionStorage.getItem("allcalls"));
        let filtereddata = alldata.filter((call) => call.employeeName.match(new RegExp($("#srch").val(), 'i')));
        buildCallList(filtereddata, false);
    }); // srch keyup


    /*validation*/
    $("#CallModalForm").on("keyup change", "input, select, textarea", function (e) {
        $("#modalstatus").removeClass();
        if ($("#CallModalForm").valid()) {
            $("#modalstatus").attr("class", "badge text-success"); //green 
            $("#modalstatus").text("data entered is valid");
            $("#actionbutton").prop("disabled", false);
        }
        else {
            $("#modalstatus").attr("class", "badge text-danger");  //red 
            $("#modalstatus").text("fix errors");
            $("#actionbutton").prop("disabled", true);
        }
    }); // CallModalForm keyup


    $("#CallModalForm").validate({
        rules: {
            ddlProblems: { required: true },
            ddlEmployees: { required: true },
            ddlTechnicians: { required: true },
            notes: { maxlength: 350, required: true}
        },
        errorElement: "div",
        messages: {
            ddlProblems: {
                required: "select Problem"
            },
            ddlEmployees: {
                required: "select Employee"
            },
            ddlTechnicians: {
                required: "select Technician"
            },
            notes: {
                required: "required 1-350 chars.", maxlength: "required 1-350 chars."
            }
        }
    }); //CallModalForm.validate 


}); // jQuery ready method


const errorRtn = (problemJson, status) => {
    if (status > 499) {
        $("#status").text("Problem server side, see debug console");
    } else {
        let keys = Object.keys(problemJson.errors)
        problem = {
            status: status,
            statusText: problemJson.errors[keys[0]][0], // first error 
        };
        $("#status").text("Problem client side, see browser console");
        console.log(problem);
    } // else 
} 