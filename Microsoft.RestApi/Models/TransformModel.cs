﻿namespace Microsoft.RestApi.Models
{
    using System.Collections.Generic;

    using Microsoft.OpenApi.Models;

    public class TransformModel
    {
        public OpenApiDocument OpenApiDoc { get; set; }

        public OpenApiTag OpenApiTag { get; set; }

        public KeyValuePair<OperationType, OpenApiOperation> Operation { get; set; }

        public string ServiceName { get; set; }

        public string GroupName { get; set; }

        public string OperationName { get; set; }

    }
}