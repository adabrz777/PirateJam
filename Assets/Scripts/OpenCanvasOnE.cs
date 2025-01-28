using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCanvasOnE : MonoBehaviour
{
    public GameObject canvas; // Reference to the canvas GameObject
    private bool isPlayerInTrigger = false; // Tracks if the player is in the trigger zone

    void Start()
    {
        // Ensure the canvas is inactive at the start
        if (canvas != null)
        {
            canvas.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Canvas not assigned in the inspector!");
        }
    }

    void Update()
    {
        // Check if the player is in the trigger zone and the 'E' key is pressed
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            if (canvas != null)
            {
                // Toggle the canvas visibility
                canvas.SetActive(!canvas.activeSelf);
            }
        }
    }

    // Detect when the player enters the trigger zone
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
        }
    }

    // Detect when the player exits the trigger zone
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;

            // Optionally hide the canvas when the player leaves the area
            if (canvas != null)
            {
                canvas.SetActive(false);
            }
        }
    }
}
