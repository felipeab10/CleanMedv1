using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CleanMed.Servicos
{
    public class CustomValidationDataAttribute: ValidationAttribute, IClientValidatable
    {
        public CustomValidationDataAttribute()
        {

        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value != null)
            {
                value = (DateTime)value;
                // This assumes inclusivity, i.e. exactly six years ago is okay
                if (DateTime.Now.AddYears(-6).CompareTo(value) <= 0 && DateTime.Now.CompareTo(value) >= 0)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("Date must be within the last six years!");
                }
            }
            return new ValidationResult("data em Branco");

        }


        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(
        ModelMetadata metadata,
        ControllerContext context)
        {
            var modelClientValidationRule = new ModelClientValidationRule
            {
                ValidationType = "data",
                ErrorMessage = this.FormatErrorMessage(metadata.DisplayName)
            };

            return new List<ModelClientValidationRule> { modelClientValidationRule };
        }
    }
}
