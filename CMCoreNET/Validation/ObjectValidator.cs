using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CMCoreNET.Validation
{
    public static class ObjectValidator
    {
        public static bool Validate(object model)
        {
            bool isValid = true;

            try
            {
                Validator.ValidateObject(model, new ValidationContext(model, null, null), true);
            }
            catch 
            { 
                isValid = false; 
            }
            
            return isValid;
        }
    }
}
