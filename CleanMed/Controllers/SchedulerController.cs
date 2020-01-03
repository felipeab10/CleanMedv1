using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanMed.Data;
using CleanMed.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanMed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulerController : ControllerBase
    {
        private readonly Contexto db;
        public SchedulerController(Contexto contexto)
        {
            db = contexto;
        }

        // GET: api/scheduler
        // GET: api/scheduler
        public IEnumerable<WebAPIEvent> Get(DateTime from, DateTime to)
        {
            return db.Agendamentos
               //.Where(e => e.HoraAgenda < to && e.HoraAgenda.AddMinutes(30) >= from)
               .ToList()
               .Select(e => (WebAPIEvent)e);
        }
       
    }
}