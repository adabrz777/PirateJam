using UnityEngine;
using UnityEngine.SceneManagement;
using static CEnemy;
using static CPlayerMovement;

public class CEnemy : MonoBehaviour {
	float brave;
	int playerHp;
	public int myHp = 10;

	public float timeBetweenDecisions = 1.5f;
	public PlayerStats playerStats;


	float actualTimeBetweenDecisions = -1;

	public enum EnemyState { None, Walking, WalkingBackward, Jump, Crouch, Attacking };
	public EnemyState myState = EnemyState.None;

	public enum AttackType { None, Top, Mid, Bottom };
	public AttackType attackType = AttackType.None;

	bool isGrounded = false;

	public float timeForAttack = 0.75f;
	private float actualAttackingTime = -1;
	public float attackDistance = 7f;

	public float movingSpeed = 120f;
	public float jumpingForce = 40f;
	public float fallingMultiplier = 2.5f;

	Rigidbody2D rb;
	Animator animator;

	// Start is called before the first frame update
	void Start() {
		rb = transform.GetComponent<Rigidbody2D>();
		animator = transform.GetComponent<Animator>();

		playerHp = GameObject.Find("Player").GetComponent<CPlayerMovement>().myHp;
	}

	// Update is called once per frame
	void Update() {
		SetAnimation(myState, attackType);
	}

	void AttackDecision() {
		// 1/2 mo¿liwoœci
		// 1 - podchodzi
		// 2 - atak

		// jeœli 2 to 1 - 3
		// 1 - attack top
		// 2 - attack mid
		// 3 - attack bottom

		AttackType attackType = AttackType.None;

		if (Random.Range(1, 100) > 50) {
			Debug.Log("Atakuje i idê do przodu");
			DoAction(EnemyState.Walking, attackType);
		} else {
			int attackDec = Random.Range(1, 100);

			if (attackDec > 33) {
				attackType = AttackType.Mid;
			} else if (attackDec > 66) {
				attackType = AttackType.Top;
			} else {
				attackType = AttackType.Bottom;
			}

			Debug.Log($"Atakuje i wykonujê atak: {attackType}");
			DoAction(EnemyState.Attacking, attackType);


		}
	}

	void DeffendDecision() {
		// 1/3 - mo¿liwoœci
		// 1 - idzie do ty³u
		// 2 - podskok
		// 3 - czy kucn¹æ

		AttackType attackType = AttackType.None;
		//EnemyState newState = EnemyState.None;
		EnemyState newState = myState;


		int deffDec = Random.Range(1, 100);

		if (deffDec > 33 && deffDec <= 65) {
			newState = EnemyState.WalkingBackward;
		} else if (deffDec >= 66 && isGrounded) {
			newState = EnemyState.Jump;
		} else {
			newState = EnemyState.Crouch;
		}

		DoAction(newState, attackType);



	}

	void Attack(AttackType t_attackType) {
		


		


		if (actualAttackingTime < 0) {
			myState = EnemyState.Attacking;
			actualAttackingTime = 0;
			attackType = t_attackType;
			AttackCalculations(t_attackType);
		}
			

	}


	void Move(EnemyState state) {
		Vector2 movingDirection = Vector2.zero;


		if (state == EnemyState.Walking) {
			movingDirection = new Vector2(-1, rb.velocity.y);
			myState = EnemyState.Walking;
		} else {
			movingDirection = new Vector2(1, rb.velocity.y);
			myState = EnemyState.WalkingBackward;
		}

		rb.velocity = new Vector2(movingDirection.x * movingSpeed * Time.fixedDeltaTime, rb.velocity.y);

	}

	void Jump() {
		rb.AddForce(new Vector2(0, 1) * jumpingForce, ForceMode2D.Impulse);
		isGrounded = false;
		myState = EnemyState.Jump;

	}

	void Crouch() {

		if (isGrounded) {
			Debug.Log("Kucam");
			myState = EnemyState.Crouch;
		}

	}

	void FallingDown() {
		if (rb.velocity.y < 0) {
			rb.gravityScale = fallingMultiplier;
		} else {
			rb.gravityScale = 1;
		}
	}


	private void CalculateTime() {
		if (actualAttackingTime >= 0) {
			actualAttackingTime += Time.fixedDeltaTime;
			myState = EnemyState.Attacking;
		}

		if (actualAttackingTime >= timeForAttack) {
			actualAttackingTime = -1;
			attackType = AttackType.None;
		}
	}


	private void FixedUpdate() {
		DecisionAboutBrave();
		CalculateTime();
		

		FallingDown();
	}

	void DoAction(EnemyState myState, AttackType t_attackType) {
		if (myState == EnemyState.Attacking) {
			Attack(t_attackType);
		} else if (myState == EnemyState.Crouch) {
			Crouch();
		} else if (myState == EnemyState.WalkingBackward || myState == EnemyState.Walking) {
			Move(myState);
		} else if (myState == EnemyState.Jump) {
			Jump();
		}
	}

	void DecisionAboutBrave() {
		if(myHp <=  0)
			endScene();

		if (actualTimeBetweenDecisions >= 0) {
			actualTimeBetweenDecisions += Time.fixedDeltaTime;
		}

		if (actualTimeBetweenDecisions >= timeBetweenDecisions) {
			actualTimeBetweenDecisions = -1;
		}

		brave = (playerHp / myHp) * 100;

		if (actualTimeBetweenDecisions == -1) {
			if (brave > Random.Range(0, 120)) {
				AttackDecision();
			} else {
				DeffendDecision();
			}

			actualTimeBetweenDecisions = 0;
			

		}
	}

	void SetAnimation(EnemyState t_state, AttackType t_attackType) {


		if (animator.GetInteger("Animation") != (int)t_state) {
			animator.SetInteger("Animation", (int)t_state);
			Debug.Log("Enemy: zmieniam animacjê");
		}


		if ((int)t_attackType != (int)animator.GetInteger("Attack")) {
			animator.SetInteger("Attack", (int)t_attackType);
			Debug.Log($"Enemy: zmieniam animacjê ataku na {(int)t_attackType}");
		}



	}


	void AttackCalculations(AttackType t_attackType) {
		transform.GetComponent<AudioSource>().Play();

		transform.GetComponent<Collider2D>().enabled = false;
		RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.right, attackDistance);
		transform.GetComponent <Collider2D>().enabled = true;

		Debug.DrawRay(transform.position, -transform.right * attackDistance, Color.green, 1f);
		Debug.Log("Hit: " + hit.collider.name);

		if (hit.collider != null) {
			if (hit.collider.name == "Player") {

					CPlayerMovement player = hit.collider.gameObject.GetComponent<CPlayerMovement>();

					if (t_attackType == AttackType.Top && player.myState != CharacterState.Crouch) {
						player.myHp -= 1;

					} else if (t_attackType == AttackType.Mid && player.myState != CharacterState.WalkingBackward) {
						player.myHp -= 1;

					} else if (t_attackType == AttackType.Bottom && player.myState != CharacterState.Jump) {
						player.myHp -= 1;

					}


					Debug.Log($"Trafiono Gracza! Jego hp spad³o do: {player.myHp}");



				if (player.myHp <= 0) {
					endScene();
				}



			}
		}
	}

	void endScene() {
		Debug.Log("ALCOHOL WINS");
		playerStats.played += 1;

		if (playerStats.played % 2 == 0 && playerStats.played != 0)
			SceneManager.LoadScene("EndScene");

		SceneManager.LoadScene("Scena_BEZ_Zona 1");
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if(collision.gameObject.CompareTag("Ground"))
			isGrounded = true;
	}

	private void OnCollisionExit2D(Collision2D collision) {
		if (collision.gameObject.CompareTag("Ground"))
			isGrounded = false;
	}

}
