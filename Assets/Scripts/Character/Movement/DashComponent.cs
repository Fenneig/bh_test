using System.Collections;
using Mirror;
using UnityEngine;

namespace Character.Movement
{
    public class DashComponent : NetworkBehaviour
    {
        [Header("References")] 
        [SerializeField] private Transform _orientation;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private MovementComponent _playerMovement;
        [SerializeField] private GameObject _attack;

        [Header("Dashing")]
        [SerializeField] private float _dashForce;
        [SerializeField] private float _dashUpwardForce;
        [SerializeField] private float _maxDashSpeed;
        [SerializeField] private float _dashDuration;

        [Header("Cooldown")] [SerializeField] private float _dashCooldown;

        private float _dashCooldownTimer;
        private Vector3 _forceToApply;
        private float _speed;

        private void Start()
        {
            _speed = _playerMovement.Speed;
        }

        private void Update()
        {
            if (_dashCooldownTimer > 0) _dashCooldownTimer -= Time.deltaTime;
        }

        public void Dash()
        {
            if (_dashCooldownTimer > 0) return;
            _attack.SetActive(true);
            _dashCooldownTimer = _dashCooldown;
            _playerMovement.Speed = _maxDashSpeed;
            Vector3 direction = _orientation.forward.normalized;
            _forceToApply = direction * _dashForce + _orientation.up * _dashUpwardForce;
            _rigidbody.useGravity = false;

            StartCoroutine(DoDash());
        }

        private IEnumerator DoDash()
        {
            _rigidbody.AddForce(_forceToApply, ForceMode.Impulse);

            yield return new WaitForSeconds(_dashDuration);
            
            _attack.SetActive(false);
            _playerMovement.Speed = _speed;
            _rigidbody.useGravity = true;
        }
    }
}