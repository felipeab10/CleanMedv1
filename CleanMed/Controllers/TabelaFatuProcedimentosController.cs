using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CleanMed.Data;
using CleanMed.Models;
using CleanMed.Servicos;
using Microsoft.AspNetCore.Authorization;

namespace CleanMed.Controllers
{
    [Authorize(Roles = "ADMINISTRADOR,FATURAMENTO")]
    public class TabelaFatuProcedimentosController : Controller
    {
        private readonly Contexto _context;

        public TabelaFatuProcedimentosController(Contexto context)
        {
            _context = context;
        }

        // GET: TabelaFatuProcedimentos
        public async Task<IActionResult> ValorProcedimento(int TabelaFaturamentoId, int? pageNumber, int searchId, int searchProcedimentoId, int ConvenioId)
        {
            ViewData["ConvenioId"] = ConvenioId;
            ViewData["TabelaFaturamentoId"] = TabelaFaturamentoId;
            var tabelaFaturamentoProcesso = from s in _context.TabelaFatuProcedimentos
                                            where s.TabelaFaturamentoId == TabelaFaturamentoId
                                select s;
            if (searchId > 0)
            {

                tabelaFaturamentoProcesso = tabelaFaturamentoProcesso.Where(s => s.TabelaFatuProcedimentoId == searchId);
            }
            if(searchProcedimentoId > 0)
            {
                tabelaFaturamentoProcesso = tabelaFaturamentoProcesso.Where(s => s.ProcedimentoId == searchProcedimentoId);
            }

            TempData["TabelaNome"] =  _context.TabelaFaturamentos.Where(a => a.TabelaFaturamentoId == TabelaFaturamentoId).Select(a => a.Descricao).First();
            ViewData["TabelaFaturamentoId"] = TabelaFaturamentoId;
            ViewData["ProcedimentoId"] = new SelectList(_context.Procedimentos, "ProcedimentoId", "Descricao");
            int pageSize = 5;
            return View(await PaginatedList<TabelaFatuProcedimento>.CreateAsync(tabelaFaturamentoProcesso.AsNoTracking().Include(a=> a.Procedimento).Include(a => a.TabelaFaturamento), pageNumber ?? 1, pageSize));
        }


        // GET: TabelaFatuProcedimentos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tabelaFatuProcedimento = await _context.TabelaFatuProcedimentos
                .Include(t => t.Procedimento)
                .Include(t => t.TabelaFaturamento)
                .FirstOrDefaultAsync(m => m.TabelaFatuProcedimentoId == id);
            if (tabelaFatuProcedimento == null)
            {
                return NotFound();
            }

            return View(tabelaFatuProcedimento);
        }

        // GET: TabelaFatuProcedimentos/Create
        public IActionResult Create(int TabelaFaturamentoId, int ConvenioId)
        {
            ViewData["ConvenioId"] = ConvenioId;
            ViewData["ProcedimentoId"] = new SelectList(_context.Procedimentos, "ProcedimentoId", "Descricao");
            ViewData["TabelaFaturamentoId"] = new SelectList(_context.TabelaFaturamentos, "TabelaFaturamentoId", "Descricao");
            ViewData["TabelaFaturamentoId"] = TabelaFaturamentoId;
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( TabelaFatuProcedimento tabelaFatuProcedimento,float ValorTotal, int ConvenioId)
        {
            ViewData["ConvenioId"] = ConvenioId;
            if (ModelState.IsValid)
            {
                var teste = ValorTotal;
               
                _context.Add(tabelaFatuProcedimento);
                TempData["Mensagem"] = "Adicionado com sucesso";
                await _context.SaveChangesAsync();
                return RedirectToAction("ValorProcedimento",new { TabelaFaturamentoid = tabelaFatuProcedimento.TabelaFaturamentoId,ConvenioId = ConvenioId});
            }
            ViewData["ProcedimentoId"] = new SelectList(_context.Procedimentos, "ProcedimentoId", "Descricao", tabelaFatuProcedimento.ProcedimentoId);
            ViewData["TabelaFaturamentoId"] = new SelectList(_context.TabelaFaturamentos, "TabelaFaturamentoId", "Descricao", tabelaFatuProcedimento.TabelaFaturamentoId);
            return View(tabelaFatuProcedimento);
        }

        // GET: TabelaFatuProcedimentos/Edit/5
        public async Task<IActionResult> Edit(int? id, int ConvenioId)
        {
            ViewData["ConvenioId"] = ConvenioId;
            if (id == null)
            {
                return NotFound();
            }

            var tabelaFatuProcedimento = await _context.TabelaFatuProcedimentos.FindAsync(id);
            if (tabelaFatuProcedimento == null)
            {
                return NotFound();
            }
           
            ViewData["ProcedimentoId"] = new SelectList(_context.Procedimentos.Where(a=> a.ProcedimentoId == tabelaFatuProcedimento.ProcedimentoId).ToList(), "ProcedimentoId", "Descricao");
            ViewData["TabelaFaturamentoId"] = new SelectList(_context.TabelaFaturamentos, "TabelaFaturamentoId", "Descricao", tabelaFatuProcedimento.TabelaFaturamentoId);
            ViewData["TabelaFaturamentoId"] = tabelaFatuProcedimento.TabelaFaturamentoId;
            return View(tabelaFatuProcedimento);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  TabelaFatuProcedimento tabelaFatuProcedimento,int ConvenioId)
        {
            ViewData["ConvenioId"] = ConvenioId;
            if (id != tabelaFatuProcedimento.TabelaFatuProcedimentoId)
            {
                return NotFound();
            }
            var dtMinimo = DateTime.Parse("01/01/1900");
           
            if (ModelState.IsValid)
            {
                try
                {
                    TempData["Mensagem"] = "Atualizado com sucesso";
                    _context.Update(tabelaFatuProcedimento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TabelaFatuProcedimentoExists(tabelaFatuProcedimento.TabelaFatuProcedimentoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("ValorProcedimento", new { TabelaFaturamentoid = tabelaFatuProcedimento.TabelaFaturamentoId, ConvenioId = ConvenioId });
            }
            ViewData["ProcedimentoId"] = new SelectList(_context.Procedimentos.Where(a => a.ProcedimentoId == tabelaFatuProcedimento.ProcedimentoId).ToList(), "ProcedimentoId", "Descricao");
            ViewData["TabelaFaturamentoId"] = new SelectList(_context.TabelaFaturamentos, "TabelaFaturamentoId", "Descricao", tabelaFatuProcedimento.TabelaFaturamentoId);
            return View(tabelaFatuProcedimento);
        }

        // GET: TabelaFatuProcedimentos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tabelaFatuProcedimento = await _context.TabelaFatuProcedimentos
                .Include(t => t.Procedimento)
                .Include(t => t.TabelaFaturamento)
                .FirstOrDefaultAsync(m => m.TabelaFatuProcedimentoId == id);
            if (tabelaFatuProcedimento == null)
            {
                return NotFound();
            }

            return View(tabelaFatuProcedimento);
        }

        // POST: TabelaFatuProcedimentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tabelaFatuProcedimento = await _context.TabelaFatuProcedimentos.FindAsync(id);
            _context.TabelaFatuProcedimentos.Remove(tabelaFatuProcedimento);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TabelaFatuProcedimentoExists(int id)
        {
            return _context.TabelaFatuProcedimentos.Any(e => e.TabelaFatuProcedimentoId == id);
        }
       
        [HttpPost]
        public JsonResult Procedimento(string Prefix)
        {
            var ListaProcedimentos = (from p in _context.Procedimentos.ToList()
                                      where p.Descricao.StartsWith(Prefix)
                                      select new { ProcedimentoId = p.ProcedimentoId ,Descricao = p.Descricao }
                                      );
            return Json(ListaProcedimentos);
        }
    
    [HttpGet]
    public async Task<IActionResult> Procedimento()
        {
            try
            {
                string term = HttpContext.Request.Query["term"].ToString();
                var names = _context.Procedimentos.Where(p => p.Descricao.Contains(term)).Select(p => new {ProcedimentoId = p.ProcedimentoId,Descricao = p.Descricao }).ToList();
                return Ok(names);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
