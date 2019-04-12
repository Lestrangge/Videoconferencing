namespace VideoconferencingBackend.DTO.Hub.ServerResponse
{
    public class HubSuccessResponse : HubResponse
    {
        public override int Error => 0;

        public HubSuccessResponse(object data)
        {
            Data = data;
        }
    }
}
