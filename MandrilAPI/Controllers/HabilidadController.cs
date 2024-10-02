using MandrilAPI.Models;
using MandrilAPI.Services;
using Microsoft.AspNetCore.Mvc;
using MandrilAPI.Helpers;

namespace MandrilAPI.Controllers;

[ApiController]
[Route("api/mandirl/{mandrilId}/[controller]")]
public class HabilidadController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Habilidad>> GetHabilidad(int mandrilId)
    {
        var mandril = MandrilDataStore.Current.Mandriles.FirstOrDefault(x => x.Id == mandrilId);
        if (mandril == null){
            return NotFound(Mensajes.Mandril.NotFound);
        }

        return Ok(mandril.Habilidades);
    }

    [HttpGet("{habilidadId}")]
    public ActionResult<Habilidad> GetHabilidad(int mandrilId, int habilidadId)
    {
        var mandril = MandrilDataStore.Current.Mandriles.FirstOrDefault(x => x.Id == mandrilId);
        if (mandril == null){
            return NotFound(Mensajes.Mandril.NotFound);
        }

        var habilidad = mandril.Habilidades?.FirstOrDefault(h => h.Id == habilidadId);
        if (habilidad ==  null)
        {
            return NotFound(Mensajes.Habilidad.NotFound);
        }

        return Ok(habilidad);
    }   

    [HttpPost]
    public ActionResult<Habilidad> PostHabilidad(int mandrilId, HabilidadInsert habilidadInsert)
    {
        var mandril = MandrilDataStore.Current.Mandriles.FirstOrDefault(x => x.Id == mandrilId);
        if (mandril == null){
            return NotFound(Mensajes.Mandril.NotFound);
        }

        var habilidadExistente = mandril.Habilidades.FirstOrDefault(h => h.Nombre == habilidadInsert.Nombre);

        if (habilidadExistente != null)
        {
            return BadRequest(Mensajes.Habilidad.NotFound);
        }

        var maxHabilidad = mandril.Habilidades.Max(h => h.Id);

        var habilidadNueva = new Habilidad(){
            Id = maxHabilidad + 1,
            Nombre = habilidadInsert.Nombre,
            Potencia = habilidadInsert.Potencia
        }; 

        mandril.Habilidades.Add(habilidadNueva);

        return CreatedAtAction(nameof(GetHabilidad),
        new { mandrilId = mandrilId, habilidadId = habilidadNueva.Id},
        habilidadNueva);
    }

    [HttpPut("{habilidadId}")]
    public ActionResult<Habilidad> PutHabilidad(int mandrilId, int habilidadId, HabilidadInsert habilidadInsert)
    {
        var mandril = MandrilDataStore.Current.Mandriles.FirstOrDefault(x => x.Id == mandrilId);
        if (mandril == null){
            return NotFound(Mensajes.Mandril.NotFound);
        }

        var habilidad = mandril.Habilidades?.FirstOrDefault(h => h.Id == habilidadId);
        if (habilidad == null){
            return BadRequest(Mensajes.Habilidad.NotFound);
        }

        var habilidadRepetida = mandril.Habilidades?.FirstOrDefault(h => h.Id != habilidadId && h.Nombre == habilidadInsert.Nombre);
        if (habilidadRepetida != null){
            return BadRequest(Mensajes.Habilidad.NombreExistente);
        }

        habilidad.Nombre = habilidadInsert.Nombre;
        habilidad.Potencia = habilidadInsert.Potencia;

        return NoContent();

    }

    [HttpDelete]
    public ActionResult<Habilidad> DeleteHabilidad(int mandrilId, int habilidadId)
    {
        var mandril = MandrilDataStore.Current.Mandriles.FirstOrDefault(x => x.Id == mandrilId);
        if (mandril == null){
            return NotFound("El mandril solicitado no existe");
        }

        var habilidad = mandril.Habilidades?.FirstOrDefault(h => h.Id == habilidadId);
        if (habilidad == null){
            return BadRequest(Mensajes.Mandril.NotFound);
        }
        
        mandril.Habilidades?.Remove(habilidad);

        return NoContent();
    }
}