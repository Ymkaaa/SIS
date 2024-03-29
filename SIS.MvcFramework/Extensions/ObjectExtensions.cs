﻿using System.Xml.Serialization;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Serialization;
using SIS.MvcFramework.Mapping;

namespace SIS.MvcFramework.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToXml(this object obj)
        {
            using (StringWriter stringWriter = new StringWriter())
            {
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(stringWriter, obj);
                return stringWriter.ToString();
            }
        }
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }
    }
}
