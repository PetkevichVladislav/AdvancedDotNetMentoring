using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections;
using CartingService.API.Infrastructure.HATEOAS.Models;
using CartingService.BusinessLogicLayer.DTO;
using CartingService.API.Infrastructure.HATEOAS.Generators;

namespace CartingService.API.Infrastructure.HATEOAS
{
    public class JsonHateoasFormatter : OutputFormatter
    {
        private Dictionary<Type, Func<IUrlHelper, object, IEnumerable<Link>>> linkGenerators = new Dictionary<Type, Func<IUrlHelper, object, IEnumerable<Link>>>
        {
            { typeof(Cart), (urlHelper, cart) => new CartLinkGenerator(urlHelper).GenerateLink((Cart)cart) },
        };

        public JsonHateoasFormatter()
        {
            SupportedMediaTypes.Add("application/json+hateoas");
        }

        private T GetService<T>(OutputFormatterWriteContext context)
        {
            return (T)context.HttpContext.RequestServices.GetService(typeof(T));
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            var contextAccessor = GetService<IActionContextAccessor>(context);
            var urlHelperFactory = GetService<IUrlHelperFactory>(context);
            var actionDescriptorProvider = GetService<IActionDescriptorCollectionProvider>(context);
            var urlHelper = urlHelperFactory.GetUrlHelper(contextAccessor.ActionContext);
            var response = context.HttpContext.Response;

            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };

            if (context.Object is SerializableError error)
            {
                var errorOutput = JsonConvert.SerializeObject(error, serializerSettings);
                response.ContentType = "application/json";
                return response.WriteAsync(errorOutput);
            }
            
            var result = context.Object;
            if (context.ObjectType.GetInterfaces().Contains(typeof(IEnumerable)))
            {
                var listType = context.ObjectType.GetGenericArguments().First();

                result = ((IEnumerable<object>)result)
                    .Select(obj => CreateResource(urlHelper, obj));
            }
            else
            {
                result = CreateResource(urlHelper, context.Object);
            }


            var output = JsonConvert.SerializeObject(result, serializerSettings);
            response.ContentType = "application/json+hateoas";
            return response.WriteAsync(output);
        }

        private Resource CreateResource(IUrlHelper helper, object? obj)
        {
            IEnumerable<Link> links = linkGenerators.TryGetValue(obj?.GetType(), out var linkGenerator)
               ? linkGenerator(helper, obj)
               : Enumerable.Empty<Link>();

            return new Resource
            {
                Data = obj,
                Links = links,
            };
        }
    }
}
