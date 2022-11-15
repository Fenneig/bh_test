using Character;
using Mirror;
using UnityEngine;

namespace Components
{
    public class DamageCheckComponent : NetworkBehaviour
    {
        [Command]
        public void ApplyDamage(GameObject target)
        {
            Player source = GetComponent<Player>();
            if (source == null) return;
            
            HealthComponent healthComponent = target.GetComponent<HealthComponent>();
            if (healthComponent == null || healthComponent.IsInvulnerable) return;
            
            source.Score++;
            healthComponent.GetHit();
        }
    }
}