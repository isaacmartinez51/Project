using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Continental.CUP.Repositories.Data.Entities
{
    [Table("Reader")]
    public class ReaderEModel
    {
        [Key]
        public int ReaderID { get; set; }

        [Display(Name = "Tipo de Reader")]
        public int ReaderKindID { get; set; }

        [Display(Name = "Nombre de portal")]
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Display(Name = "Direccón IP")]
        [Required]
        [StringLength(20)]
        public string IpAddress { get; set; }

        [Display(Name = "Número de Serie")]
        [Required]
        [StringLength(20)]
        public string Serial { get; set; }

    }
}
