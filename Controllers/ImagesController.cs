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
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetImage(int id)
        {
            var image = await _imageRepository.GetImageById(id);
            if (image == null) return NotFound(new { message = $"An existing record with id '{id}' was not found." });
            return Ok(image);
        }

        [HttpGet]
        public ActionResult GetImages([FromQuery] ImageParameters imageParameters, int? userId = null)
        {
            if (userId.HasValue)
            {
                var images = _imageRepository.GetImagesIncludingFavouritePaged(imageParameters, (int)userId);

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
            else
            {
                var images = _imageRepository.GetImagesPaged(imageParameters);

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

        }

        [HttpPost]
        public async Task<ActionResult> UploadImage([FromForm] IFormFile imgFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _imageRepository.InsertImage(imgFile);

            if (result == null) return BadRequest("Image could not be uploaded");

            return Created("Image uploaded successfully!", imgFile);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteImage(int id)
        {
            Image result = await _imageRepository.GetImageById(id);

            if (result == null)
            {
                return NotFound(new { message = $"An existing record with id '{id}' was not found." });
            }

            await _imageRepository.RemoveImage(result);
            return NoContent();

        }
    }
}
