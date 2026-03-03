//using Microsoft.AspNetCore.Http;
//using Microsoft.OpenApi;
//using Microsoft.OpenApi.Models;
//using Swashbuckle.AspNetCore.SwaggerGen;
//using System.Linq;

//namespace Rafiq.API.ServicesExtension
//{
//    public class FileUploadOperationFilter : IOperationFilter
//    {
//        public void Apply(OpenApiOperation operation, OperationFilterContext context)
//        {
//            var fileParams = context.MethodInfo.GetParameters()
//                .Where(p => p.ParameterType == typeof(IFormFile));

//            if (!fileParams.Any()) return;

//            operation.RequestBody = new OpenApiRequestBody
//            {
//                Content =
//                {
//                    ["multipart/form-data"] = new OpenApiMediaType
//                    {
//                        Schema = new OpenApiSchema
//                        {
//                            Type = JsonSchemaType.Object,
//                            Properties = fileParams.ToDictionary(
//                                p => p.Name!,
//                                p => new OpenApiSchema
//                                {
//                                    Type = JsonSchemaType.String,
//                                    Format = "binary"
//                                }
//                            ),
//                            Required = fileParams.Select(p => p.Name!).ToHashSet()
//                        }
//                    }
//                }
//            };
//        }
//    }
//}