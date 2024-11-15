using Microsoft.Data.Sqlite;
public class PresupuestosRepository{
    private string CadenaDeConexion;
    public PresupuestosRepository(){
        CadenaDeConexion = "Data Source=db/Tienda.db;";
    }

    public void CrearNuevoPresupuesto(Presupuestos presupuesto){

        using(SqliteConnection connection = new SqliteConnection(CadenaDeConexion)){

            connection.Open();
            string queryString = @"INSERT INTO Presupuestos (NombreDestinatario, FechaCreacion) 
                                VALUES (@destinatario, @fecha);
                                ";
            
            var command = new SqliteCommand( queryString, connection);
            command.Parameters.AddWithValue("@destinatario", presupuesto.NombreDestinatario);
            command.Parameters.AddWithValue("@fecha", presupuesto.FechaCrecion);
            command.ExecuteNonQuery();

            int idpresupuesto;
            queryString = @"SELECT idPresupuesto FROM Presupuestos 
                            WHERE FechaCreacion = '@fecha';";
            command = new SqliteCommand( queryString, connection);
            
            command.Parameters.AddWithValue("@fecha", presupuesto.FechaCrecion);
            using(SqliteDataReader reader = command.ExecuteReader())
            {
                reader.Read();
                idpresupuesto = Convert.ToInt32(reader["idPresupuesto"]);
            }
            
            queryString = @"INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto ,Cantidad)
                            VALUES (@idpresupuesto, @idproducto, @cantidad);";
            command = new SqliteCommand( queryString, connection);
            
            foreach (var detalle in presupuesto.Detalle)
            {
                command.Parameters.AddWithValue("@idpresupuesto",idpresupuesto);
                command.Parameters.AddWithValue("@idproducto", detalle.Producto.IdProductos);
                command.Parameters.AddWithValue("@cantidad", detalle.Cantidad);
                command.ExecuteNonQuery();
            }
            connection.Close();
        }

    }


    public List<Presupuestos> ListarPresupuestos(){
        var presupuestos = new List<Presupuestos>();

        using(SqliteConnection conection = new SqliteConnection(CadenaDeConexion)){
            var queryString = @"SELECT 
                                    P.idPresupuesto,
                                    P.NombreDestinatario,
                                    P.FechaCreacion
                                FROM 
                                    Presupuestos P;";
            conection.Open();
            var command = new SqliteCommand(queryString, conection);

            using(SqliteDataReader reader = command.ExecuteReader()){
                while(reader.Read()){
                    var presupuesto = new Presupuestos();
                    presupuesto.IdPresupuesto = Convert.ToInt32(reader["idPresupuesto"]);
                    presupuesto.NombreDestinatario = reader["NombreDestinatario"].ToString();
                    presupuesto.FechaCrecion = Convert.ToDateTime(reader["FechaCreacion"]);
                    presupuestos.Add(presupuesto);
                }
            }
        }

        return presupuestos;
    }

    public List<Productos> DetallesPresupuestoId(int id){//Borrar
        var listaProductos = new List<Productos>();
        using(SqliteConnection conection = new SqliteConnection(CadenaDeConexion)){
            var queryString = @"SELECT 
                                    P.idPresupuesto,
                                    P.NombreDestinatario,
                                    P.FechaCreacion,
                                    PR.idProducto,
                                    PR.Descripcion AS Producto,
                                    PR.Precio,
                                    PD.Cantidad,
                                    (PR.Precio * PD.Cantidad) AS Subtotal
                                FROM 
                                    Presupuestos P
                                JOIN 
                                    PresupuestosDetalle PD ON P.idPresupuesto = PD.idPresupuesto
                                JOIN 
                                    Productos PR ON PD.idProducto = PR.idProducto
                                WHERE 
                                    P.idPresupuesto = @id;";
            conection.Open();
            var command = new SqliteCommand(queryString, conection);
            
            command.Parameters.AddWithValue("@id",id);
            using(SqliteDataReader reader = command.ExecuteReader()){
                while(reader.Read()){
                    var producto = new Productos();
                    producto.IdProductos = Convert.ToInt32(reader["idProducto"]);
                    producto.Descripcion = reader["Producto"].ToString();
                    producto.Precio = Convert.ToInt32(reader["Precio"]);
                    listaProductos.Add(producto);
                }
            }

        }
        return listaProductos;

    }

    public void EliminarPresupuestoPorId(int idPresupuesto)
    {
        using (SqliteConnection connection = new SqliteConnection(CadenaDeConexion))
        {
            connection.Open();

            var query = @"DELETE FROM Presupuestos WHERE idPresupuesto = @IdP;";
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@IdP", idPresupuesto);
            command.ExecuteNonQuery();

            query =@"DELETE FROM PresupuestosDetalle WHERE idPresupuesto = @Id;";
            command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@Id", idPresupuesto);
            command.ExecuteNonQuery();

            connection.Close();
        }
    }

    public Presupuestos ObtenerPresupuestoId(int id){
        var presupuesto = new Presupuestos();

        using(SqliteConnection connection = new SqliteConnection(CadenaDeConexion)){

            connection.Open();
            var queryString = @"SELECT idPresupuesto, NombreDestinatario, FechaCreacion 
                                FROM Presupuestos 
                                WHERE idPresupuesto = @id";
            
            var command = new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@id",id);
            using(SqliteDataReader reader = command.ExecuteReader()){
                while(reader.Read()){
                    presupuesto.IdPresupuesto = Convert.ToInt32(reader["idPresupuesto"]);
                    presupuesto.NombreDestinatario = reader["NombreDestinatario"].ToString();
                    presupuesto.FechaCrecion = Convert.ToDateTime(reader["FechaCreacion"]);
                }
            }
            var detalles = new List<PresupuestoDetalle>();
            queryString = @"SELECT idPresupuesto, idProducto, Cantidad 
                            FROM PresupuestosDetalle 
                            WHERE idPresupuesto = @id";
            
            command = new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@id",id);
            using(SqliteDataReader reader = command.ExecuteReader()){
                while(reader.Read()){
                    var detalle = new PresupuestoDetalle();
                    detalle.Cantidad = Convert.ToInt32(reader["Cantidad"]);
                    detalle.Producto = new ProductosRepository().ObtenerProductoId(Convert.ToInt32(reader["idProducto"]));
                    detalles.Add(detalle);
                }
            }
            presupuesto.Detalle = detalles;
        }
        return presupuesto;
    }

    public void ModificarPresupuesto(Presupuestos presupuesto)
    {
        string query = @"UPDATE Presupuestos SET NombreDestinatario = @destinatario, FechaCreacion = @fecha WHERE idPresupuesto = @Id";
        using (SqliteConnection connection = new SqliteConnection(CadenaDeConexion))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query,connection);
            command.Parameters.AddWithValue("@destinatario", presupuesto.NombreDestinatario);
            command.Parameters.AddWithValue("@fecha", presupuesto.FechaCrecion);
            command.Parameters.AddWithValue("@Id", presupuesto.IdPresupuesto);
            command.ExecuteNonQuery();
            connection.Close();            
        }
    }

    public void AgregarProducto (int idPresupuesto, int idProducto, int cantidad){
        var query = @"INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) 
                    VALUES (@idPresupuesto, @idProducto, @cantidad);";

        using(SqliteConnection connection = new SqliteConnection(CadenaDeConexion)){
            connection.Open();
            SqliteCommand command = new SqliteCommand(query,connection);
            command.Parameters.AddWithValue("@idPresupuesto",idPresupuesto);
            command.Parameters.AddWithValue("@idProducto",idProducto);
            command.Parameters.AddWithValue("@Cantidad",cantidad);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public void EliminarProducto(int idPresupuesto, int idProducto){
        var query= @"DELETE FROM PresupuestosDetalle 
                    Where idPresupuesto = @presupuesto AND idProducto = @producto";
        using(SqliteConnection connection = new SqliteConnection(CadenaDeConexion)){
            connection.Open();
            SqliteCommand = nes SqliteCommand(query,connection);
            command.Parameters.AddWithValue("@presupuesto");
            command.Parameters.AddWithValue("@producto",idProducto);
        }

    }

    
}