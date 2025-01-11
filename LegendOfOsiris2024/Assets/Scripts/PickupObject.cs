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
        rb.velocity = direction * throwForce;  

       
        rb.angularVelocity = Random.Range(-50f, 50f);  
    }

   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isPickedUp && collision.gameObject.CompareTag("Player"))
        {
           
            Physics2D.IgnoreCollision(collision.collider, col);
        }
    }
}
