using System.Collections;
using Mirror;
using UnityEngine;

namespace Components
{
    public class HealthComponent : NetworkBehaviour
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private GameObject _attackZone;
        
        [SerializeField] private float _invulnerableTime = 3f;

        private Color _previousColor;
        private bool _isInvulnerable;
        public bool IsInvulnerable => _isInvulnerable;

        public void GetHit()
        {
            _attackZone.SetActive(false);
            _previousColor = _renderer.material.color;
            StartCoroutine(EnableInvulnerable());
        }

        private IEnumerator EnableInvulnerable()
        {
            _isInvulnerable = true;
            _renderer.material.color = Color.red;
            yield return new WaitForSeconds(_invulnerableTime);
            _renderer.material.color = _previousColor;
            _isInvulnerable = false;
        }
    }
}