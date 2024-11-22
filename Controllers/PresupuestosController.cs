using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


public class PresupuestosController : Controller
{
    private readonly ILogger<PresupuestosController> _logger;
    private PresupuestosRepository repoPresupuestos;

    public PresupuestosController(ILogger<PresupuestosController> logger)
    {
        _logger = logger;
        repoPresupuestos = new PresupuestosRepository();
    }
    public IActionResult Index()
    {
        return View(repoPresupuestos.ListarPresupuestos());
    }
    //En el controlador de Presupuestos: Listar, Crear, Modificar y Eliminar Presupuestos.

    public IActionResult DetallesDelPresupuesto(int id)
    {
        return View(repoPresupuestos.ObtenerPresupuestoId(id));
    }

    public IActionResult AltaPresupuesto()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CrearPresupuesto(Presupuestos presupuesto)
    {
        repoPresupuestos.CrearNuevoPresupuesto(presupuesto);
        return RedirectToAction("Index");
    }


    [HttpGet]

    public IActionResult AgregarProductoAPresupuesto(int id)
    {
        ProductosRepository repoProductos = new ProductosRepository();
        List<Productos> productos = repoProductos.ListarProductos();
        ViewData["Productos"] = productos.Select(p => new SelectListItem
        {
            Value = p.IdProductos.ToString(),
            Text = p.Descripcion
        }).ToList();

        return View(id);
    }

    [HttpPost]
    public IActionResult AgregarProductoEnPresupuesto(int idPresupuesto, int idProducto, int cantidad)
    {
        repoPresupuestos.AgregarProducto(idPresupuesto, idProducto, cantidad);
        return RedirectToAction("Index");
    }

    [HttpGet]

    public IActionResult EliminarProductoAPresupuesto(int id)
    {
        Presupuestos presupuesto = repoPresupuestos.ObtenerPresupuestoId(id);
        ViewData["Productos"] = presupuesto.Detalle.Select(p => new SelectListItem
        {
            Value = p.Producto.IdProductos.ToString(),
            Text = p.Producto.Descripcion
        }).ToList();

        return View(id);
    }

    [HttpPost]
    public IActionResult EliminarProductoEnPresupuesto(int idPresupuesto, int idProducto)
    {
        repoPresupuestos.EliminarProducto(idPresupuesto, idProducto);
        return RedirectToAction("Index");
    }


    [HttpGet]
    public IActionResult ModificarPresupuestos(int id)
    {
        var producto = repoPresupuestos.ObtenerPresupuestoId(id);
        return View(producto);
    }
    public IActionResult ModificarPresupuestos(Presupuestos presupuesto)
    {
        repoPresupuestos.ModificarPresupuesto(presupuesto);
        return RedirectToAction("Index");
    }


    public IActionResult EliminarPresupuesto(int id)
    {
        return View(repoPresupuestos.ObtenerPresupuestoId(id));
    }
    public IActionResult EliminarPresupuestoid(int id)
    {
        repoPresupuestos.EliminarPresupuestoPorId(id);
        return RedirectToAction("Index");
    }

}