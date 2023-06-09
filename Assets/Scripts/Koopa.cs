using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Koopa : MonoBehaviour
{
    public Sprite shellSprite;
    private DeathAnimation deathAnimation;

    private bool pushed;
    private bool shelled;

    public float shellSpeed = 12f;

    public void Awake()
    {
        deathAnimation = GetComponent<DeathAnimation>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!shelled && col.gameObject.CompareTag("Player"))
        {
            Player player = col.gameObject.GetComponent<Player>();
            if (player.starpower)
            {
                Hit();
            } else if(col.transform.DotTest(transform, Vector2.down))
            {
                EnterShell();
            } else {
                player.Hit();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (shelled && other.CompareTag("Player"))
        {
            if (!pushed)
            {
                Vector2 direction = new Vector2(transform.position.x - other.transform.position.x, 0f);
                PushShell(direction);
            }
            else
            {
                Player player = other.GetComponent<Player>();

                if (player.starpower)
                {
                    Hit();
                } else {
                    player.Hit();
                }
                
            }
        } else if (!shelled && other.gameObject.layer == LayerMask.NameToLayer("Shell"))
        {
            Hit();
        }
    }

    private void EnterShell()
    {
        shelled = true;

        GetComponent<AnimatedSprite>().enabled = false;
        GetComponent<EntityMovement>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = shellSprite;
    } 

    private void PushShell(Vector2 direction)
    {
        pushed = true;

        GetComponent<Rigidbody2D>().isKinematic = false;

        EntityMovement movement = GetComponent<EntityMovement>();
        movement.direction = direction.normalized;
        movement.speed = shellSpeed;
        movement.enabled = true;

        gameObject.layer = LayerMask.NameToLayer("Shell");

    }

    private void Hit()
    {
        GetComponent<AnimatedSprite>().enabled = false;
        GetComponent<EntityMovement>().enabled = false;
        deathAnimation.enabled = true;

        Destroy(gameObject, 3f);
    }

    private void OnBecameVisible()
    {
        if (pushed) {
            Destroy(gameObject);
        }
    }
}
