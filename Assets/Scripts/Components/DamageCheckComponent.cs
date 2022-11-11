using Character;
using UnityEngine;

namespace Components
{
    public class DamageCheckComponent : MonoBehaviour
    {
        public void ApplyDamage(GameObject target)
        {
            Debug.Log($"{gameObject.GetComponentInParent<Player>().PlayerName} hit {target.name}");
        }
    }
}