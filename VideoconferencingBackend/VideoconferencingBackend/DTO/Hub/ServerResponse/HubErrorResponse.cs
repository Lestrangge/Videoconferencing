namespace VideoconferencingBackend.DTO.Hub.ServerResponse
{
    public class HubErrorResponse : HubResponse
    {
        public HubErrorResponse(int error, string reason)
        {
            Error = error;
            Data = reason;
        }
    }
}
