public class User{
    publicint Id;
    publicstring Username{get;set;} = string.Empty;
    publicstring Password{get;set;} = string.Empty;
    publicAccessLevel AccessLevel {get;set;};
}

public enum AccessLevel{
    Admin,
    Editor,
    Invitado,
    Empleado
}