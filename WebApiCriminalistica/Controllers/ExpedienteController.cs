using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using WebApiCriminalistica.Components;
using WebApiCriminalistica.Data;
using WebApiCriminalistica.Models;

namespace WebApiCriminalistica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpedienteController : ControllerBase
    {
        private readonly WebApiCriminalisticaContext _context;

        public Result<Expediente> res = new Result<Expediente>();
        string data;

        public ExpedienteController(WebApiCriminalisticaContext context)
        {
            _context = context;
        }

        // GET: api/Expediente/1,2
        [HttpGet("paginate/{pagina},{cantidad},{unidad}")]
        public async Task<ActionResult<Result<Expediente>>> GetExpediente(int pagina, int cantidad, int unidad)
        {
            Paginate paginate = new Paginate();
            paginate.cantidadMostrar = cantidad;
            paginate.pagina = pagina;

            using (var DBcontext = _context)
            {
                try
                {
                    var queryable = DBcontext.Expediente
                        .AsNoTracking()
                        .Include(e => e.estadoNavegacion)
                        .Include(p => p.perito)
                        .Where(t => t.unidadCreacion == unidad && t.activo == true)
                        .OrderBy(o => o.fechaExpte)
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

        // GET: api/Expediente/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Expediente>> GetExpediente(int id)
        {
            using (var DBcontext = _context)
            {
                try
                {
                    var obj = DBcontext.Expediente
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

        // PUT: api/Expediente/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExpediente(int id, Expediente expediente)
        {
            using (var DBcontext = _context)
            {
                try
                {
                    var obj = DBcontext.Expediente.FirstOrDefault(r => r.id == id);
                    if (obj != null)
                    {
                        obj.fechaExpte = expediente.fechaExpte;
                        obj.nroNota = expediente.nroNota;
                        obj.origenExpte = expediente.origenExpte;
                        obj.extracto = expediente.extracto;
                        obj.nroIntervencion = expediente.nroIntervencion;
                        obj.informeTecnico = expediente.informeTecnico;
                        obj.peritoInterviniente = expediente.peritoInterviniente;
                        obj.tipoPericia = expediente.tipoPericia;
                        obj.estadoExpte = expediente.estadoExpte;
                        obj.observacion = expediente.observacion;
                        obj.numerointerno = expediente.numerointerno;
                        obj.fechaModificacion = expediente.fechaModificacion;
                        obj.usuarioModifica = expediente.usuarioModifica;
                        obj.personalInterviniente = expediente.personalInterviniente;
                        obj.fechaBaja2 = expediente.fechaBaja2;

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

        // POST: api/Expediente
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Expediente>> PostExpte(Expediente expte)
        {
            var nota = expte.nroNota;
            using (var DBcontext = _context)
            {
                try
                {
                    var verificar = DBcontext.Expediente.SingleOrDefault(r => r.nroNota == nota && r.unidadCreacion == expte.unidadCreacion);

                    if (verificar is null)
                    {
                        Expediente obj = new Expediente();

                        //  obj = expte;

                        obj.unidadCreacion = expte.unidadCreacion;
                        obj.fechaExpte = expte.fechaExpte;
                        obj.nroNota = nota;
                        obj.origenExpte = expte.origenExpte;
                        obj.extracto = expte.extracto;
                        obj.nroIntervencion = expte.nroIntervencion;
                        obj.informeTecnico = expte.informeTecnico;
                        obj.peritoInterviniente = expte.peritoInterviniente;
                        obj.tipoPericia = expte.tipoPericia;
                        obj.estadoExpte = expte.estadoExpte;
                        obj.observacion = expte.observacion;
                        obj.numerointerno = expte.numerointerno;
                        obj.personalInterviniente = expte.personalInterviniente;
                        obj.fechaBaja2 = expte.fechaBaja2;



                        obj.fechaCreacion = DateTime.Now;
                        obj.usuarioCrea = expte.usuarioCrea;
                        obj.activo = true;

                        DBcontext.Expediente.Add(obj);
                        await DBcontext.SaveChangesAsync();

                        res.dato = obj;
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

        // DELETE: api/Expediente/5
        [HttpDelete("{id},{usuario}")]
        public async Task<IActionResult> DeleteExpte(int id, int usuario)
        {
            using (var DBcontext = _context)
            {
                try
                {
                    var obj = DBcontext.Expediente.SingleOrDefault(r => r.id == id);

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

        // GET: api/Expediente/filterExpediente/{criterio}
        [HttpGet("filterExpediente/{criterio},{unidad}")]
        public async Task<ActionResult<Expediente>> filter(string criterio, int unidad)
        {
            try
            {
                using (var DBcontext = _context)
                {
                    if (!String.IsNullOrEmpty(criterio))
                    {
                        var busqueda = await DBcontext.Expediente
                            .AsNoTracking()
                            .Include(s => s.estadoNavegacion)
                            .Include(j => j.perito)
                            .Where(s => s.nroIntervencion.Contains(criterio) || s.nroNota.Contains(criterio) || s.numerointerno.Contains(criterio) && s.unidadCreacion == unidad && s.activo == true)
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
        public async Task<ActionResult<Result<Expediente>>> FiltroBusquedaAvanzada(
            string fecha1, string fecha2, string nroIntervencion, string nroNota, int unidad)
        {

            try
            {
                using (var DBcontext = _context)
                {

                    var planillaQueryable = DBcontext.Expediente.AsQueryable();

                    if (fecha1 != null && fecha2 != null)
                    {
                        var fechaExpt1 = Convert.ToDateTime(fecha1);
                        var fechaExpt2 = Convert.ToDateTime(fecha2);

                        planillaQueryable = planillaQueryable.Where(f => f.fechaExpte >= fechaExpt1 && f.fechaExpte <= fechaExpt2);

                    } else if (fecha1 != null && fecha2 == "fechaVacia") {
                        fecha1 = fecha2;
                        var fechaExpt1 = Convert.ToDateTime(fecha1);
                        planillaQueryable = planillaQueryable.Where(f => f.fechaExpte == fechaExpt1);

                    }
                    if (!String.IsNullOrEmpty(nroIntervencion))
                    {

                        planillaQueryable = planillaQueryable.Where(d => d.nroIntervencion == nroIntervencion);

                    }
                    if (!String.IsNullOrEmpty(nroNota))
                    {

                        planillaQueryable = planillaQueryable.Where(l => l.nroNota == nroNota);

                    }


                    var planilla = await planillaQueryable
                        .Include(e => e.estadoNavegacion)
                        .Include(uC => uC.usuarioCreaNavegacion)
                        .Include(uM => uM.usuarioModificaNavegacion)
                        .AsNoTracking()
                        .Where(a => a.unidadCreacion == unidad && a.activo == true)
                        .OrderBy(ordenar => ordenar.fechaExpte)
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


        private bool ExpedienteExists(int id)
        {
            return _context.Expediente.Any(e => e.id == id);
        }



        // GET: api/Expediente/5
        [HttpGet("nroInterno/{unidad}")]
        public async Task<ActionResult<Expediente>> GetExpedienteNroInterno(int unidad)
        {
            using (var DBcontext = _context)
            {
                string annoAtual = DateTime.Now.Year.ToString();
                string Atual = annoAtual.Substring(annoAtual.Length - 2);

                try
                {
                    var obj = DBcontext.Expediente

                        .OrderByDescending(r => r.id)
                        .Where(r => r.unidadCreacion == unidad)
                        .FirstOrDefault();

                    if (obj != null)
                    {

                        string[] datoscortados = obj.numerointerno.Split('-');
                        var interno = datoscortados[0];

                        var anno = datoscortados[1];

                        int suma = 0;
                        var internoNuevo = "";
                        if (anno.Equals(Atual))
                        {
                            suma = Convert.ToInt32(interno) + 1;
                            internoNuevo = Convert.ToString(suma) + "-" + anno;
                        }
                        else
                        {

                            internoNuevo = "1-" + Atual;
                        }


                        if (obj != null)

                            res.dato = internoNuevo;
                        res.code = "200";
                        res.message = "Dato obtenido correctamente";
                    }
                    else if (obj is null)
                    {
                        res.dato = "510-23";
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
    } 
}


