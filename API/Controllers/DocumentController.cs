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


        public DocumentController(DocumentUploadService uploadService,
                                    DocumentLoadService loadService,
                                    DocumentModifyService modifyService,
                                    DocumentRemoveService removeService) {
            _uploadService = uploadService;
            _loadService = loadService;
            _modifyService = modifyService;
            _removeService = removeService;
        }




        [HttpPost]
        [Route("/upload")]
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



    }
}
