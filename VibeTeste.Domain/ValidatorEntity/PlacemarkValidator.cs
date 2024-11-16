using FluentValidation;
using VibeTeste.Domain.Entities;

namespace VibeTeste.Domain.ValidatorEntity
{
    public class PlacemarkValidator : AbstractValidator<PlacemarkEntity>
    {
        public PlacemarkValidator()
        {
            RuleFor(placemark => placemark.Cliente).NotNull()
                                                   .WithMessage("Cliente não pode ser nulo");

            RuleFor(placemark => placemark.Bairro).NotNull()
                                                   .WithMessage("Bairro não pode ser nulo");

            RuleFor(placemark => placemark.Referencia).NotNull()
                                                   .WithMessage("Referencia não pode ser nulo")
                                                   .MinimumLength(3)
                                                   .WithMessage("Referencia deve ter no minimo 3 caracteres");

            RuleFor(placemark => placemark.RuaCruzamento).NotNull()
                                                   .WithMessage("RuaCruzamento não pode ser nulo")
                                                   .MinimumLength(3)
                                                   .WithMessage("RuaCruzamento deve ter no minimo 3 caracteres");
        }
    }
}
