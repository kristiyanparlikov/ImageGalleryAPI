using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace photo_gallery_api.Entities
{
    public class Image
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ImageId { get; set; }

        [Required]
        [Column(TypeName="nvarchar(255)")]
        public string ImageName { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public string ImagePath { get; set; } = string.Empty;

        [JsonIgnore]
        public ICollection<Favourite> Users { get; set; }
    }
}
