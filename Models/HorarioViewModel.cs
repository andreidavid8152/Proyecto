using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Proyecto.Models
{
    public class HorarioViewModel
    {
        public int ID { get; set; }
        public int LocalID { get; set; }

        [Required(ErrorMessage = "La hora de inicio es requerida.")]
        public TimeSpan HoraInicio { get; set; }

        [Required(ErrorMessage = "La hora de fin es requerida.")]
        [GreaterThanStart("HoraInicio", ErrorMessage = "La hora de fin debe ser al menos 2 horas después de la hora de inicio.")]
        public TimeSpan HoraFin { get; set; }

        // Relaciones
        [JsonIgnore]
        public LocalViewModel? Local { get; set; }
    }

    public class GreaterThanStartAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public GreaterThanStartAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var startTime = (TimeSpan)validationContext.ObjectType.GetProperty(_comparisonProperty).GetValue(validationContext.ObjectInstance);
            var endTime = (TimeSpan)value;

            if (endTime - startTime < TimeSpan.FromHours(2))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
