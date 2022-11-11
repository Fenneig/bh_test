using System;
using UnityEngine;
using UnityEngine.Events;

namespace Components
{
    public class EnterTriggerComponent : MonoBehaviour
    {
        [SerializeField] private string[] _tags;
        [SerializeField] private EnterEvent _action;

        private void OnTriggerEnter(Collider other)
        {
            foreach (var tag in _tags)
                if (other.gameObject.CompareTag(tag)) _action?.Invoke(other.gameObject);
        }

        [Serializable] public class EnterEvent : UnityEvent<GameObject> { }
    }
}