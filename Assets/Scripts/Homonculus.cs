using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.PlayerSettings;

public class Homunculus : MonoBehaviour
{
    private NavMeshAgent agent;

    [SerializeField] private BaseEnemy nearEnemy = null;


    
    [SerializeField] public float speed;
    [SerializeField] private float explosionDamage;
    [SerializeField] public float explosionRange;


    [SerializeField] private LayerMask damagableObjects;

    [SerializeField] private GameObject explosionVFX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nearEnemy = FindNearestEnemy();

        agent = GetComponent<NavMeshAgent>();

        agent.speed = speed;

        
    }

    // Update is called once per frame
    void Update()
    {
        if (nearEnemy != null)
        {
            agent.SetDestination(nearEnemy.transform.position);


            if (Vector3.Distance(transform.position, nearEnemy.transform.position) <= 1f)
            {
                Explode();
            }
        }
        else
        {
            nearEnemy = FindNearestEnemy();
        }
    }


    private BaseEnemy FindNearestEnemy()
    {
        BaseEnemy nearestEnemy = null;
        float nearestDistance = float.MaxValue;

        BaseEnemy[] enemies = FindObjectsByType<BaseEnemy>(FindObjectsSortMode.None);

        foreach (BaseEnemy enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            NavMeshHit hit;
            var path = new NavMeshPath();

            if (NavMesh.SamplePosition(enemy.transform.position, out hit, 1.0f, NavMesh.AllAreas) && NavMesh.CalculatePath(transform.position, enemy.transform.position, 1, path) && path.status == NavMeshPathStatus.PathComplete && distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
        }



        return nearestEnemy;


    }

    private void Explode()
    {
        Instantiate(explosionVFX, transform.position, Quaternion.identity);
        FindFirstObjectByType<AudioManager>().PlayWorldSound("Explosion", transform.position, gameObject, explosionRange * 4);

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRange, damagableObjects);

        foreach (Collider collider in hitColliders)
        {
            BaseCharacter character = collider.GetComponent<BaseCharacter>();
            if (character != null)
            {
                character.TakeDamage(explosionDamage);
            }
        }


        Destroy(gameObject);
        

    }
}
