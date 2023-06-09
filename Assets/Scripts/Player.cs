using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerSpriteRenderer smallRenderer;
    public PlayerSpriteRenderer bigRenderer;
    public PlayerSpriteRenderer activeRenderer;
    public SpriteRenderer spriteRenderer {get; private set;}

    private DeathAnimation deathAnimation;
    public bool big => bigRenderer.enabled;
    public bool small => smallRenderer.enabled;
    public bool dead =>deathAnimation.enabled;
    public bool starpower { get; private set;}
   
    private CapsuleCollider2D capsuleCollider;

    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        deathAnimation = GetComponent<DeathAnimation>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        activeRenderer = smallRenderer;
    }

    public void Hit()
    {
        if (!dead && !starpower){
            if (big) {
            Shrink();
            } else {
            Death();
            }
        }
        
    }

    private void Death()
    {
        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        deathAnimation.enabled = true;

        GameManager.Instance.ResetLevel(3f);
    }

    public void Grow()
    {
        smallRenderer.enabled = false;
        bigRenderer.enabled = true;
        activeRenderer = bigRenderer;

        capsuleCollider.size = new Vector2 (1f,2f);
        capsuleCollider.offset = new Vector2(0f, 0.08f);

        StartCoroutine(ScaleAnimation());
    }

    public void Shrink()
    {
        smallRenderer.enabled = true;
        bigRenderer.enabled = false;
        activeRenderer = smallRenderer;

        capsuleCollider.size = new Vector2 (1f, 1f);
        capsuleCollider.offset = new Vector2 (0f, -0.42f);
        StartCoroutine(ScaleAnimation());

    }

    private IEnumerator ScaleAnimation()
    {
        float elapsed = 0f;
        float duration = 0.5f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            if (Time.frameCount % 4 == 0)
            {
                smallRenderer.enabled = !smallRenderer.enabled;
                bigRenderer.enabled = !smallRenderer.enabled;
            }

            yield return null;
        }

        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        activeRenderer.enabled = true;
    }

    public void StarPower()
    {
        StartCoroutine(StarpowerAnimation());
    }

    private IEnumerator StarpowerAnimation(float duration = 10f)
    {
        starpower = true;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            if (Time.frameCount % 4 == 0)
            {
                activeRenderer.spriteRenderer.color = Random.ColorHSV(0f,1f,1f,1f,1f, 1f);
            }

            yield return null;

        }
        activeRenderer.spriteRenderer.color = Color.white;

        starpower = false;
    }
}
