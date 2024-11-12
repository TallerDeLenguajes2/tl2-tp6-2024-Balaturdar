using System.ComponentModel;
using Microsoft.Data.Sqlite;

public class ProductosRepository{
    private string CadenaDeConexion;

    public ProductosRepository()
    {
        CadenaDeConexion = "Data Source=db/Tienda.db";
    }

    public void CrearProducto(Productos producto){
        using(SqliteConnection connection = new SqliteConnection(CadenaDeConexion)){
            string queryString = @"INSERT INTO Productos (Descripcion, Precio) 
                                VALUES (@descripcion, @precio);
                                ";
            connection.Open();
            var command = new SqliteCommand( queryString, connection);

            command.Parameters.AddWithValue("@precio", producto.Precio);
            command.Parameters.AddWithValue("@descripcion", producto.Descripcion);

            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public void ModProducto(Productos producto){
        string queryString = @"UPDATE Productos SET Descripcion = @Descripcion, Precio = @Precio WHERE idProducto = @Id";

        using (SqliteConnection connection = new SqliteConnection(CadenaDeConexion))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(queryString,connection);
            command.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
            command.Parameters.AddWithValue("@Precio", producto.Precio);
            command.Parameters.AddWithValue("@Id", producto.IdProductos);
            command.ExecuteNonQuery();
            connection.Close();            
        }
    }

    public List<Productos> ListarProductos(){

        var productos = new List<Productos>();
        using(SqliteConnection connection = new SqliteConnection(CadenaDeConexion)){
            string queryString = @"SELECT idProducto, Descripcion, Precio From Productos";
            var command = new SqliteCommand( queryString, connection);
            connection.Open();
            using(SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var producto = new Productos();
                    producto.IdProductos = Convert.ToInt32(reader["idProducto"]);
                    producto.Descripcion = reader["Descripcion"].ToString();
                    producto.Precio = Convert.ToInt32(reader["Precio"]);
                    productos.Add(producto);
                }
            }
            connection.Close();
        }
        return productos;
    }

    public Productos ObtenerProductoId(int id){
        var producto = new Productos();
        using(SqliteConnection connection = new SqliteConnection(CadenaDeConexion)){
            string queryString = @"SELECT idProducto, Descripcion, Precio From Productos WHERE idProducto = @id";
            var command = new SqliteCommand( queryString, connection);
            command.Parameters.AddWithValue("@id", id.ToString());
            connection.Open();
            using(SqliteDataReader reader = command.ExecuteReader())
            {
                reader.Read();
                producto.IdProductos = Convert.ToInt32(reader["idProducto"]);
                producto.Descripcion = reader["Descripcion"].ToString();
                producto.Precio = Convert.ToInt32(reader["Precio"]);
            }
            command.ExecuteReader();
            connection.Close();
        }
        return producto;
    }

    public void EliminarProductoId(int id){

        using(SqliteConnection connection = new SqliteConnection(CadenaDeConexion)){
            string queryString = @"DELETE FROM Productos
                                   WHERE idProducto = @id;";
            
            var command = new SqliteCommand( queryString, connection);
            connection.Open();
            
            command.Parameters.AddWithValue("@id", id);
            
            command.ExecuteNonQuery();

            connection.Close();
        }
    }

}