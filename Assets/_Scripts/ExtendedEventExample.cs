using ExtendedLibrary.Events;
using System.Collections;
using UnityEngine;

public class ExtendedEventExample : MonoBehaviour
{
    public ExtendedEvent onDoSomething;

    private void DoSomething()
    {
        Debug.Log("DoSomething");
    }

    private void DoSomething(int a)
    {
        Debug.LogFormat("DoSomething ({0})", a);
    }

    private void DoSomething(int a, string b)
    {
        Debug.LogFormat("DoSomething ({0}, {1})", a, b);
    }


    private void DoSomething(Matrix4x4 a, Vector4 b)
    {
        Debug.LogFormat("DoSomething ({0}, {1})", a, b);
    }

    private void DoSomething(ClassA a)
    {
        Debug.Log("DoSomething (ClassA)");
    }


    // Incorrectly shown in the Parameter editor
    //private void DoSomething(Matrix4x4[] a)
    //{
    //    Debug.LogFormat("DoSomething ({0})", a);
    //}

    private void Start()
    {
        StartCoroutine(OnStart());
    }

    private IEnumerator OnStart()
    {
        yield return new WaitForSeconds(5);

        this.onDoSomething.Invoke();
    }
}


[System.Serializable]
public class ClassA
{
    public int a;
    public Vector3[] arr;
    public ClassB[] brr;

    // Incorrectly shown in the Parameter editor
    //public Matrix4x4 m;
}

[System.Serializable]
public class ClassB
{
    public int a;
    public byte[] arr;

    // Incorrectly shown in the Parameter editor
    //public Matrix4x4 m;
}