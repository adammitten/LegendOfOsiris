using System.Collections;
using UnityEngine;

public class PickupObject : MonoBehaviour
{
    private bool isPickedUp = false;
    private bool isFlying = false;
    private Rigidbody2D rb;
    private Collider2D col;
    private Transform playerTransform;

    [Header("Throwing Settings")]
    public float throwForce = 10f;
    public float maxThrowDistance = 5f;

    private Vector2 startPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        ResetObject();
    }

    public void PickUp(Transform player)
    {
        if (isPickedUp || isFlying) return;

        playerTransform = player;
        isPickedUp = true;

        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true;
        col.enabled = false;

        transform.SetParent(playerTransform);
        transform.localPosition = new Vector3(0, 1, 0); 
    }

    public void Throw(Vector2 direction)
    {
        if (!isPickedUp || isFlying) return;

        isPickedUp = false;
        isFlying = true;

        transform.SetParent(null);
        rb.isKinematic = false;
        col.enabled = true;

        Collider2D playerCol = playerTransform.GetComponent<Collider2D>();
        StartCoroutine(EnableCollisionAfterDelay(playerCol, 1f));

        direction = SnapToCardinal(direction);
       
        rb.velocity = direction * throwForce;
        rb.angularVelocity = Random.Range(-50f, 50f);
        startPosition = transform.position;

        StartCoroutine(CheckMaxDistance());
    }

    private Vector2 SnapToCardinal(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            return new Vector2(Mathf.Sign(dir.x), 0);
        else
            return new Vector2(0, Mathf.Sign(dir.y));
    }

    private IEnumerator CheckMaxDistance()
    {
        while (Vector2.Distance(startPosition, transform.position) < maxThrowDistance)
        {
            yield return null;
        }

        Land();
    }

    private IEnumerator EnableCollisionAfterDelay(Collider2D playerCol, float delay = 0.2f)
    {
        Physics2D.IgnoreCollision(col, playerCol, true);
        yield return new WaitForSeconds(delay);
        Physics2D.IgnoreCollision(col, playerCol, false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isFlying)
        {
            Land();
        }
    }

    private void Land()
    {
        if(!isFlying) return;

        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true;

        isFlying = false;
        isPickedUp = false;

        col.enabled = true;
    }

    private void ResetObject()
    {
        isPickedUp = false;
        isFlying = false;

        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true;
        col.enabled = false;

        transform.SetParent(null);
    }

    public void ReturnToPool()
    {
        ObjectPooler.Instance.ReturnToPool(gameObject);
    }
}