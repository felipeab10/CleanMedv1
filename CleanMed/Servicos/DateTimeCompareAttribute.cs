using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CleanMed.Servicos
{
    public class DateTimeCompareAttribute :
        ValidationAttribute, IClientModelValidator
    {
        public string NameCompare { get; set; }
        public DateTimeCompareAttribute(string nameCompare)
        {
            NameCompare = nameCompare;
        }
        protected override ValidationResult IsValid(object value,
                                                    ValidationContext validationContext)
        {
            if (validationContext.ObjectInstance != null)
            {
                Type _t = validationContext.ObjectInstance.GetType();
                PropertyInfo _d = _t.GetProperty(NameCompare);
                if (_d != null)
                {
                    DateTime _dt1 = (DateTime)value;
                    DateTime _dt0 = (DateTime)_d
                         .GetValue(validationContext.ObjectInstance, null);
                    if (_dt1 != null &&
                        _dt0 != null &&
                        _dt0 <= _dt1)
                    {
                        return ValidationResult.Success;
                    }
                }
            }
            return new ValidationResult("Final Date is less than Initial Data");
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            context.Attributes.Add("data-val-datetimecompare", ErrorMessageString);
            context.Attributes.Add("data-val-datetimecompare-param", NameCompare);
        }
    }
    
    }

