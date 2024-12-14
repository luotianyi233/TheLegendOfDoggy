using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MagicBall : MonoBehaviour
{
    public enum BallStates { HitPlayer,HitNothing} 

    private Rigidbody rb;

    public  BallStates ballStates;

    [Header("Basic Settings")]

    public float force;

    public int damage;

    public GameObject target;

    private Vector3 direction;

    //public GameObject breakEffect;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.one;
        ballStates = BallStates.HitPlayer;
        FlyToTarget();
    }

    private void FixedUpdate()
    {
        if (rb.velocity.sqrMagnitude<1)
        {
            ballStates = BallStates.HitNothing;
        }
        if (ballStates == BallStates.HitNothing)
            Destroy(gameObject);
    }

    public void FlyToTarget()
    {
        if(target == null)
            target = FindObjectOfType<PlayerController>().gameObject;
        direction = (target.transform.position - transform.position).normalized;
        rb.AddForce(direction * force, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision other)
    {
        switch(ballStates)
        {
            case BallStates.HitPlayer:
                if (other.gameObject.CompareTag("Player"))
                {
                    other.gameObject.GetComponent<NavMeshAgent>().isStopped=true;
                    other.gameObject.GetComponent<NavMeshAgent>().velocity = direction * force;

                    other.gameObject.GetComponent<Animator>().SetTrigger("Dizzy");
                    other.gameObject.GetComponent<CharacterStats>().TakeDamage(damage,other.gameObject.GetComponent<CharacterStats>());

                    ballStates = BallStates.HitNothing;
                }
                break;
        }
    }
    
}
