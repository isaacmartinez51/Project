using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Continental.CUP.Repositories.ViewModels
{
    public class UserVModel
    {
        public int UserID { get; set; }

        [Display(Name = "Nombre de la Persona")]
        [Required]
        [StringLength(150)]
        public string FirtsName { get; set; }

        [Display(Name = "Apellido de la persona")]
        [Required]
        [StringLength(80)]
        public string LastName { get; set; }

        [Display(Name = "Nombre de Usuario (UserName)")]
        [Required]
        [StringLength(80)]
        public string UserName { get; set; }

        [Display(Name = "Contraseña")]
        [StringLength(50)]
        public string Password { get; set; }

        public bool IsSuperAdmin { get; set; }

        public short Status { get; set; }

        public List<UserRoleVModel> UserRoleModelList { get; set; }

        public string[] IDParamList { get; set; }

        public string[] ValueParamList { get; set; }

        public List<ActionDetailVModel> ActionDetailList { get; set; }

        public string DigitalSignature { get; set; }

    }
}
