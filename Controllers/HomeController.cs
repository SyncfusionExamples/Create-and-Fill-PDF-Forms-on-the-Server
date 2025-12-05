using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using Syncfusion.Drawing;
using FormFieldsSample.Models;

namespace FormFieldsSample.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        var model = new UserRegisterationModel
        {
            Name = "John Doe",
            EmailID = "john.doe@example.com",
            PhoneNumber = "011-54893-232",
            DateOfBirth = new DateTime(1995, 5, 12),
            Gender = "Male",
            MaritalStatus = "Single",
            Occupation = "Software Engineer",
            Address = "123 Main Street, Chennai",
            ReceiveNotifications = true,
            GenderList = new SelectList(new[] { "Male", "Female", "Other" }),
            MaritalStatusList = new SelectList(new[] { "Single", "Married", "Other" }),
            OccupationList = new SelectList(new[] { "Doctor", "Teacher", "Software Engineer", "Student", "Entrepreneur", "Other" })
        };
        return View(model);
    }

    // POST: /Home/CreateForm       
    [HttpPost]
    public IActionResult CreateForm(UserRegisterationModel model)
    {
        // Create a new PDF document
        using (PdfDocument pdfDocument = new PdfDocument())
        {
            // Add a new page to the PDF document
            PdfPage pdfPage = pdfDocument.Pages.Add();
            // Get graphics object to draw text and labels on the page
            PdfGraphics pdfGraphics = pdfPage.Graphics;
            //Set the standard font.
            PdfFont titleFont = new PdfStandardFont(PdfFontFamily.Helvetica, 18, PdfFontStyle.Bold);
            PdfFont subtitleFont = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Bold);
            PdfFont labelFont = new PdfStandardFont(PdfFontFamily.Helvetica, 11);
            PdfFont fieldFont = new PdfStandardFont(PdfFontFamily.Helvetica, 11);
            // Layout settings for positioning labels and field
            float labelX = 40f;       // Label column
            float fieldX = 220f;      // Field column
            float rowHeight = 30f;    // Vertical spacing
            float fieldWidth = 300f;  // Field width
            float fieldHeight = 22f;  // Field height
            float currentY = 40f;     // Start position
                                      // Title and subtitle
            pdfGraphics.DrawString("Survey and Research Registration Form", titleFont, PdfBrushes.Black, new PointF(labelX, currentY));
            currentY += 28f;
            pdfGraphics.DrawString("Registration Details", subtitleFont, PdfBrushes.Black, new PointF(labelX, currentY));
            currentY += 28f;
            // Helper method for text fields
            void AddTextField(string fieldName, string labelText, string value)
            {
                // Draw label text
                pdfGraphics.DrawString(labelText, labelFont, PdfBrushes.Black, new PointF(labelX, currentY));
                // Create a text box field and set its properties
                var textField = new PdfTextBoxField(pdfPage, fieldName)
                {
                    Bounds = new RectangleF(fieldX, currentY - 4, fieldWidth, fieldHeight),
                    Font = fieldFont,
                    ToolTip = labelText,
                    Text = value ?? string.Empty
                };
                // Add the text field to the PDF form
                pdfDocument.Form.Fields.Add(textField);
                // Move to the next row
                currentY += rowHeight;
            }
            // Add text fields
            AddTextField("FullName", "Full Name:", model.Name);
            AddTextField("Email", "Email Address:", model.EmailID);
            AddTextField("Phone", "Phone Number:", model.PhoneNumber);
            AddTextField("DateOfBirth", "Date of Birth:", model.DateOfBirth.ToString("yyyy-MM-dd"));
            // Add Gender selection using radio buttons
            pdfGraphics.DrawString("Gender:", labelFont, PdfBrushes.Black, new PointF(labelX, currentY));
            var genderField = new PdfRadioButtonListField(pdfPage, "Gender");
            pdfDocument.Form.Fields.Add(genderField);
            float radioY = currentY - 4;
            float radioSpacing = 110f;
            // Create radio button options for Gender
            var maleOption = new PdfRadioButtonListItem("Male") { Bounds = new RectangleF(fieldX, radioY, 12, 12) };
            var femaleOption = new PdfRadioButtonListItem("Female") { Bounds = new RectangleF(fieldX + radioSpacing, radioY, 12, 12) };
            var otherOption = new PdfRadioButtonListItem("Other") { Bounds = new RectangleF(fieldX + radioSpacing * 2, radioY, 12, 12) };
            genderField.Items.Add(maleOption);
            genderField.Items.Add(femaleOption);
            genderField.Items.Add(otherOption);
            // Draw labels next to radio butto
            pdfGraphics.DrawString("Male", labelFont, PdfBrushes.Black,
                new PointF(maleOption.Bounds.Right + 8, maleOption.Bounds.Top + (maleOption.Bounds.Height - labelFont.Size) / 2));
            pdfGraphics.DrawString("Female", labelFont, PdfBrushes.Black,
                new PointF(femaleOption.Bounds.Right + 8, femaleOption.Bounds.Top + (femaleOption.Bounds.Height - labelFont.Size) / 2));
            pdfGraphics.DrawString("Other", labelFont, PdfBrushes.Black,
                new PointF(otherOption.Bounds.Right + 8, otherOption.Bounds.Top + (otherOption.Bounds.Height - labelFont.Size) / 2));

            genderField.SelectedIndex = model.Gender == "Male" ? 0 : model.Gender == "Female" ? 1 : 2;
            currentY += rowHeight;
            // Add Marital Status selection using radio buttons
            pdfGraphics.DrawString("Marital Status:", labelFont, PdfBrushes.Black, new PointF(labelX, currentY));
            var maritalField = new PdfRadioButtonListField(pdfPage, "MaritalStatus");
            pdfDocument.Form.Fields.Add(maritalField);
            // Create radio button options for Marital Status
            var singleOption = new PdfRadioButtonListItem("Single") { Bounds = new RectangleF(fieldX, currentY - 4, 12, 12) };
            var marriedOption = new PdfRadioButtonListItem("Married") { Bounds = new RectangleF(fieldX + radioSpacing, currentY - 4, 12, 12) };
            var otherMaritalOption = new PdfRadioButtonListItem("Other") { Bounds = new RectangleF(fieldX + radioSpacing * 2, currentY - 4, 12, 12) };
            maritalField.Items.Add(singleOption);
            maritalField.Items.Add(marriedOption);
            maritalField.Items.Add(otherMaritalOption);
            pdfGraphics.DrawString("Single", labelFont, PdfBrushes.Black,
                new PointF(singleOption.Bounds.Right + 8, singleOption.Bounds.Top + (singleOption.Bounds.Height - labelFont.Size) / 2));
            pdfGraphics.DrawString("Married", labelFont, PdfBrushes.Black,
                new PointF(marriedOption.Bounds.Right + 8, marriedOption.Bounds.Top + (marriedOption.Bounds.Height - labelFont.Size) / 2));
            pdfGraphics.DrawString("Other", labelFont, PdfBrushes.Black,
                new PointF(otherMaritalOption.Bounds.Right + 8, otherMaritalOption.Bounds.Top + (otherMaritalOption.Bounds.Height - labelFont.Size) / 2));
            maritalField.SelectedIndex = model.MaritalStatus == "Single" ? 0 : model.MaritalStatus == "Married" ? 1 : 2;
            currentY += rowHeight;
            // Add combo box fields (Occupation)
            pdfGraphics.DrawString("Occupation:", labelFont, PdfBrushes.Black, new PointF(labelX, currentY));
            var occupationField = new PdfComboBoxField(pdfPage, "Occupation")
            {
                Bounds = new RectangleF(fieldX, currentY - 4, fieldWidth, fieldHeight),
                Font = fieldFont,
                ToolTip = "Occupation"
            };
            occupationField.Items.Add(new PdfListFieldItem("Doctor", "Doctor"));
            occupationField.Items.Add(new PdfListFieldItem("Teacher", "Teacher"));
            occupationField.Items.Add(new PdfListFieldItem("Software Engineer", "Software Engineer"));
            occupationField.Items.Add(new PdfListFieldItem("Student", "Student"));
            occupationField.Items.Add(new PdfListFieldItem("Entrepreneur", "Entrepreneur"));
            occupationField.Items.Add(new PdfListFieldItem("Other", "Other"));
            occupationField.SelectedIndex = model.Occupation == "Doctor" ? 0 :
                                            model.Occupation == "Teacher" ? 1 :
                                            model.Occupation == "Software Engineer" ? 2 :
                                            model.Occupation == "Student" ? 3 :
                                            model.Occupation == "Entrepreneur" ? 4 : 5;
            pdfDocument.Form.Fields.Add(occupationField);
            currentY += rowHeight;
            // Add Address field
            AddTextField("Address", "Address:", model.Address);
            // Add Notifications checkbox
            pdfGraphics.DrawString("Notifications:", labelFont, PdfBrushes.Black, new PointF(labelX, currentY));
            var notifyField = new PdfCheckBoxField(pdfPage, "ReceiveNotifications")
            {
                Bounds = new RectangleF(fieldX, currentY - 4, 12, 12),
                ToolTip = "Receive Notifications",
                Checked = model.ReceiveNotifications
            };
            pdfDocument.Form.Fields.Add(notifyField);
            MemoryStream stream = new MemoryStream();
            pdfDocument.Save(stream);
            //If the position is not set to '0' then the PDF will be empty.
            stream.Position = 0;
            FileStreamResult fileStreamResult = new FileStreamResult(stream, "application/pdf");
            fileStreamResult.FileDownloadName = "SurveyRegistrationForm.pdf";
            return fileStreamResult;
        }
    }
}
