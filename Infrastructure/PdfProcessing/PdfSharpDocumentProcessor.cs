using Infrastructure.PdfProcessing;
using PdfSharpCore.Pdf.IO;

//     _
//    (-)
//  _(   )_
//  (_`/._)

// a di da phat con xin duoc code no bug 

namespace DocReader.Service.DocumentProcessor {
    public class PdfSharpDocumentProcessor : IDocumentProcessor {

        // 
        public async Task<IEnumerable<(int PageNumber, byte[] PageBytes)>> SplitPdfAsync(Stream pdfStream) {

            // imputDoc dai dien cho toan bo pdf gốc
            using var inputDoc = PdfReader.Open(pdfStream, PdfDocumentOpenMode.Import);
            var res = new List<(int, byte[])>();

            //for (int i = 0; i < pdfStream.Length; i++) { Console.WriteLine(i.ToString());        


            for (int i = 0; i < inputDoc.PageCount; i++) {
                // bay gio minh muon la 1 trang -> 1 file
                // tao 1 file pdf moi
                var outputDoc = new PdfSharpCore.Pdf.PdfDocument();
                var page = inputDoc.Pages[i];
                //
                outputDoc.AddPage(page);



                var ms = new MemoryStream();
                // write outputdoc (1 trang) vao memory stream
                outputDoc.Save(ms);
                // convert stream -> byte
                res.Add((i + 1, ms.ToArray()));
            }


            return res;
        }




    }
}