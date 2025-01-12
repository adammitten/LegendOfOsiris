using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float runSpeed = 7f;
    private float currentSpeed;
    public VectorValue startingPosition;

    private bool isRunning = false;
    private bool isHoldingObject = false;

    public Rigidbody2D rb;
    public Animator anim;

    Vector2 movement;

    public GameObject heldObject = null;
    public float pickupRange = 2f;
    public float throwForce = 10f;
    public float maxThrowDistance = 10f;
    public LayerMask pickUpLayer;

    public GameObject objectPrefab;

    public Transform holdPosition;

    void Start()
    {
        currentSpeed = moveSpeed;
        transform.position = startingPosition.initialValue;
    }

    private void Update()
    {
        bool isPickUpAnimationPlaying = anim.GetCurrentAnimatorStateInfo(0).IsName("PickUp");

        if (!isPickUpAnimationPlaying)
        {
            ProcessMovementInput();
            HandleRunning();
        }

        HandleInteraction();
        UpdateAnimator();
    }

    private void ProcessMovementInput()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    private void HandleRunning()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRunning = true;
            currentSpeed = runSpeed;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
            currentSpeed = moveSpeed;
        }
    }

    private void HandleInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isHoldingObject)
            {
                ThrowObject();
            }
            else
            {
                TryPickUpObject();
            }
        }
    }

    private void UpdateAnimator()
    {
        anim.SetFloat("Horizontal", movement.x);
        anim.SetFloat("Vertical", movement.y);
        anim.SetFloat("Speed", movement.sqrMagnitude);

        if (movement.sqrMagnitude > 0)
        {
            anim.SetFloat("LastMoveX", movement.x);
            anim.SetFloat("LastMoveY", movement.y);
        }
        else
        {
            anim.SetFloat("LastMoveX", anim.GetFloat("LastMoveX"));
            anim.SetFloat("LastMoveY", anim.GetFloat("LastMoveY"));
        }
        

        anim.SetBool("isRunning", isRunning && movement.sqrMagnitude > 0);
    }

    void FixedUpdate()
    { 
        rb.MovePosition(rb.position + movement * currentSpeed * Time.fixedDeltaTime);

        if (isHoldingObject && heldObject != null)
        {
            heldObject.transform.position = holdPosition.position;
        }
    }

    void TryPickUpObject()
    {
        Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(transform.position, pickupRange, pickUpLayer);

        foreach (Collider2D obj in objectsInRange)
        {
            if (obj.CompareTag("PickupObject"))
            {
                PickupObject pickup = obj.GetComponent<PickupObject>();
                if (pickup != null && heldObject == null)
                {
                    heldObject = obj.gameObject;
                    pickup.PickUp(transform);
                    isHoldingObject = true;
                    anim.SetTrigger("PickUp");
                    break;
                }
            }
        }
    }

    void ThrowObject()
    {
        if (heldObject != null)
        {
            PickupObject pickup = heldObject.GetComponent<PickupObject>();
            if (pickup != null)
            {
                Rigidbody2D rb = heldObject.GetComponent<Rigidbody2D>();
                rb.isKinematic = false;

                Vector2 throwDirection = transform.up.normalized;
                float distance = Vector2.Distance(transform.position, heldObject.transform.position);
                float clampedThrowForce = Mathf.Clamp(throwForce * distance, 0, maxThrowDistance); 
                rb.AddForce(throwDirection * clampedThrowForce, ForceMode2D.Impulse);

                pickup.Throw(throwDirection);
                anim.SetTrigger("Throw");
                heldObject = null;
                isHoldingObject = false;
            }
        }
    }

    public void CreateAndThrowObject()
    {
        if (objectPrefab != null) 
        { 
            GameObject newObject = Instantiate(objectPrefab, transform.position, Quaternion.identity);

            PickupObject pickupScript = newObject.GetComponent<PickupObject>();
            if(pickupScript != null)
            {
                pickupScript.PickUp(transform);
            }
        }
    }
}
