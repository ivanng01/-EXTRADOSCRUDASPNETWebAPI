using EjApi.Datos;
using EjApi.Modelo;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace EjApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController: ControllerBase
    {
        //Crear usuario  
        [HttpPost("Crear")]
        public async Task<ActionResult> CrearUsuario(Usuario nUs)
        {
            try
            {
                if (nUs.Nombre_Usuario == null || nUs.Mail_Usuario == null || nUs.Pass_Usuario == null)
                {
                    return BadRequest("Error en los datos de la solicitud");
                }
                InfoUsuarios datoUsuario = new InfoUsuarios();
                var usuarioFinal = await datoUsuario.CrearUsuario(nUs.Nombre_Usuario, nUs.Mail_Usuario, nUs.Pass_Usuario);
                return Ok(usuarioFinal);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error al crear un nuevo usuario, verifique solicitud {e.Message}");
                return BadRequest("Error al crear nuevo usuario, verifique solicitud");
            }  
        }
        //Obtener usuario por id
        [HttpGet("Obtener/{id}")]
        public async Task<IActionResult> ObtenerUsuarioId(int id)
        {
            InfoUsuarios datoUsuario = new InfoUsuarios();
            Usuario nUs = await datoUsuario.ObtenerUsuarioID(id);

            try
            {
                if (nUs == null) 
                { 
                    return BadRequest("Usuario no encontrado por ID"); 
                }
                return Ok(nUs);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error al obtener usuario por ID {e.Message}");
                return BadRequest("Error al obtener usuario por ID" + e.Message);
            }
        }
        //Modificar usuario por id
        [HttpPut("Modificar/{id}")]
        public async Task<IActionResult> ModificarUsuarioId(int id,[FromBody] Usuario nUs)
        {
            try
            {
                if (id != nUs.Id_Usuario )
                {
                   return BadRequest("Usuario ID erroneo");
                }    
                InfoUsuarios dataUser = new InfoUsuarios();
                var user = await dataUser.ModificarUsuario(nUs.Id_Usuario, nUs.Nombre_Usuario, nUs.Mail_Usuario, nUs.Pass_Usuario);
                return Ok($"Datos Modificados: Usuario {nUs.Id_Usuario}, Nombre {nUs.Nombre_Usuario} Mail {nUs.Mail_Usuario}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error al modificar usuario {e.Message}");
                return BadRequest("Error al modificar usuario" + e.Message);
            }
        }
        //Eliminar usuario por id
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> EliminarUsuarioId(int id)
        {
            try
            {
                InfoUsuarios datoUsuario = new InfoUsuarios();
                var user = await datoUsuario.ObtenerUsuarioID(id);
                if (user == null) 
                {
                    return BadRequest("Usuario no encontrado por ID");
                }
                await datoUsuario.EliminarUsuario(id);
                return Ok(user);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error al eliminar usuario por ID {e.Message}");
                return BadRequest("Error al eliminar usuario por ID" + e.Message);
            }
        }
    }
}
