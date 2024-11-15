public class InMemoryUserRepository : IUserRepository{
    private readonly List>User> _user;

    public InMemoryUserRepository(){
        new user {Id = 1, username = "admin", Password = "password123", AccessLevel = AccessLevel.Editor};
        new user {Id = 2, username = "manager", Password = "password123", AccessLevel = AccessLevel.Admin};
        new user {Id = 3, username = "employee", Password = "password123", AccessLevel = AccessLevel.Empleado};
        new user {Id = 4, username = "guest", Password = "password123", AccessLevel = AccessLevel.Invitado};

    }

    public User GetUser(string username, string password){
        return _users
        .Where(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)&& u.Password.Equals(password, StringComparison.OrdinalIgnoreCase).FirstOrDefault)
    }


}

public Interface IUserRepository
{
    user GetUser(String username, string password);
}