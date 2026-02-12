using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.Rendering;
using UnityEditor;

public class BaseEnemy : BaseCharacter
{
    public NavMeshAgent agent;

    public float normalSpeed;
    public float sprintSpeed;

    public float sightRange;

    public bool Patrolling;
    public bool Investigating;
    public bool Chasing;

    public LayerMask playerDetect;



    [Header("Patrolling")]
    public Vector3[] patrolPoints;
    [SerializeField] private float gizmoRadius = 0.3f;  
    [SerializeField] private Color patrolPointColor = Color.blue;  
    [SerializeField] private Color selectedPointColor = Color.red;  
    [SerializeField] private bool showPatrolLines = true;
    public int patrolPointIndex;
    public Vector3 currentPatrolPoint;


    [Header("Manual Movement Activation")]
    public Vector3 manualPos;
    public bool move;

    private Coroutine movementCoroutine;
    public Vector3 lastSeenPos;
    private GameObject player;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = normalSpeed;
        player = GameObject.FindFirstObjectByType<FPController>().gameObject;


        patrolPointIndex = 0;
        currentPatrolPoint = patrolPoints[patrolPointIndex];
        movementCoroutine = StartCoroutine(MoveToPos(currentPatrolPoint));
    }




    public virtual void StateUpdate()
    {
        if (move == true)
        {
            move = false;

            if (movementCoroutine != null) StopCoroutine(movementCoroutine);

            movementCoroutine = StartCoroutine(MoveToPos(manualPos));
        }


        if (Chasing == true)
        {
            if (PlayerInLOS())
            {
                lastSeenPos = player.transform.position;
                agent.SetDestination(lastSeenPos);
                agent.speed = sprintSpeed;
            }
            else
            {
                if (Vector3.Distance(transform.position, lastSeenPos) <= 2f)
                {
                    StartCoroutine(ResumePatrol());
                }
            }
        }

    }


    public void StartChase()
    {
        Debug.Log("Starting Chase");

        StopAllCoroutines();

        Investigating = false;

        Chasing = true;


    }

    public IEnumerator MoveToPos(Vector3 pos)
    {
        Debug.Log("Starting to move To Pos");
        agent.SetDestination(pos);

        while (Vector3.Distance(transform.position, pos) >= 1f)
        {
            yield return null;
            Debug.Log("Moving to pos");
        }

        if (Vector3.Distance(transform.position, currentPatrolPoint) <= 2f)
        {
            patrolPointIndex++;

            if (patrolPointIndex > patrolPoints.Length - 1) patrolPointIndex = 0;
            currentPatrolPoint = patrolPoints[patrolPointIndex];
        }

        Investigating = false;

        movementCoroutine = StartCoroutine(MoveToPos(currentPatrolPoint));
    }

    public void Investigate(Vector3 pos)
    {
        if (movementCoroutine != null) StopCoroutine(movementCoroutine);
     

        movementCoroutine = StartCoroutine(MoveToPos(pos));

        Investigating = true;
    }



    public IEnumerator ResumePatrol()
    {
        Chasing = false;

        yield return new WaitForSeconds(1);
    
        Patrolling = true;

        agent.speed = normalSpeed;

        StopCoroutine(movementCoroutine);
        movementCoroutine = StartCoroutine(MoveToPos(currentPatrolPoint));
    }

    public bool PlayerInLOS()
    {
        RaycastHit hit;
        Vector3 dir = (player.transform.position - transform.position).normalized;

        // Draw debug line in Scene view
        Debug.DrawRay(transform.position, dir * sightRange, Color.red, 0.0f, false);

        if (Physics.Raycast(transform.position, dir, out hit, sightRange, playerDetect))
        {
            Debug.Log("Hit SOMETHING");
            if (hit.collider.CompareTag("Player"))
            {
                // Change line color to green when player is detected
                Debug.DrawRay(transform.position, dir * hit.distance, Color.green, 0.0f, false);
                Debug.Log("Player in Sight");
                return true;
            }
            else
            {
                // Draw yellow line when hitting something else
                Debug.DrawRay(transform.position, dir * hit.distance, Color.yellow, 0.0f, false);
                Debug.Log("Player is NOT in Sight");
                return false;
            }
        }
        else
        {
            // Draw red line when nothing is hit within sightRange
            Debug.DrawRay(transform.position, dir * sightRange, Color.red, 0.0f, false);
            Debug.Log("Player is NOT in Sight");
            return false;
        }
    }


    #region Patrol Gizmos
    private void OnDrawGizmos()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
            return;

        // Draw patrol points
        for (int i = 0; i < patrolPoints.Length; i++)
        {
            // Use selected color for current patrol point in play mode
            if (Application.isPlaying && i == patrolPointIndex)
                Gizmos.color = selectedPointColor;
            else
                Gizmos.color = patrolPointColor;

            // Draw the point
            Gizmos.DrawSphere(patrolPoints[i], gizmoRadius);

            // Label the point with its index
#if UNITY_EDITOR
            UnityEditor.Handles.Label(patrolPoints[i] + Vector3.up * gizmoRadius * 2, "Point " + i);
#endif
        }

        // Draw lines between patrol points if enabled
        if (showPatrolLines && patrolPoints.Length > 1)
        {
            Gizmos.color = patrolPointColor;
            for (int i = 0; i < patrolPoints.Length; i++)
            {
                Vector3 startPoint = patrolPoints[i];
                Vector3 endPoint = patrolPoints[(i + 1) % patrolPoints.Length]; // Loop back to first point
                Gizmos.DrawLine(startPoint, endPoint);
            }
        }

        // Draw a line from this guard to the current patrol point in play mode
        if (Application.isPlaying && Patrolling)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, currentPatrolPoint);
        }

        // Draw the last player position when chasing or investigating
        if (Application.isPlaying && (Chasing || Investigating))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(lastSeenPos, gizmoRadius * 1.5f); // Slightly larger than patrol points
#if UNITY_EDITOR
            UnityEditor.Handles.Label(lastSeenPos + Vector3.up * gizmoRadius * 2, "Last Player Position");
#endif
        }
    }

    // This helps you add or move patrol points in the editor
#if UNITY_EDITOR
    [ContextMenu("Add Patrol Point")]
    private void AddPatrolPoint()
    {
        Vector3 newPointPosition = transform.position;
        if (patrolPoints != null && patrolPoints.Length > 0)
        {
            // Position new point a bit forward from the last point
            newPointPosition = patrolPoints[patrolPoints.Length - 1] + transform.forward * 2f;
        }

        // Create new array with one extra slot
        Vector3[] newPatrolPoints = new Vector3[(patrolPoints != null) ? patrolPoints.Length + 1 : 1];

        // Copy existing points
        if (patrolPoints != null)
        {
            for (int i = 0; i < patrolPoints.Length; i++)
            {
                newPatrolPoints[i] = patrolPoints[i];
            }
        }

        // Add new point at the end
        newPatrolPoints[newPatrolPoints.Length - 1] = newPointPosition;

        // Assign the new array
        patrolPoints = newPatrolPoints;

        UnityEditor.EditorUtility.SetDirty(this);
    }

    [ContextMenu("Clear All Patrol Points")]
    private void ClearPatrolPoints()
    {
        patrolPoints = new Vector3[0];
        UnityEditor.EditorUtility.SetDirty(this);
    }


#endif
#endregion
}
