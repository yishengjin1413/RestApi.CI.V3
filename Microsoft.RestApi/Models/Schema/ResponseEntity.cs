﻿namespace Microsoft.RestApi.Models
{
    using System;
    using System.Collections.Generic;

    using YamlDotNet.Serialization;

    [Serializable]
    public class ResponseEntity : NamedEntity
    {
        [YamlMember(Alias = "description")]
        public string Description { get; set; }

        [YamlMember(Alias = "statusCode")]
        public string StatusCode { get; set; }

        [YamlMember(Alias = "bodies")]
        public IList<ResponseMediaTypeAndBodyEntity> ResponseMediaTypeAndBodies { get; set; }

        [YamlMember(Alias = "headers")]
        public IList<ResponseHeaderEntity> ResponseHeaders { get; set; }

        [YamlIgnore]
        public IList<ResponseLinkEntity> ResponseLinks { get; set; }
    }

    public class ResponseMediaTypeAndBodyEntity : IdentifiableEntity
    {
        [YamlMember(Alias = "mediaType")]
        public string MediaType { get; set; }

        [YamlMember(Alias = "typesTitle")]
        public string TypesTitle { get; set; }

        [YamlMember(Alias = "types")]
        public IList<PropertyTypeEntity> ResponseBodyTypes { get; set; }

        [YamlMember(Alias = "schemas")]
        public IList<PropertyEntity> ResponseBodySchemas { get; set; }
    }
}
