using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Top, Bottom, Right, Left borders
        if (transform.position.y > 0)
        {
            other.transform.position = new Vector2(other.transform.position.x, -other.transform.position.y + 0.7f);
        } else if (transform.position.y < 0)
        {
            other.transform.position = new Vector2(other.transform.position.x, -other.transform.position.y - 0.7f);
        } else if (transform.position.x > 0)
        {
            other.transform.position = new Vector2(-other.transform.position.x + 0.7f, other.transform.position.y);
        } else if (transform.position.x < 0)
        {
            other.transform.position = new Vector2(-other.transform.position.x - 0.7f, other.transform.position.y);
        }
    }
}
