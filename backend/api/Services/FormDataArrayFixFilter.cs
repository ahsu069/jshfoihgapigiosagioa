using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace api.Services
{
    public class FormDataArrayFixFilter: IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.RequestBody?.Content.ContainsKey("multipart/form-data") != true)
                return;

            var formData = operation.RequestBody.Content["multipart/form-data"];
            var schema = formData.Schema;

            if (schema.Properties == null)
                return;

            foreach (var prop in schema.Properties)
            {
                if (prop.Value.Type == "array")
                {
                    prop.Value.Items = new OpenApiSchema
                    {
                        Type = "object",
                        Properties = new Dictionary<string, OpenApiSchema>
                        {
                            ["barang_id"] = new OpenApiSchema { Type = "string" },
                            ["jumlah_bar"] = new OpenApiSchema { Type = "string" },
                        }
                    };
                }
            }
        }
    }
}