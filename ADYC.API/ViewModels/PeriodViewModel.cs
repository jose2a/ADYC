using System.ComponentModel.DataAnnotations;

namespace ADYC.API.ViewModels
{
    public class PeriodDto
    {
        public string Url { get; set; }
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}