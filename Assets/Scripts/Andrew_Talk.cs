using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Andrew_Talks : MonoBehaviour
{
    public GameObject DialoguePanel;
    public TMP_Text DialogueText;
    public string[] dialogue;
    private int index = 0;  // Start at the first dialogue line
    public float wordSpeed;
    public bool playerIsClose;
    private Coroutine texting;

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.E) && playerIsClose)
        {
            if (DialoguePanel.activeInHierarchy)
            {
                zeroText();
            }
            else
            {
                DialoguePanel.SetActive(true);
                texting = StartCoroutine(Typing());
            }
        }
    }

    public void zeroText()
    {
        if (texting != null)
        {
            StopCoroutine(texting);
        }
        DialogueText.text = "";
        DialoguePanel.SetActive(false);
    }

    IEnumerator Typing()
    {
        DialogueText.text = "";  // Clear text before displaying new dialogue
        foreach (char letter in dialogue[index].ToCharArray())
        {
            DialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }

        // Move to the next dialogue line
        index++;

        // Reset index if we reached the end
        if (index >= dialogue.Length)
        {
            index = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsClose = false;
            zeroText();
        }
    }
}
