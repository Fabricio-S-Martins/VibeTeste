using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibeTeste.Application.DTO;
using VibeTeste.Application.InterfacesServices;

namespace VibeTeste.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlacemarksController : ControllerBase
    {
        private readonly IPlacemarkService _placemarkService;
        private IValidator<PlacemarkFilterDTO> _placemarkValidator;

        public PlacemarksController(IPlacemarkService placemarkService, IValidator<PlacemarkFilterDTO> placemarkValidator)
        {
            _placemarkService = placemarkService;
            _placemarkValidator = placemarkValidator;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Lista placemarks existentes.")]
        [SwaggerResponse(200, "Lista encontrada.", typeof(PlacemarkFilterDTO))]
        [SwaggerResponse(400, "Parametros incorretos.")]
        [SwaggerResponse(404, "Lista não encontrada.")]
        public async Task<IActionResult> ListaPlacemarkJson([FromQuery] PlacemarkFilterDTO placemark)
        {
            ValidationResult validator = await _placemarkValidator.ValidateAsync(placemark);
            if (!validator.IsValid)
                return BadRequest(validator.Errors);

            var retornoPlacemark = await _placemarkService.FiltraPlacemarks(placemark);
            if (retornoPlacemark == null || !retornoPlacemark.Any())
                return NotFound("Placemark não encontrado.");

            return Ok(retornoPlacemark);
        }

        [HttpPost("export")]
        [SwaggerOperation(Summary = "Exporta placemark.")]
        [SwaggerResponse(200, "Placemark exportado.", typeof(PlacemarkFilterDTO))]
        [SwaggerResponse(400, "Parametros incorretos.")]
        [SwaggerResponse(404, "Placemark não encontrado.")]
        public async Task<IActionResult> ExportaPlacemark([FromBody] PlacemarkFilterDTO placemark)
        {
            ValidationResult validator = await _placemarkValidator.ValidateAsync(placemark);
            if (!validator.IsValid)
                return BadRequest(validator.Errors);

            var retornoPlacemark = await _placemarkService.FiltraPlacemarks(placemark);
            if (retornoPlacemark == null || !retornoPlacemark.Any())
                return NotFound("Placemark não encontrado.");

            var kmlDocument = await _placemarkService.ExportaPlacemark(retornoPlacemark);

            byte[] kmlBytes = Encoding.UTF8.GetBytes(kmlDocument.ToString());

            string fileName = "placemarks_export.kml";

            return File(kmlBytes, "application/vnd.google-earth.kml+xml", fileName);
        }

        [HttpGet("filters")]
        [SwaggerOperation(Summary = "Filtra placemark existente.")]
        [SwaggerResponse(200, "Placemark filtrado.", typeof(PlacemarkFilterDTO))]
        [SwaggerResponse(400, "Parametros incorretos.")]
        [SwaggerResponse(404, "Placemark não encontrado.")]
        public async Task<IActionResult> FiltraPlacemarksDisponiveis()
        {
            var retornoPlacemark = await _placemarkService.BuscaValoresUnicos();
            if (retornoPlacemark == null)
                return NotFound("Nenhum Placemark foi encontrado.");

            return Ok(retornoPlacemark);
        }
    }
}