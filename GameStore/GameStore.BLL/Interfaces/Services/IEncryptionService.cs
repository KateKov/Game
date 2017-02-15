namespace GameStore.BLL.Interfaces.Services
{
    public interface IEncryptionService
    {
       string CreateSalt();
       string EncryptPassword(string password, string salt);
    }
}
