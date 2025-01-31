using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FinishText : MonoBehaviour
{
    public TMP_Text text;
    public PlayerStats stats;


    // Start is called before the first frame update
    void Start()
    {
        text.text = $"You won with alcohol {stats.playerWins} times. But are you sure about that?";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
