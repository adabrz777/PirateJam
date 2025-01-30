using UnityEngine;

public class NoPlayerZone : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                // Push the player upwards or away
                playerRb.velocity = new Vector2(playerRb.velocity.x, 5f);
            }
        }
    }
}
