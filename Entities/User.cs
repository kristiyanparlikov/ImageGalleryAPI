using System.ComponentModel.DataAnnotations;

namespace photo_gallery_api.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        public string Email { get; set; } = string.Empty;

        public ICollection<Favourite> Favourites { get; set; }
    }
}
