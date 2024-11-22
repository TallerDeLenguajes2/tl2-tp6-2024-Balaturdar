using Microsoft.AspNetCore.Mvc;

public class ProductosController : Controller
{
    private readonly ILogger<ProductosController> _logger;
    private ClientesRepository repoClientes;

    public ProductosController(ILogger<ProductosController> logger)
    {
        _logger = logger;
        repoClientes = new ClientesRepository();
    }
    public IActionResult Index()
    {
        var clientes = repoClientes.ListarClientes();
        return View(productos);
    }
}