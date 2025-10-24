using FluentValidation;
using SyC.Sorteo.Application.DTOs.Requests;

namespace SyC.Sorteo.Application.Validations
{
    public class InscripcionValidator : AbstractValidator<InscripcionRequest>
    {
        public InscripcionValidator()
        {
            RuleFor(x => x.TipoDocumento)
                .NotEmpty().WithMessage("El tipo de documento es obligatorio.");

            RuleFor(x => x.NumeroDocumento)
                .NotEmpty().WithMessage("El número de documento es obligatorio.");

            RuleFor(x => x.NombresApellidos)
                .NotEmpty().WithMessage("Los nombres y apellidos son obligatorios.");

            RuleFor(x => x.FechaNacimiento)
                .Must(f => f <= DateTime.Today.AddYears(-18))
                .WithMessage("Debe ser mayor de edad.");

            RuleFor(x => x.Correo)
                .NotEmpty().EmailAddress().WithMessage("Debe ingresar un correo válido.");

            RuleFor(x => x.Documento)
                .NotNull().WithMessage("Debe adjuntar un documento de identidad (PDF o imagen).");
        }
    }
}
