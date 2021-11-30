using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject trashPrefab;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private int maxNumberOfBigTrash = 8;
    [SerializeField] private float borderXCoord;
    [SerializeField] private float borderYCoord;
    private bool isAlive = true;
    [HideInInspector] public float time;
    public static int score = 0;
    public static int lives = 3;
    [SerializeField] private GameObject[] livesSprites;

    public delegate void CollisionEvent();

    private float MinSpawnDelay(float t)
    {
        float f(float x) => -0.04f * x + 3f;

        if (t <= 35)
        {
            return f(t);
        } else {
            return f(35);
        }
    }

    private float MaxSpawnDelay(float t)
    {
        float f(float x) => -(1f/7f) * x + 9;

        if (t <= 35)
        {
            return f(t);
        } else {
            return f(35);
        }
    }

    private IEnumerator SpawnTrash()
    {
        while (isAlive)
        {
            int bigTrashCount = 0;
            var trash = GameObject.FindGameObjectsWithTag("Trash");
            foreach (var obj in trash)
            {
                if (!obj.GetComponent<Trash>().isDebris)
                {
                    bigTrashCount++;
                }
            }

            if (bigTrashCount < maxNumberOfBigTrash)
            {
                int numToSpawn = Random.Range(1, maxNumberOfBigTrash - bigTrashCount);
                for (var i = 0; i < numToSpawn; i++)
                {
                    float angle = Random.Range(0, 360) * Mathf.Deg2Rad;
                    float x = Mathf.Cos(angle) * borderXCoord * (angle > 0 || angle < Mathf.PI ? -1 : 1);
                    float y = Mathf.Sin(angle) * borderYCoord * (angle > 3 * Mathf.PI / 2 || angle < Mathf.PI / 2 ? -1 : 1);

                    Instantiate(trashPrefab, new Vector3(x, y, 0), Quaternion.Euler(0, 0, Random.Range(0, 360)));
                }
            }

            yield return new WaitForSeconds(Random.Range(MinSpawnDelay(time), MaxSpawnDelay(time)));
        }
    }

    private void OnEnable()
    {
        PlayerController.onCollisionEnter += HandleCollision;
    }
    private void OnDisable()
    {
        PlayerController.onCollisionEnter -= HandleCollision;
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        lives = 3;

        if (borderXCoord == null)
        {
            borderXCoord = GameObject.Find("Top Border").transform.position.x - 0.5f;
        }
        if (borderYCoord == null)
        {
            borderYCoord = GameObject.Find("Rigth Border").transform.position.y - 0.5f;
        }

        StartCoroutine(SpawnTrash());
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
    }

    private void HandleCollision()
    {
        if (--lives > 0)
        {
            livesSprites[lives].SetActive(false);
        } else
        {
            isAlive = false;
            Time.timeScale = 0f;
            livesSprites[lives].SetActive(false);
            gameOverUI.SetActive(true);
        }
    }
}
