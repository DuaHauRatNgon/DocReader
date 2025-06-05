namespace Core.Models.Domain {
    public class DocumentPage {
        public Guid Id { get; set; }
        public Guid DocumentId { get; set; }
        public int PageNumber { get; set; }
        public Document Document { get; set; }
        public string FilePath { get; set; }
    }
}
