using Microsoft.AspNetCore.Mvc;

public class ProductosController : Controller
{
    private readonly ILogger<ProductosController> _logger;
    private ProductosRepository repoProductos;

    public ProductosController(ILogger<ProductosController> logger)
    {
        _logger = logger;
        repoProductos = new ProductosRepository();
    }
    public IActionResult Index()
    {
        var productos = repoProductos.ListarProductos();
        return View(productos);
    }
    public IActionResult AltaProducto(){
        return View();
    }
    [HttpPost]
    public IActionResult CrearProducto(Productos producto)
    {
        repoProductos.CrearProducto(producto);
        return RedirectToAction ("Index");
    }
    [HttpGet]
    public IActionResult ModificarProducto(int id)
    {
        var producto  = repoProductos.ObtenerProductoId(id);
        return View(producto);
    }
    [HttpPost]
    public IActionResult ModificarProducto(Productos producto)
    {
        repoProductos.ModProducto(producto);
        return RedirectToAction ("Index"); 
    }
   [HttpGet]
    public IActionResult EliminarProducto(int id)
    {
        return View(repoProductos.ObtenerProductoId(id));
    }
    [HttpGet]
    public IActionResult EliminarProductoPorId(int id)
    {
        repoProductos.EliminarProductoId(id);
        return RedirectToAction ("Index");
    }
}
