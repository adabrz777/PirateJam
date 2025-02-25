using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static CEnemy;

public class CPlayerMovement : MonoBehaviour
{
    public int horizontalInput = 0;
    bool SisDown = false;
    bool isJumpKeyDown = false;
    bool isJKeyDown = false;
    bool isKKeyDown = false;
    bool isLKeyDown = false;




    bool isGrounded = false;

    public float movingSpeed = 3f;
    public float jumpingForce = 100f;
    public float fallingMultiplier = 2f;
    public float attackDistance = 7f;

    public float timeForAttack = 0.75f;
    private float actualAttackingTime = -1;

    public PlayerStats playerStats;

	public enum CharacterState { None, Walking, WalkingBackward, Jump, Crouch, Attacking };
    public CharacterState myState = CharacterState.None;


    public enum AttackType { None, Top, Mid, Bottom };
    public AttackType attackType = AttackType.None;


    Rigidbody2D rb;
    Animator animator;

    public int myHp = 10;



    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        animator = transform.GetComponent<Animator>();
    }

    void GetKeys() {
        
		horizontalInput = (int)Input.GetAxisRaw("Horizontal");

		SisDown = Input.GetKey(KeyCode.S);
        

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
            isJumpKeyDown = true;


        if(Input.GetKeyDown(KeyCode.J))
            isJKeyDown = true;

        if(Input.GetKeyDown(KeyCode.K))
            isKKeyDown = true;

        if(Input.GetKeyDown(KeyCode.L))
            isLKeyDown = true;
	}

    // Update is called once per frame
    void Update()
    {
        GetKeys();
        SetAnimation(myState, attackType);


    }

    void SetAnimation(CharacterState t_state, AttackType t_attackType) {


       if(animator.GetInteger("Animation") != (int)t_state) {
            animator.SetInteger("Animation", (int)t_state);
            Debug.Log("Player: zmieniam animacj�");
        }


        if ((int)t_attackType != (int)animator.GetInteger("Attack")) {
            animator.SetInteger("Attack", (int)t_attackType);
            Debug.Log("Player: zmieniam animacj� ataku");
        }



    }
	private void CalculateTime() {
		if (actualAttackingTime >= 0) {
			actualAttackingTime += Time.fixedDeltaTime;
			myState = CharacterState.Attacking;
		}

		if (actualAttackingTime >= timeForAttack) {
			actualAttackingTime = -1;
			attackType = AttackType.None;
		}
	}

	void AttackCalculations(AttackType t_attackType) {
		transform.GetComponent<Collider2D>().enabled = false;
		RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, attackDistance);
		transform.GetComponent<Collider2D>().enabled = true;

		Debug.DrawRay(transform.position, transform.right * attackDistance, Color.green, 1f);
		Debug.Log("Hit: " + hit.collider.name);

		if (hit.collider != null) {
			if (hit.collider.name == "Enemy") {
                CEnemy enemy = hit.collider.gameObject.GetComponent<CEnemy>();
               
                if (t_attackType == AttackType.Top && enemy.myState != EnemyState.Crouch) {
                    enemy.myHp -= 1;

                }else if(t_attackType == AttackType.Mid && enemy.myState != EnemyState.WalkingBackward) {
                    enemy.myHp -= 1;
                
                }else if(t_attackType == AttackType.Bottom && enemy.myState != EnemyState.Jump) {
                    enemy.myHp -= 1;
                
                }


				Debug.Log($"Trafiono Wroga! Jego hp spad�o do: {enemy.myHp}");


                if(enemy.myHp <= 0) {
                   endScene();
				}
			}
		}
	}


    void endScene() {
		Debug.Log("PLAYER WINS");
		playerStats.playerWins += 1;

		playerStats.played += 1;

		if (playerStats.played % 2 == 0 && playerStats.played != 0)
			SceneManager.LoadScene("EndScene");


		SceneManager.LoadScene("Scena_BEZ_Zona 1");
	}


	void Attack() {
        if(actualAttackingTime < 0) {
			if (isJKeyDown) {
				myState = CharacterState.Attacking;
				actualAttackingTime = 0;
				attackType = AttackType.Top;
				AttackCalculations(attackType);

			} else if (isKKeyDown) {
				myState = CharacterState.Attacking;
				actualAttackingTime = 0;
				attackType = AttackType.Mid;
				AttackCalculations(attackType);

			} else if (isLKeyDown) {
				myState = CharacterState.Attacking;
				actualAttackingTime = 0;
				attackType = AttackType.Bottom;
				AttackCalculations(attackType);
			}

            transform.GetComponent<AudioSource>().Play();

        }
            


        isJKeyDown = false;
        isKKeyDown = false;
        isLKeyDown = false;
    }


    void HorizontalMovement() {
        rb.velocity = new Vector2(movingSpeed * horizontalInput * Time.fixedDeltaTime , rb.velocity.y);

        if (horizontalInput == -1)
            myState = CharacterState.WalkingBackward;
        else if (horizontalInput == 1)
            myState = CharacterState.Walking;
            
        
    }

    void Jump() {
        if(isJumpKeyDown) {
            isJumpKeyDown = false;
            
            if(isGrounded) {
                rb.AddForce(new Vector2(0, 1) * jumpingForce, ForceMode2D.Impulse);
                isGrounded = false;
            }

        }

        if (isGrounded == false) {
            myState = CharacterState.Jump;
        }
            


    }

    void Crouch() {
		if (SisDown) {
			if (isGrounded) {
                myState = CharacterState.Crouch;
            }
		}
	}

    void FallingDown() {
        if(rb.velocity.y < 0) {
            rb.gravityScale = fallingMultiplier;
        } else {
            rb.gravityScale = 1;
        }
    }

	private void FixedUpdate() {
        myState = CharacterState.None;

        Jump();
        Crouch();


		HorizontalMovement();
        CalculateTime();
        Attack();



        FallingDown();
	}

	
	private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
	}

	private void OnCollisionExit2D(Collision2D collision) {
		if (collision.gameObject.CompareTag("Ground"))
			isGrounded = false;
	}



}
