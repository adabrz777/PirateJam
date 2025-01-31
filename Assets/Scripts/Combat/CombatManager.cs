using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CombatManager : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;

	public TMP_Text playerHpUI;
	public TMP_Text enemyHpUI;

    CPlayerMovement playerMovement;
    CEnemy enemyMovement;

    int playerHpInt;
    int enemyHpInt;

    string playerHp;
    string enemyHp;


	// Start is called before the first frame update
	void Start()
    {
        playerMovement = player.GetComponent<CPlayerMovement>();
        enemyMovement = enemy.GetComponent<CEnemy>();
    }

    // Update is called once per frame
    void Update()
    {
        playerHpInt = playerMovement.myHp;
        enemyHpInt = enemyMovement.myHp;


        playerHp = "";
        enemyHp = "";

        


        for (int i = 1; i <= playerHpInt; i++)
            playerHp += "\u2588";


		for (int i = 1; i <= enemyHpInt; i++)
			enemyHp += "\u2588";


		playerHpUI.text = playerHp;
        enemyHpUI.text = enemyHp;
    }
}
