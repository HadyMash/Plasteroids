using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float bubbleSpeed = 5f;
    [SerializeField] private float bubbleSelfDestructDelay = 2.5f;

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(bubbleSelfDestructDelay);
        Destroy(gameObject);
    }

    private void Start()
    {
        StartCoroutine(DestroyAfterTime());
        var angleInRad = Mathf.Deg2Rad * transform.rotation.eulerAngles.z;
        rb.velocity += new Vector2(Mathf.Cos(angleInRad) * bubbleSpeed, Mathf.Sin(angleInRad) * bubbleSpeed);
    }
    public void DestroyBubble()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        var rb = gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
        audioSource.PlayOneShot(audioSource.clip);
        Destroy(gameObject, audioSource.clip.length);
    }
}
