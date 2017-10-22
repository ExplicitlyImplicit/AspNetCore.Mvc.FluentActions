using System.ComponentModel.DataAnnotations;

namespace SimpleMvc.Models
{
    public class ModelStateFormModel
    {
        [Required]
        public string TextValue { get; set; }
    }
}
