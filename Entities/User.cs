using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace photo_gallery_api.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        public string Email { get; set; } = string.Empty;

        [JsonIgnore]
        public ICollection<Favourite> Images { get; set; }
    }
}
