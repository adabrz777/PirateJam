using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Data;

public class Andrew_Talks : MonoBehaviour
{
    public GameObject DialoguePanel;
    public TMP_Text DialogueText;
    public string[] dialogue;
    private int index;
    public float wordSpeed;
    public bool playerIsClose;
    public Coroutine texting;




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
                index = UnityEngine.Random.Range(0, dialogue.Length - 1); 
                DialoguePanel.SetActive(true);
                texting = StartCoroutine(Typing());
            }
        }
    }

    public void zeroText()
    {
        StopCoroutine(texting);
        DialogueText.text = "";
        index = 0;
        DialoguePanel.SetActive(false);
    }

    IEnumerator Typing()
    {
        foreach(char letter in dialogue[index].ToCharArray())
        {
            DialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
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