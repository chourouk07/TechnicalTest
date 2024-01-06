using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region player movement variables
    [SerializeField] private float moveSpeed = 5f;
    private Vector2 _moveDirection;
    #endregion


    public void OnMove(InputAction.CallbackContext context)
    {
        _moveDirection = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        Move();
    }
    public void Move()
    {
        Vector3 movement = new Vector3(_moveDirection.x,0f, _moveDirection.y);
        // Check if the movement vector is non-zero before rotating
        if (movement.magnitude > 0.001f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
        }
        transform.Translate(movement*moveSpeed*Time.deltaTime, Space.World);
    }
}
