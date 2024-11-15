using Microsoft.AspNetCore.Mvc;


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

    public IActionResult AltaPresupuesto(){
        return View();
    }
    [HttpPost]
    public IActionResult CrearPresupuesto(Presupuestos presupuesto){
        repoPresupuestos.CrearNuevoPresupuesto(presupuesto);
        return RedirectToAction("Index");
    }
    [HttpGet]

    public IActionResult AgregarProductoAPresupuesto(int id)
    {
        ProductosRepository repoProductos = new ProductosRepository();
        List<Producto> productos = repoProductos.ObtenerProductos();
        ViewData["Productos"] = productos.Select(p => new SelectListItem
        {
            Value = p.IdProducto.ToString(), 
            Text = p.Descripcion 
        }).ToList();

        return View(id);
    }

    [HttpPost]

    public IActionResult AgregarProductoEnPresupuesto(int idPresupuesto, int idProducto, int cantidad)
    {
        repoPresupuestos.AgregarProducto(idPresupuesto, idProducto, cantidad);
        return RedirectToAction ("Index");
    }

     
    [HttpGet]
    public IActionResult ModificarPresupuesto(int id)
    {
        var producto  = repoPresupuestos.ObtenerPresupuestoId(id);
        return View(producto);
    }
    public IActionResult ModificarPresupuesto(Presupuestos presupuesto){
        repoPresupuestos.ModificarPresupuesto(presupuesto);
        return RedirectToAction("Index");
    }

    
    public IActionResult EliminarPresupuesto(int id){
        return View(repoPresupuestos.ObtenerPresupuestoId(id));
    }
    public IActionResult EliminarPresupuestoid(int id){
        repoPresupuestos.EliminarPresupuestoPorId(id);
        return RedirectToAction ("Index");
    }


    [HttpGet]

    public IActionResult EliminarProductoAPresupuesto(int id)
    {
        Presupuesto presupuesto = repoPresupuestos.ObtenerPresupuestoPorId(id);
        ViewData["Productos"] = presupuesto.Detalle.Select(p => new SelectListItem
        {
            Value = p.Producto.IdProducto.ToString(), 
            Text = p.Producto.Descripcion 
        }).ToList();

        return View(id);
    }

    [HttpPost]

    public IActionResult EliminarProductoEnPresupuesto(int idPresupuesto, int idProducto)
    {
        repoPresupuestos.EliminarProducto(idPresupuesto, idProducto);
        return RedirectToAction ("Index");
    }



    
}