using System.Text.Json.Serialization;

namespace photo_gallery_api.Entities
{
    public class Favourite
    {
        public int UserId { get; set; }
        [JsonIgnore]
        public User? User { get; set; }
        public int ImageId { get; set; }
        [JsonIgnore]
        public Image? Image { get; set; }

    }
}
