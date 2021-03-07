using System.Collections;
using System.Collections.Generic;
using TFramework.Physics;
using UnityEngine;

public class NewBehaviourScript : ColliderBase
{
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        eventSystem.Publish(new TestEvent(10));
    }
}
