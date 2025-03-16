using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour
{
    private bool isPickedUp = false;
    private Rigidbody2D rb;
    private Collider2D col;

    [Header("Throwing Settings")]
    public float throwForce = 10f;
    public float maxThrowDistance = 5f;

    private Vector2 startPosition;

    public void PickUp(Transform playerTransform)
    {
        if (isPickedUp) return;

        isPickedUp = true;
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        rb.isKinematic = true;
        col.enabled = false;   
        transform.SetParent(playerTransform);
        transform.localPosition = new Vector3(0, 1, 0);
    }

    public void Throw(Vector2 direction)
    {
        if (!isPickedUp) return;

        isPickedUp = false;
        transform.SetParent(null);  
        rb.isKinematic = false;  
        col.enabled = true;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            direction = new Vector2(Mathf.Sign(direction.x), 0);
        }
        else
        {
            direction = new Vector2(0, Mathf.Sign(direction.y));
        }


        rb.velocity = direction * throwForce;
        rb.angularVelocity = Random.Range(-50f, 50f);

        startPosition = transform.position;
        StartCoroutine(CheckMaxDistance());
    }
    private IEnumerator CheckMaxDistance()
    {
        while (Vector2.Distance(startPosition, transform.position) < maxThrowDistance)
        {
            yield return null;
        }

        Destroy(gameObject);
    }

   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isPickedUp && collision.gameObject.CompareTag("Player"))
        {
           
            Physics2D.IgnoreCollision(collision.collider, col);
        }
    }
}
