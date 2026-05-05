using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "TestDistract", menuName = "Abilities/TestDistract")]
public class TestDistract : BaseAbility
{
    public float range;
    public float upgradedRange;
    private Transform player;

    public override void OnStart()
    {
        AbilityManager abilityManager = FindFirstObjectByType<AbilityManager>();
        abilityManager.incrementCoolown.AddListener(IncrementCooldown);
        player = GameObject.FindWithTag("Player").transform;
    }

    public override void Activate()
    {
        float activeRange = upgraded ? upgradedRange : range;

        BaseEnemy nearestEnemy = GetNavMeshNearestEnemy(activeRange);

        if (nearestEnemy != null)
        {
            Debug.Log("Trying to distract enemy");
            nearestEnemy.Investigate(player.position);
        }

        onCooldown = true;
    }

    private BaseEnemy GetNavMeshNearestEnemy(float activeRange)
    {
        Collider[] colliders = Physics.OverlapSphere(player.position, activeRange);

        BaseEnemy nearestEnemy = null;
        float shortestPathDistance = Mathf.Infinity;

        foreach (Collider collider in colliders)
        {
            BaseEnemy enemy = collider.GetComponent<BaseEnemy>();
            if (enemy == null) continue;

            NavMeshPath path = new NavMeshPath();
            bool pathFound = NavMesh.CalculatePath(player.position, enemy.transform.position, NavMesh.AllAreas, path);

            if (!pathFound || path.status == NavMeshPathStatus.PathInvalid) continue;

            float pathDistance = GetPathLength(path);

            if (pathDistance < shortestPathDistance)
            {
                shortestPathDistance = pathDistance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }

    private float GetPathLength(NavMeshPath path)
    {
        float length = 0f;

        if (path.corners.Length < 2) return length;

        for (int i = 1; i < path.corners.Length; i++)
        {
            length += Vector3.Distance(path.corners[i - 1], path.corners[i]);
        }

        return length;
    }
}