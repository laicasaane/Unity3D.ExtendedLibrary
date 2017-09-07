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
