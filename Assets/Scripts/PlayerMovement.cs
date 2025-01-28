using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Szybkość poruszania się postaci
    public Rigidbody2D rb;      // Komponent Rigidbody2D
    private Vector2 movement;   // Wektor ruchu
    private bool facingRight = true; // Kierunek, w którym patrzy postać

    void Update()
    {
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
        // Przesunięcie postaci
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void Flip()
    {
        // Zmień kierunek patrzenia postaci
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1; // Odwrócenie znaku osi X
        transform.localScale = scale;
    }
}
