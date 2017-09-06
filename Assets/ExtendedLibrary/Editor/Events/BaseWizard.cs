using System;
using System.Reflection;
using System.Reflection.Emit;
using UnityEditor;
using UnityEngine;

namespace ExtendedLibrary.Events
{
    public abstract class BaseWizard<TAction> : ScriptableWizard
    {
        public string path;
        public SerializedProperty serializedProperty;

        public Action<TAction> onClose;

        protected static Type CreateCloneObjectType(string name, string fieldName, Type fieldType)
        {
            var baseType = typeof(ScriptableObject);

            var appDomain = AppDomain.CurrentDomain;
            var assemblyName = new AssemblyName(baseType.Assembly.FullName);
            var assemblyBuilder = appDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);

            var typeBuilder = moduleBuilder.DefineType(name, TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.Serializable, baseType);
            var field = typeBuilder.DefineField(fieldName, fieldType, FieldAttributes.Public | FieldAttributes.HasDefault);

            var attributeConstructor = typeof(SerializeField).GetConstructor(new Type[0]);
            var attributeBuilder = new CustomAttributeBuilder(attributeConstructor, new object[0]);

            field.SetCustomAttribute(attributeBuilder);

            return typeBuilder.CreateType();
        }
    }
}
