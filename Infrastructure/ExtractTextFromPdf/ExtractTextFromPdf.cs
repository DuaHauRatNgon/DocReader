using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using static System.Net.Mime.MediaTypeNames;

namespace Infrastructure.ExtractTextFromPdf {


    public static class ExtractTextFromPdf {



        public static string Extract(string path, int pageStart = 1, int pageEnd = 3) {


            var sb = new StringBuilder();

            //PdfDocument document = PdfDocument.Open(path);

            using (PdfDocument document = PdfDocument.Open(path)) {
                for (int i = pageStart; i <= pageEnd; i++) {
                    //var page = document.GetPages().FirstOrDefault();
                    var page = document.GetPage(i);
                    //Console.WriteLine(page.Text);
                    //Console.WriteLine();

                    //sb.AppendLine(page.ToString()); // 
                    sb.AppendLine(page.Text);
                }
            }
            return sb.ToString();
        }




        public static string Extract(string docId, int maxPage = 3) {
            string _basePath = Path.Combine(Directory.GetCurrentDirectory(),
                                                            "storage", "documents");
            string dir = Path.Combine(_basePath, docId);

            var sb = new StringBuilder();
            for (int i = 1; i <= maxPage; i++) {
                string path = Path.Combine(dir, $"page_{i}.pdf");
                using var pdf = PdfDocument.Open(path);
                var page = pdf.GetPage(1);
                sb.AppendLine(page.Text);
                sb.AppendLine();
            }

            string outputPath = Path.Combine(dir, "summary_input.txt");
            File.WriteAllText(outputPath, sb.ToString());

            return sb.ToString().Trim();
        }

    }
}
