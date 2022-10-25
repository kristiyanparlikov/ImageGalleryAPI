using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using photo_gallery_api.Repository;
using photo_gallery_api.Entities;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Cors;
using Newtonsoft.Json;

namespace photo_gallery_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;

        public ImageController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        [HttpGet]
        public ActionResult GetImages([FromQuery] ImageParameters imageParameters)
        {
            var images = _imageRepository.GetImages(imageParameters);

            var metadata = new
            {
                images.TotalCount,
                images.PageSize,
                images.CurrentPage,
                images.TotalPages,
                images.HasNext,
                images.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(images);
        }

        [HttpPost]
        public async Task UploadImage([FromForm] IFormFile imgFile)
        {
            try
            {
                if (await _imageRepository.InsertImage(imgFile) != null)
                {   
                    Console.WriteLine("File upload Successfull");
                }
                else
                {
                    Console.WriteLine("File upload Failed");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteImage(int id)
        {
            Image result = await _imageRepository.GetImageById(id);
            if (result == null)
            {
                return NotFound();
            }
            await _imageRepository.DeleteImage(result);
            return NoContent();

        }
    }
}
