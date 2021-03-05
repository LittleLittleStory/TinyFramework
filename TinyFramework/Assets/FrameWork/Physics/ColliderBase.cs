using System.Collections;
using System.Collections.Generic;
using TFrameWork.Events;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TFrameWork.Physics
{
    public class ColliderBase : MonoBehaviour
    {
        [Inject]
        protected IEventSystem eventSystem;

        private void Start()
        {
            GameLanucher.Instance.Container.InjectGameObject(gameObject);
        }

        public virtual void OnTriggerEnter2D(Collider2D collision)
        {

        }

        public virtual void OnTriggerStay2D(Collider2D collision)
        {

        }

        public virtual void OnTriggerExit2D(Collider2D collision)
        {

        }
    }
}
