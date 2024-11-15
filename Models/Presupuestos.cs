public class Presupuestos{
    private int idPresupuesto;
    private string nombreDestinatario;
    private DateTime fechaCrecion;
    private List<PresupuestoDetalle> detalle;

    public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }
    public string NombreDestinatario { get => nombreDestinatario; set => nombreDestinatario = value; }
    public List<PresupuestoDetalle> Detalle { get => detalle; set => detalle = value; }
    public DateTime FechaCrecion { get => fechaCrecion; set => fechaCrecion = value; }

    public double MontoPresupuesto(){
        
        int monto = detalle.Sum(d => d.Cantidad*d.Producto.Precio);
        return monto;
    }

    public double MontoPresupuestoConIva(){
        return MontoPresupuesto() * 1.21;
    }
    public int CantidadProductos(){
        return Convert.ToInt32(detalle.Sum(d => d.Cantidad));
    }

}