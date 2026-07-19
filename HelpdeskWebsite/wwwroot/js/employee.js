/**
 * File name:	    employee.js
 * Purpose: 		Provides client-side functionality for managing Helpdesk employees, including retrieving, 
 *                          displaying, adding, updating, and deleting employee data via API calls.
 * Author:			Milana Meftakhutdinova
 * Date: 			November 5, 2025
 */


$(() => {  // main jQuery routine - executes every on page load, $ is short for jquery 

    const getAll = async (msg) => {
        try {
            $("#employeeList").text("Finding Employee Information...");
            let response = await fetch(`/api/employee`);
            if (response.ok) {
                let payload = await response.json(); // this returns a promise, so we await it 
                buildEmployeeList(payload);
                msg === "" ? // are we appending to an existing message 
                    $("#status").text("Employees Loaded") : $("#status").text(`${msg} - Employees Loaded`);
            } else if (response.status !== 404) { // probably some other client side error 
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else { // else 404 not found 
                $("#status").text("no such path on server");
            } // else

            // get department data
            response = await fetch(`/api/department`);
            if (response.ok) {
                let deps = await response.json(); // this returns a promise, so we await it 
                sessionStorage.setItem("alldepartments", JSON.stringify(deps));
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


    const buildEmployeeList = (data, usealldata = true) => { 
        $("#employeeList").empty();
        div = $(`<div class="list-group-item row d-flex" id="status">Employee Info</div> 
                  <div class= "list-group-item row d-flex text-center" id="heading"> 
                  <div class="col-4 h4">Title</div> 
                  <div class="col-4 h4">First</div> 
                  <div class="col-4 h4">Last</div> 
               </div>`);
        div.appendTo($("#employeeList"));
        usealldata ? sessionStorage.setItem("allemployees", JSON.stringify(data)) : null;
        btn = $(`<button class="list-group-item row d-flex" id="0">...click to add employee</button>`);
        btn.appendTo($("#employeeList"));
        data.forEach(emp => {
            btn = $(`<button class="list-group-item row d-flex" id="${emp.id}">`);
            btn.html(`<div class="col-4" id="employeetitle${emp.id}">${emp.title}</div> 
                      <div class="col-4" id="employeefname${emp.id}">${emp.firstname}</div> 
                      <div class="col-4" id="employeelastnam${emp.id}">${emp.lastname}</div>`
            );
            btn.appendTo($("#employeeList"));
        }); // forEach 
    }; // buildEmployeeList 

    getAll(""); // first grab the data from the server

    $("#employeeList").on('click', (e) => {
        if (!e) e = window.event;
        let id = e.target.parentNode.id;
        if (id === "employeeList" || id === "") {
            id = e.target.id;
        } // clicked on row somewhere else  
        if (id !== "status" && id !== "heading") {
            let data = JSON.parse(sessionStorage.getItem("allemployees"));
            id === "0" ? setupForAdd() : setupForUpdate(id, data);
        } else {
            return false; // ignore if they clicked on heading or status 
        }
    }); // employeeListClick


    const add = async () => {
        try {
            emp = new Object();
            emp.title = $("#TextBoxTitle").val();
            emp.firstname = $("#TextBoxFirstName").val();
            emp.lastname = $("#TextBoxLastName").val();
            emp.email = $("#TextBoxEmail").val();
            emp.phoneno = $("#TextBoxPhone").val();
            emp.departmentId = parseInt($("#ddlDepartments").val());
            emp.id = -1;
            emp.timer = null;
            let em = JSON.parse(sessionStorage.getItem("employee"));
            emp.staffPicture64 = em.staffPicture64;
            // send the employee info to the server asynchronously using POST 
            let response = await fetch("/api/employee", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json; charset=utf-8"
                },
                body: JSON.stringify(emp)
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
            // set up a new client side instance of Employee
            let emp = JSON.parse(sessionStorage.getItem("employee"));
            // pouplate the properties
            emp.title = $("#TextBoxTitle").val();
            emp.firstname = $("#TextBoxFirstName").val();
            emp.lastname = $("#TextBoxLastName").val();
            emp.email = $("#TextBoxEmail").val();
            emp.phoneno = $("#TextBoxPhone").val();
            emp.departmentId = parseInt($("#ddlDepartments").val()); 
            // send the updated back to the server asynchronously using Http PUT
            let response = await fetch("/api/employee", {
                method: "PUT",
                headers: { "Content-Type": "application/json; charset=utf-8" },
                body: JSON.stringify(emp),
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
        loadDepartmentDDL(-1);
        $("#TextBoxTitle").val("");
        $("#TextBoxFirstName").val("");
        $("#TextBoxLastName").val("");
        $("#TextBoxEmail").val("");
        $("#TextBoxPhone").val("");
        sessionStorage.removeItem("employee");
        let validator = $("#EmployeeModalForm").validate();
        validator.resetForm();
        sessionStorage.removeItem("picture");
        $("#uploadstatus").text("");
        $("#imageHolder").html("");
        $("#uploader").val(""); 
        $("#modalstatus").attr("class", "list-group-item row d-flex justify-content-center text-center");
    }; // clearModalFields


    const setupForAdd = () => {
        $("#actionbutton").val("add");
        $("#modaltitle").html("<h4>add employee</h4>");
        $("#theModal").modal("toggle");
        $("#modalstatus").text("add new employee");
        $("#theModalLabel").text("Add");
        $("#deletebutton").hide();
        $("#dialog").hide();
        clearModalFields();
        let employee = {};
        sessionStorage.setItem("employee", JSON.stringify(employee));
        $("#imageHolder").html("");
    }; // setupForAdd


    const setupForUpdate = (id, data) => {
        $("#actionbutton").val("update");
        $("#modaltitle").html("<h4>update employee</h4>");
        clearModalFields();
        data.forEach(employee => {
            if (employee.id === parseInt(id)) {
                $("#TextBoxTitle").val(employee.title);
                $("#TextBoxFirstName").val(employee.firstname);
                $("#TextBoxLastName").val(employee.lastname);
                $("#TextBoxEmail").val(employee.email);
                $("#TextBoxPhone").val(employee.phoneno);
                sessionStorage.setItem("employee", JSON.stringify(employee));
                $("#modalstatus").text("update data");
                $("#theModal").modal("toggle");
                $("#theModalLabel").text("Update");
                $("#deletebutton").show();
                $("#dialog").hide();
                loadDepartmentDDL(employee.departmentId);
                $("#imageHolder").html(`<img height="120" width="110" src="data:img/png;base64,${employee.staffPicture64}" />`);
            } // if 
        }); // data.forEach 
    }; // setupForUpdate


    $("#actionbutton").on("click", () => {
        $("#actionbutton").val() === "update" ? update() : add();
    }); // actionbutton click


    const _delete = async () => {
        let employee = JSON.parse(sessionStorage.getItem("employee"));
        try {
            let response = await fetch(`/api/employee/${employee.id}`, {
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
            });

            $("#yesbutton").on("click", () => {
                $("#dialog").hide();
                $("#modalstatus").text("delete successful!");
                $("#dialogbutton").show();
                _delete();
            });
    }); // deletebutton click


    const loadDepartmentDDL = (empdep) => {
        html = '';
        $('#ddlDepartments').empty();
        let alldepartments = JSON.parse(sessionStorage.getItem('alldepartments'));
        alldepartments.forEach((dep) => { html += `<option value="${dep.id}">${dep.departmentName}</option>` });
        $('#ddlDepartments').append(html);
        $('#ddlDepartments').val(empdep);
    }; // loadDepartmentDDL


    /*validation*/
    document.addEventListener("keyup", e => {
        $("#modalstatus").removeClass();
        if ($("#EmployeeModalForm").valid()) {
            $("#modalstatus").attr("class", "badge text-success"); //green 
            $("#modalstatus").text("data entered is valid");
            $("#actionbutton").prop("disabled", false);
        }
        else {
            $("#modalstatus").attr("class", "badge text-danger");  //red 
            $("#modalstatus").text("fix errors");
            $("#actionbutton").prop("disabled", true);
        }
    });


    $("#EmployeeModalForm").validate({
        rules: {
            TextBoxTitle: { maxlength: 4, required: true, validTitle: true },
            TextBoxFirstName: { maxlength: 25, required: true },
            TextBoxLastName: { maxlength: 25, required: true },
            TextBoxEmail: { maxlength: 40, required: true, email: true },
            TextBoxPhone: { maxlength: 15, required: true }
        },
        errorElement: "div",
        messages: {
            TextBoxTitle: {
                required: "required 1-4 chars.", maxlength: "required 1-4 chars.", validTitle: "Mr. Ms. Mrs. or Dr."
            },
            TextBoxFirstName: {
                required: "required 1-25 chars.", maxlength: "required 1-25 chars."
            },
            TextBoxLastName: {
                required: "required 1-25 chars.", maxlength: "required 1-25 chars."
            },
            TextBoxPhone: {
                required: "required 1-15 chars.", maxlength: "required 1-15 chars."
            },
            TextBoxEmail: {
                required: "required 1-40 chars.", maxlength: "required 1-40 chars.", email: "need valid email format"
            }
        }
    }); //EmployeeModalForm.validate


    $.validator.addMethod("validTitle", (value) => {    //custome rule 
        return (value === "Mr." || value === "Ms." || value === "Mrs." || value === "Dr.");
    }, ""); //.validator.addMethod


    $("#srch").on("keyup", () => {
        let alldata = JSON.parse(sessionStorage.getItem("allemployees"));
        let filtereddata = alldata.filter((emp) => emp.lastname.match(new RegExp($("#srch").val(), 'i')));
        buildEmployeeList(filtereddata, false);
    }); // srch keyup


    $("input:file").on("change", () => {
        try {
            const reader = new FileReader();
            const file = $("#uploader")[0].files[0];
            $("#uploadstatus").text("");
            file ? reader.readAsBinaryString(file) : null;
            reader.onload = (readerEvt) => {
                // get binary data then convert to encoded string 
                const binaryString = reader.result;
                const encodedString = btoa(binaryString);
                // replace the picture in session storage 
                let employee = JSON.parse(sessionStorage.getItem("employee"));
                employee.staffPicture64 = encodedString;
                sessionStorage.setItem("employee", JSON.stringify(employee));
                $("#uploadstatus").text("retrieved local pic")
            };
        } catch (error) {
            $("#uploadstatus").text("pic upload failed")
        }
    }); // input file change

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