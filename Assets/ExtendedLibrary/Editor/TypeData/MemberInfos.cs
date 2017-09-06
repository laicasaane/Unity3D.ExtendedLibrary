using UnityEngine;

namespace ExtendedLibrary
{
    public class MemberInfos
    {
        public readonly Object target;
        public readonly TypeData typeData;
        public readonly MemberData memberData;

        public MemberInfos(Object target, TypeData typeData, MemberData memberData)
        {
            this.target = target;
            this.typeData = typeData;
            this.memberData = memberData;
        }
    }
}
