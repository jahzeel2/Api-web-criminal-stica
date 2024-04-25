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
    public class PeritosController : ControllerBase
    {
        private readonly WebApiCriminalisticaContext _context;

        private readonly IConfiguration configuration;

        public Result<Peritos> res = new Result<Peritos>();
        string data;

        public PeritosController(WebApiCriminalisticaContext context)
        {
            _context = context;
        }

        // GET: api/Peritos
        [HttpGet("paginate/{pagina},{cantidad},{unidad}")]
        public async Task<ActionResult<Result<Peritos>>> GetPeritos(int pagina, int cantidad, int unidad)
        {
            Paginate paginate = new Paginate();
            paginate.cantidadMostrar = cantidad;
            paginate.pagina = pagina;

            using (var DBcontext = _context)
            {
                try
                {
                    var queryable = DBcontext.Peritos
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

        // GET: api/Peritos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Peritos>> GetPeritos(int id)
        {
            using (var DBcontext = _context)
            {
                try
                {
                    var obj = DBcontext.Peritos
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

        // PUT: api/Peritos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPeritos(int id, Peritos perito)
        {
            using (var DBcontext = _context)
            {
                try
                {
                    var obj = DBcontext.Peritos.FirstOrDefault(r => r.id == id);
                    if (obj != null)
                    {
                        if (obj.tipoPersona == "Civil")
                        {
                            obj.nombre = perito.nombre;
                            obj.apellido = perito.apellido;
                            obj.dni = perito.dni;
                            obj.tipoPerito = perito.tipoPerito;

                        }else if(obj.tipoPersona == "Personal Policial")
                        {
                            obj.tipoPerito = perito.tipoPerito;
                        }

                        DBcontext.Entry(obj).State = EntityState.Modified;
                        await DBcontext.SaveChangesAsync();

                        //res.dato = obj;
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

        // POST: api/Peritos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Peritos>> PostPerito(Peritos perito)
        {
            using (var DBcontext = _context)
            {
                try
                {
                    var verificar = DBcontext.Peritos.SingleOrDefault(r => r.dni == perito.dni && r.unidadCreacion == perito.unidadCreacion && r.activo == true);

                    if (verificar is null)
                    {
                        Peritos obj = new Peritos();
                        obj.nombre = perito.nombre;
                        obj.apellido = perito.apellido;
                        obj.dni = perito.dni;
                        obj.tipoPersona = "policia";
                        obj.idPersonalPolicial = perito.idPersonalPolicial;
                        obj.idPersonalCivil = perito.idPersonalCivil;
                        obj.tipoPerito = perito.tipoPerito;
                        obj.fechaAlta = DateTime.Now;
                        obj.usuarioAlta = perito.usuarioAlta;
                        obj.unidadCreacion = perito.unidadCreacion;
                        obj.activo = true;

                        DBcontext.Peritos.Add(obj);
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
        // DELETE: api/Peritos/5
        [HttpDelete("{id},{usuario}")]
        public async Task<IActionResult> DeletePerito(int id, int usuario)
        {
            using (var DBcontext = _context)
            {
                try
                {
                    var obj = DBcontext.Peritos.SingleOrDefault(r => r.id == id);

                    if (obj != null)
                    {
                        //baja logica
                        obj.fechaBaja = DateTime.Now;
                        obj.usuarioBaja = usuario;
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

        [HttpGet("filterPerito/{criterio},{unidad}")]
        public async Task<ActionResult<Peritos>> filter(string criterio, int unidad)
        {
            try
            {
                using (var DBcontext = _context)
                {
                    if (!String.IsNullOrEmpty(criterio))
                    {
                        var busqueda = await DBcontext.Peritos
                            .AsNoTracking()
                            .Where(s => s.nombre.Contains(criterio) || s.apellido.Contains(criterio) || s.dni == Int64.Parse(criterio) && s.unidadCreacion == unidad && s.activo == true)
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


        private bool PeritosExists(int id)
        {
            return _context.Peritos.Any(e => e.id == id);
        }
    }
}
