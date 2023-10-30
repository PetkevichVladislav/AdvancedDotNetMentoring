﻿using CartingService.API.Infrastructure.Swagger.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using static CartingService.API.Infrastructure.Swagger.Models.SwaggerConfig;

namespace CartingService.API.Infrastructure.Swagger.Filters
{
    internal class SwaggerParameterFilters : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            try
            {
                var maps = context.MethodInfo.GetCustomAttributes(true).OfType<MapToApiVersionAttribute>().SelectMany(attr => attr.Versions).ToList();
                var version = maps[0].MajorVersion;
                if (!context.ApiDescription.RelativePath.Contains("{version}"))
                {
                    switch (SwaggerConfig.CurrentVersioningMethod)
                    {
                        case VersioningType.CustomHeader:
                            operation.Parameters.Add(new OpenApiParameter { Name = SwaggerConfig.CustomHeaderParam, In = ParameterLocation.Header, Required = false, Schema = new OpenApiSchema { Type = "String", Default = new OpenApiString(version.ToString()) } });
                            break;
                        case VersioningType.QueryString:
                            operation.Parameters.Add(new OpenApiParameter { Name = SwaggerConfig.QueryStringParam, In = ParameterLocation.Query, Schema = new OpenApiSchema { Type = "String", Default = new OpenApiString(version.ToString()) } });
                            break;
                        case VersioningType.AcceptHeader:
                            operation.Parameters.Add(new OpenApiParameter { Name = "Accept", In = ParameterLocation.Header, Required = false, Schema = new OpenApiSchema { Type = "String", Default = new OpenApiString($"application/json;{SwaggerConfig.AcceptHeaderParam}=" + version.ToString()) } });
                            break;

                    }
                }

                var versionParameter = operation.Parameters.Single(p => p.Name == "version");
                if (versionParameter != null)
                {
                    operation.Parameters.Remove(versionParameter);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
