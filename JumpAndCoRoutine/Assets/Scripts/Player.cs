using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private float velocity;
    [SerializeField] private float jumpForce;

    private Rigidbody2D rigidBody;
    private Transform playerTransform;
    private SpriteRenderer spriteRenderer;

    private bool canJump = true;
    private int doubleJump = 0;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        playerTransform = GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxis("Horizontal") * velocity * Time.deltaTime;
        playerTransform.Translate(new Vector3(moveX, 0));

        if (Input.GetButtonDown("Jump") && canJump)
        {
            doubleJump++;
            if (doubleJump >= 2)
            {
                canJump = false;
            }
            rigidBody.AddForce(Vector2.up * jumpForce);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        canJump = true;
        doubleJump = 0;

        if (collision.gameObject.tag == "Enemy")
        {
            StartCoroutine(DestroyPlayer());
        }

    }

    //Corotina com alteração do alpha do material não funcionou
    private IEnumerator DestroyPlayer()
    {
        Color color = spriteRenderer.material.color;
        for (float alpha = spriteRenderer.color.a; alpha < 0; alpha -= 0.1f)
        {
            color.a = alpha;
            yield return new WaitForSeconds(0.5f);
            spriteRenderer.material.color = color;
        }

        yield return new WaitForSeconds(1f);
        playerTransform.position = new Vector3(0, 0, 0);
    }

}
