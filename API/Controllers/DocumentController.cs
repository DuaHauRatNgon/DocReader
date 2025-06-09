using Microsoft.AspNetCore.Mvc;

using Application.DTOs;
using Application.Services;

namespace DocReader.Controllers {

    [ApiController]
    [Route("/api/documents")]
    public class DocumentController : ControllerBase {



        private readonly DocumentUploadService _uploadService;
        private readonly DocumentLoadService _loadService;
        private readonly DocumentModifyService _modifyService;
        private readonly DocumentRemoveService _removeService;
        private readonly DocumentLoadingByBatchService _loadPageService;


        public DocumentController(DocumentUploadService uploadService,
                                    DocumentLoadService loadService,
                                    DocumentModifyService modifyService,
                                    DocumentRemoveService removeService,
                                    DocumentLoadingByBatchService loadPageService) {
            _uploadService = uploadService;
            _loadService = loadService;
            _modifyService = modifyService;
            _removeService = removeService;
            _loadPageService = loadPageService;
        }




        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> Upload([FromForm] UploadDocumentRequest request) {
            var docId = await _uploadService.UploadAsync(request);
            //return OkResult(docId);  
            // ok vs okresult 
            return Ok(new { documentId = docId });
        }




        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAll() {
            var res = await _loadService.GetAllAsync();
            return Ok(res);
        }




        [HttpGet]
        [Route("{docId}")]
        public async Task<IActionResult> GetById(string docId) {
            var res = await _loadService.GetByIdAsync(docId);
            return Ok(res);
        }




        [HttpPut("{id}")]
        public async Task<IActionResult> Modify(string id, [FromBody] ModifyDocumentRequest req) {
            await _modifyService.ModifyAsync(id, req);
            return Ok();
        }





        [HttpDelete("{documentId}")]
        public async Task<IActionResult> Remove([FromRoute] Guid documentId) {
            await _removeService.Remove(documentId);
            return NoContent();
        }







        [HttpGet]
        [Route("{id}/pages/batch/base64")]
        public async Task<IActionResult> GetDocumentPagesBase64(Guid id, int pageNumber, int batch_size) {
            var pages = await _loadPageService.GetsSinglePageAsync(id, pageNumber, batch_size);

            var base64Result = from p in pages
                               select new {
                                   pageNumber = p.PageNumber,
                                   pageContentBase64Encoded = p.ContentBase64Encoded
                               };

            return Ok(base64Result);
        }






        [HttpGet]
        [Route("{docId}/pages/batch/zip")]
        public async Task<IActionResult> GetDocumentPagesZip(Guid docId, int pageNumber, int batch_size) {

            return Ok();
        }
    }
}
