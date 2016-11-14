using System.ComponentModel.DataAnnotations;

namespace SimpleMvcWithViews.Models
{
    public class ModelStateFormModel
    {
        [Required]
        public string TextValue { get; set; }
    }
}
