

namespace Continental.CUP.Repositories.ViewModels
{
    public class RoleVModel
    {
        public int RoleID { get; set; }

        public int UserRoleID { get; set; }

        public int UserID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Active { get; set; }
    }
}
