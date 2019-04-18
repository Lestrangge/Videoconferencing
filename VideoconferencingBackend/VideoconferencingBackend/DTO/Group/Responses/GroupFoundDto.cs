namespace VideoconferencingBackend.DTO.Group.Responses
{
    public class GroupFoundDto
    {
        public string Name;
        public string Description;
        public string AvatarLink;
        public string GroupGuid;
        public bool InCall; 
        public GroupFoundDto() { }
        public GroupFoundDto(Models.DBModels.Group group)
        {
            Name = group.Name;
            Description = group.Description;
            AvatarLink = group.AvatarLink;
            GroupGuid = group.GroupGuid;
            InCall = group.InCall ?? false;
        }
    }
}
