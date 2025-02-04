namespace Pizza_Star.VIewModel
{
    public class EditRoleViewModel
    {
        public string UserId { get; set; }
        public IEnumerable<string> UserRoles { get; set; }
        public IEnumerable<string> AllRoles { get; set; }
        public IEnumerable<string> SelectedRoles { get; set; }
    }
}
