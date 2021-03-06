﻿namespace Microsoft.RestApi.Models
{
    using System;

    using Newtonsoft.Json;
    using YamlDotNet.Serialization;

    [Serializable]
    public class IdentifiableEntity
    {
        [YamlMember(Alias = "uid", Order = -10)]
        public string Id { get; set; }
    }
}
