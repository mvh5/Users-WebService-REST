using System;
namespace API_WS
{
    [ToString]
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

        
    }
}
