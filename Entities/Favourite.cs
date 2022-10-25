namespace photo_gallery_api.Entities
{
    public class Favourite
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int ImageId { get; set; }
        public Image Image { get; set; }

    }
}
