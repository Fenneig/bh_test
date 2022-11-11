using Character;
using UnityEngine;

namespace Components
{
    public class DamageCheckComponent : MonoBehaviour
    {
        public void ApplyDamage(GameObject target)
        {
            NameComponent targetNameComponent = target.GetComponent<NameComponent>();
            if (targetNameComponent != null)
                Debug.Log($"{gameObject.GetComponentInParent<NameComponent>().PlayerName} hit {targetNameComponent.PlayerName}");
        }
    }
}