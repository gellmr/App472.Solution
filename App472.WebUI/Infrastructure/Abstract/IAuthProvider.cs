namespace App472.WebUI.Infrastructure.Abstract
{
    public interface IAuthProvider{
        bool Authenticate(string username, string password);
        void SignOut();
    }
}