using System;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using LetterAmazer.Business.Services.Domain.Products.ProductDetails;

namespace LetterAmazer.Business.Utils.Helpers
{
    public static class PdfHelper
    {
        private const int DocumentMargin = 50;

        public static string GetContentsOfPdf(string fileName)
        {
            StringBuilder text = new StringBuilder();

            if (File.Exists(fileName))
            {
                PdfReader pdfReader = new PdfReader(fileName);

                for (int page = 1; page <= pdfReader.NumberOfPages; page++)
                {
                    ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                    string currentText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);

                    currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));
                    text.Append(currentText);
                }
                pdfReader.Close();
            }
            return text.ToString();
        }


        public static int GetPagesCount(string path)
        {
            var pdfReader = new PdfReader(path);
            int numberOfPages = pdfReader.NumberOfPages;
            pdfReader.Close();
            return numberOfPages;
        }

        public static void ConvertPdfSize(string path, LetterSize fromSize, LetterSize toSize)
        {
            if (fromSize != LetterSize.A4 || toSize != LetterSize.Letter)
            {
                throw new ArgumentException("Function only supports from size A4 to size letter");
            }

            string original = path;
            string inPDF = path;
            string outPDF = "temp.pdf";
            using (PdfReader pdfr = new PdfReader(inPDF))
            {
                using (Document doc = new Document(PageSize.LETTER))
                {
                    Document.Compress = true;

                    PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(outPDF, FileMode.Create));
                    doc.Open();

                    PdfContentByte cb = writer.DirectContent;

                    PdfImportedPage page;

                    for (int i = 1; i < pdfr.NumberOfPages + 1; i++)
                    {
                        page = writer.GetImportedPage(pdfr, i);
                        cb.AddTemplate(page, PageSize.LETTER.Width / pdfr.GetPageSize(i).Width, 0, 0, PageSize.LETTER.Height / pdfr.GetPageSize(i).Height, 0, 0);
                        doc.NewPage();
                    }

                    doc.Close();
                }
                pdfr.Close();
            }
            
            File.Delete(original);
            File.Copy(outPDF, original);
        }

        public static byte[] ConvertToPdf(string textToConvert)
        {
            byte[] pdfBytes;
            MemoryStream memoryStream = new MemoryStream();
            memoryStream.Position = 0;
            memoryStream.Flush();

            TextReader reader = new StringReader(textToConvert);
            Document pdfDoc = new Document(PageSize.A4, DocumentMargin, DocumentMargin, DocumentMargin, DocumentMargin);
            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);

            // step 3: we create a worker parse the document
            HTMLWorker worker = new HTMLWorker(pdfDoc);

            pdfDoc.Open();
            worker.StartDocument();
            worker.Parse(reader);

            worker.EndDocument();

            worker.Close();
            pdfDoc.Close();
            pdfBytes = memoryStream.ToArray();

            return pdfBytes;
        }

        public static void ConvertToPdf(string storepath, string textToConvert)
        {
            TextReader reader = new StringReader(textToConvert);
            Document pdfDoc = new Document(PageSize.A4, DocumentMargin, DocumentMargin, DocumentMargin, DocumentMargin);
            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, new FileStream(storepath, FileMode.OpenOrCreate));

            // step 3: we create a worker parse the document
            HTMLWorker worker = new HTMLWorker(pdfDoc);

            pdfDoc.Open();
            worker.StartDocument();
            worker.Parse(reader);

            worker.EndDocument();
            worker.Close();
            pdfDoc.Close();
        }
    }
}
