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
    public class MovimientoExpteController : ControllerBase
    {
        private readonly WebApiCriminalisticaContext _context;

        public Result<MovimientoExpte> res = new Result<MovimientoExpte>();
        string data;
        public MovimientoExpteController(WebApiCriminalisticaContext context)
        {
            _context = context;
        }

        // GET: api/MovimientoExpte/1,2
        [HttpGet("paginate/{pagina},{cantidad}")]
        public async Task<ActionResult<Result<MovimientoExpte>>> GetMovimientoExpte(int pagina, int cantidad)
        {
            Paginate paginate = new Paginate();
            paginate.cantidadMostrar = cantidad;
            paginate.pagina = pagina;

            using (var DBcontext = _context)
            {
                try
                {
                    var queryable = DBcontext.MovimientoExpte
                        .AsNoTracking()
                        .Where(t => t.activo == true)
                        .OrderBy(o => o.expedienteNavegacion.fechaExpte)
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


        // GET: api/MovimientoExpte/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovimientoExpte>> GetMovimientoExpte(int id)
        {
            using (var DBcontext = _context)
            {
                try
                {
                    var obj = DBcontext.MovimientoExpte
                        .AsNoTracking()
                        .SingleOrDefault(r => r.id == id);

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

        // PUT: api/MovimientoExpte/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovimientoExpte(int id, MovimientoExpte expte)
        {
            using (var DBcontext = _context)
            {
                try
                {
                    var obj = DBcontext.MovimientoExpte.FirstOrDefault(r => r.id == id);
                    if (obj != null)
                    {
                        obj.destinoPolicial = expte.destinoPolicial;
                        obj.destinoNoPolicial = expte.destinoNoPolicial;
                        obj.tipoMovimiento = expte.tipoMovimiento;
                        obj.fechaRecepcion = expte.fechaRecepcion;
                        obj.usuarioRecibe = expte.usuarioRecibe;
                        obj.observaciones = expte.observaciones;

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

        // POST: api/MovimientoExpte
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MovimientoExpte>> PostMovimientoExpte(MovimientoExpte expte)
        {
           // var expediente = expte.expte;
            var tipoMoviento = expte.tipoMovimiento;
           // var estado = expte.estadoExpte;

            using (var DBcontext = _context)
            {
                try
                {
                    // var verificar = DBcontext.MovimientoExpte.SingleOrDefault(r => r.expte == expediente && r.activo == true);

                    //if (verificar is null)
                    MovimientoExpte obj = new MovimientoExpte();
                    // {
                    if (tipoMoviento == "SALIDA")
                    {
                        obj.expte = expte.expte;
                        obj.destinoPolicial = expte.destinoPolicial;
                        obj.destinoNoPolicial = expte.destinoNoPolicial;
                        obj.usuarioEnvia = expte.usuarioEnvia;
                        obj.tipoMovimiento = expte.tipoMovimiento;
                        obj.observaciones = expte.observaciones;
                        obj.fechaEnvio = DateTime.Now;
                        obj.activo = true;
                    }
                    if (tipoMoviento == "RECEPCION")
                    {
                        obj.expte = expte.expte;
                        obj.destinoPolicial = expte.destinoPolicial;
                        obj.destinoNoPolicial = expte.destinoNoPolicial;
                        obj.usuarioRecibe = expte.usuarioRecibe;
                        obj.tipoMovimiento = expte.tipoMovimiento;
                        obj.observaciones = expte.observaciones;
                        obj.fechaRecepcion = DateTime.Now;
                        obj.activo = true;
                    }

                    DBcontext.MovimientoExpte.Add(obj);
                    await DBcontext.SaveChangesAsync();

                    res.dato = obj;
                    res.code = "200";
                    res.message = "Dato insertado correctamente";
                    //}
                    // else
                    //{
                    //res.code = "204";
                    //res.message = "El dato ingresado ya existe en la base de datos";
                    //}
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

        // DELETE: api/MovimientoExpte/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovimientoExpte(int id)
        {
            using (var DBcontext = _context)
            {
                try
                {
                    var obj = DBcontext.MovimientoExpte.SingleOrDefault(r => r.id == id);

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

        // GET: api/MovimientoExpte/filterMovimientoExpte/{criterio}
        [HttpGet("filterMovimientoExpte/{criterio}")]
        public async Task<ActionResult<MovimientoExpte>> filter(string criterio)
        {
            try
            {
                using (var DBcontext = _context)
                {
                    if (!String.IsNullOrEmpty(criterio))
                    {
                        var busqueda = await DBcontext.MovimientoExpte
                            .AsNoTracking()
                            .Where(s => s.expedienteNavegacion.nroIntervencion.Contains(criterio) || s.expedienteNavegacion.nroNota.Contains(criterio) && s.activo == true)
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

        [HttpGet("filterBusqAvanzada/{fecha1},{fecha2},{nroIntervencion},{nroNota}")]
        public async Task<ActionResult<Result<MovimientoExpte>>> FiltroBusquedaAvanzada(
            string fecha1, string fecha2, string nroIntervencion, string nroNota)
        {

            try
            {
                using (var DBcontext = _context)
                {

                    var planillaQueryable = DBcontext.MovimientoExpte.AsQueryable();

                    if (fecha1 != null && fecha2 != null)
                    {
                        var fechaExpt1 = Convert.ToDateTime(fecha1);
                        var fechaExpt2 = Convert.ToDateTime(fecha2);

                        planillaQueryable = planillaQueryable.Where(f => f.expedienteNavegacion.fechaExpte >= fechaExpt1 && f.expedienteNavegacion.fechaExpte <= fechaExpt2);

                    }
                    else if (fecha1 != null && fecha2 == "fechaVacia")
                    {
                        fecha1 = fecha2;
                        var fechaExpt1 = Convert.ToDateTime(fecha1);
                        planillaQueryable = planillaQueryable.Where(f => f.expedienteNavegacion.fechaExpte == fechaExpt1);

                    }
                    if (!String.IsNullOrEmpty(nroIntervencion))
                    {

                        planillaQueryable = planillaQueryable.Where(d => d.expedienteNavegacion.nroIntervencion == nroIntervencion);

                    }
                    if (!String.IsNullOrEmpty(nroNota))
                    {

                        planillaQueryable = planillaQueryable.Where(l => l.expedienteNavegacion.nroNota == nroNota);

                    }


                    var planilla = await planillaQueryable
                        .Include(e => e.expedienteNavegacion.estadoNavegacion)
                        .Include(uC => uC.expedienteNavegacion.usuarioCreaNavegacion)
                        .Include(uM => uM.expedienteNavegacion.usuarioModificaNavegacion)
                        .AsNoTracking()
                        .Where(a => a.activo == true)
                        .OrderBy(ordenar => ordenar.expedienteNavegacion.fechaExpte)
                        .ToListAsync();

                    if (planilla.Count > 0)
                    {
                        res.data = planilla;
                        res.code = "200";
                        res.message = "Busqueda realizada correctamente";
                    }
                    else
                    {
                        res.code = "204";
                        res.message = "No existe el dato de búsqueda";
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

        private bool MovimientoExpteExists(int id)
        {
            return _context.MovimientoExpte.Any(e => e.id == id);
        }
    }
}
