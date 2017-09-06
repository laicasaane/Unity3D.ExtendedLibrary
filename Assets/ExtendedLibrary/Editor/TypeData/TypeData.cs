using System.Reflection;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace ExtendedLibrary
{
    [Serializable]
    public class TypeData
    {
        private const string GET = "get_";
        private const string SET = "set_";

        private const string FIELDS_PROPERTIES_FORMAT = "{0} : {1}";
        private const string METHODS_FORMAT = "{0} ({1})";

        private const string COMPONENT_FIELDS_LABEL_FORMAT = "{1}/Fields/{0}";
        private const string COMPONENT_PROPERTIES_LABEL_FORMAT = "{1}/Properties/{0}";
        private const string COMPONENT_METHODS_LABEL_FORMAT = "{1}/Methods/{0}";

        public static TypeData GetTypeData(Type type, BindingFlags flags)
        {
            var infos = new List<MemberInfo>();
            var shortLabels = new List<string>();
            var longLabels = new List<string>();

            var fields = (type.GetFields(flags)
                .Where(f => f.GetCustomAttributes(TypeExtension.ObsoleteAttributeType, true).Length == 0)).ToArray();

            var properties = (type.GetProperties(flags)
                .Where(p => p.CanWrite)
                .Where(p => p.GetCustomAttributes(TypeExtension.ObsoleteAttributeType, true).Length == 0)).ToArray();

            var methods = (type.GetMethods(flags)
                .Where(m => !m.Name.StartsWith(GET) && !m.Name.StartsWith(SET))
                .Where(m => m.GetCustomAttributes(TypeExtension.ObsoleteAttributeType, true).Length == 0)).ToArray();

            for (var i = 0; i < fields.Length; i++)
            {
                var typeName = fields[i].FieldType.GetSerializableName();

                if (string.IsNullOrEmpty(typeName))
                    continue;

                var shortLabel = string.Format(FIELDS_PROPERTIES_FORMAT, fields[i].Name, typeName);

                infos.Add(fields[i]);
                shortLabels.Add(shortLabel);
                longLabels.Add(string.Format(COMPONENT_FIELDS_LABEL_FORMAT, shortLabel, type.Name));
            }

            for (var i = 0; i < properties.Length; i++)
            {
                var typeName = properties[i].PropertyType.GetSerializableName();

                if (string.IsNullOrEmpty(typeName))
                    continue;

                var shortLabel = string.Format(FIELDS_PROPERTIES_FORMAT, properties[i].Name, typeName);

                infos.Add(properties[i]);
                shortLabels.Add(shortLabel);
                longLabels.Add(string.Format(COMPONENT_PROPERTIES_LABEL_FORMAT, shortLabel, type.Name));
            }

            for (var i = 0; i < methods.Length; i++)
            {
                string parameterTypes;

                if (!TryGetParameterTypes(methods[i], out parameterTypes))
                    continue;

                var shortLabel = string.Format(METHODS_FORMAT, methods[i].Name, parameterTypes);

                infos.Add(methods[i]);
                shortLabels.Add(shortLabel);
                longLabels.Add(string.Format(COMPONENT_METHODS_LABEL_FORMAT, shortLabel, type.Name));
            }

            var typeData = new TypeData(type.Name, type.FullName, type.AssemblyQualifiedName);
            typeData.members = new MemberData[infos.Count];

            for (var i = 0; i < infos.Count; i++)
            {
                var info = infos[i];
                var shortLabel = shortLabels[i];

                var memberData = new MemberData(info.Name);
                memberData.guiLabel = new GUIContent(shortLabel);
                memberData.longLabel = longLabels[i];
                memberData.parameters = new ParameterData[0];

                switch (info.MemberType)
                {
                    case MemberTypes.Field:
                        memberData.type = MemberType.Field;
                        var fieldInfo = info as FieldInfo;
                        memberData.returnType = fieldInfo.FieldType.GetSerializableObjectType();
                        memberData.returnTypeName = fieldInfo.FieldType.GetSerializableName();
                        memberData.assemblyQualifiedName = fieldInfo.FieldType.GetSerializableAssemblyQualifiedName();
                        break;

                    case MemberTypes.Property:
                        memberData.type = MemberType.Property;
                        var propertyInfo = info as PropertyInfo;
                        memberData.returnType = propertyInfo.PropertyType.GetSerializableObjectType();
                        memberData.returnTypeName = propertyInfo.PropertyType.GetSerializableName();
                        memberData.assemblyQualifiedName = propertyInfo.PropertyType.GetSerializableAssemblyQualifiedName();
                        break;

                    case MemberTypes.Method:
                        memberData.type = MemberType.Method;
                        var methodInfo = info as MethodInfo;
                        memberData.returnType = methodInfo.ReturnType.GetSerializableObjectType();
                        memberData.returnTypeName = methodInfo.ReturnType.GetNormalTypeName();
                        memberData.assemblyQualifiedName = methodInfo.ReturnType.AssemblyQualifiedName;

                        var parameters = methodInfo.GetParameters();
                        memberData.parameters = new ParameterData[parameters.Length];

                        for (var k = 0; k < parameters.Length; ++k)
                        {
                            var param = parameters[k];
                            var parameterData = new ParameterData(param.Name);
                            parameterData.typeName = param.ParameterType.GetSerializableName();
                            parameterData.returnType = param.ParameterType.GetSerializableObjectType();
                            parameterData.assemblyQualifiedName = param.ParameterType.GetSerializableAssemblyQualifiedName();

                            memberData.parameters[k] = parameterData;
                        }
                        break;
                }

                typeData.members[i] = memberData;
            }

            return typeData;
        }

        private static bool TryGetParameterTypes(MethodInfo info, out string parameterTypes)
        {
            if (info.IsGenericMethod)
            {
                parameterTypes = string.Empty;
                return false;
            }

            var parameters = info.GetParameters();

            if (parameters == null || parameters.Length <= 0)
            {
                parameterTypes = string.Empty;
                return true;
            }
            else
            {
                var typeNames = new string[parameters.Length];

                for (var i = 0; i < parameters.Length; ++i)
                {
                    var parameter = parameters[i];
                    var typeName = parameter.ParameterType.GetSerializableName();

                    if (string.IsNullOrEmpty(typeName))
                    {
                        parameterTypes = string.Empty;
                        return false;
                    }

                    typeNames[i] = typeName;
                }

                parameterTypes = string.Join(", ", typeNames);
                return true;
            }
        }

        public string name;
        public string fullName;
        public string assemblyQualifiedName;
        public MemberData[] members;

        public TypeData()
        {
        }

        public TypeData(string name, string fullName, string assemblyQualifiedName)
        {
            this.name = name;
            this.fullName = fullName;
            this.assemblyQualifiedName = assemblyQualifiedName;
        }
    }

    [Serializable]
    public class ParameterData
    {
        public string name;
        public string typeName;
        public ObjectType returnType;
        public string assemblyQualifiedName;

        public ParameterData()
        {
        }

        public ParameterData(string name)
        {
            this.name = name;
        }
    }

    [Serializable]
    public class MemberData
    {
        public string name;
        public MemberType type;
        public string returnTypeName;
        public ObjectType returnType;
        public string assemblyQualifiedName;
        public string longLabel;
        public GUIContent guiLabel;
        public ParameterData[] parameters;

        public MemberData()
        {
        }

        public MemberData(string name)
        {
            this.name = name;
        }
    }
}