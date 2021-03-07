using System.Collections;
using System.Collections.Generic;
using TFramework.UI;
using UnityEngine;
using UnityEngine.UI;


public class ViewTest : ViewBase
{
    public Text text;
    public override void Init(GameObject gameObject)
    {
        base.Init(gameObject);
        text = transform.Find("Text").GetComponent<Text>();
    }
}

