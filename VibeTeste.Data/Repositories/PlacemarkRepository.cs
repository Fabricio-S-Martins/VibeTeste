using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using VibeTeste.Domain.Entities;
using VibeTeste.Domain.InterfaceRepositories;

namespace VibeTeste.Data.Repositories
{
    public class PlacemarkRepository : IPlacemarkRepository
    {
        private readonly string _filePath;

        public PlacemarkRepository(string filePath)
        {
            _filePath = filePath;
        }

        public async Task<IEnumerable<PlacemarkEntity>> BuscaPlacemark()
        {
            XNamespace nameSpace = "http://www.opengis.net/kml/2.2";
            var document = XDocument.Load(_filePath);
            var kmlPercorrido = document.Descendants(nameSpace + "Placemark")
                .Select(p => new PlacemarkEntity
                 {
                     Nome = (string)p.Descendants(nameSpace + "name").FirstOrDefault(d => d != null)?.Value,
                     Descricao = (string)p.Descendants(nameSpace + "description").FirstOrDefault(d => d != null)?.Value,
                     Cliente = (string)p.Descendants(nameSpace + "Data").FirstOrDefault(d => (string)d.Attribute("name") == "CLIENTE")?.Element(nameSpace + "value"),
                     Situacao = (string)p.Descendants(nameSpace + "Data").FirstOrDefault(d => (string)d.Attribute("name") == "SITUAÇÃO")?.Element(nameSpace + "value"),
                     Bairro = (string)p.Descendants(nameSpace + "Data").FirstOrDefault(d => (string)d.Attribute("name") == "BAIRRO")?.Element(nameSpace + "value"),
                     Referencia = (string)p.Descendants(nameSpace + "Data").FirstOrDefault(d => (string)d.Attribute("name") == "REFERENCIA")?.Element(nameSpace + "value"),
                     RuaCruzamento = (string)p.Descendants(nameSpace + "Data").FirstOrDefault(d => (string)d.Attribute("name") == "RUA/CRUZAMENTO")?.Element(nameSpace + "value"),
                     Data = (string)p.Descendants(nameSpace + "Data").FirstOrDefault(d => (string)d.Attribute("name") == "DATA")?.Element(nameSpace + "value"),
                     Coordenadas = (string)p.Descendants(nameSpace + "Data").FirstOrDefault(d => (string)d.Attribute("name") == "COORDENADAS")?.Element(nameSpace + "value"),
                     Imagens = p.Descendants(nameSpace + "Data").Where(d => (string)d.Attribute("name") == "gx_media_links").Select(d => (string)d.Element(nameSpace + "value"))
                 });
            return kmlPercorrido;
        }

        public async Task<XDocument> ExportarKml(IEnumerable<PlacemarkEntity> placemarks)
        {
            XNamespace ns = "http://www.opengis.net/kml/2.2";
            var document = XDocument.Load(_filePath);

            XDocument kmlDoc = new XDocument(
                new XElement(ns + "kml",
                    new XElement(ns + "Document",
                        placemarks.Select(p =>
                        {
                            var placemarkElement = document.Descendants(ns + "Placemark")
                            .FirstOrDefault(x => x.Descendants(ns + "Data")
                                .Any(d => (string)d.Attribute("name") == "CLIENTE" && (string)d.Value.ToUpper() == p.Cliente.ToUpper()));

                            if (placemarkElement == null)
                                return null;

                            return new XElement(ns + "Placemark",
                            placemarkElement.Elements()
                        );
                        })
                        .Where(x => x != null)
                    )
                )
            );
            return kmlDoc;
        }
    }
}
