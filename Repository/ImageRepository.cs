using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using photo_gallery_api.Entities;
using photo_gallery_api.Models;
using System.Collections;

namespace photo_gallery_api.Repository
{

    public interface IImageRepository
    {
        IEnumerable<Image> GetFavourites(int userId);
        PagedList<ImageModel> GetFavouritesPaged(ImageParameters imageParameters, int userId);       
        Task<bool> InsertFavourite(int userId, int imageId);
        Task<bool> RemoveFavourite(int userId, int imageId);
        PagedList<Image> GetImagesPaged(ImageParameters imageParameters);
        Task<Image> GetImageById(int imageId);
        Task<Image> InsertImage(IFormFile imgfile);
        Task<bool> RemoveImage(Image image);


        PagedList<ImageModel> GetImagesIncludingFavouritePaged(ImageParameters imageParameters, int userId);
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



        public PagedList<Image> GetImagesPaged(ImageParameters imageParameters)
        {
            return PagedList<Image>.ToPagedList(_context.Images, imageParameters.PageNumber, imageParameters.PageSize);
        }

        public PagedList<ImageModel> GetImagesIncludingFavouritePaged(ImageParameters imageParameters, int userId)
        {
            //retrieve the images in a page
            PagedList<Image> images = PagedList<Image>.ToPagedList(_context.Images, imageParameters.PageNumber, imageParameters.PageSize);

            //retrieve user's favourite images
            List<Image> favourites = GetFavourites(userId).ToList();

            //add default favourite property to all images in the page
            List<ImageModel> favouriteImages = images.ConvertAll(i => new ImageModel { ImageId = i.ImageId, ImageName = i.ImageName, ImagePath = i.ImagePath, Favourite = false }).ToList();

            //change favourite property to match user's favourites
            foreach(ImageModel image in favouriteImages)
            {
                foreach(Image favourite in favourites)
                {
                    if(image.ImageId == favourite.ImageId)
                    {
                        image.Favourite = true;
                    }
                }
            }
            //return all images in a page
            return new PagedList<ImageModel>(favouriteImages, _context.Images.Count(), imageParameters.PageNumber, imageParameters.PageSize);
            
        }

        public IEnumerable<Image> GetFavourites(int userId)
        {
            IEnumerable<Image> images = _context.Images.Where(image => image.Users.Any(user => user.UserId == userId));
            return images;
        }

        public PagedList<ImageModel> GetFavouritesPaged(ImageParameters imageParameters, int userId)
        {
            //retrieve user favourites 
            var source = _context.Images.Where(image => image.Users.Any(user => user.UserId == userId));

            //compose a paged list of the user favourites
            PagedList<Image> favourites = PagedList<Image>.ToPagedList(source, imageParameters.PageNumber, imageParameters.PageSize);

            //add default favourite property to all images in the page
            List<ImageModel> favouriteImages = favourites.ConvertAll(i => new ImageModel { ImageId = i.ImageId, ImageName = i.ImageName, ImagePath = i.ImagePath, Favourite = true }).ToList();

            //return favourites in a page
            return new PagedList<ImageModel>(favouriteImages, source.Count(), imageParameters.PageNumber, imageParameters.PageSize);
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

        public async Task<bool> RemoveImage(Image image)
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

        public async Task<bool> InsertFavourite(int userId, int imageId)
        {
            var user = _context.Users.FirstOrDefault(x => x.UserId == userId);
            var image = _context.Images.FirstOrDefault(i => i.ImageId == imageId);

            var favourite = new Favourite
            {
                Image = image,
                User = user,
            };

            _context.Add(favourite);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveFavourite(int userId, int imageId)
        {
            var favourite = _context.Favourites.First(row => row.UserId == userId && row.ImageId == imageId);
            if (favourite == null) return false;
            _context.Remove(favourite);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
