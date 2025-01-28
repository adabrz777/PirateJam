using UnityEngine;

public class OpenCanvasOnTrigger : MonoBehaviour
{
    public GameObject canvasToOpen;

    public string triggeringTag = "Player";

    void Start()
    {
        if (canvasToOpen != null)
        {
            canvasToOpen.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(triggeringTag))
        {
            if (canvasToOpen != null)
            {
                canvasToOpen.SetActive(true);
                EnableCursor();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(triggeringTag))
        {
            if (canvasToOpen != null)
            {
                canvasToOpen.SetActive(false);
                DisableCursor();
            }
        }
    }
    private void EnableCursor()
    {
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true; // Make the cursor visible
    }

    private void DisableCursor()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor back
        Cursor.visible = false; // Hide the cursor
    }
}
