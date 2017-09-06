using UnityEngine;

public class NewFieldsExample : MonoBehaviour
{
    public byte byteField = byte.MaxValue;

    public sbyte sbyteField = sbyte.MinValue;

    public short shortField = short.MinValue;

    public ushort ushortField = ushort.MaxValue;

    public uint uintField = uint.MaxValue;

    public long longField = long.MaxValue;

    public ulong ulongField = ulong.MaxValue;

    public char charField = 'A';

    public Vector4 vector4Field = Vector4.one;

    public Quaternion quaternionField = Quaternion.identity;

    public Matrix4x4 matrix4x4Field = Matrix4x4.identity;
}
