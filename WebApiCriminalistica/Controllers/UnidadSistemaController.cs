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
    public class UnidadSistemaController : ControllerBase
    {
        private readonly WebApiCriminalisticaContext _context;

        public Result<UnidadSistema> res = new Result<UnidadSistema>();
        string data;

        public UnidadSistemaController(WebApiCriminalisticaContext context)
        {
            _context = context;
        }

        // GET: api/UnidadSistema
        [HttpGet("obtenerDatos")]
        public async Task<ActionResult<Result<UnidadSistema>>> GetSistema()
        {
            using (var DBcontext = _context)
            {
                try
                {
                    var datos = await DBcontext.UnidadSistema.ToListAsync();

                    if (datos.Count > 0)
                    {
                        res.data = datos;
                        res.code = "200";
                        res.message = "Datos obtenidos correctamente";
                    }
                    else if (datos.Count < 0)
                    {
                        res.data = datos;
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

        // GET: api/UnidadSistema/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UnidadSistema>> GetUnidadSistema(int id)
        {
            using (var DBcontext = _context)
            {
                try
                {
                    var obj = DBcontext.UnidadSistema.SingleOrDefault(r => r.id == id);
                    if (obj != null)
                    {
                        res.dato = obj;
                        res.code = "200";
                        res.message = "Dato obtenido correctamente";
                    }
                    else if (obj == null)
                    {
                        res.dato = obj;
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

        // PUT: api/UnidadSistema/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUnidadSistema(int id, UnidadSistema unidad)
        {
            using (var DBcontext = _context)
            {
                try
                {
                    var obj = DBcontext.UnidadSistema.FirstOrDefault(r => r.id == id);
                    if (obj != null)
                    {
                        obj.nombre = unidad.nombre;

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

        // POST: api/UnidadSistema
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UnidadSistema>> PostUnidadSistema(UnidadSistema unidad)
        {
            var nombre = unidad.nombre;
            using (var DBcontext = _context)
            {
                try
                {
                    var verificar = DBcontext.UnidadSistema.SingleOrDefault(r => r.nombre == nombre);

                    if (verificar is null)
                    {
                        UnidadSistema obj = new UnidadSistema();
                        obj.nombre = unidad.nombre;

                        DBcontext.UnidadSistema.Add(obj);
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

        // DELETE: api/UnidadSistema/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            using (var DBcontext = _context)
            {
                try
                {
                    var obj = DBcontext.UnidadSistema.SingleOrDefault(r => r.id == id);

                    if (obj != null)
                    {
                        //baja logica
                        //obj.activo = false;
                        //DBcontext.Entry(obj).State = EntityState.Modified;
                        //await DBcontext.SaveChangesAsync();

                        //baja eliminar BD
                        // rol r = DBcontext.rol.Single(us => us.id == id);
                        DBcontext.Remove(obj);
                        await DBcontext.SaveChangesAsync();

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

        private bool UnidadSistemaExists(int id)
        {
            return _context.UnidadSistema.Any(e => e.id == id);
        }
    }
}
