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
    public class EstadosController : ControllerBase
    {
        private readonly WebApiCriminalisticaContext _context;

        //private readonly IConfiguration configuration;

        public Result<Estados> res = new Result<Estados>();
        string data;
       
        //public EstadosController(IConfiguration configuration)
        //{

        //    this.configuration = configuration;
        //}

        public EstadosController(WebApiCriminalisticaContext context)
        {
            _context = context;
        }

        // GET: api/Estados
        [HttpGet("paginate/{pagina},{cantidad},{unidad}")]
        public async Task<ActionResult<Result<Estados>>> GetEstados(int pagina, int cantidad, int unidad)
        {
            Paginate paginate = new Paginate();
            paginate.cantidadMostrar = cantidad;
            paginate.pagina = pagina;

            using (var DBcontext = _context)
            {
                try
                {
                   var queryable = DBcontext.Estados
                        .Include(e => e.unidad)
                        .AsNoTracking()
                        .Where(t => t.unidadCreacion == unidad && t.activo == true)
                        .OrderBy(o => o.nombre)
                        .AsQueryable();

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

        // GET: api/Estados/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Estados>> GetEstado(int id)
        {
            using (var DBcontext = _context)
            {
                try
                {
                    var obj = DBcontext.Estados
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
        
        // PUT: api/Estados/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstado(int id, Estados estado)
        {
            using (var DBcontext = _context)
            {
                try
                {
                    var obj = DBcontext.Estados.FirstOrDefault(r => r.id == id);
                    if (obj != null)
                    {
                        obj.nombre = estado.nombre;

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
               
        // POST: api/Estados
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Estados>> PostEstado(Estados estados)
        {
            using (var DBcontext = _context)
            {
                try
                {
                    var verificar = DBcontext.Estados.SingleOrDefault(r => r.nombre == estados.nombre && r.unidadCreacion == estados.unidadCreacion && r.activo == true);

                    if (verificar is null)
                    {
                        Estados obj = new Estados();
                        obj.nombre = estados.nombre;
                        obj.unidadCreacion = estados.unidadCreacion;
                        obj.activo = true;

                        DBcontext.Estados.Add(obj);
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

        // DELETE: api/Estados/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstados(int id)
        {
            using (var DBcontext = _context)
            {
                try
                {
                    var obj = DBcontext.Estados.SingleOrDefault(r => r.id == id);

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

        // GET: api/Sistemas/filterSistemas/{criterio}
        [HttpGet("filterEstado/{criterio},{unidad}")]
        public async Task<ActionResult<Estados>> filter(string criterio, int unidad)
        {
            try
            {
                using (var DBcontext = _context)
                {
                    if (!String.IsNullOrEmpty(criterio))
                    {
                        var busqueda = await DBcontext.Estados
                            .AsNoTracking()
                            .Where(s => s.nombre.Contains(criterio) && s.unidadCreacion == unidad && s.activo == true)
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

        private bool EstadosExists(int id)
        {
            return _context.Estados.Any(e => e.id == id);
        }
    }
}
