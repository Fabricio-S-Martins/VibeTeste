using System.Collections.Generic;

namespace VibeTeste.Domain.Entities
{
    public class PlacemarkEntity : PlacemarkFilterEntity
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Data { get; set; }
        public string Coordenadas { get; set; }
        public IEnumerable<string> Imagens { get; set; }
    }
}
