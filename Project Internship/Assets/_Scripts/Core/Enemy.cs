using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Pawn {
    /*
    [Header("Moving Settings")]
    public bool isMovable;
    public float Movement_Speed, Ray_Distance;
    
    bool isRight = true;

    [SerializeField] Transform Player_Pos;
    [SerializeField] LayerMask Player_LayerMask;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void Update()
    {
        if(isMovable)
        {
            transform.Translate(Vector3.forward * Movement_Speed * Time.deltaTime);

            bool isGroundDetected = Physics2D.OverlapCircle(Player_Pos.position, 0.1f, Player_LayerMask);

            if(!isGroundDetected)
            {
                if (isRight)
                {
                    transform.localRotation = Quaternion.Euler(0, 90, 0);
                    isRight = false;
                }
                else
                {
                    transform.localRotation = Quaternion.Euler(0, -90, 0);
                    isRight = true;
                }
            }
        }
    } */

    void Start() {
        Init();
        animator = GetComponentInChildren<Animator>();
    }

    void FixedUpdate() {
        UpdateInAir();

        float vx = rigidbody.velocity.x;
        if (vx < -0.1f) vx = 5f;
        animator.SetFloat("forwardSpeed", vx);
        animator.SetBool("isGrounded",inAir);
    }

    public override void Death(GameObject instigator = null) {
        base.Death(instigator);
        animator.SetTrigger("death");
    }

    #region Attack Functionality. Refer to Pawn for full implementation.
    public override void OnAttackStartup(MonoBehaviour instigator, Collider2D[] hitboxes, Quaternion direction, float attackSpeed) {
        rigidbody.velocity = Vector2.zero;
        animator.SetTrigger("attack");
    }

    public override void OnAttackActive(MonoBehaviour instigator, Collider2D[] hitboxes, Quaternion direction, float attackSpeed) {
        hitboxes[0].enabled = true;
    }

    public override void OnAttackRecovery(MonoBehaviour instigator, Collider2D[] hitboxes, Quaternion direction, float attackSpeed) {
        hitboxes[0].enabled = false;
    }
    #endregion

    /*
    private void OnTriggerEnter2D(Collider2D enter) {

        Protagonist player = enter.gameObject.GetComponent<Protagonist>();
       
        if (enter.gameObject.tag == "Player"){
            if (enter.transform.position.x < transform.position.x) {
                enter.gameObject.GetComponent<Protagonist>().isKnockbackRight = true;
            } else
                enter.gameObject.GetComponent<Protagonist>().isKnockbackRight = false;
        }
    }*/
}
