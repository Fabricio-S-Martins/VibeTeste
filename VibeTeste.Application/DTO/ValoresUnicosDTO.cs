using System.Collections.Generic;

namespace VibeTeste.Application.DTO
{
    public class ValoresUnicosDto
    {
        public IEnumerable<string> Clientes { get; set; }
        public IEnumerable<string> Situacoes { get; set; }
        public IEnumerable<string> Bairros { get; set; }
    }
}
