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
    public IActionResult CrearPresupuesto(Presupuestos presupuesto){
        repoPresupuestos.CrearNuevoPresupuesto(presupuesto);
        return RedirectToAction("Index");
    }

    public IActionResult AgregarProducto(int id){
        //repoproductos
        return View(id);
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