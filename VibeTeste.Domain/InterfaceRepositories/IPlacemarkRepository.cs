using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using VibeTeste.Domain.Entities;

namespace VibeTeste.Domain.InterfaceRepositories
{
    public interface IPlacemarkRepository
    {
        Task<IEnumerable<PlacemarkEntity>> BuscaPlacemark();
        Task<XDocument> ExportarKml(IEnumerable<PlacemarkEntity> placemarks);
    }
}
