using System;
using UnityEngine;
using FullSerializer;

namespace ExtendedLibrary
{
    public static class JsonConverter
    {
        private static fsSerializer serializer = new fsSerializer();

        public static object ToObject(this string json, Type type)
        {
            object result = null;

            try
            {
                if (!string.IsNullOrEmpty(json) && type != null)
                {
                    var data = fsJsonParser.Parse(json);
                    serializer.TryDeserialize(data, type, ref result);
                }

                return result;
            }
            catch
            {
                if (!string.IsNullOrEmpty(json) && type != null)
                    return JsonUtility.FromJson(json, type);

                return result;
            }
        }

        public static T ToObject<T>(this string json)
        {
            var result = default(T);

            try
            {
                if (!string.IsNullOrEmpty(json))
                {
                    var data = fsJsonParser.Parse(json);
                    serializer.TryDeserialize<T>(data, ref result);
                }

                return result;
            }
            catch
            {
                if (!string.IsNullOrEmpty(json))
                    return JsonUtility.FromJson<T>(json);

                return result;
            }
        }

        public static string ToJson(this object value, Type type)
        {
            try
            {
                fsData result;
                serializer.TrySerialize(type, value, out result);

                return fsJsonPrinter.CompressedJson(result);
            }
            catch
            {
                return JsonUtility.ToJson(value);
            }
        }

        public static string ToJson<T>(T value)
        {
            try
            {
                fsData result;
                serializer.TrySerialize(value, out result);

                return fsJsonPrinter.CompressedJson(result);
            }
            catch
            {
                return JsonUtility.ToJson(value);
            }
        }

        public static string ToJson<T>(this object value)
        {
            try
            {
                return ToJson((T) value);
            }
            catch
            {
                return JsonUtility.ToJson(value);
            }
        }

        public static string ToJson(this object value)
        {
            try
            {
                return ToJson(value, value.GetType());
            }
            catch
            {
                return JsonUtility.ToJson(value);
            }
        }
    }
}
