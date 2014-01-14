using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

namespace LetterAmazer.Business.Services.Services.LetterContent
{
    public class PdfManager
    {
        private const int Margin = 50;

        public PdfManager()
        {
            
        }

        public int GetPagesCount(string path)
        {
            PdfReader pdfReader = new PdfReader(path);
            int numberOfPages = pdfReader.NumberOfPages;
            pdfReader.Close();
            return numberOfPages;
        }

        public byte[] ConvertToPdf(string textToConvert)
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

        public void ConvertToPdf(string storepath, string textToConvert)
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
