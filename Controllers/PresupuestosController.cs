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
        var presupuestos = repoPresupuestos.ListarPresupuestos;
        return View(presupuestos);
    }
    //En el controlador de Presupuestos: Listar, Crear, Modificar y Eliminar Presupuestos.
    public IActionResult AltaPresupuesto(){
        return View();
    }
    public IActionResult CrearPresupuesto(Presupuestos presupuesto){
        repoPresupuestos.CrearNuevoPresupuesto(presupuesto);
        return RedirectToAction("Index");
    }

    [HttpGet]

    public IActionResult EliminarProductoAPresupuesto(int id)
    {
        var presupuesto = repoPresupuestos.ObtenerPresupuestoId(id);
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
        //repoPresupuestos.EliminarProducto(idPresupuesto, idProducto);
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
    
}