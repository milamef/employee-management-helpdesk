using HelpdeskDAL;
using HelpdeskViewModels;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace HelpdeskWebsite.Reports
{
    public class Reports
    {
        public async Task GenerateEmployeeReport(string rootpath)
        {
            var employeeVM = new EmployeeViewModel();
            List<EmployeeViewModel> employees = await employeeVM.GetAll();

            PageSize pg = PageSize.A4;
            var helvetica = PdfFontFactory.CreateFont(StandardFontFamilies.HELVETICA);
            PdfWriter writer = new(rootpath + "/pdfs/employeereport.pdf",
                               new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0));
            PdfDocument pdf = new(writer);
            Document document = new(pdf);  // PageSize(595, 842)

            document.Add(new Image(ImageDataFactory.Create(rootpath + "/img/Logo.png"))
                    .ScaleAbsolute(200, 125)
                    .SetFixedPosition(((pg.GetWidth() - 200) / 2), 710));

            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("\n"));

            document.Add(new Paragraph("Current Employees")
                .SetFont(helvetica)
                .SetFontSize(24)
                .SetBold()
                .SetTextAlignment(TextAlignment.CENTER));

            document.Add(new Paragraph("\n"));

            Table table = new(3);
            table
                .SetWidth(298) // roughly 50% 
                .SetTextAlignment(TextAlignment.CENTER)
                .SetRelativePosition(0, 0, 0, 0)
                .SetHorizontalAlignment(HorizontalAlignment.CENTER);

            table.AddCell(new Cell().Add(new Paragraph("Title")
                 .SetFontSize(16)
                 .SetBold()
                 .SetPaddingLeft(18)
                 .SetTextAlignment(TextAlignment.LEFT))
                 .SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("First Name")
                 .SetFontSize(16)
                 .SetBold()
                 .SetPaddingLeft(16)
                 .SetTextAlignment(TextAlignment.LEFT))
                 .SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Last Name")
                 .SetBold()
                 .SetFontSize(16)
                 .SetPaddingLeft(16)
                 .SetTextAlignment(TextAlignment.LEFT))
                 .SetBorder(Border.NO_BORDER));

            // Print the list of employees
            foreach (var employee in employees)
            {
                table.AddCell(new Cell().Add(new Paragraph(employee.Title)
                .SetFontSize(14)
                .SetPaddingLeft(24)
                .SetTextAlignment(TextAlignment.LEFT))
                .SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(employee.Firstname)
                .SetFontSize(14)
                .SetPaddingLeft(24)
                .SetTextAlignment(TextAlignment.LEFT))
                .SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(employee.Lastname)
                .SetFontSize(14)
                .SetPaddingLeft(24)
                .SetTextAlignment(TextAlignment.LEFT))
                .SetBorder(Border.NO_BORDER));
            }
            document.Add(table);

            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("\n"));

            document.Add(new Paragraph("Employee report written on - " + DateTime.Now)
                    .SetFontSize(10)
                    .SetTextAlignment(TextAlignment.CENTER));

            document.Close();
        }


        public async Task GenerateCallReport(string rootpath)
        {
            var callVM = new CallViewModel();
            List<CallViewModel> calls = await callVM.GetAll();

            PageSize pg = PageSize.A4.Rotate();
            var helvetica = PdfFontFactory.CreateFont(StandardFontFamilies.HELVETICA);
            PdfWriter writer = new(rootpath + "/pdfs/callreport.pdf",
                               new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0));
            PdfDocument pdf = new(writer);
            Document document = new(pdf, pg);  // PageSize(595, 842)

            document.Add(new Image(ImageDataFactory.Create(rootpath + "/img/Logo.png"))
                    .ScaleToFit(150, 100)
                    .SetHorizontalAlignment(HorizontalAlignment.CENTER));

            document.Add(new Paragraph("Current Calls")
                .SetFont(helvetica)
                .SetFontSize(24)
                .SetBold()
                .SetTextAlignment(TextAlignment.CENTER));


            Table table = new(6);
            table
                .SetWidth(700) // roughly 50% 
                .SetTextAlignment(TextAlignment.CENTER)
                .SetRelativePosition(0, 0, 0, 0)
                .SetHorizontalAlignment(HorizontalAlignment.CENTER);

            table.AddCell(new Cell().Add(new Paragraph("Opened")
                 .SetFontSize(16)
                 .SetBold()
                 .SetPaddingLeft(18)
                 .SetTextAlignment(TextAlignment.LEFT))
                 .SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Last Name")
                 .SetFontSize(16)
                 .SetBold()
                 .SetPaddingLeft(16)
                 .SetTextAlignment(TextAlignment.LEFT))
                 .SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Tech")
                 .SetBold()
                 .SetFontSize(16)
                 .SetPaddingLeft(16)
                 .SetTextAlignment(TextAlignment.LEFT))
                 .SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Problem")
                 .SetBold()
                 .SetFontSize(16)
                 .SetPaddingLeft(16)
                 .SetTextAlignment(TextAlignment.LEFT))
                 .SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Status")
                 .SetBold()
                 .SetFontSize(16)
                 .SetPaddingLeft(16)
                 .SetTextAlignment(TextAlignment.LEFT))
                 .SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Closed")
                 .SetBold()
                 .SetFontSize(16)
                 .SetPaddingLeft(16)
                 .SetTextAlignment(TextAlignment.LEFT))
                 .SetBorder(Border.NO_BORDER));

            //Print the list of calls
            foreach (var call in calls)
            {
                string statusText;
                string closedDateText;

                if (call.OpenStatus)
                {
                    statusText = "Open";
                    closedDateText = "-";
                }
                else
                {
                    statusText = "Closed";
                    closedDateText = call.DateClosed.HasValue
                        ? call.DateClosed.Value.ToShortDateString()
                        : "-";
                }

                table.AddCell(new Cell().Add(new Paragraph(call.DateOpened.ToShortDateString())
                .SetFontSize(11)
                .SetPaddingLeft(24)
                .SetTextAlignment(TextAlignment.LEFT))
                .SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(call.EmployeeName)
                .SetFontSize(11)
                .SetPaddingLeft(24)
                .SetTextAlignment(TextAlignment.LEFT))
                .SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(call.TechName)
                .SetFontSize(11)
                .SetPaddingLeft(24)
                .SetTextAlignment(TextAlignment.LEFT))
                .SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(call.ProblemDescription)
                .SetFontSize(11)
                .SetPaddingLeft(24)
                .SetTextAlignment(TextAlignment.LEFT))
                .SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(statusText)
                .SetFontSize(11)
                .SetPaddingLeft(24)
                .SetTextAlignment(TextAlignment.LEFT))
                .SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(closedDateText)
                .SetFontSize(11)
                .SetPaddingLeft(24)
                .SetTextAlignment(TextAlignment.LEFT))
                .SetBorder(Border.NO_BORDER));
            }
            document.Add(table);

            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("\n"));

            document.Add(new Paragraph("Call report written on - " + DateTime.Now)
                    .SetFontSize(10)
                    .SetTextAlignment(TextAlignment.CENTER));

            document.Close();
        }
    }
}
