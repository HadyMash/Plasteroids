using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip trashDestroyed;
    [SerializeField] private AudioClip debrisDestroyed;

    [System.Serializable]
    public struct TrashAsteroid {
        public Sprite sprite;
        public Sprite[] debris;
    }
    [SerializeField] private TrashAsteroid[] trashAsteroids;   

    private Vector2 velocity;
    private bool isDebris;

    public Trash(Vector2 velocity, bool isDebris = false)
    {
        this.velocity = velocity;
        this.isDebris = isDebris;
    }

    private void Start()
    {
        rb.velocity = velocity;

        if (isDebris)
        {
            audioSource.pitch += Random.Range(-0.3f, 0.3f);
            audioSource.volume += Random.Range(-0.2f, 0f);
            audioSource.PlayOneShot(trashDestroyed);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Bubble")
        {
            Shatter();
        }
    }

    private void Shatter()
    {
        Debug.Log("Shatter");
        // TODO: Implement
        if (!isDebris) {
            SpawnDebris();
        }
    }

    private void SpawnDebris()
    {
        
        Debug.Log("SpawnDebris");
        // TODO: Implement
    }
}

public struct TrashData
{
    public static float minfDebrisOneSize = 5f;
    // public static float
}