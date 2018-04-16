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
    [Route("api/Dose")]
    [Authorize]
    public class DoseController : Controller
    {
        private readonly HospitalContext _context;

        public DoseController(HospitalContext context)
        {
            _context = context;
        }

        // GET: api/Dose
        [HttpGet]
        public IEnumerable<Dose> GetDoses()
        {
            return _context.Doses.Where(m => m.Administrada.Equals(false));
        }

        // GET: api/Dose/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDose([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dose = await _context.Doses.SingleOrDefaultAsync(m => m.ID == id && m.Administrada.Equals(false));

            if (dose == null)
            {
                return NotFound();
            }

            return Ok(dose);
        }

        // PUT: api/Dose/5
        [Authorize(Roles = "Enfermeira")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDose([FromRoute] int id, [FromBody] Dose dose)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != dose.ID)
            {
                return BadRequest();
            }

            _context.Entry(dose).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoseExists(id))
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

        // POST: api/Dose
        [HttpPost]
        public async Task<IActionResult> PostDose([FromBody] Dose dose)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Doses.Add(dose);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDose", new { id = dose.ID }, dose);
        }

        // DELETE: api/Dose/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDose([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dose = await _context.Doses.SingleOrDefaultAsync(m => m.ID == id);
            if (dose == null)
            {
                return NotFound();
            }

            _context.Doses.Remove(dose);
            await _context.SaveChangesAsync();

            return Ok(dose);
        }

        private bool DoseExists(int id)
        {
            return _context.Doses.Any(e => e.ID == id);
        }
    }
}