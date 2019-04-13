namespace VideoconferencingBackend.DTO.Hub.ServerRequest
{
    public class InitiateCallRequestDto
    {
        public string Sdp { get; set; }
        public string GroupGuid { get; set; }
    }
}
