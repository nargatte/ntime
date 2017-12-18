using System.ComponentModel.DataAnnotations;

namespace BaseCore.Models
{
    public class PageBindingModel
    {
        [Range(0, 100, ErrorMessage = "Page must has maximum 100 elements")]
        public int ItemsOnPage { get; set; }
        public int PageNumber { get; set; }
    }
}