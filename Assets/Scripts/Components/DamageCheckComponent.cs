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
            {
                HealthComponent healthComponent = target.GetComponent<HealthComponent>();
                if (!healthComponent.IsInvulnerable)
                {
                    healthComponent.GetHit();
                    ScoreTable.Instance.AddPoint(gameObject.GetComponentInParent<NameComponent>().PlayerName);
                }
            }
        }
    }
}