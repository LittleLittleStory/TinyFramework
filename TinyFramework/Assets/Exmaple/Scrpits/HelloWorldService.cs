using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;

public class HelloWorldService :IService
{
    public List<int> test = new List<int>();
    public void Hello()
    {
        UnityEngine.Debug.Log("Hello world");
    }
}
