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

    public Rigidbody2D rb;
    public Animator anim;

    Vector2 movement;

    void Start()
    {
        currentSpeed = moveSpeed;

        transform.position = startingPosition.initialValue;
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        anim.SetFloat("Horizontal", movement.x);
        anim.SetFloat("Vertical", movement.y);
        anim.SetFloat("Speed", movement.sqrMagnitude);

        HandleRunning();

        if (movement.x != 0 || movement.y != 0)
        {
            anim.SetFloat("LastMoveX", movement.x);
            anim.SetFloat("LastMoveY", movement.y);
        }

        else if (movement.sqrMagnitude == 0) 
        {
            anim.SetFloat("Speed", 0);
            anim.SetBool("isRunning",  false);
        }
    }

    void HandleRunning()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRunning = true;
            currentSpeed = runSpeed;

            anim.SetBool("isRunning", true);
        }

        if (Input.GetKeyUp(KeyCode.LeftShift)) 
        {
            isRunning = false;
            currentSpeed = moveSpeed;

            anim.SetBool("isRunning", false);
        }

        if (isRunning && (movement.x != 0 || movement.y != 0))
        {
            anim.SetBool("isRunning", true);
        }

        else if (movement.sqrMagnitude == 0 || !isRunning)
        {
            anim.SetBool("isRunning", false);
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * currentSpeed * Time.fixedDeltaTime);
    }
}
