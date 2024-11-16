using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using VibeTeste.Application.DTO;

namespace VibeTeste.Application.InterfacesServices
{
    public interface IPlacemarkService
    {
        Task<IEnumerable<PlacemarkFilterDTO>> FiltraPlacemarks(PlacemarkFilterDTO placemarkDto);
        Task<object> BuscaValoresUnicos();
        Task<XDocument> ExportaPlacemark(IEnumerable<PlacemarkFilterDTO> placemarks);
    }
}
