using System;
using System.Collections.Generic;
using System.Reflection;

namespace ExtendedLibrary
{
    /// <summary>
    /// source: https://github.com/Thundernerd/Unity3D-ExtendedEvent
    /// </summary>
    public static class TypeExtension
    {
        public const string UNITY_NAMESPACE = "Unity";
        public static readonly Type ListType = typeof(List<>);
        public static readonly Type ObsoleteAttributeType = typeof(ObsoleteAttribute);
        public static readonly Type UnityObjectType = typeof(UnityEngine.Object);
        public static readonly Type ComponentType = typeof(UnityEngine.Component);

        private static readonly List<Type> NotSupportedTypes = new List<Type> {
            typeof(Type),
            typeof(object)
        };

        public static bool IsSet(this Enum input, Enum value)
        {
            return (Convert.ToInt32(input) & Convert.ToInt32(value)) != 0;
        }

        public static bool Is(this Type value, ObjectType type)
        {
            return value.GetSerializableObjectType() == type;
        }

        public static string GetSerializableAssemblyQualifiedName(this Type type)
        {
            if (type.IsAbstract || type.IsInterface || NotSupportedTypes.Contains(type))
                return string.Empty;

            if (type.IsArray)
            {
                var elementName = INTERNAL_GetTypeFullName(type.GetElementType());

                return (string.IsNullOrEmpty(elementName) ? string.Empty : type.AssemblyQualifiedName);
            }

            if (type.IsGenericType)
            {
                if (type.GetGenericTypeDefinition() != ListType)
                    return string.Empty;

                var genericArgs = type.GetGenericArguments();

                if (genericArgs.Length != 1)
                    return string.Empty;

                var genericName = INTERNAL_GetTypeFullName(genericArgs[0]);

                return (string.IsNullOrEmpty(genericName) ? string.Empty : type.AssemblyQualifiedName);
            }

            return INTERNAL_GetTypeFullName(type);
        }

        public static ObjectType GetSerializableObjectType(this Type type)
        {
            if (type.IsAbstract || type.IsInterface || NotSupportedTypes.Contains(type))
                return ObjectType.Void;

            if (type.IsArray)
            {
                var elementType = INTERNAL_ObjectType(type.GetElementType());

                return (elementType == ObjectType.Void ? ObjectType.Void : ObjectType.Array);
            }

            if (type.IsGenericType)
            {
                if (type.GetGenericTypeDefinition() != ListType)
                    return ObjectType.Void;

                var genericArgs = type.GetGenericArguments();

                if (genericArgs.Length != 1)
                    return ObjectType.Void;

                var genericType = INTERNAL_ObjectType(genericArgs[0]);

                return (genericType == ObjectType.Void ? ObjectType.Void : ObjectType.List);
            }

            return INTERNAL_ObjectType(type);
        }

        public static string GetNormalTypeName(this Type type)
        {
            var builtInTypeName = INTERNAL_GetNormalBuiltInTypeName(type);

            if (!string.IsNullOrEmpty(builtInTypeName))
                return builtInTypeName;

            return type.Name;
        }

        public static string GetSerializableName(this Type type)
        {
            if (type.IsAbstract || type.IsInterface || NotSupportedTypes.Contains(type))
                return string.Empty;

            if (type.IsArray)
            {
                var elementName = INTERNAL_GetTypeName(type.GetElementType());

                return (string.IsNullOrEmpty(elementName) ? string.Empty : string.Format("{0}[]", elementName));
            }

            if (type.IsGenericType)
            {
                if (type.GetGenericTypeDefinition() != ListType)
                    return string.Empty;

                var genericArgs = type.GetGenericArguments();

                if (genericArgs.Length != 1)
                    return string.Empty;

                var genericName = INTERNAL_GetTypeName(genericArgs[0]);

                return (string.IsNullOrEmpty(genericName) ? string.Empty : string.Format("List<{0}>", genericName));
            }

            return INTERNAL_GetTypeName(type);
        }

        private static string INTERNAL_GetTypeFullName(Type type)
        {
            if (type.IsEnum)
            {
                return type.AssemblyQualifiedName;
            }

            var code = Type.GetTypeCode(type);

            switch (code)
            {
                case TypeCode.Boolean:
                case TypeCode.Char:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.String:
                    return type.AssemblyQualifiedName;
            }

            if (!type.IsPrimitive)
            {
                if (type.IsGenericType || type.IsArray)
                {
                    return string.Empty;
                }

                if (type.Assembly.FullName.Contains(UNITY_NAMESPACE))
                {
                    try
                    {
                        var result = Enum.Parse(typeof(ObjectType), type.Name);

                        if (result != null && result is ObjectType)
                            return type.AssemblyQualifiedName;
                    }
                    catch { }
                }

                if ((type.Attributes & TypeAttributes.Serializable) == TypeAttributes.Serializable)
                {
                    if (type.IsSubclassOf(UnityObjectType))
                    {
                        return type.AssemblyQualifiedName;
                    }

                    if (type.GetConstructor(Type.EmptyTypes) == null)
                        return string.Empty;

                    return type.AssemblyQualifiedName;
                }
            }

            return string.Empty;
        }

        private static ObjectType INTERNAL_ObjectType(Type type)
        {
            if (type.IsEnum)
            {
                return ObjectType.Enum;
            }

            var code = Type.GetTypeCode(type);

            switch (code)
            {
                case TypeCode.Boolean:
                    return ObjectType.Boolean;

                case TypeCode.Char:
                    return ObjectType.Char;

                case TypeCode.SByte:
                    return ObjectType.SByte;

                case TypeCode.Byte:
                    return ObjectType.Byte;

                case TypeCode.Int16:
                    return ObjectType.Int16;

                case TypeCode.UInt16:
                    return ObjectType.UInt16;

                case TypeCode.Int32:
                    return ObjectType.Int32;

                case TypeCode.UInt32:
                    return ObjectType.UInt32;

                case TypeCode.Int64:
                    return ObjectType.Int64;

                case TypeCode.UInt64:
                    return ObjectType.UInt64;

                case TypeCode.Single:
                    return ObjectType.Single;

                case TypeCode.Double:
                    return ObjectType.Double;

                case TypeCode.String:
                    return ObjectType.String;
            }

            if (!type.IsPrimitive)
            {
                if (type.IsGenericType || type.IsArray)
                {
                    return ObjectType.Void;
                }

                if (type.Assembly.FullName.Contains(UNITY_NAMESPACE))
                {
                    try
                    {
                        var result = (ObjectType)Enum.Parse(typeof(ObjectType), type.Name);

                        return result;
                    }
                    catch
                    { }
                }

                if ((type.Attributes & TypeAttributes.Serializable) == TypeAttributes.Serializable)
                {
                    if (type.IsSubclassOf(UnityObjectType))
                    {
                        return ObjectType.UnityObject;
                    }

                    if (type.GetConstructor(Type.EmptyTypes) == null)
                        return ObjectType.Void;

                    return ObjectType.SerializableType;
                }
            }

            return ObjectType.Void;
        }

        private static string INTERNAL_GetTypeName(Type type)
        {
            if (type.IsEnum)
            {
                return type.Name;
            }

            var builtInTypeName = INTERNAL_GetNormalBuiltInTypeName(type);

            if (!string.IsNullOrEmpty(builtInTypeName))
                return builtInTypeName;

            if (!type.IsPrimitive)
            {
                if (type.IsGenericType || type.IsArray)
                {
                    return string.Empty;
                }

                if (type.Assembly.FullName.Contains(UNITY_NAMESPACE))
                {
                    try
                    {
                        var result = Enum.Parse(typeof(ObjectType), type.Name);

                        if (result != null && result is ObjectType)
                            return type.Name;
                    }
                    catch { }
                }

                if ((type.Attributes & TypeAttributes.Serializable) == TypeAttributes.Serializable)
                {
                    if (type.IsSubclassOf(UnityObjectType))
                    {
                        return type.Name;
                    }

                    if (type.GetConstructor(Type.EmptyTypes) == null)
                        return string.Empty;

                    return type.Name;
                }
            }

            return string.Empty;
        }

        private static string INTERNAL_GetNormalBuiltInTypeName(Type type)
        {
            var code = Type.GetTypeCode(type);

            switch (code)
            {
                case TypeCode.Boolean:
                    return "bool";

                case TypeCode.Char:
                    return "char";

                case TypeCode.SByte:
                    return "sbyte";

                case TypeCode.Byte:
                    return "byte";

                case TypeCode.Int16:
                    return "short";

                case TypeCode.UInt16:
                    return "ushort";

                case TypeCode.Int32:
                    return "int";

                case TypeCode.UInt32:
                    return "uint";

                case TypeCode.Int64:
                    return "long";

                case TypeCode.UInt64:
                    return "ulong";

                case TypeCode.Single:
                    return "float";

                case TypeCode.Double:
                    return "double";

                case TypeCode.String:
                    return "string";

                default:
                    return string.Empty;
            }
        }
    }
}
