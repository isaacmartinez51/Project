using System.ComponentModel.DataAnnotations;

namespace Continental.CUP.Repositories.ViewModels
{
    public class ReaderKindVModel
    {
        public int ReaderKindID { get; set; }

        [Required(ErrorMessage = "Por favor ingresa la marca del Reader.")]
        [Display(Name = "Marca")]
        [StringLength(50)]
        public string Brand { get; set; }

        [Display(Name = "Modelo")]
        [Required(ErrorMessage = "Por favor ingresa el modelo del Reader.")]
        [StringLength(50)]
        public string ModelName { get; set; }

        [Display(Name = "Número de antenas")]
        [Required(ErrorMessage = "Por favor ingresa el total de antenas del Reader.")]
        public int? AntennaNumber { get; set; }
    }
}
