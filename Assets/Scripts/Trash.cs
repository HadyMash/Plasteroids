using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Trash : MonoBehaviour
{
    private TextMeshProUGUI scoreText;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip trashDestroyed;
    [SerializeField] private AudioClip debrisDestroyed;
    [SerializeField] private bool isMainMenuTrash = false;

    [System.Serializable]
    public struct TrashAsteroid {
        public Sprite sprite;
        public Sprite[] debris;
    }
    [SerializeField] private TrashAsteroid[] trashAsteroids;

    public bool isDebris;

    private TrashAsteroid currentTrashAsteroid;

    private void Start()
    {
        // transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360)); *done by GameManager when spawning in the trash
        if (!isMainMenuTrash)
        {
            scoreText = GameObject.Find("Score Text").GetComponent<TextMeshProUGUI>();
        }
        var scale = Random.Range(0.5f, 1.5f);
        var currentScale = transform.localScale.x;
        var newScale = currentScale += scale;
        transform.localScale = new Vector3(newScale, newScale, newScale);
        if (!isDebris)
        {
            currentTrashAsteroid = trashAsteroids[Random.Range(0, trashAsteroids.Length - 1)];
            gameObject.GetComponent<SpriteRenderer>().sprite = currentTrashAsteroid.sprite;
            gameObject.AddComponent<PolygonCollider2D>();
            
            int a = (Random.Range(0, 2) * 2) - 1;
            float x = Random.Range(a == 1 ? TrashData.debrisMinSpeed : -TrashData.debrisMaxSpeed, a == 1 ? TrashData.debrisMaxSpeed : -TrashData.debrisMinSpeed);
            a = (Random.Range(0, 2) * 2) - 1;
            float y = Random.Range(a == 1 ? TrashData.debrisMinSpeed : -TrashData.debrisMaxSpeed, a == 1 ? TrashData.debrisMaxSpeed : -TrashData.debrisMinSpeed);
            rb.velocity = new Vector2(x, y);
            rb.angularVelocity = Random.Range(-10f, 10f);
        } else {
            audioSource.pitch += Random.Range(-0.3f, 0.3f);
            audioSource.volume += Random.Range(-0.2f, 0f);
            audioSource.PlayOneShot(trashDestroyed);

            int a = (Random.Range(0, 2) * 2) - 1;
            float x = Random.Range(a == 1 ? TrashData.debrisMinSpeed : -TrashData.debrisMaxSpeed, a == 1 ? TrashData.debrisMaxSpeed : -TrashData.debrisMinSpeed);
            a = (Random.Range(0, 2) * 2) - 1;
            float y = Random.Range(a == 1 ? TrashData.debrisMinSpeed : -TrashData.debrisMaxSpeed, a == 1 ? TrashData.debrisMaxSpeed : -TrashData.debrisMinSpeed);
            rb.velocity = new Vector2(x, y);
            rb.angularVelocity = Random.Range(-20f, 20f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Bubble")
        {
            other.GetComponent<Bubble>().DestroyBubble();
            Shatter();
        }
    }

    private void Shatter()
    {
        if (!isDebris) {
            SpawnDebris();
            Destroy(gameObject);
        } else
        {
            scoreText.text = (++GameManager.score).ToString();
            Destroy(gameObject);
        }
    }

    private void SpawnDebris()
    {
        int k = Random.Range(TrashData.minNumberOfDebris, TrashData.maxNumberOfDebris);
        for (int i = 1; i < k + 1; i++)
        {
            GameObject debris = (GameObject)Instantiate(gameObject, transform.position, transform.rotation);
            debris.transform.localScale = new Vector3(debris.transform.localScale.x * 1.5f, debris.transform.localScale.y * 1.5f, debris.transform.localScale.z * 1.5f);
            debris.GetComponent<Trash>().isDebris = true;

            Destroy(debris.GetComponent<PolygonCollider2D>());
            var SpriteRenderer = debris.GetComponent<SpriteRenderer>();
            debris.GetComponent<SpriteRenderer>().sprite = currentTrashAsteroid.debris[Random.Range(0, currentTrashAsteroid.debris.Length - 1)];
            SpriteRenderer.flipX = ((Random.Range(0, 2) * 2) - 1) == 1;
            SpriteRenderer.flipY = ((Random.Range(0, 2) * 2) - 1) == 1;
            debris.AddComponent<PolygonCollider2D>();
        }
    }
}

public struct TrashData
{
    public static float trashMinSpeed = 0.5f;
    public static float trashMaxSpeed = 2f;
    public static float debrisMinSpeed = 1.5f;
    public static float debrisMaxSpeed = 2.5f;

    public static int minNumberOfDebris = 2;
    public static int maxNumberOfDebris = 4;
}