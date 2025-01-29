using UnityEngine;

public class CEnemy : MonoBehaviour {
	float brave;
	int playerHp;
	int myHp = 100;

	public float timeBetweenDecisions = 1.5f;
	float actualTimeBetweenDecisions = -1;

	public enum EnemyState { None, Walking, WalkingBackward, Jump, Crouch, Attacking };
	public EnemyState myState = EnemyState.None;

	public enum AttackType { None, Top, Mid, Bottom };
	public AttackType attackType = AttackType.None;

	bool isGrounded = false;

	public float timeForAttack = 0.75f;
	private float actualAttackingTime = -1;

	public float movingSpeed = 120f;
	public float jumpingForce = 40f;
	public float fallingMultiplier = 2.5f;

	Rigidbody2D rb;

	// Start is called before the first frame update
	void Start() {
		rb = transform.GetComponent<Rigidbody2D>();
		playerHp = GameObject.Find("Player").GetComponent<CPlayerMovement>().hp;
	}

	// Update is called once per frame
	void Update() {

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

			DoAction(EnemyState.Attacking, attackType);

		}
	}

	void DeffendDecision() {
		// 1/3 - mo¿liwoœci
		// 1 - idzie do ty³u
		// 2 - podskok
		// 3 - czy kucn¹æ

		AttackType attackType = AttackType.None;
		EnemyState newState = EnemyState.None;



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

	void Attack(AttackType attackType) {
		if (actualAttackingTime >= 0) {
			actualAttackingTime += Time.fixedDeltaTime;
			myState = EnemyState.Attacking;
		}

		if (actualAttackingTime >= timeForAttack) {
			actualAttackingTime = -1;
			attackType = AttackType.None;
		}

		if (actualAttackingTime < 0)
			if (attackType == AttackType.Top) {
				myState = EnemyState.Attacking;
				actualAttackingTime = 0;
				attackType = AttackType.Top;

			} else if (attackType == AttackType.Mid) {
				myState = EnemyState.Attacking;
				actualAttackingTime = 0;
				attackType = AttackType.Mid;

			} else if (attackType == AttackType.Bottom) {
				myState = EnemyState.Attacking;
				actualAttackingTime = 0;
				attackType = AttackType.Bottom;
			}

	}


	void Move(EnemyState state) {
		Vector2 movingDirection = Vector2.zero;


		if (state == EnemyState.Walking) {
			movingDirection = new Vector2(-1, rb.velocity.y);
		} else {
			movingDirection = new Vector2(1, rb.velocity.y);
		}

		rb.velocity = new Vector2(movingDirection.x * movingSpeed * Time.fixedDeltaTime, rb.velocity.y);

	}

	void Jump() {
		rb.AddForce(new Vector2(0, 1) * jumpingForce, ForceMode2D.Impulse);
		isGrounded = false;


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





	private void FixedUpdate() {
		DecisionAboutBrave();

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
		if (actualTimeBetweenDecisions >= 0) {
			actualTimeBetweenDecisions += Time.fixedDeltaTime;
		}

		if (actualTimeBetweenDecisions >= timeBetweenDecisions) {
			actualTimeBetweenDecisions = -1;
		}

		brave = (playerHp / myHp) * 100;

		if (actualTimeBetweenDecisions == -1) {
			if (brave > Random.Range(0, 100)) {
				AttackDecision();
			} else {
				DeffendDecision();
			}

			actualTimeBetweenDecisions = 0;
			Debug.Log("Zdecydowa³em");

		}
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
