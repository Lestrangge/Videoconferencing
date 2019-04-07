namespace VideoconferencingBackend.Interfaces.Services.Authentication
{
    public interface IHasherService
    {
        string Hash(string toHash);
    }
}
