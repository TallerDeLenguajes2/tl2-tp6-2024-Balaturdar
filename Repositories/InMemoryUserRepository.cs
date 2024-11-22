public class InMemoryUserRepository : IUserRepository{
    
    private readonly List<User> _user;

    public InMemoryUserRepository(){
        _user = new List<User>{
        new User {Id = 1, Username = "admin", Password = "password123", AccessLevel = AccessLevel.Editor},
        new User {Id = 2, Username = "manager", Password = "password123", AccessLevel = AccessLevel.Admin},
        new User {Id = 3, Username = "employee", Password = "password123", AccessLevel = AccessLevel.Empleado},
        new User {Id = 4, Username = "guest", Password = "password123", AccessLevel = AccessLevel.Invitado}
        };
    }

    public User GetUser(string username, string password){
        return _user.Where(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)
                             && u.Password.Equals(password, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
    }


}