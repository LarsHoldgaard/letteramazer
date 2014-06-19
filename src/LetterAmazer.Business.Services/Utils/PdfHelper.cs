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


        public static int GetPagesCount(byte[] pdfFile)
        {
            var pdfReader = new PdfReader(pdfFile);
            int numberOfPages = pdfReader.NumberOfPages;
            pdfReader.Close();
            return numberOfPages;
        }


        public static byte[] ConvertPdfSize(byte[] inPDF, LetterSize fromSize, LetterSize toSize)
        {
            if (fromSize != LetterSize.A4 || toSize != LetterSize.Letter)
            {
                throw new ArgumentException("Function only supports from size A4 to size letter");
            }
            byte[] finalBytes;

            using (var outPDF = new MemoryStream())
            {
                using (PdfReader pdfr = new PdfReader(inPDF))
                {
                    using (Document doc = new Document(PageSize.LETTER))
                    {
                        Document.Compress = true;

                        PdfWriter writer = PdfWriter.GetInstance(doc, outPDF);
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
                finalBytes = outPDF.ToArray();
            }

            return finalBytes;
        }


        public static byte[] WriteSameStrOnAllPages(byte[] inPDF, string str)
        {
            byte[] finalBytes;

            //Another in-memory PDF
            using (var ms = new MemoryStream())
            {
                //Bind a reader to the bytes that we created above
                using (var reader = new PdfReader(inPDF))
                {
                    //Bind a stamper to our reader
                    using (var stamper = new PdfStamper(reader, ms))
                    {

                        //Setup a font to use
                        var baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

                        //Get the raw PDF stream "on top" of the existing content
                        int readerNumber = reader.NumberOfPages+1;
                        for (int i = 1; i < readerNumber; i++)
                        {
                            var cb = stamper.GetOverContent(i);
                            //Draw some text
                            cb.BeginText();
                            cb.SetFontAndSize(baseFont, 7);
                            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, str, 8, 5, 0);
                            cb.EndText();
                        }
                    }
                }

                //Once again, grab the bytes before closing things out
                finalBytes = ms.ToArray();
            }
            return finalBytes;
        }


        public static byte[] WriteIdOnPdf(byte[] inPDF, string str)
        {
            byte[] finalBytes;

            //Another in-memory PDF
            using (var ms = new MemoryStream())
            {
                //Bind a reader to the bytes that we created above
                using (var reader = new PdfReader(inPDF))
                {
                    //Store our page count
                    var pageCount = reader.NumberOfPages;

                    //Bind a stamper to our reader
                    using (var stamper = new PdfStamper(reader, ms))
                    {

                        //Setup a font to use
                        var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

                        //Get the raw PDF stream "on top" of the existing content
                        var cb = stamper.GetOverContent(1);

                        //Draw some text
                        cb.BeginText();
                        
                        cb.SetFontAndSize(baseFont, 8);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT,str,58,805,0);
                        cb.EndText();
                        
                    }
                }

                //Once again, grab the bytes before closing things out
                finalBytes = ms.ToArray();
            }
            return finalBytes;
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
