namespace VideoconferencingBackend.DTO.User.Requests
{
    public class UserUpdateDto
    {
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string AvatarLink { get; set; }
    }
}
