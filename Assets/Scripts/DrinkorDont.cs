using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DrinkorDont : MonoBehaviour
{
    public void ScenaPiwko()
    {
        SceneManager.LoadScene("CombatBeer");
    }

    public void ScenaWodka()
    {
        SceneManager.LoadScene("CombatVodka");
    }
}
