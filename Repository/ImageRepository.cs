using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using photo_gallery_api.Entities;
using System.Collections;

namespace photo_gallery_api.Repository
{

    public interface IImageRepository
    {
        //IEnumerable<Image> GetUserFavourites(int userId);
        PagedList<Image> GetUserFavourites(ImageParameters imageParameters, int userId);
        PagedList<Image> GetImages(ImageParameters imageParameters);
        Task<Image> GetImageById(int imageId);
        Task<Image> InsertImage(IFormFile imgfile);
        Task<bool> DeleteImage(Image image);
    }

    public class ImageRepository : IImageRepository
    {
        ApplicationDbContext _context;
        IWebHostEnvironment _hostingEnvironment;

        public ImageRepository(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public PagedList<Image> GetImages(ImageParameters imageParameters)
        {
            return PagedList<Image>.ToPagedList(_context.Images, imageParameters.PageNumber, imageParameters.PageSize);
        }

        //public IEnumerable<Image> GetUserFavourites(int userId)
        //{
        //    IEnumerable<Image> images = _context.Images.Where(image => image.Users.Any(user => user.UserId == userId));
        //    return images;
        //}

        public PagedList<Image> GetUserFavourites(ImageParameters imageParameters, int userId)
        {
            var source = _context.Images.Where(image => image.Users.Any(user => user.UserId == userId));
            return PagedList<Image>.ToPagedList(source, imageParameters.PageNumber, imageParameters.PageSize);
        }

        public async Task<Image> InsertImage(IFormFile imgFile)
        {
            if (await SaveImage(imgFile) != true) return null;

            string imgExt = Path.GetExtension(imgFile.FileName).ToLower();
            string imgName = imgFile.FileName.Substring(0, imgFile.FileName.Length - imgExt.Length);

            Image image = new Image{
                ImageName = imgName,
                ImagePath = Path.Combine("Images", imgFile.FileName),
            };
            Image result = _context.Images.Add(image).Entity;
            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<bool> DeleteImage(Image image)
        {
            _context.Images.Remove(image);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Image> GetImageById(int imageId)
        {
            return await _context.Images.FindAsync(imageId);
        }

        public async Task<bool> SaveImage(IFormFile imgFile)
        {
            string folderPath = Path.Combine(_hostingEnvironment.WebRootPath, "Images");
            string imgExt = Path.GetExtension(imgFile.FileName).ToLower();

            if (imgExt == ".jpg" || imgExt == ".png")
            {
                using (var fileStream = new FileStream(Path.Combine(folderPath, imgFile.FileName), FileMode.Create))
                {
                    await imgFile.CopyToAsync(fileStream);
                    return true;
                }
            }
            else
            {
                return false;
            }
        }


    }
}
