﻿namespace VideoconferencingBackend.DTO.Group.Responses
{
    public class GroupFoundDto
    {
        public string Name;
        public string Description;
        public string AvatarLink;
        public string GroupGuid;

        public GroupFoundDto(Models.DBModels.Group group)
        {
            Name = group.Name;
            Description = group.Description;
            AvatarLink = group.AvatarLink;
            GroupGuid = group.GroupGuid;
        }
    }
}
