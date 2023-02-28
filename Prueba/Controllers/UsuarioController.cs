using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Prueba.Models; //Invocamos a los models
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Prueba.Controllers
{
    [ApiController] 
    [Route("Usuario")]
    public class UsuarioController : Controller
    {
        // Definir las propiedades de configuracion
        #region Propiedades
        public IConfiguration _configuration;
        #endregion

        // Constructor del controlador
        #region Constructor
        public UsuarioController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion


        [HttpPost]
        [Route("Login")]
        public dynamic IniciarSesion([FromBody] Object optData)
        {
            var data = JsonConvert.DeserializeObject<dynamic>(optData.ToString()); //convertir un JSON a una cadena de texto, la cual será muy útil para convertir un objeto JSON o Dictionary a un string para enviarlo en una petición la podremos realizar mediante el método SerializeObject de la clase JsonConvert

            //Estas variables son propias del controlador
            string usuarioC = data["usuario"]; //No se puede convertir en string, genera error
            string claveC = data["clave"]; //Estos son los datos que se digitan en postman por el puerto 7256 el externo
            
            Usuario usuario = Usuario.BaseDatos().Where(x => x.usuario == usuarioC && x.clave == claveC).FirstOrDefault();

            if (usuario == null)
            {
                return new
                {
                    success = false,
                    message = "Credenciales Incorrectas",
                    result = ""
                };
            }

            var jwt = _configuration.GetSection("Jwt").Get<Jwt>();

            var claims = new[] //defina un vector llamado claims que contendra el asunto, la fecha de hoy, el idusuario y nombre usuario
            { 
                new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, DateTime.UtcNow.ToString()),
                new Claim("id", usuario.idUsuario), //Esto es capaz de verificar el numero de id en la clase JWT
                new Claim("usuario", usuario.usuario),
            };

            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var singIn = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken( //Creacion del token
                jwt.Issuer,  //Debe tener el valor del localhost externo en el puerto tal..
                jwt.Audience, //Debe tener el valor del localhost externo en el puerto tal..
                claims, // vector de la variable claims definida anteriormente
                expires: DateTime.Now.AddMinutes(4), //tiempo de expiracion del token 4 minutos
                signingCredentials: singIn //sesion encriptada
                );

            return new
            {
                success = true,
                message = "Exito",
                result = new JwtSecurityTokenHandler().WriteToken(token)
            };


        }
    }
}
