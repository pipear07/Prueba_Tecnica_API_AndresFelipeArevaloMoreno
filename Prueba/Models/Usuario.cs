namespace Prueba.Models
{
    public class Usuario
    {
        public string idUsuario { get; set; } //id del usuario

        public string usuario { get; set; } // nombre del usuario 

        public string clave { get; set; } //  clave del usuario

        public string rol { get; set; } // rol del usuario

        public static List<Usuario> BaseDatos() //instancie una lista de Usuarios llama BaseDatos
        {//aqui estoy simulando una lista de usuarios como si fuera una base de datos
            var list = new List<Usuario>()
            {
                new Usuario //Instancie la clase Usuario
                { //No puede insertar fichas
                    idUsuario = "1",
                    usuario = "pipear",
                    clave = "12345",
                    rol = "usuario"
                },
                new Usuario
                { //Tiene privilegios para insertar fichas
                    idUsuario = "2",
                    usuario = "admin",
                    clave = "12345",
                    rol = "administrador"
                },
                new Usuario
                { //No puede insertar fichas
                    idUsuario = "3",
                    usuario = "prueba",
                    clave = "12345",
                    rol = "usuario"
                }
            };

            return list; //retorne la variable lista
        }

    }
}
