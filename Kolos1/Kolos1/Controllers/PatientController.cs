using System;
using Kolos1.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Kolos1.Controllers {

    [ApiController]
    [Route("api/patients")]
    public class PatientController : ControllerBase{
        private IPatientDbService _service;
        public IConfiguration Configuration { get; set; }

        public PatientController(IPatientDbService service, IConfiguration configuration) {
            this._service = service;
            this.Configuration = configuration;
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePatient(int id) {
            int code = _service.deletePatient(id);
            if (code == 200) {
                return Ok(code);
            } else {
                return BadRequest(code);
            }
        }
    }
}
