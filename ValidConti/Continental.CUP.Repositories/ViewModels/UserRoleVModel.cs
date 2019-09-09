using System.ComponentModel.DataAnnotations;

namespace Continental.CUP.Repositories.ViewModels
{

    public class UserRoleVModel
    {
        //TODO: Validar si se puede cambiar la propiedas para que no permita null "?"
        public int? UserRoleID { get; set; }
        public int RoleID { get; set; }

        public int? UserID { get; set; }

        public bool Active { get; set; }

        [Display(Name = "Nombre del Perfil")]
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        //[Display(Name = "Descripción")]
        //[StringLength(150)]
        //public string Description { get; set; }
    }
}
