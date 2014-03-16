using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace LetterAmazer.Business.Utils.Helpers
{
    public static class PdfHelper
    {
        private const int Margin = 50;

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

        public static byte[] ConvertToPdf(string textToConvert)
        {
            byte[] pdfBytes;
            MemoryStream memoryStream = new MemoryStream();
            memoryStream.Position = 0;
            memoryStream.Flush();

            TextReader reader = new StringReader(textToConvert);
            Document pdfDoc = new Document(PageSize.A4, Margin, Margin, Margin, Margin);
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
            Document pdfDoc = new Document(PageSize.A4, Margin, Margin, Margin, Margin);
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
