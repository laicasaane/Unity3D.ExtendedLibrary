using ExtendedLibrary.Events;
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
}
