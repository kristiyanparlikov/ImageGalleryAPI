using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using photo_gallery_api.Entities;
using photo_gallery_api.Models;
using photo_gallery_api.Repository;

namespace photo_gallery_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavouritesController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;

        public FavouritesController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        [HttpGet]
        public ActionResult GetFavourites(int userId, [FromQuery] ImageParameters imageParameters)
        {
            var images = _imageRepository.GetFavouritesPaged(imageParameters, userId);
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
        public async Task<ActionResult> AddFavourite(FavouriteModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (await _imageRepository.InsertFavourite(model.UserId, model.ImageId))
                {
                    return Ok("Image saved to favourites");
                }
                else
                {
                    return BadRequest("Image could not be added to favourites");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteFavourite(FavouriteModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (await _imageRepository.RemoveFavourite(model.UserId, model.ImageId))
                {
                    return Ok("Image removed from favourites");
                }
                else
                {
                    return BadRequest("Image could not be removed from favourites");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}
