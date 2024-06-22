using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EmployeesWhoWorkedTogether.Filters
{
    public class FileUploadOperation 
        : IOperationFilter
    {
        public void Apply(
            OpenApiOperation operation, 
            OperationFilterContext context)
        {
            var fileParams = context.ApiDescription.ParameterDescriptions
                .Where(p => p.ModelMetadata.ContainerType == typeof(IFormFile))
                .ToList();

            if (!fileParams.Any()) return;

            operation.Parameters.Clear();

            foreach (var fileParam in fileParams)
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = fileParam.Name,
                    In = ParameterLocation.Header,
                    Description = "Upload File",
                    Required = true,
                    Schema = new OpenApiSchema
                    {
                        Type = "string",
                        Format = "binary"
                    }
                });
            }
        }
    }
}
