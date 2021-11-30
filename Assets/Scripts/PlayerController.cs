using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ConstantForce2D constantForce2D;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float turnSpeed = 200f;
    [SerializeField] private float moveSpeed = 7;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject bubblePrefab;
    [SerializeField] private GameObject bubbleSpawnPoint;
    [SerializeField] private float fireRate = 0.15f;
    [SerializeField] private GameObject bubbleShield;
    [SerializeField] private float invinsibilityTime = 1.5f;
    [SerializeField] private int bubbleShieldFlashes = 3;
    private bool isInvinsible = false;
    public static event GameManager.CollisionEvent onCollisionEnter;
    private float nextFire = 0f;

    private void Update()
    {
        if (transform.rotation.eulerAngles.z > 90 && transform.rotation.eulerAngles.z < 270)
        {
            spriteRenderer.flipY = true;
        }
        else {
            spriteRenderer.flipY = false;
        }
    }

    public void OnTurn(InputAction.CallbackContext context)
    {
        // if(!context.canceled)
        rb.angularVelocity = context.ReadValue<float>() * turnSpeed;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // if (!context.canceled)
        constantForce2D.relativeForce = new Vector2(context.ReadValue<float>() * moveSpeed, 0);
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (Time.time > nextFire)
        {
            GameObject bubble = (GameObject)Instantiate(bubblePrefab, bubbleSpawnPoint.transform.position, transform.rotation);
            nextFire = Time.time + fireRate;
            bubble.GetComponent<Rigidbody2D>().velocity += rb.velocity;
            audioSource.PlayOneShot(audioSource.clip);
        }
    }

    IEnumerator MakeInvinsible()
    {
        bubbleShield.SetActive(true);
        yield return new WaitForSeconds(invinsibilityTime - 0.5f);
        int flashes = bubbleShieldFlashes;
        while (flashes-- > 0)
        {
            bubbleShield.SetActive(false);
            yield return new WaitForSeconds(0.5f / (bubbleShieldFlashes * 2f));
            bubbleShield.SetActive(true);
            yield return new WaitForSeconds(0.5f / (bubbleShieldFlashes * 2f));
        }
        bubbleShield.SetActive(false);
        isInvinsible = false;
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Trash" && !isInvinsible)
        {
            onCollisionEnter();
            if (GameManager.lives > 0)
            {
                isInvinsible = true;
                StartCoroutine(MakeInvinsible());
            }
        }
    }
}
