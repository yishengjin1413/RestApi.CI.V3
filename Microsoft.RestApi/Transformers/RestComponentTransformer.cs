﻿namespace Microsoft.RestApi.Transformers
{
    using System.Linq;

    using Microsoft.OpenApi.Any;
    using Microsoft.OpenApi.Models;
    using Microsoft.RestApi.Models;
    using Newtonsoft.Json.Linq;

    public class RestComponentTransformer
    {
        public static ComponentEntity Transform(TransformModel transformModel)
        {
            var properties = TransformHelper.GetPropertiesFromSchema(transformModel.OpenApiSchema, transformModel.ComponentGroupId);
            var component = new ComponentEntity
            {
                Id = transformModel.ComponentId,
                Name = transformModel.ComponentName,
                Service = transformModel.ServiceName,
                GroupName = transformModel.ComponentGroupName,
                ApiVersion = transformModel.OpenApiDoc.Info.Version,
                Description = transformModel.OpenApiSchema.Description ?? transformModel.OpenApiSchema.Title,
                PropertyItems = properties.ToList(),
                Example = GetComponentExample(transformModel.OpenApiSchema)
            };

            return component;
        }

        private static string GetComponentExample(OpenApiSchema schema)
        {
            if(schema.Example == null)
            {
                return null;
            }
            
            return Utility.FormatJsonString(GetComponentExampleCore(schema.Example));
        }

        private static JToken GetComponentExampleCore(IOpenApiAny example)
        {
            if (example.AnyType == AnyType.Object)
            {
                var exampleObject = (OpenApiObject)example;
                JObject jObject = new JObject();
                foreach (var eo in exampleObject)
                {
                    var value = GetComponentExampleCore(eo.Value);
                    jObject.Add(new JProperty(eo.Key, value));
                }
                return jObject;
            }
            else if (example.AnyType == AnyType.Array)
            {
                var exampleArray = (OpenApiArray)example;
                JArray jArray = new JArray();
                foreach (var ea in exampleArray)
                {
                    jArray.Add(GetComponentExampleCore(ea));
                }
                return jArray;
            }
            else
            {
                return TransformHelper.GetValueFromPrimitiveType(example);
            }
        }
    }
}
