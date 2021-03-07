using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace TFramework.UI
{
    public class ViewBase
    {
        public GameObject gameObject;

        public Transform transform;

        public CompositeDisposable Disposables;
        public virtual void Init(GameObject gameObject) 
        {
            transform = gameObject.transform;
            Disposables = new CompositeDisposable();
        }
    }
}
