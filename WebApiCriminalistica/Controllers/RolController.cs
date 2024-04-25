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
    public class RolController : ControllerBase
    {
        private readonly WebApiCriminalisticaContext _context;

        public Result<Rol> res = new Result<Rol>();
        string data;

        public RolController(WebApiCriminalisticaContext context)
        {
            _context = context;
        }

        [HttpGet("lst")]
        public async Task<ActionResult<Result<Rol>>> GetRol()
        {

            using (var DBcontext = _context)
            {
                try
                {
                    var consulta = DBcontext.Rol
                        .AsNoTracking()
                        .Where(rol => rol.nombre != "MANAGER" && rol.activo == true)
                        .ToList();

                    if (consulta.Count > 0)
                    {
                        res.data = consulta;
                        res.code = "200";
                        res.message = "Datos obtenidos correctamente";
                    }
                    else
                    {
                        res.data = consulta;
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

        // GET: api/Rol
        [HttpGet("paginate/{pagina},{cantidad}")]
        public async Task<ActionResult<Result<Rol>>> GetRols(int pagina, int cantidad)
        {
            Paginate paginate = new Paginate();
            paginate.cantidadMostrar = cantidad;
            paginate.pagina = pagina;

            using (var DBcontext = _context)
            {
                try
                {
                    var queryable = DBcontext.Rol.AsNoTracking().Where(rol => rol.id > 3).AsQueryable();
                    //var queryable = DBcontext.Rol
                        //.AsNoTracking()
                        //.Where(t => t.activo == true)
                        //.OrderBy(o => o.nombre)
                        //.AsQueryable();

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
                    else if (queryable is null)
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

        // GET: api/Rol/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Rol>> GetRol(int id)
        {
            using (var DBcontext = _context)
            {
                try
                {
                    var obj = DBcontext.Rol
                        .AsNoTracking()
                        .SingleOrDefault(r => r.id == id && r.activo == true);

                    if (obj != null)
                    {
                        res.dato = obj;
                        res.code = "200";
                        res.message = "Dato obtenido correctamente";
                    }
                    else if (obj is null)
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

        // PUT: api/Rol/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRol(int id, Rol rol)
        {
            using (var DBcontext = _context)
            {
                try
                {
                    var obj = DBcontext.Rol.FirstOrDefault(r => r.id == id);

                    if (obj != null)
                    {
                        obj.nombre = rol.nombre;

                        DBcontext.Entry(obj).State = EntityState.Modified;
                        await DBcontext.SaveChangesAsync();

                        res.dato = obj;
                        res.code = "200";
                        res.message = "Dato modificado correctamente";
                    }
                    else if (obj is null)
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

        // POST: api/Rol
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Rol>> PostRol(Rol rol)
        {
            var nombre = rol.nombre;
            using (var DBcontext = _context)
            {
                try
                {
                    var verificar = DBcontext.Rol.SingleOrDefault(r => r.nombre == nombre);

                    if (verificar is null)
                    {
                        Rol obj = new Rol();
                        obj.nombre = nombre;
                        obj.activo = true;

                        DBcontext.Rol.Add(obj);
                        await DBcontext.SaveChangesAsync();

                        //res.dato = obj;
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

        // DELETE: api/Rol/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRol(int id)
        {
            using (var DBcontext = _context)
            {
                try
                {
                    var obj = DBcontext.Rol.SingleOrDefault(r => r.id == id);

                    if (obj != null)
                    {
                        //baja logica
                        //entidad entity = DBcontext.entidad.SingleOrDefault(r => r.id == id);
                        obj.activo = false;
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

        // GET: api/Rol/filterRol/{criterio}
        [HttpGet("filterRol/{criterio}")]
        public async Task<ActionResult<Rol>> filter(string criterio)
        {
            try
            {
                using (var DBcontext = _context)
                {
                    if (!String.IsNullOrEmpty(criterio))
                    {
                        var busqueda = await DBcontext.Rol
                            .AsNoTracking()
                            .Where(s => s.nombre.Contains(criterio) && s.activo == true)
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

        private bool RolExists(int id)
        {
            return _context.Rol.Any(e => e.id == id);
        }
    }
}
