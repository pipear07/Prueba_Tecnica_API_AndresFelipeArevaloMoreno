using System.Security.Claims;

namespace Prueba.Models
{
    public class Jwt //Definimos la clase Jwt que contendra las variables de appsettings.json en jwt
    {
        public string Key { get; set; }  //Llave

        public string Issuer { get; set; } //localhost externo usado con el puerto 

        public string Audience { get; set; } //localhost externo usado con el puerto

        public string Subject { get; set; } // Asunto de la llave

        //metodo para validar el token
        public static dynamic validarToken(ClaimsIdentity identity) //Metodo static para acceder a él, dentro de cualquier metodo
        {
            try
            {   //Aqui verifica si el token es valido contando los valores claims
                if (identity.Claims.Count() == 0)
                {
                    return new
                    { 
                        success = false,
                        message = "Verifica si estas enviando un token valido",
                        result = ""
                    };
                }
                //Aqui implementamos el id que colocamos en usuarioController donde se declara la variable claims
                var id = identity.Claims.FirstOrDefault(x => x.Type == "id").Value; //Encuentre la columna llamada id y almacene su valor
                Usuario usuario = Usuario.BaseDatos().FirstOrDefault(x =>  x.idUsuario == id); //Esto se puede cambiar por una base de datos
        
                return new
                { //Si llega a ser exitoso retornara esto
                    success = true,
                    message = "Exito",
                    result = usuario
                };
            }
            catch (Exception ex)
            {
                return new
                { //Si ocurre un error nos mostrara la exception
                    success = false,
                    message = "Catch "+ex.Message,
                    result = ""
                };

            }
        }
    }
}
