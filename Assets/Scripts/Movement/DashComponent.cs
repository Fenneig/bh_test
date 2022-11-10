﻿using System.Collections;
using Character;
using UnityEngine;

namespace Movement
{
    public class DashComponent : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private Transform _orientation;

        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Player _player;

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
            _speed = _player.Speed;
        }

        private void Update()
        {
            if (_dashCooldownTimer > 0) _dashCooldownTimer -= Time.deltaTime;
        }

        public void Dash()
        {
            if (_dashCooldownTimer > 0) return;
        
            _dashCooldownTimer = _dashCooldown;
            _player.Speed = _maxDashSpeed;
            Vector3 direction = _orientation.forward.normalized;
            _forceToApply = direction * _dashForce + _orientation.up * _dashUpwardForce;
            _rigidbody.useGravity = false;

            StartCoroutine(DoDash());
        }

        private IEnumerator DoDash()
        {
            _rigidbody.AddForce(_forceToApply, ForceMode.Impulse);

            yield return new WaitForSeconds(_dashDuration);

            _player.Speed = _speed;
            _rigidbody.useGravity = true;
        }
    }
}