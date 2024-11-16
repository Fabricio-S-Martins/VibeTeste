using FluentValidation;
using VibeTeste.Application.DTO;

namespace VibeTeste.Application.ValidatorDTO
{
    public class PlacemarkFilterDTOValidator : AbstractValidator<PlacemarkFilterDTO>
    {
        public PlacemarkFilterDTOValidator()
        {
            RuleFor(placemarkDto => placemarkDto.Cliente).NotNull()
                                                         .WithMessage("Cliente não pode ser nulo");

            RuleFor(placemarkDto => placemarkDto.Bairro).NotNull()
                                                        .WithMessage("Bairro não pode ser nulo");

            RuleFor(placemarkDto => placemarkDto.Referencia).NotNull()
                                                            .WithMessage("Referencia não pode ser nulo")
                                                            .MinimumLength(3)
                                                            .WithMessage("Referencia deve ter no minimo 3 caracteres");

            RuleFor(placemarkDto => placemarkDto.RuaCruzamento).NotNull()
                                                               .WithMessage("RuaCruzamento não pode ser nulo")
                                                               .MinimumLength(3)
                                                               .WithMessage("RuaCruzamento deve ter no minimo 3 caracteres");
        }
    }
}
