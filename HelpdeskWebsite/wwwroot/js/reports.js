$(() => {
    // reports.js 
    $("#employeereport").on("click", async (e) => {
        try {
            $("#lblstatus").text("generating report on server - please wait...");
            let response = await fetch(`/api/employeereport`);
            if (!response.ok)
                // check for response.status 
                throw new Error(
                    `Status - ${response.status}, Text - ${response.statusText}`
                );
            let data = await response.json(); // this returns a promise, so we await it 
            $("#lblstatus").text("report generated");
            data.msg === "Report Generated"
                ? window.open("/pdfs/employeereport.pdf")
                : $("#lblstatus").text("problem generating report");
        } catch (error) {
            $("#lblstatus").text(error.message);
        } // try/catch 
    }); // button click


    $("#callreport").on("click", async (e) => {
        try {
            $("#lblstatus").text("generating report on server - please wait...");
            let response = await fetch(`/api/callreport`);
            if (!response.ok)
                // check for response.status 
                throw new Error(
                    `Status - ${response.status}, Text - ${response.statusText}`
                );
            let data = await response.json(); // this returns a promise, so we await it 
            $("#lblstatus").text("report generated");

            if (data.msg === "Report Generated") {
                setTimeout(() => {
                    window.open("/pdfs/callreport.pdf");
                }, 500); // 500 ms delay
            } else {
                $("#lblstatus").text("problem generating report");
            }

        } catch (error) {
            $("#lblstatus").text(error.message);
        } // try/catch 
    }); // button click
}); // jQuery