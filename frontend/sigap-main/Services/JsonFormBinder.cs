using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections;

namespace Lexa.Models
{
    public class JsonFormBinder<T> : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var modelName = bindingContext.ModelName;

            // Get the values from the form
            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                // No value found, return empty list
                bindingContext.Result = ModelBindingResult.Success(Activator.CreateInstance(typeof(System.Collections.Generic.List<T>)));
                return Task.CompletedTask;
            }

            try
            {
                var values = valueProviderResult.Values;
                IList list = (IList)Activator.CreateInstance(typeof(System.Collections.Generic.List<T>));

                foreach (var val in values)
                {
                    var obj = JsonSerializer.Deserialize<T>(val);
                    if (obj != null)
                        list.Add(obj);
                }

                bindingContext.Result = ModelBindingResult.Success(list);
            }
            catch (Exception ex)
            {
                bindingContext.ModelState.AddModelError(modelName, ex.Message);
            }

            return Task.CompletedTask;
        }
    }
}
