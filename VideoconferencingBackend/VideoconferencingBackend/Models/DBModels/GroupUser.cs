﻿namespace VideoconferencingBackend.Models.DBModels
{
    public class GroupUser
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
