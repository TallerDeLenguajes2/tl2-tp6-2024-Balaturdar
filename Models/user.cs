public class User{
    public int Id;
    public string Username{get;set;} = string.Empty;
    public string Password{get;set;} = string.Empty;
    public AccessLevel AccessLevel {get;set;}
}

public enum AccessLevel{
    Admin,
    Editor,
    Invitado,
    Empleado
}