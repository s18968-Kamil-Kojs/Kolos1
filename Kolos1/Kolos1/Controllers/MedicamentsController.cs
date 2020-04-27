using System;
using System.Collections.Generic;
using Kolos1.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Kolos1.Controllers {

    [ApiController]
    [Route("api/medicaments")]
    public class MedicamentsController : ControllerBase{
        private IMedicamentDbService _service;
        public IConfiguration Configuration { get; set; }

        public MedicamentsController(IMedicamentDbService service, IConfiguration configuration) {
            this._service = service;
            this.Configuration = configuration;
        }

        [HttpGet("{id}")]
        public IActionResult GetMedicaments(int id) {
            List<string> list = _service.getMedicament(id);
            if(list != null) {
                return Ok(list);
            } else {
                return BadRequest("Blad: " + 400);
            }
        }
    }
}
