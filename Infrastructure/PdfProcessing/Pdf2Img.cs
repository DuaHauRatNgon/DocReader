using PDFtoImage;
using SkiaSharp;
using System.IO;

public class PdfToImageSharpConverter {
    public static void ConvertPdfToJpeg(string pdfPath, string outputPath) {
        // Cách 1: dùng FileStream để ép đúng overload
        using var fileStream = File.OpenRead(pdfPath);
        using var bitmap = Conversion.ToImage(fileStream, page: 0); // đúng overload với Stream

        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(SKEncodedImageFormat.Jpeg, quality: 90);
        using var outStream = File.OpenWrite(outputPath);
        data.SaveTo(outStream);
    }
}
