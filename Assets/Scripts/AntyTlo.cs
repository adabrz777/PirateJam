using UnityEngine;

public class PlayerAntiStand : MonoBehaviour
{
    public float pushForce = 5f; // Force to push player off the object

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("tlo"))
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                // Apply a force to push the player off
                rb.velocity = new Vector2(rb.velocity.x, pushForce);
            }
        }
    }
}
