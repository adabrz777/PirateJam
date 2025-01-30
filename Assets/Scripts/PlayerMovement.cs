using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Szybkość poruszania się postaci
    public Rigidbody2D rb;      // Komponent Rigidbody2D
    private Vector2 movement;   // Wektor ruchu
    private bool facingRight = true; // Kierunek, w którym patrzy postać

    private bool onForbiddenSurface = false; // Czy gracz stoi na "tlo"
    public float pushForce = 5f; // Siła odpychania gracza od "tlo"

    void Update()
    {
        if (onForbiddenSurface)
        {
            // Jeśli gracz jest na "tlo", nie pozwalamy mu się ruszać
            movement = Vector2.zero;
            return;
        }

        // Odczytanie wejścia z klawiatury (strzałki lub WASD)
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Obrót postaci w zależności od kierunku ruchu
        if (movement.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (movement.x < 0 && facingRight)
        {
            Flip();
        }
    }

    void FixedUpdate()
    {
        if (!onForbiddenSurface)
        {
            // Przesunięcie postaci tylko jeśli nie jest na "tlo"
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    void Flip()
    {
        // Zmień kierunek patrzenia postaci
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1; // Odwrócenie znaku osi X
        transform.localScale = scale;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("tlo"))
        {
            onForbiddenSurface = true;

            // Jeśli gracz stoi na "tlo", odpychamy go do góry
            rb.velocity = new Vector2(rb.velocity.x, pushForce);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("tlo"))
        {
            onForbiddenSurface = false;
        }
    }
}
