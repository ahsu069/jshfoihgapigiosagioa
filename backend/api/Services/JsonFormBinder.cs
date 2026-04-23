using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;
namespace api.Services
{
    public class JsonFormBinder<T> : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));
            var key = bindingContext.ModelName;
            if (string.IsNullOrEmpty(key))
                key = bindingContext.FieldName;
            var values = bindingContext.ValueProvider.GetValue(key);
            if (values == ValueProviderResult.None)
            {
                var form = bindingContext.HttpContext.Request.Form;
                if (form.ContainsKey(key))
                    values = new ValueProviderResult(form[key]);
            }
            if (values == ValueProviderResult.None)
            {
                var form = bindingContext.HttpContext.Request.Form;
                var prefix = bindingContext.ModelName;

                // If single object: detect barang_id / jumlah_bar keys without prefix
                var hasSingleObjectFields = form.Keys.Any(k => !k.Contains('[') &&
                    (k.Equals("barang_id", StringComparison.OrdinalIgnoreCase) ||
                    k.Equals("jumlah_bar", StringComparison.OrdinalIgnoreCase)));

                if (hasSingleObjectFields)
                {
                    // Manually construct JSON array for fallback
                    var dict = form.ToDictionary(k => k.Key, v => v.Value.ToString());
                    var obj = new Dictionary<string, object?>
                    {
                        { "barang_id", dict.ContainsKey("barang_id") ? dict["barang_id"] : null },
                        { "jumlah_bar", dict.ContainsKey("jumlah_bar") ? dict["jumlah_bar"] : null }
                    };

                    var json = JsonSerializer.Serialize(new[] { obj });
                    var result = JsonSerializer.Deserialize<T>(json);
                    bindingContext.Result = ModelBindingResult.Success(result);
                    return Task.CompletedTask;
                }
            }

            if (values.Length == 0)
            {
                bindingContext.Result = ModelBindingResult.Success(default(T));
                return Task.CompletedTask;
            }

            try
            {
                // Swagger sometimes sends multiple entries, each JSON string
                if (values.Length > 1)
                {
                    var list = new List<object>();
                    foreach (var val in values)
                    {
                        if (!string.IsNullOrWhiteSpace(val))
                        {
                            var obj = JsonSerializer.Deserialize<object>(val);
                            if (obj != null)
                                list.Add(obj);
                        }
                    }

                    // Rebuild as JSON array
                    var combinedJson = JsonSerializer.Serialize(list);
                    var result = JsonSerializer.Deserialize<T>(combinedJson);
                    bindingContext.Result = ModelBindingResult.Success(result);
                }
                else
                {
                    var rawValue = values.FirstValue;

                    // If it's already an array string "[{...}, {...}]"
                    if (rawValue!.TrimStart().StartsWith("["))
                    {
                        var result = JsonSerializer.Deserialize<T>(rawValue);
                        bindingContext.Result = ModelBindingResult.Success(result);
                    }
                    else
                    {
                        // Swagger often sends each item as object string, so wrap it
                        var singleObj = JsonSerializer.Deserialize<object>(rawValue);
                        var jsonArray = JsonSerializer.Serialize(new List<object> { singleObj! });
                        var result = JsonSerializer.Deserialize<T>(jsonArray);
                        bindingContext.Result = ModelBindingResult.Success(result);
                    }
                }
            }
            catch (Exception ex)
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, ex.Message);
                bindingContext.Result = ModelBindingResult.Failed();
            }

            return Task.CompletedTask;
        }
    }
}