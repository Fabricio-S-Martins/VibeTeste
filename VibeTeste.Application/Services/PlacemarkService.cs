using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using VibeTeste.Application.DTO;
using VibeTeste.Application.InterfacesServices;
using VibeTeste.Domain.Entities;
using VibeTeste.Domain.InterfaceRepositories;

namespace VibeTeste.Application.Services
{
    public class PlacemarkService : IPlacemarkService
    {
        private readonly IPlacemarkRepository _placemarkRepository;
        private readonly IMapper _entityToDTO;

        public PlacemarkService(IPlacemarkRepository placemarkRepository, IMapper entityToDTO)
        {
            _placemarkRepository = placemarkRepository;
            _entityToDTO = entityToDTO;
        }

        public async Task<IEnumerable<PlacemarkFilterDTO>> FiltraPlacemarks(PlacemarkFilterDTO placemarkDto)
        {
            IEnumerable<PlacemarkEntity> placemarks = await _placemarkRepository.BuscaPlacemark();

            var placemarkDTOs = placemarks.Where(p => string.IsNullOrEmpty(placemarkDto.Cliente) ||
                                                      string.Equals(p.Cliente, placemarkDto.Cliente, StringComparison.OrdinalIgnoreCase))

                                          .Where(p => string.IsNullOrEmpty(placemarkDto.Situacao) ||
                                                      string.Equals(p.Situacao, placemarkDto.Situacao, StringComparison.OrdinalIgnoreCase))

                                          .Where(p => string.IsNullOrEmpty(placemarkDto.Bairro) ||
                                                      string.Equals(p.Bairro, placemarkDto.Bairro, StringComparison.OrdinalIgnoreCase))

                                          .Where(p => string.IsNullOrEmpty(placemarkDto.Referencia) ||
                                                      (placemarkDto.Referencia.Length >= 3 &&
                                                       p.Referencia != null &&
                                                       p.Referencia.Contains(placemarkDto.Referencia, StringComparison.OrdinalIgnoreCase)))

                                          .Where(p => string.IsNullOrEmpty(placemarkDto.RuaCruzamento) ||
                                                      (placemarkDto.RuaCruzamento.Length >= 3 &&
                                                       p.RuaCruzamento != null &&
                                                       p.RuaCruzamento.Contains(placemarkDto.RuaCruzamento, StringComparison.OrdinalIgnoreCase)));

            return placemarkDTOs.Select(placemark => _entityToDTO.Map<PlacemarkDTO>(placemark));
        }
        public async Task<object> BuscaValoresUnicos()
        {
            var placemarks = await _placemarkRepository.BuscaPlacemark();

            var clientesUnicos = placemarks
                .Where(p => !string.IsNullOrEmpty(p.Cliente))
                .Select(c => c.Cliente)
                .Distinct()
                .OrderBy(c => c);

            var situacoesUnicas = placemarks
                .Where(p => !string.IsNullOrEmpty(p.Situacao))
                .Select(s => s.Situacao)
                .Distinct()
                .OrderBy(s => s);

            var bairrosUnicos = placemarks
                .Where(p => !string.IsNullOrEmpty(p.Bairro))
                .Select(b => b.Bairro)
                .Distinct()
                .OrderBy(b => b);

            return new ValoresUnicosDto
            {
                Clientes = clientesUnicos,
                Situacoes = situacoesUnicas,
                Bairros = bairrosUnicos
            };
        }

        public async Task<XDocument> ExportaPlacemark(IEnumerable<PlacemarkFilterDTO> placemarks)
        {
            if (placemarks == null || !placemarks.Any())
                throw new ArgumentException("Nenhum placemark foi fornecido para exportação.");

            var placemarkDTOs = placemarks.Select(placemark => _entityToDTO.Map<PlacemarkEntity>(placemark));
            return await _placemarkRepository.ExportarKml(placemarkDTOs);
        }
    }
}
