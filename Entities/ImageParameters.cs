using System.ComponentModel.DataAnnotations.Schema;

namespace photo_gallery_api.Entities
{
    [NotMapped]
    public class ImageParameters
    {

        private const int maxPageSize = 50;

        private int _pageSize = 30;

        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }
}
