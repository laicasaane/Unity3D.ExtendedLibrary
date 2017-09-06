using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace ExtendedLibrary
{
    public class MemberList
    {
        private static TypeDataDictionary _typeDataDictionary = AssetDatabase.LoadAssetAtPath<TypeDataDictionary>(TypeDataDictionary.AssetPath);

        private static readonly List<string> _labels = new List<string>();
        private static readonly List<Object> _targets = new List<Object>();
        private static readonly List<TypeData> _dataList = new List<TypeData>();
        private static readonly List<MemberData> _members = new List<MemberData>();

        public Object primaryTarget { get; private set; }

        public string[] labels { get; private set; }

        public string returnTypeName { get; private set; }

        public BindingFlags flags { get; private set; }

        public readonly List<Object> targets = new List<Object>();
        public readonly List<TypeData> dataList = new List<TypeData>();
        public readonly List<MemberData> members = new List<MemberData>();

        public MemberList(Object target, BindingFlags flags, string returnTypeName = null)
        {
            Initialize(target, flags);
        }

        public void Initialize(Object target, BindingFlags flags, string returnTypeName = null)
        {
            if (target == null)
            {
                return;
            }

            _labels.Clear();
            _targets.Clear();
            _dataList.Clear();
            _members.Clear();

            this.targets.Clear();
            this.dataList.Clear();
            this.members.Clear();

            this.primaryTarget = target;
            this.returnTypeName = returnTypeName;
            this.flags = flags;

            GetTypeData(target, ref flags);

            if (target is GameObject)
            {
                var components = ((GameObject)target).GetComponents<Component>();

                for (var i = 0; i < components.Length; i++)
                {
                    GetTypeData(components[i], ref flags);
                }
            }

            this.labels = _labels.ToArray();
            this.targets.AddRange(_targets);
            this.dataList.AddRange(_dataList);
            this.members.AddRange(_members);
        }

        public MemberInfos GetInfosAt(int index)
        {
            if (index < 0 || index >= this.dataList.Count)
                return null;

            return new MemberInfos(this.targets[index], this.dataList[index], this.members[index]);
        }

        private void GetTypeData(Object target, ref BindingFlags flags)
        {
            if (_typeDataDictionary == null)
                return;

            var type = target.GetType();
            var data = _typeDataDictionary.GetValue(type, flags);

            if (data == null)
            {
                data = TypeData.GetTypeData(type, flags);
            }

            for (var i = 0; i < data.members.Length; i++)
            {
                var member = data.members[i];

                if (!string.IsNullOrEmpty(this.returnTypeName))
                {
                    if (!member.returnTypeName.Equals(this.returnTypeName))
                        continue;
                }

                _targets.Add(target);
                _dataList.Add(data);
                _members.Add(member);
                _labels.Add(member.longLabel);
            }
        }
    }
}
