﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Kernel.Pdf.Canvas;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using iText.Layout.Properties;
using System.IO;
using System.Security.Cryptography;
using Nethereum.Metamask.Blazor.Server.DB_Models;
using System.IO;
using iText.Layout.Borders;
using System.Text;

namespace Nethereum.Metamask.Blazor.Server.Classes
{
    public class PDF
    {
        //private SHA256 documentHash = SHA256.Create();
        private string documentHash;
        private static string pdfDestination = "D:\\pdf\\{0}.pdf";
        private static string pdfShare = "\\\\DESKTOP-6QJ1MGC\\Share\\PDF\\{0}.pdf";
        private string imageDestination = "D:\\pdf\\diploma.jpg";
        private SHA256 Sha256 = SHA256.Create();

        public PDF(int hash)
        {
            //documentHash = hash;
            pdfDestination = String.Format(pdfDestination, hash);
        }
        public PDF()
        {

        }

        public MemoryStream createMemeoryStream(String byteCode)
        {
            byte[] pdfbytes = Convert.FromBase64String(byteCode);
            MemoryStream stream = new MemoryStream(pdfbytes);
            return stream;
        }

        public MemoryStream GeneratePDF(Diploma_model diploma)
        {
            /*byte[] bytes = new byte[diploma.Name.Length * sizeof(char)];
            System.Buffer.BlockCopy(diploma.Name.ToArray(), 0, bytes, 0, bytes.Length);
            Encoding w1250 = Encoding.GetEncoding(1250);
            Encoding utf8 = Encoding.GetEncoding("utf-8");
            byte[] output = Encoding.Convert(utf8, w1250, utf8.GetBytes(diploma.Name));
            diploma.Name = w1250.GetString(output);

            byte[] bytes1 = new byte[diploma.LastName.Length * sizeof(char)];
            System.Buffer.BlockCopy(diploma.LastName.ToArray(), 0, bytes, 0, bytes.Length);
            Encoding w12501 = Encoding.GetEncoding(1250);
            Encoding utf81 = Encoding.GetEncoding("utf-8");
            byte[] output1 = Encoding.Convert(utf81, w12501, utf81.GetBytes(diploma.LastName));
            diploma.LastName = w1250.GetString(output1);*/

            using (MemoryStream stream = new MemoryStream())
            using (var writer = new PdfWriter(stream))
            {
                //var writer = new PdfWriter(stream);
                var pdf = new PdfDocument(writer);
                //Fonts
                PdfFont times_new_roman = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN, iText.IO.Font.PdfEncodings.CP1257, true);
                PdfFont times_new_roman_bold = PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD, iText.IO.Font.PdfEncodings.CP1257, true);
                //Text Generic
                Text rector = new Text($"{diploma.RectorsName} {diploma.RectorsLastName} \nRektorius \nRector").SetFont(times_new_roman);
                Text dateofissueLT = new Text($"Išdavimo data:           {diploma.DateOfIssue.ToString("yyyy/MM/dd")}").SetFont(times_new_roman);
                Text dateofissueENG = new Text($"Date of issue:          {diploma.DateOfIssue.ToString("dd/MMMM/yyyy")}").SetFont(times_new_roman);
                //Text LT (fix it later)
                Text namepart = new Text($"{diploma.Name.ToUpper()} {diploma.LastName.ToUpper()} ").SetFont(times_new_roman_bold);
                Text identitypart = new Text($"(asmens kodas {diploma.IdentityNumber})").SetFont(times_new_roman);
                Text partBeforeprogramme = new Text($"{diploma.DateOfIssue.Year} metais baigė bakalauro studijų programą ").SetFont(times_new_roman);
                Text programmepart = new Text($"{diploma.StudiesProgramme.ToUpper()} ").SetFont(times_new_roman_bold);
                Text programmerGovermentIDCode = new Text($"(valstybinis kodas {diploma.StudiesProgrammeGovermentCode} )").SetFont(times_new_roman);
                Text beforedegreepart = new Text("ir jam suteiktas").SetFont(times_new_roman);
                Text degreepart = new Text($"{diploma.Degree.ToUpper()} ").SetFont(times_new_roman_bold);
                Text afterdegreepart = new Text("laipsnis").SetFont(times_new_roman);
                Text studiesdirectionpart = new Text($"Studijų kryptis - {diploma.Studiesdirection.ToUpper()}").SetFont(times_new_roman);
                //Text Eng
                Text identitypartENG = new Text($" (personal number/code {diploma.IdentityNumber})").SetFont(times_new_roman);
                //Text degreeENG = new Text($"BACHELORS DEGREE OF {diploma.Degree.ToUpper()} ").SetFont(times_new_roman);
                //Text studiesdirectionENG = new Text($"IN {diploma.Studiesdirection.ToUpper()} ").SetFont(times_new_roman);
                Text degreeENG = new Text($"BACHELORS DEGREE OF INFORMATICS ENGINEERING ").SetFont(times_new_roman);
                Text studiesdirectionENG = new Text($"IN INFORMATICS ENGINEERING ").SetFont(times_new_roman);
                Text partBeforeprogrammeENG = new Text($"in {diploma.DateOfIssue.Year} completed bachelors study programme ").SetFont(times_new_roman);
                Text programmepartENG = new Text($"INFORMATION TECHNOLOGIES").SetFont(times_new_roman_bold);
                Text programmerGovermentIDCodeENG = new Text($"(state code {diploma.StudiesProgrammeGovermentCode} )").SetFont(times_new_roman);
                Text beforedegreepartENG = new Text("and has been awarded").SetFont(times_new_roman);
                //PDF construct
                PageSize pageSize = PageSize.A4.Rotate();
                var document = new Document(pdf, pageSize);
                PdfCanvas canvas = new PdfCanvas(pdf.AddNewPage());
                canvas.AddImage(ImageDataFactory.Create(imageDestination), pageSize, false);
                //document.Add(new Paragraph(diploma.LastName).SetFont(times_new_roman).SetFontSize(50).SetTextAlignment(TextAlignment.CENTER));
                //document.Add(new Paragraph($"{diploma.Name.ToUpper()} {diploma.LastName.ToUpper()} (asmens kodas {diploma.IdentityNumber})").SetFont(times_new_roman).SetFontSize(16).SetTextAlignment(TextAlignment.CENTER));
                //Text spacing (fix it later)
                document.Add(new Paragraph("\n \n \n").SetFont(times_new_roman).SetFontSize(50).SetTextAlignment(TextAlignment.CENTER).SetMultipliedLeading(1.0f));
                document.Add(new Paragraph("\n \n").SetFont(times_new_roman).SetFontSize(25).SetTextAlignment(TextAlignment.CENTER).SetMultipliedLeading(1.0f));
                document.Add(new Paragraph("\n").SetFont(times_new_roman).SetFontSize(15).SetTextAlignment(TextAlignment.CENTER).SetMultipliedLeading(1.0f));
                document.SetProperty(Property.LEADING, new Leading(Leading.MULTIPLIED, 0.5f));
                //Text construct LT part
                document.Add(new Paragraph().Add(namepart).Add(identitypart).SetTextAlignment(TextAlignment.CENTER));
                document.Add(new Paragraph().Add(partBeforeprogramme).Add(programmepart).Add(programmerGovermentIDCode).SetTextAlignment(TextAlignment.CENTER));
                document.Add(new Paragraph().Add(beforedegreepart).SetTextAlignment(TextAlignment.CENTER));
                document.Add(new Paragraph().Add(degreepart).Add(afterdegreepart).SetTextAlignment(TextAlignment.CENTER));
                document.Add(new Paragraph().Add(studiesdirectionpart).SetTextAlignment(TextAlignment.CENTER));
                document.Add(new Paragraph("\n").SetTextAlignment(TextAlignment.CENTER));
                //Text construct ENG part
                document.Add(new Paragraph().Add(namepart).Add(identitypartENG).SetTextAlignment(TextAlignment.CENTER));
                document.Add(new Paragraph().Add(partBeforeprogrammeENG).Add(programmepartENG).Add(programmerGovermentIDCodeENG).SetTextAlignment(TextAlignment.CENTER));
                document.Add(new Paragraph().Add(beforedegreepartENG).SetTextAlignment(TextAlignment.CENTER));
                document.Add(new Paragraph().Add(degreeENG).SetTextAlignment(TextAlignment.CENTER));
                document.Add(new Paragraph().Add(studiesdirectionENG).SetTextAlignment(TextAlignment.CENTER));
                //Rector part
                document.Add(new Paragraph().Add(rector).SetFontSize(11).SetTextAlignment(TextAlignment.LEFT).SetMultipliedLeading(1.0f));
                //metadata part
                //document.Add(new Paragraph().Add(dateofissueLT).SetFontSize(10).SetTextAlignment(TextAlignment.RIGHT).SetMultipliedLeading(0.8f));
                //document.Add(new Paragraph().Add(dateofissueENG).SetFontSize(10).SetTextAlignment(TextAlignment.RIGHT).SetMultipliedLeading(0.8f));
                Table table = new Table(2, true);
                Style style = new Style().SetBorder(Border.NO_BORDER);
                table.SetWidth(200);
                table.SetHorizontalAlignment(HorizontalAlignment.RIGHT);
                //table.AddCell(new Cell("Išdavimo data:").SetTextAlignment(TextAlignment.LEFT)).AddStyle(style);
                //https://itextpdf.com/en/resources/faq/technical-support/itext-7/why-doesnt-getdefaultcellsetborderpdfpcellnoborder-have-any
                //https://kodejava.org/how-do-i-create-table-cell-that-span-multiple-columns-in-itext/
                table.AddCell(new Paragraph("Registracijos Nr").SetTextAlignment(TextAlignment.LEFT));
                table.AddCell(new Paragraph($"{diploma.RegistrationNr}").SetTextAlignment(TextAlignment.LEFT));
                table.AddCell(new Paragraph("Registracijos Nr").SetTextAlignment(TextAlignment.LEFT));
                table.AddCell(new Paragraph("").SetTextAlignment(TextAlignment.LEFT));
                table.AddCell(new Paragraph("Išdavimo data:").SetTextAlignment(TextAlignment.LEFT));
                table.AddCell(new Paragraph(diploma.DateOfIssue.ToString("yyyy/MM/dd")).SetTextAlignment(TextAlignment.LEFT));
                table.AddCell(new Paragraph("Date of sssue:").SetTextAlignment(TextAlignment.LEFT));
                table.AddCell(new Paragraph(diploma.DateOfIssue.ToString("dd/MMMM/yyyy")).SetTextAlignment(TextAlignment.LEFT));
                table.AddCell(new Paragraph("Code in system:").SetTextAlignment(TextAlignment.LEFT));
                table.AddCell(new Paragraph(diploma.quickSearch.ToString()).SetTextAlignment(TextAlignment.LEFT));
                document.Add(table);
                document.Close();
                writer.Close();
                return stream;
            }
            //var stream = new MemoryStream();
        }

        public Tuple<string, String> CreatePDF(Diploma_model diploma)
        {
            //Create PDF
            Tuple<string, String> diplomaInfo;
            using (MemoryStream pdf = GeneratePDF(diploma))
            {
                byte[] streamArray = pdf.ToArray();
                
                using (MemoryStream contents = new MemoryStream(streamArray))
                {
                    //SavePDFToDisk(contents);
                    //get PDF hash
                    documentHash = GetHashSha256(contents);
                    diplomaInfo = new Tuple<string, String>(documentHash, Convert.ToBase64String(streamArray));
                    //return documentHash = GetHashSha256(contents);
                }
            }
            return diplomaInfo;
        }

        public void SavePDFToDisk(MemoryStream contents, string fileName)
        {
            //Creates file. Required only for Debug purposes
            var bytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(bytes);
            }
            //string randomString = BitConverter.ToString(bytes);
            //string stringName = BitConverter.ToString(bytes);

            pdfDestination = String.Format(pdfShare, fileName);
            using (FileStream fs = new FileStream(pdfDestination, FileMode.OpenOrCreate))
            {
                contents.WriteTo(fs);
                fs.Flush();
            }
        }

        public string GetHashSha256(MemoryStream file)
        {
            string result = "";
            byte[] bytes = Sha256.ComputeHash(file);
            foreach (byte b in bytes)
            {
                result += b.ToString("x2");
            }
            try
            {
                //File.Delete(filename);
            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }

    }
}
