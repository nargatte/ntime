using System.ComponentModel.DataAnnotations;

namespace BaseCore.Models
{
    public class PageBindingModel
    {
        [Range(0, 1000, ErrorMessage = "Page must have maximum 1000 elements")]
        public int ItemsOnPage { get; set; }
        public int PageNumber { get; set; }
    }
}