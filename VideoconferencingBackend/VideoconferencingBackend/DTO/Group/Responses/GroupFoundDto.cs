namespace VideoconferencingBackend.DTO.Group.Responses
{
    public class GroupFoundDto
    {
        public string Name;
        public string Description;

        public GroupFoundDto(Models.DBModels.Group group)
        {
            Name = group.Name;
            Description = group.Description;
        }
    }
}
