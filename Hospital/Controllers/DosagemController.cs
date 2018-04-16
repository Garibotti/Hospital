using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hospital.Model;
using Microsoft.AspNetCore.Authorization;

namespace Hospital.Controllers
{
    [Produces("application/json")]
    [Route("api/Dosagem")]
    [Authorize]
    public class DosagemController : Controller
    {
        private readonly HospitalContext _context;

        public DosagemController(HospitalContext context)
        {
            _context = context;
        }

        // GET: api/Dosagem
        [HttpGet]
        public IEnumerable<Dosagem> GetDosagens()
        {
            return _context.Dosagens
                           .Include(Dosagem => Dosagem.Dose).ToList();//Eager loading related data.
        }

        // GET: api/Dosagem/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDosagem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dosagem = await _context.Dosagens.SingleOrDefaultAsync(m => m.ID == id);

            if (dosagem == null)
            {
                return NotFound();
            }

            return Ok(dosagem);
        }

        // PUT: api/Dosagem/5
        [Authorize(Roles = "Medico")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDosagem([FromRoute] int id, [FromBody] Dosagem dosagem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != dosagem.ID)
            {
                return BadRequest();
            }

            _context.Entry(dosagem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DosagemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Dosagem
        [Authorize(Roles = "Medico")]
        [HttpPost]
        public async Task<IActionResult> PostDosagem([FromBody] Dosagem dosagem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Dosagens.Add(dosagem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDosagem", new { id = dosagem.ID }, dosagem);
        }

        // DELETE: api/Dosagem/5
        [Authorize(Roles = "Medico")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDosagem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dosagem = await _context.Dosagens.SingleOrDefaultAsync(m => m.ID == id);
            if (dosagem == null)
            {
                return NotFound();
            }

            _context.Dosagens.Remove(dosagem);
            await _context.SaveChangesAsync();

            return Ok(dosagem);
        }

        private bool DosagemExists(int id)
        {
            return _context.Dosagens.Any(e => e.ID == id);
        }
    }
}