using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApiCriminalistica.Components;
using WebApiCriminalistica.Data;
using WebApiCriminalistica.Models;

namespace WebApiCriminalistica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioCriminalisticaController : ControllerBase
    {
        private readonly WebApiCriminalisticaContext _context;

        public Result<UsuarioCriminalistica> res = new Result<UsuarioCriminalistica>();
        string data;

        public UsuarioCriminalisticaController(WebApiCriminalisticaContext context)
        {
            _context = context;
        }

        // GET: api/UsuarioCriminalistica
        [HttpGet("paginate/{pagina},{cantidad},{unidad}")]
        public async Task<ActionResult<Result<UsuarioCriminalistica>>> GetUsuario(int pagina, int cantidad, int unidad)
        {
            Paginate paginate = new Paginate();
            paginate.cantidadMostrar = cantidad;
            paginate.pagina = pagina;

            using (var DBcontext = _context)
            {
                try
                {
                    var queryable = DBcontext.UsuarioCriminalistica
                        .AsNoTracking()
                        .Include(usuario => usuario.rolNavigation)
                        .OrderBy(s => s.apellido)
                        .Where(usu => usu.baja == false && usu.sistema == unidad).AsQueryable();

                    double conteo = await queryable.CountAsync();
                    double TotalPaginas = Math.Ceiling(conteo / paginate.cantidadMostrar);

                    int totalPaginas = Convert.ToInt32(TotalPaginas);
                    int totalRegistros = Convert.ToInt32(conteo);

                    if (queryable != null)
                    {
                        res.data = queryable.Paginar(paginate).ToList();
                        res.totalRegistros = totalRegistros;
                        res.totalPaginas = totalPaginas;
                        res.code = "200";
                        res.message = "Datos obtenidos correctamente";
                    }
                    else if (queryable == null)
                    {
                        res.data = queryable.ToList();
                        res.code = "204";
                        res.message = "No existen datos en la base de datos";
                    }
                }
                catch (Exception ex)
                {
                    res.error = "Error al obtener el dato " + ex.Message;
                }


                data = JsonConvert.SerializeObject(res);

                return Ok(data);
            }
        }

        // GET: api/UsuarioCriminalistica/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioCriminalistica>> GetUsuario(int id)
        {
            using (var DBcontext = _context)
            {
                try
                {
                    var obj = DBcontext.UsuarioCriminalistica.AsNoTracking().Include(r => r.rolNavigation).SingleOrDefault(r => r.usuarioRepo == id);
                    if (obj != null)
                    {
                        res.dato = obj;
                        res.code = "200";
                        res.message = "Dato obtenido correctamente";
                    }
                    else if (obj == null)
                    {
                        res.dato = obj;
                        res.code = "401";
                        res.message = "No existen datos en la base de datos";
                    }
                }
                catch (Exception ex)
                {
                    res.error = "Error al obtener el dato " + ex.Message;
                }

                data = JsonConvert.SerializeObject(res);

                return Ok(data);
            }
        }

        // PUT: api/UsuarioCriminalistica/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, UsuarioCriminalistica usuarios)
        {
            using (var DBcontext = _context)
            {
                try
                {
                    var obj = DBcontext.UsuarioCriminalistica.FirstOrDefault(r => r.id == id);
                    if (obj != null)
                    {
                        obj.nombre = usuarios.nombre;
                        obj.persona = usuarios.persona;
                        obj.civil = usuarios.norDni;
                        obj.nombre = usuarios.nombre;
                        obj.apellido = usuarios.apellido;
                        obj.tipoPersona = usuarios.tipoPersona;
                        obj.rol = usuarios.rol;

                        DBcontext.Entry(obj).State = EntityState.Modified;
                        await DBcontext.SaveChangesAsync();

                        res.dato = obj;
                        res.code = "200";
                        res.message = "Dato modificado correctamente";
                    }
                    else if (obj == null)
                    {
                        res.code = "204";
                        res.message = "No existen datos en la base de datos";
                    }
                }
                catch (Exception ex)
                {
                    res.error = "Error al obtener el dato " + ex.Message;
                }

                data = JsonConvert.SerializeObject(res);

                return Ok(data);
            }
        }

        [HttpPut("bajaUsuario/{id}")]
        public async Task<IActionResult> PutBaja(int id, UsuarioCriminalistica usuarios)
        {
            using (var DBcontext = _context)
            {
                try
                {
                    var obj = DBcontext.UsuarioCriminalistica.FirstOrDefault(r => r.id == id);
                    if (obj != null)
                    {
                        //baja logica
                        //obj.usuarioBaja = usuarios.usuarioBaja
                        if (usuarios.baja)
                        {
                            obj.fechaBaja = DateTime.Now;
                            obj.usuarioBaja = usuarios.usuarioBaja;
                            obj.baja = usuarios.baja;
                        }
                        else
                        {
                            obj.baja = usuarios.baja;
                        }

                        DBcontext.Entry(obj).State = EntityState.Modified;
                        await DBcontext.SaveChangesAsync();

                        res.dato = obj;
                        res.code = "200";
                        res.message = "Dato modificado correctamente";
                    }
                    else if (obj == null)
                    {
                        res.code = "204";
                        res.message = "No existen datos en la base de datos";
                    }
                }
                catch (Exception ex)
                {
                    res.error = "Error al obtener el dato " + ex.Message;
                }

                data = JsonConvert.SerializeObject(res);

                return Ok(data);
            }
        }

        // POST: api/UsuarioCriminalistica
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UsuarioCriminalistica>> PostUsuario(UsuarioCriminalistica usuario)
        {
            var nombre = usuario.nombre;
            var apellido = usuario.apellido;
            var norDni = usuario.norDni;

            using (var DBcontext = _context)
            {
                try
                {
                    var verificar = DBcontext.UsuarioCriminalistica
                        .SingleOrDefault(r => r.nombre == nombre && r.apellido == apellido && r.norDni == norDni);

                    if (verificar is null)
                    {
                        UsuarioCriminalistica obj = new UsuarioCriminalistica();
                        obj.usuarioRepo = usuario.usuarioRepo;
                        obj.userCreaRepo = usuario.userCreaRepo;
                        obj.fechaAlta = usuario.fechaAlta;
                        obj.persona = usuario.persona;
                        obj.civil = usuario.civil;
                        obj.norDni = norDni;
                        obj.nombre = nombre;
                        obj.apellido = apellido;
                        obj.tipoPersona = usuario.tipoPersona;
                        obj.sistema = usuario.sistema;
                        obj.cifrado = usuario.cifrado;
                        //obj.rol = 14;
                        obj.rol = usuario.rol;
                        obj.activo = true;

                        DBcontext.UsuarioCriminalistica.Add(obj);
                        await DBcontext.SaveChangesAsync();

                        res.code = "200";
                        res.message = "Dato insertado correctamente";
                    }
                    else
                    {
                        res.code = "204";
                        res.message = "El dato ingresado ya existe en la base de datos";
                    }
                }
                catch (Exception ex)
                {
                    res.code = "500";
                    res.message = "No se pudo insertar los datos en la base de datos";
                    res.error = "Error al insertar el dato " + ex.Message;
                }

                data = JsonConvert.SerializeObject(res);

                return Ok(data);
            }
        }

        // DELETE: api/UsuarioCriminalistica/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id, UsuarioCriminalistica usuarios)
        {
            using (var DBcontext = _context)
            {
                try
                {
                    var obj = DBcontext.UsuarioCriminalistica.SingleOrDefault(r => r.id == id);

                    if (obj != null)
                    {
                        //baja logica
                        //obj.usuarioBaja = usuarios.usuarioBaja
                        obj.fechaBaja = DateTime.Now;
                        obj.baja = true;

                        DBcontext.Entry(obj).State = EntityState.Modified;
                        await DBcontext.SaveChangesAsync();

                        //baja eliminar BD
                        // rol r = DBcontext.rol.Single(us => us.id == id);
                        //DBcontext.Remove(obj);
                        //await DBcontext.SaveChangesAsync();

                        res.code = "200";
                        res.message = "Dato eliminado correctamente";
                    }
                    else
                    {
                        res.code = "204";
                        res.message = "No se pudo eliminar los datos de la base de datos";
                    }
                }
                catch (Exception ex)
                {
                    res.code = "500";
                    res.message = "No se pudo eliminar el dato de la base de datos";
                    res.error = "Error al insertar el dato " + ex.Message;
                }

                data = JsonConvert.SerializeObject(res);

                return Ok(data);
            }
        }

        // GET: api/UsuarioCriminalistica/filterUsuarios/{criterio}
        [HttpGet("filterUsuarios/{criterio},{unidad}")]
        public async Task<ActionResult<Result<UsuarioCriminalistica>>> filter(string criterio, int unidad)
        {
            try
            {
                using (var DBcontext = _context)
                {
                    if (!String.IsNullOrEmpty(criterio))
                    {
                        var busqueda = await DBcontext.UsuarioCriminalistica
                            .AsNoTracking()
                            .Include(r => r.rolNavigation)
                            .Where(s => s.apellido.Contains(criterio) || s.nombre.Contains(criterio) || s.norDni.ToString() == criterio && s.sistema == unidad && s.activo == true)
                            .ToListAsync();

                        if (busqueda.Count > 0)
                        {
                            res.data = busqueda;
                            res.code = "200";
                            res.message = "Búsqueda realizada correctamente";
                        }
                        else
                        {
                            res.data = busqueda;
                            res.code = "204";
                            res.message = "No se encontro los datos en la base de datos";
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                res.code = "500";
                res.message = "No se pudo realizar la busqueda de datos";
                res.error = "Error al obtener los datos " + ex.Message;
            }

            data = JsonConvert.SerializeObject(res);

            return Ok(data);
        }

        private bool UsuarioCriminalisticaExists(int id)
        {
            return _context.UsuarioCriminalistica.Any(e => e.id == id);
        }
    }
}
