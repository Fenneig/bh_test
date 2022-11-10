using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _camera;
    [SerializeField] private CharacterController _controller;
    [Header("Settings")]
    [SerializeField] private float _speed;
    [SerializeField] private float _turnSmoothTime = 0.1f;

    private float _turnSmoothVelocity;
    private Vector2 _moveDirection;

    public Vector2 MoveDirection
    {
        set => _moveDirection = value;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Vector3 direction = new Vector3(_moveDirection.x, 0f, _moveDirection.y).normalized;
        if (direction.magnitude > 0)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity,
                _turnSmoothTime);
            
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            _controller.Move(moveDirection * _speed * Time.deltaTime);
        }
    }

    public void Attack()
    {
        Debug.Log("attacked");
    }
}