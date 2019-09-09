using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Continental.CUP.Repositories.Data.Entities
{
    [Table("ReaderKind")]
    public class ReaderKindEModel
    {
        [Key]
        public int ReaderKindID { get; set; }

        [Required]
        [StringLength(50)]
        public string Brand { get; set; }

        [Required]
        [StringLength(50)]
        public string ModelName { get; set; }

       
        public int AntennaNumber { get; set; }
    }
}
