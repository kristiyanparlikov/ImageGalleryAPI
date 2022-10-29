using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace photo_gallery_api.Entities
{
    [NotMapped]
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}