public interface IUserRepository
{
    User GetUser(string username, string password);
}