using MandrilAPI.Models;
using MandrilAPI.Services;
using Microsoft.AspNetCore.Mvc;

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
            return NotFound("El mandril solicitado no existe");
        }

        return Ok(mandril.Habilidades);
    }

    [HttpGet("{habilidadId}")]
    public ActionResult<Habilidad> GetHabilidad(int mandrilId, int habilidadId)
    {
        var mandril = MandrilDataStore.Current.Mandriles.FirstOrDefault(x => x.Id == mandrilId);
        if (mandril == null){
            return NotFound("El mandril solicitado no existe");
        }

        var habilidad = mandril.Habilidades?.FirstOrDefault(h => h.Id == habilidadId);
        if (habilidad ==  null)
        {
            return NotFound("La habilidad solicitada no existe.");
        }

        return Ok(habilidad);
    }   

    [HttpPost]
    public ActionResult<Habilidad> PostHabilidad(int mandrilId, HabilidadInsert habilidadInsert)
    {
        var mandril = MandrilDataStore.Current.Mandriles.FirstOrDefault(x => x.Id == mandrilId);
        if (mandril == null){
            return NotFound("El mandril solicitado no existe");
        }

        var habilidadExistente = mandril.Habilidades.FirstOrDefault(h => h.Nombre == habilidadInsert.Nombre);

        if (habilidadExistente != null)
        {
            return BadRequest("Ya existe otra habilidad con el mismo nombre.");
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
            return NotFound("El mandril solicitado no existe");
        }

        var habilidad = mandril.Habilidades?.FirstOrDefault(h => h.Id == habilidadId);
        if (habilidad == null){
            return BadRequest("No existe la habilidad solicitada");
        }

        var habilidadRepetida = mandril.Habilidades?.FirstOrDefault(h => h.Id != habilidadId && h.Nombre == habilidadInsert.Nombre);

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
            return BadRequest("No existe la habilidad solicitada");
        }
        
        mandril.Habilidades?.Remove(habilidad);

        return NoContent();
    }
}