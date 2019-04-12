namespace VideoconferencingBackend.DTO.Hub.ServerRequest
{
    public class SendChatMessageRequestDto
    {
        public string Text { get; set; }
        public string GroupGuid { get; set; }
    }
}
