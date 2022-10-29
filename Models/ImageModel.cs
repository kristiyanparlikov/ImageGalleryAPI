namespace photo_gallery_api.Models
{
    public class ImageModel
    {
        public int ImageId { get; set; }

        public string ImageName { get; set; } = string.Empty;

        public string ImagePath { get; set; } = string.Empty;

        public bool Favourite { get; set; }
    }
}
