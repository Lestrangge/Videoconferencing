namespace VideoconferencingBackend.Models.DBModels
{
    public class Handle
    {
        public long HandleId { get; set; }
        public string Type { get; set; }
        public User User { get; set; }
    }
}
