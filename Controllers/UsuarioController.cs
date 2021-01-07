using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API_WS.Controllers
{
    [ApiController]
    [Route("api/[controller]")] //Ruta donde se encuentra el controlador
    public class UsuarioController : ControllerBase
    {

        /*
         * Para acceder al appsettings.json se debe utilizar IConfiguration
         * 
         */

        private readonly IConfiguration configuration;

        public UsuarioController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpPost]
        public bool PostCrearUsuario(Usuario usuario)
        {
           
            Console.WriteLine(usuario.ToString());

            //Se obtiene el configurationString

            string connectionString = configuration.GetConnectionString("mysql");
            Console.WriteLine(connectionString);

            string SQL = "insert into usuarios(nombre, apellido, usuario, contrasenia) " +
                "values(@nombre, @apellido, @usuario, @contrasenia)";

            MySqlConnection mySqlConnection = new MySqlConnection(connectionString);

            try
            {
                mySqlConnection.Open();
                //Se genera el store procedure
                MySqlCommand mySqlCommand = new MySqlCommand(SQL, mySqlConnection)
                {
                    Connection = mySqlConnection
                };

                mySqlCommand.Parameters.AddWithValue("@nombre", usuario.Nombre);
                mySqlCommand.Parameters.AddWithValue("@apellido", usuario.Apellido);
                mySqlCommand.Parameters.AddWithValue("@usuario", usuario.User);
                mySqlCommand.Parameters.AddWithValue("@contrasenia", usuario.Password);

                mySqlCommand.Prepare();

                int resultado = mySqlCommand.ExecuteNonQuery();

                if (resultado >= 1)
                {
                    return true;
                } else
                {
                    return false;
                }


            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            } finally
            {
                mySqlConnection.Close();
            }

        }

        //GET: api/User/{correo}
        [HttpGet("{correo}")]
        public List<Usuario> GetUsuarios(string correo)
        {
            //Se obtiene el configuration string
            string connectionString = configuration.GetConnectionString("mysql");

            string SQL = "select id, nombre, apellido, usuario, contrasenia from" +
                           " usuarios where usuario like @correo ";

            MySqlConnection mySqlConnection = new MySqlConnection(connectionString);
            try
            {
                //Abrir conexion
                mySqlConnection.Open();
                MySqlCommand mySqlCommand = new MySqlCommand(SQL, mySqlConnection)
                {
                    Connection = mySqlConnection
                };

                mySqlCommand.Parameters.AddWithValue("@correo", "%"+correo+"%");

                mySqlCommand.Prepare();
                //Se ejecuta el lector de datos
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

                List<Usuario> lista = new List<Usuario>();
                while (mySqlDataReader.Read())
                {
                    Usuario u = new Usuario()
                    {
                        Id = mySqlDataReader.GetInt32(0),
                        Nombre = mySqlDataReader.GetString(1),
                        Apellido = mySqlDataReader.GetString(2),
                        User = mySqlDataReader.GetString(3),
                        Password = mySqlDataReader.GetString(4),
                    };

                    lista.Add(u);
                }
                return lista;

            } catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                mySqlConnection.Close();
            }

           
        }

        [HttpPut]
        public bool UpdateUsuario(Usuario usuario)
        {

            Console.WriteLine(usuario.ToString());

            //Se obtiene el configurationString

            string connectionString = configuration.GetConnectionString("mysql");
            Console.WriteLine(connectionString);

            string SQL = "update usuarios set nombre = @nombre, " +
                "apellido = @apellido, usuario = @usuario, contrasenia = @contrasenia " +
                "where id = @id ";
                

            MySqlConnection mySqlConnection = new MySqlConnection(connectionString);

            try
            {
                mySqlConnection.Open();
                //Se genera el store procedure
                MySqlCommand mySqlCommand = new MySqlCommand(SQL, mySqlConnection)
                {
                    Connection = mySqlConnection
                };

                mySqlCommand.Parameters.AddWithValue("@nombre", usuario.Nombre);
                mySqlCommand.Parameters.AddWithValue("@apellido", usuario.Apellido);
                mySqlCommand.Parameters.AddWithValue("@usuario", usuario.User);
                mySqlCommand.Parameters.AddWithValue("@contrasenia", usuario.Password);
                mySqlCommand.Parameters.AddWithValue("@id", usuario.Id);

                mySqlCommand.Prepare();

                int resultado = mySqlCommand.ExecuteNonQuery();

                if (resultado >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }


            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                mySqlConnection.Close();
            }


        }

        // DELETE: api/User/{id}
        // Ej: http://localhost:5000/api/User/100
        [HttpDelete("{id}")]
        public bool DeleteUsuario(int id)
        {

            

            //Se obtiene el configurationString

            string connectionString = configuration.GetConnectionString("mysql");
            Console.WriteLine(connectionString);

            string SQL = "delete from usuarios where id = @id";


            MySqlConnection mySqlConnection = new MySqlConnection(connectionString);

            try
            {
                mySqlConnection.Open();
                //Se genera el store procedure
                MySqlCommand mySqlCommand = new MySqlCommand(SQL, mySqlConnection)
                {
                    Connection = mySqlConnection
                };

          
                mySqlCommand.Parameters.AddWithValue("@id", id);

                mySqlCommand.Prepare();

                int resultado = mySqlCommand.ExecuteNonQuery();

                if (resultado >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }


            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                mySqlConnection.Close();
            }

        }
        

    }
}
