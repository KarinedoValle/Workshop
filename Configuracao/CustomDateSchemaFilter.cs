using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;

public class CustomDateSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == typeof(List<DateTime>) && schema.Example == null)
        {
            var localNow = DateTime.Now;
            var exampleArray = new OpenApiArray
            {
                new OpenApiString(localNow.ToString("yyyy-MM-ddTHH:mm:ss"))
            };

            schema.Example = exampleArray;
        }

        if (schema.Properties != null && schema.Properties.ContainsKey("datas"))
        {
            var localNow = DateTime.Now;
            schema.Properties["datas"].Example = new OpenApiArray
            {
                new OpenApiString(localNow.ToString("yyyy-MM-ddTHH:mm:ss"))
            };
        }
    }
}
