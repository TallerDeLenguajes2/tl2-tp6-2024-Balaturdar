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

        return new List<Presupuestos>();
    }

    public List<Productos> DetallesPresupuestoId(int id){
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
        return new List<Productos>();

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

    
}