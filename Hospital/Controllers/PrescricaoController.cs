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
    [Route("api/Prescricao")]
    [Authorize]
    public class PrescricaoController : Controller
    {
        private readonly HospitalContext _context;

        public PrescricaoController(HospitalContext context)
        {
            _context = context;
        }

        // GET: api/Prescricao
        [HttpGet]
        public IEnumerable<Prescricao> GetPrescricoes()
        {
            return _context.Prescricoes
                           .Include(Prescricao => Prescricao.Dosagens).ToList();
        }

        // GET: api/Prescricao/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPrescricao([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var prescricao = await _context.Prescricoes.SingleOrDefaultAsync(m => m.ID == id);

            if (prescricao == null)
            {
                return NotFound();
            }

            return Ok(prescricao);
        }

        // PUT: api/Prescricao/5
        [Authorize(Roles = "Medico")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPrescricao([FromRoute] int id, [FromBody] Prescricao prescricao)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != prescricao.ID)
            {
                return BadRequest();
            }
            if(prescricao.liberada){
                Prescricao prescricaoPopulada = populaListaDeDoses(prescricao);
            }
            /*var prescricaoDB =  _context.Prescricoes
                                             .Include(p=>p.Dosagens)
                                             .SingleOrDefaultAsync(m => m.ID == id);
            _context.
            //_context.Entry(prescricaoDB).State = EntityState.Detached;
            //prescricaoDB.Dosagens.Clear();
            //prescricaoDB.Dosagens.AddRange(prescricao.Dosagens);
            //_context.Entry(prescricaoDB).State = EntityState.Modified;*/
            _context.Entry(prescricao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrescricaoExists(id))
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

        // POST: api/Prescricao
        [Authorize(Roles = "Medico")]
        [HttpPost]
        public async Task<IActionResult> PostPrescricao([FromBody] Prescricao prescricao)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Prescricoes.Add(prescricao);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPrescricao", new { id = prescricao.ID }, prescricao);
        }

        // DELETE: api/Prescricao/5
        [Authorize(Roles = "Medico")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrescricao([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var prescricao = await _context.Prescricoes.SingleOrDefaultAsync(m => m.ID == id);
            if (prescricao == null)
            {
                return NotFound();
            }
            _context.Prescricoes.Remove(prescricao);
            await _context.SaveChangesAsync();

            return Ok(prescricao);
        }

        // LIBERAR: api/Prescricao/Liberar/5
        [Authorize(Roles = "Medico")]
        [HttpPost]
        [Route("api/Prescricao/Liberar")]
        public Task<IActionResult> LiberarPrescricao([FromBody] Prescricao prescricao){
            Prescricao prescricaoPopulada = populaListaDeDoses(prescricao);

            return PostPrescricao(prescricao);
        }

        private Prescricao populaListaDeDoses(Prescricao prescricao){
            List<Dose> listaDoses = new List<Dose>();
            DateTime horarioDose;
            foreach (var dosagem in prescricao.Dosagens)
            {
                horarioDose = dosagem.HorarioDeInicio;
                while(prescricao.DataDeFim.Subtract(horarioDose).TotalHours > dosagem.Frequencia ){
                    horarioDose = horarioDose.AddHours(dosagem.Frequencia);
                    Dose dose = new Dose(); 
                    dose.DataHora = horarioDose;
                    listaDoses.Add(dose);
                }
                dosagem.Dose = listaDoses;
            }  

            return prescricao;
        }


        private bool PrescricaoExists(int id)
        {
            return _context.Prescricoes.Any(e => e.ID == id);
        }
    }
}