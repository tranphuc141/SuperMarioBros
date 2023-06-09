using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private new Camera camera;
    private new Rigidbody2D rigidbody;

    public float moveSpeed = 8f;
    public float maxJumpHeight = 5f;
    public float maxJumpTime= 1f;
    public float jumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f);
    public float gravity => (-2f * maxJumpHeight) / Mathf.Pow((maxJumpTime / 2f), 2);

    private Vector2 velocity;

    private float inputAxes;

    public bool grounded { get; private set;}
    public bool jumping { get; private set;}
    public bool running => Mathf.Abs(velocity.x) > 0.25f ||Mathf.Abs(inputAxes) > 0.25f;
    public bool sliding => (inputAxes > 0f && velocity.x < 0f) ||(inputAxes < 0f && velocity.x > 0f);

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        camera = Camera.main;
    }

    private void Update()
    {
        HorizontalMovement();
        grounded = rigidbody.Raycast(Vector2.down);

        if (grounded){
            GroundedMovement();
        }
        ApplyGravity();
  
    }
    private void GroundedMovement()
    {
        jumping = false;
        velocity.y = Mathf.Max(velocity.y, 0f);
        jumping = velocity.y > 0f; 
        if (Input.GetButtonDown("Jump"))
        {
            velocity.y = jumpForce;
            jumping = true;
        }
    }

    private void HorizontalMovement()
    {
        inputAxes = Input.GetAxis("Horizontal");
        velocity.x = Mathf.MoveTowards(velocity.x, inputAxes * moveSpeed, moveSpeed * Time.deltaTime);
        if (rigidbody.Raycast(Vector2.right * velocity.x))
        {
            velocity.x = 0f;
        }
        
        if (velocity.x > 0){
            transform.eulerAngles = Vector3.zero;
        } else if (velocity.x < 0) {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
    }

    private void ApplyGravity()
    {
        bool falling = velocity.y <= 0 || !Input.GetButton("Jump");
        float multiplier = falling ? 2f : 1f;
        velocity.y += gravity * Time.deltaTime * multiplier;
        velocity.y = Mathf.Max(velocity.y, gravity / 2f);
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidbody.position;
        position += velocity * Time.fixedDeltaTime;

        Vector2 leftEdge = camera.ScreenToWorldPoint(Vector2.zero);
        Vector2 rightEdge = camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        position.x = Mathf.Clamp(position.x, leftEdge.x + 0.5f, rightEdge.x - 0.5f);

        rigidbody.MovePosition(position);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if(transform.DotTest(collision.transform, Vector2.down))
            {
                velocity.y = jumpForce / 2f;
                jumping = true;
            }
        }
        if (collision.gameObject.layer != LayerMask.NameToLayer("PowerUp"))
        {
            if (transform.DotTest(collision.transform, Vector2.up)) {
                velocity.y = -5f;
                rigidbody.velocity = velocity;
            }
        }
    }
}
