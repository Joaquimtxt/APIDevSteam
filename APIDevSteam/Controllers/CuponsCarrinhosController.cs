using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIDevSteam.Data;
using APIDevSteam.Models;

namespace APIDevSteam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuponsCarrinhosController : ControllerBase
    {
        private readonly ApiDevsteamContext _context;

        public CuponsCarrinhosController(ApiDevsteamContext context)
        {
            _context = context;
        }

        // GET: api/CuponsCarrinhos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CupomCarrinho>>> GetCuponsCarrinhos()
        {
            return await _context.CuponsCarrinhos.ToListAsync();
        }

        // GET: api/CuponsCarrinhos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CupomCarrinho>> GetCupomCarrinho(Guid id)
        {
            var cupomCarrinho = await _context.CuponsCarrinhos.FindAsync(id);

            if (cupomCarrinho == null)
            {
                return NotFound();
            }

            return cupomCarrinho;
        }

        // PUT: api/CuponsCarrinhos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCupomCarrinho(Guid id, CupomCarrinho cupomCarrinho)
        {
            if (id != cupomCarrinho.CupomCarrinhoId)
            {
                return BadRequest();
            }

            _context.Entry(cupomCarrinho).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CupomCarrinhoExists(id))
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

        // POST: api/CuponsCarrinhos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CupomCarrinho>> PostCupomCarrinho(CupomCarrinho cupomCarrinho)
        {
            _context.CuponsCarrinhos.Add(cupomCarrinho);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCupomCarrinho", new { id = cupomCarrinho.CupomCarrinhoId }, cupomCarrinho);
        }

        // DELETE: api/CuponsCarrinhos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCupomCarrinho(Guid id)
        {
            var cupomCarrinho = await _context.CuponsCarrinhos.FindAsync(id);
            if (cupomCarrinho == null)
            {
                return NotFound();
            }

            _context.CuponsCarrinhos.Remove(cupomCarrinho);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CupomCarrinhoExists(Guid id)
        {
            return _context.CuponsCarrinhos.Any(e => e.CupomCarrinhoId == id);
        }


        //[HttpPost]Aplicar cupom
        [HttpPost]
        [Route("AplicarCupom")]
        public async Task<IActionResult> AplicarCupom(Guid carrinhoId, Guid cupomId)
        {
            // Verifica se o carrinho existe
            var carrinho = await _context.Carrinhos.FindAsync(carrinhoId);
            if (carrinho == null)
            {
                return NotFound("Carrinho não encontrado.");
            }

            // Verifica se o cupom existe
            var cupom = await _context.Cupons.FindAsync(cupomId);
            if (cupom == null)
            {
                return NotFound("Cupom não encontrado.");
            }

            // Verifica se o cupom está ativo
            if (cupom.Ativo != true || (cupom.DataValidade.HasValue && cupom.DataValidade.Value < DateTime.Now))
            {
                return BadRequest("Cupom inválido ou expirado.");
            }

            // Verifica se o limite de uso foi atingido
            var usosDoCupom = await _context.CuponsCarrinhos.CountAsync(cc => cc.CupomId == cupomId);
            if (cupom.LimiteUso.HasValue && usosDoCupom >= cupom.LimiteUso.Value)
            {
                return BadRequest("Limite de uso do cupom atingido.");
            }

            // Calcula o valor total do carrinho considerando os descontos dos jogos
            var itensCarrinho = await _context.ItensCarrinhos.Where(i => i.CarrinhoId == carrinhoId).ToListAsync();
            if (!itensCarrinho.Any())
            {
                return BadRequest("Carrinho vazio.");
            }

            decimal valorTotalCarrinho = itensCarrinho.Sum(item => item.ValorTotal);

            // Aplica o desconto do cupom
            decimal desconto = (valorTotalCarrinho * cupom.Desconto) / 100;
            decimal valorComDesconto = valorTotalCarrinho - desconto;

            // Atualiza o valor total do carrinho
            carrinho.ValorTotal = valorComDesconto;

            // Salva a relação entre o cupom e o carrinho
            var cupomCarrinho = new CupomCarrinho
            {
                CarrinhoId = carrinhoId,
                CupomId = cupomId
            };
            _context.CuponsCarrinhos.Add(cupomCarrinho);

            // Salva as alterações no banco de dados
            _context.Entry(carrinho).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                ValorOriginal = valorTotalCarrinho,
                Desconto = desconto,
                ValorFinal = valorComDesconto
            });
        }
    }
