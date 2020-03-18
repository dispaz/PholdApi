using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PholdApi.Filters
{
    public class SwaggerImageOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var fileParams = context.MethodInfo.GetParameters()
                .Where(p => p.ParameterType.FullName.Equals(typeof(Microsoft.AspNetCore.Http.IFormFile).FullName));
            if (fileParams.Any() && fileParams.Count() == 1)
            {
                operation.Parameters = new List<OpenApiParameter>
                {
                    new OpenApiParameter
                    {
                        Name = fileParams.First().Name,
                        Required = true
                        
                    }
                };
            }
        }
    }
}
