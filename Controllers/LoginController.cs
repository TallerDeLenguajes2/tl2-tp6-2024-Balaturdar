using Microsoft.AspNetCore.Mvc;


public class LoginController : Controller
{
    readonly IUserRepository _inMemoryUserRepository;

    public LoginController(IUserRepository inmem ){
        _inMemoryUserRepository = inmem;
    }

    public IActionResult Index(){
        var model = new LoginViewModel{
            IsAuthenticated = HttpContext.Session.GetString("IsAuthenticated") == "true"
        };
        return View(model);
    }

    public IActionResult Login(LoginViewModel model){
         if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
        {
            model.ErrorMessage = "Por favor ingrese su nombre de usuario y contraseña.";
            return View("Index", model);
        }

        User usuario = InMemoryUserRepository.Get(model.Username, model.Password);
        if(usuario != null){
            //return RedirectToAction("Index","Home");
            HttpContext.Session.SetString("IsAuthenticated","true");
            HttpContext.Session.SetString("User",Username);
            HttpContext.Session.SetString("AccesLevel",usuario.AccesLevel.ToString());
            return RedirectToAction("Index","Home");
        }

        model.ErrorMessage = "Credenciales Invalidas.";
        model.IsAuthenticated = false;
        return View("Index",model);




    }

    public IActionResult Logout()
    {
        // Limpiar la sesión
        HttpContext.Session.Clear();

        // Redirigir a la vista de login
        return RedirectToAction("Index");
    }



}
