using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] FixedJoystick _joystick;
    [SerializeField] Rigidbody2D _playerRb;
    [SerializeField] float _moveSpeed = 5;

    void FixedUpdate()
    {
        _playerRb.velocity = new Vector2 (_joystick.Horizontal * _moveSpeed,
            _joystick.Vertical * _moveSpeed);
    }
}
