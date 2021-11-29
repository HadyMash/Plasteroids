using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ConstantForce2D constantForce2D;
    [SerializeField] private float turnSpeed = 200f;
    [SerializeField] private float moveSpeed = 7;
    [SerializeField] private GameObject bubblePrefab;
    [SerializeField] private GameObject bubbleSpawnPoint;
    [SerializeField] private float fireRate = 0.15f;
    private float nextFire = 0f;

    public void OnTurn(InputAction.CallbackContext context)
    {
        // if(!context.canceled)
        rb.angularVelocity = context.ReadValue<float>() * turnSpeed;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // if (!context.canceled)
        constantForce2D.relativeForce = new Vector2(0, context.ReadValue<float>() * moveSpeed);
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (Time.time > nextFire)
        {
            GameObject bubble = (GameObject)Instantiate(bubblePrefab, bubbleSpawnPoint.transform.position, transform.rotation);
            nextFire = Time.time + fireRate;
            bubble.GetComponent<Rigidbody2D>().velocity += rb.velocity;
        }
    }
}
