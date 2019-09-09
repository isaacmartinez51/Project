using System.ComponentModel.DataAnnotations;

namespace Continental.CUP.Repositories.ViewModels
{
    public class ActionDetailVModel
    {
        public int ActionDetailID { get; set; }

        [Display(Name = "")]
        public int ActionID { get; set; }

        [Display(Name = "")]
        [Required]
        [StringLength(150)]
        public string ContainerName { get; set; }

        [Display(Name = "")]
        [Required]
        [StringLength(150)]
        public string ControlID { get; set; }



        //[Display(Name = "")]
        //[JsonIgnore]
        //public ActionModel ActionModel { get; set; }
    }
}
