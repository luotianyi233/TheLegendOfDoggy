using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Rock : MonoBehaviour
{
    public enum RockStates { HitPlayer,HitEnemy,HitNothing} 

    private Rigidbody rb;

    public  RockStates rockStates;

    [Header("Basic Settings")]

    public float force;

    public int damage;

    public GameObject target;

    private Vector3 direction;

    public GameObject breakEffect;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.one;
        rockStates = RockStates.HitPlayer;
        FlyToTarget();
    }

    private void FixedUpdate()
    {
        if (rb.velocity.sqrMagnitude<1)
        {
            rockStates = RockStates.HitNothing;
        }
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
        switch(rockStates)
        {
            case RockStates.HitPlayer:
                if (other.gameObject.CompareTag("Player"))
                {
                    other.gameObject.GetComponent<NavMeshAgent>().isStopped=true;
                    other.gameObject.GetComponent<NavMeshAgent>().velocity = direction * force;

                    other.gameObject.GetComponent<Animator>().SetTrigger("Dizzy");
                    other.gameObject.GetComponent<CharacterStats>().TakeDamage(damage,other.gameObject.GetComponent<CharacterStats>());

                    rockStates = RockStates.HitNothing;
                }
                break;

            case RockStates.HitEnemy:
                if (other.gameObject.GetComponent<Golem_Boss>())
                {
                    var otherStats = other.gameObject.GetComponent<CharacterStats>();
                    otherStats.TakeDamage(damage, otherStats);
                    Instantiate(breakEffect, transform.position,Quaternion.identity);
                    Destroy(gameObject);
                }
                break;
        }
    }
    
}
