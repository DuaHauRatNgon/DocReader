namespace Infrastructure.PdfProcessing {
    public interface IDocumentProcessor {
        Task<IEnumerable<(int PageNumber, byte[] PageBytes)>> SplitPdfAsync(Stream pdfStream);
    }

}
