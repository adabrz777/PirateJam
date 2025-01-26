using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Szybkość poruszania się postaci
    public Rigidbody2D rb;      // Komponent Rigidbody2D
    private Vector2 movement;   // Wektor ruchu

    void Update()
    {
        // Odczytanie wejścia z klawiatury (strzałki lub WASD)
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        // Przesunięcie postaci
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
