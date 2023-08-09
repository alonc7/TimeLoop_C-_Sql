using System;
using System.Web.Mvc;

namespace TimeLoop_Api
{
    public class CustomDateModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            // Implement the logic to bind the model here
            var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (valueResult == null)
            {
                return null;
            }

            var dateValue = valueResult.AttemptedValue;
            if (!DateTime.TryParseExact(dateValue, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out var parsedDate))
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Invalid date format");
                return null;
            }

            return parsedDate;
        }
    }
}
