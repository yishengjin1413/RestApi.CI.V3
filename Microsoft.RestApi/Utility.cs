﻿namespace Microsoft.RestApi
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;
    using Microsoft.RestApi.Common;
    using Newtonsoft.Json;

    public static class Utility
    {
        private static readonly JsonSerializer JsonSerializer = new JsonSerializer
        {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.Indented
        };

        public static readonly Regex YamlHeaderRegex = new Regex(@"^\-{3}(?:\s*?)\n([\s\S]+?)(?:\s*?)\n\-{3}(?:\s*?)(?:\n|$)", RegexOptions.Compiled | RegexOptions.Singleline, TimeSpan.FromSeconds(10));
        public static readonly YamlDotNet.Serialization.Deserializer YamlDeserializer = new YamlDotNet.Serialization.Deserializer();
        public static readonly YamlDotNet.Serialization.Serializer YamlSerializer = new YamlDotNet.Serialization.Serializer();
        public static readonly string Pattern = @"(?:{0}|[A-Z]+?(?={0}|[A-Z][a-z]|$)|[A-Z](?:[a-z]*?)(?={0}|[A-Z]|$)|(?:[a-z]+?)(?={0}|[A-Z]|$))";
        public static readonly HashSet<string> Keyword = new HashSet<string> { "BI", "IP", "ML", "MAM", "OS", "VM", "VMs", "APIM", "vCenters", "oneNote" };

        public static object GetYamlHeaderByMeta(string filePath, string metaName)
        {
            var yamlHeader = GetYamlHeader(filePath);
            object result;
            if (yamlHeader != null && yamlHeader.TryGetValue(metaName, out result))
            {
                return result;
            }
            return null;
        }

        public static Dictionary<string, object> GetYamlHeader(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File path {filePath} not exists when parsing yaml header.");
            }

            var markdown = File.ReadAllText(filePath);

            var match = YamlHeaderRegex.Match(markdown);
            if (match.Length == 0)
            {
                return null;
            }

            // ---
            // a: b
            // ---
            var value = match.Groups[1].Value;
            try
            {
                using (StringReader reader = new StringReader(value))
                {
                    return YamlDeserializer.Deserialize<Dictionary<string, object>>(reader);
                }
            }
            catch (Exception)
            {
                Console.WriteLine();
                return null;
            }
        }

        public static void Serialize(string path, object obj)
        {
            using (var stream = File.Create(path))
            using (var writer = new StreamWriter(stream))
            {
                JsonSerializer.Serialize(writer, obj);
            }
        }

        public static string FirstLetterToLower(this string str)
        {
            if (str == null)
            {
                return null;
            }
            if (str.Length > 1)
            {
                return char.ToLower(str[0]) + str.Substring(1);
            }
            return str.ToLower();
        }

        public static string FirstLetterToUpper(this string str)
        {
            if (str == null)
            {
                return null;
            }
            if (str.Length > 1)
            {
                return char.ToUpper(str[0]) + str.Substring(1);
            }
            return str.ToUpper();
        }

        public static string ExtractPascalNameByRegex(string name)
        {
            if (name.Contains(" "))
            {
                return name;
            }
            if (name.Contains("_") || name.Contains("-"))
            {
                return name.Replace('_', ' ').Replace('-', ' ');
            }
            if (name.Contains("."))
            {
                name = name.Replace(".", "");
            }

            var result = new List<string>();
            var p = string.Format(Pattern, string.Join("|", Keyword));
            while (name.Length > 0)
            {
                var m = Regex.Match(name, p);
                if (!m.Success)
                {
                    return name;
                }
                result.Add(m.Value.ToLower());
                name = name.Substring(m.Length);
            }
            return string.Join(" ", result).FirstLetterToUpper();
        }

        public static string FormatJsonString(object jsonValue)
        {
            if (jsonValue == null)
            {
                return null;
            }
            try
            {
                return JsonUtility.ToIndentedJsonString(jsonValue).Replace("\r\n", "\n");
            }
            catch
            {
                return null;
            }
        }

        private static string Normalize(string path)
        {
            return string.IsNullOrEmpty(path) ? string.Empty : path.Replace(" ", "").Replace("..", ".").Trim('.').ToLower();
        }

        public static string GetId(string serviceName, string groupName, string operationName)
        {
            var id = $"{Normalize(serviceName)}.{Normalize(groupName)}.{Normalize(operationName)}";
            return Normalize(id);
        }

        public static string GetPath(string serviceName, string groupName, string operationName)
        {
            var id = GetId(serviceName, groupName, operationName);
            return Path.Combine(id.Split('.'));
        }
    }
}
