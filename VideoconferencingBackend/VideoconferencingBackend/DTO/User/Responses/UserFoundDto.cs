namespace VideoconferencingBackend.DTO.User.Responses
{
    public class UserFoundDto
    {
        public string Login { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string AvatarLink { get; set; }

        public UserFoundDto(Models.DBModels.User user)
        {
            Login = user.Login;
            Name = user.Name;
            Surname = user.Surname;
            AvatarLink = user.AvatarLink;
        }
    }
}
