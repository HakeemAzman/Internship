using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Controller {

    // Contains all of the states that this AI controller goes through.
    public enum State { patrolling, pursuing }
    State state;

    Vector3 origin;
    float currentMoveDirection = 1f;

    [Header("Patrol Settings")]
    public Transform target; // The target that we are pursuing.
    public float maximumPatrolDistance = 4f;
    public float detectionRange = 3f;
    public float stoppingDistance = 2f;
    public float attackDelay = 1f;
    public float attackDelayVariance = 1f;

    Coroutine attackOrder;

    // Start is called before the first frame update
    void Start() {
        Init();
        origin = transform.position;
        state = State.patrolling;
        if(!target) target = FindObjectOfType<PlayerController>().transform;
    }

    void Reset() {
        // Automatically assign the target first. Will make the system more complex later on.
        target = FindObjectOfType<Protagonist>().transform;
        Autofill();
    }

    // Update is called once per frame
    void Update() {

        if(!controlled.IsAlive()) return;

        Pawn w = controlled as Pawn;
        float playerDist;

        switch(state) {

            // When in the patrolling state, we just move left and right, being limited
            // by the maximum patrol distance.
            case State.patrolling:
                
                // If we are close to the target, change the state to pursuing.
                if(target) {
                    playerDist = Vector2.Distance(transform.position, target.position);
                    if(playerDist < detectionRange) {
                        state = State.pursuing;
                        break;
                    }
                }
                
                // Patrolling script for PatrollingEnemyController, built on Platformer.cs script.
                if (origin.x - transform.position.x > maximumPatrolDistance) {
                    currentMoveDirection = 1f;
                } else if (origin.x - transform.position.x < -maximumPatrolDistance) {
                    currentMoveDirection = -1f;
                } else if (currentMoveDirection == 0)
                    currentMoveDirection = Random.Range(-1,2);
                
                break;

            // When in the pursuing state, we will pursue an enemy until <stoppingDistance>, after
            // which we will try and hit them with attacks.
            case State.pursuing:

                // If the target is dead, continue patrolling.
                if(!target) {
                    state = State.patrolling;
                    break;
                }

                // Pursue the player.
                playerDist = Vector2.Distance(transform.position, target.position);
                if(playerDist < stoppingDistance){
                    currentMoveDirection = 0f;
                    if(!w.GetCurrentAttack() && attackOrder == null)
                        attackOrder = StartCoroutine(IssueAttackOrder());
                } else if(playerDist > detectionRange) {
                    state = State.patrolling;
                }  else {
                    if(transform.position.x > target.position.x) currentMoveDirection = -1f;
                    else currentMoveDirection = 1f;
                }

                break;
        }   
        
        w.Move(currentMoveDirection);
    }

    IEnumerator IssueAttackOrder() {
        yield return new WaitForSeconds(attackDelay + Random.value * attackDelayVariance);
        (controlled as Pawn).ReceiveAttackInput("Attack");
        attackOrder = null;
    }
}
