using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Grunt_Boss : EnemyController
{
    [Header("Skill")]

    public float kickForce = 10;

    public GameObject magicballPrefab;

    public Transform shootPos;

    //Animation Event

    public void KickOff()
    {
        if (attackTarget != null)
        {
            transform.LookAt(attackTarget.transform);

            Vector3 direction = (attackTarget.transform.position - transform.position).normalized;
            //direction.Normalize();

            attackTarget.GetComponent<NavMeshAgent>().isStopped = true;
            attackTarget.GetComponent<NavMeshAgent>().velocity = direction * kickForce;
            attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");
        }
    }
    public void ThrowRock()
    {
        if(attackTarget != null)
        {
            var magicball = Instantiate(magicballPrefab, shootPos.position, Quaternion.identity);
            magicball.GetComponent<MagicBall>().target = attackTarget;
            
        }
    }
}
