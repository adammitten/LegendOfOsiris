using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour
{
    private bool isPickedUp = false;
    private Rigidbody2D rb;
    private Collider2D col;

    [Header("Throwing Settings")]
    public float throwForce = 10f;  // Exposing throw force to the inspector for flexibility

    // Called when the object is picked up
    public void PickUp(Transform playerTransform)
    {
        if (isPickedUp) return;

        isPickedUp = true;
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        rb.isKinematic = true;
        col.enabled = false;  // Disable collision when picked up
        transform.position = playerTransform.position;  // Set object position to the player
        transform.SetParent(playerTransform);  // Make the object follow the player
    }

    // Called when the object is thrown
    public void Throw(Vector2 direction)
    {
        if (!isPickedUp) return;

        isPickedUp = false;
        transform.SetParent(null);  // Detach object from player
        rb.isKinematic = false;  // Enable physics again
        col.enabled = true;  // Re-enable collision
        rb.velocity = direction * throwForce;  // Apply throw force to the object

        // Optional: You could also add a small angular velocity for a more realistic throw
        rb.angularVelocity = Random.Range(-50f, 50f);  // Randomize angular velocity for a more natural throw
    }

    // Optional: You could add a method to handle collision with the player while it's being held
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isPickedUp && collision.gameObject.CompareTag("Player"))
        {
            // Prevent the object from colliding with the player while being held
            Physics2D.IgnoreCollision(collision.collider, col);
        }
    }
}
