using Microsoft.Data.Sqlite;

public class ProductosRepository{
    private string CadenaDeConexion;

    public ProductosRepository()
    {
        CadenaDeConexion = "Data Source=db/Tienda.db";
    }

    public Cliente ObtenerClienteporId(int id){
        var cliente = new Cliente();
        using(SqliteConnection connection = new SqliteConnection(CadenaDeConexion)){
            var query = @"SELECT ClienteId, Nombre, Email, Telefono 
                            FROM Clientes WHERE ClienteId = @id";
            connection.Open();
            var command = new SqliteCommand( query, connection);
            command.Parameters.AddWithValue("@id", id);
            using(SqliteDataReader reader = command.ExecuteReader()){
                reader.read();
                cliente.ClienteId = Convert.ToInt32(reader["ClienteId"]);
                cliente.Nombre = reader["Nombre"].ToString();
                cliente.Email = reader["Email"];
                cliente.Telefono = reader["Telefono"];
            }
            connection.Close();
        }
        return cliente;
    }

    public void CrearCliente(Cliente cliente){
        using(SqliteConnection connection = new SqliteConnection(CadenaDeConexion)){
            var query = @"INSERT INTO Clientes (ClienteId, Nombre, Email, Telefono) 
                        VALUES(@id, @nombre, @email, @telefono)";
            connection.Open();
            command.Parameters.AddWithValue("@id",cliente.ClienteId);
            command.Parameters.AddWithValue("@nombre",cliente.Nombre);
            command.Parameters.AddWithValue("@email",cliente.Email);
            command.Parameters.AddWithValue("@telefono",cliente.Telefono);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public void EliminarClientePorId(int id){
         using(SqliteConnection connection = new SqliteConnection(CadenaDeConexion)){
            var query = @"DELETE FROM Clientes
                                   WHERE CLienteId = @id;";
            
            connection.Open();

            var command = new SqliteCommand(query,connection);
            
            command.Parameters.AddWithValue("@id", id);
            
            command.ExecuteNonQuery();

            connection.Close();
        }

    }

    public List<Clientes> ListarClientes(){
        var clientes = new List<Clientes>();
        using(SqliteConnection connection = new SqliteConnection()){
            var query = @"SELECT ClienteId, Nombre, Email, Telefono FROM Clientes";
            connection.Open();
            var command = new SqliteCommand( query, connection);

            using(SqliteDataReader reader = command.ExecuteReader()){
                while(reader.read();){
                    var cliente = new Cliente();
                    cliente.ClienteId = Convert.ToInt32(reader["ClienteId"]);
                    cliente.Nombre = reader["Nombre"].ToString();
                    cliente.Email = reader["Email"];
                    cliente.Telefono = reader["Telefono"];
                    clientes.Add(cliente);
                }
            }
            connection.Close();
        }
    }

}
