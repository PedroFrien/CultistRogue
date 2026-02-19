using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.Rendering;
using UnityEditor;

public abstract class BaseEnemy : BaseCharacter
{
    public NavMeshAgent agent;

    public float normalSpeed;
    public float sprintSpeed;

    public float sightRange;

    public bool Patrolling;
    public bool Investigating;
    public bool Chasing;

    public bool playerInCone;
    public float uprightRate;
    public float crouchDetectRate;
    public float detectRate => playerCon.CrouchInput ? crouchDetectRate : uprightRate;
    public float timeToDetect;
    public float detectValue;

    public LayerMask playerDetect;

    private Animator animator;

    public Healthbar exclamationPoint;
    public GameObject questionMark;

    



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
    private FPController playerCon;

    public GameObject bulletTrail;
    public Transform firePoint;

    [Header("Ranged Attributes")]
    public bool isRanged;
    public float range;
    public float bulletDamage;
    public float shootDelay;
    public float betweenShotDelay;
    public bool shooting;
    private Coroutine shootingCoroutine;




    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = normalSpeed;
        player = GameObject.FindFirstObjectByType<FPController>().gameObject;


        patrolPointIndex = 0;
        currentPatrolPoint = patrolPoints[patrolPointIndex];
        movementCoroutine = StartCoroutine(MoveToPos(currentPatrolPoint));

        animator = GetComponent<Animator>();

        playerCon = player.GetComponent<FPController>();
    }




    public virtual void StateUpdate()
    {
        if (move == true)
        {
            move = false;

            if (movementCoroutine != null) StopCoroutine(movementCoroutine);

            movementCoroutine = StartCoroutine(MoveToPos(manualPos));
        }


        if (shooting)
        {
            transform.LookAt(player.transform);
        }

        if (Chasing == true)
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= range && !shooting && PlayerInLOS())
            {                
                shootingCoroutine = StartCoroutine(Shoot());
            }


            else if (PlayerInLOS() && !shooting)
            {
                lastSeenPos = player.transform.position;
                agent.SetDestination(lastSeenPos);
                agent.speed = sprintSpeed;
            }
            else
            {
                if (Vector3.Distance(transform.position, lastSeenPos) <= 2f && !shooting)
                {
                    StartCoroutine(ResumePatrol());
                }
            }
        }

        if (playerInCone)
        {
            IncrementPopup(detectRate);
        }
        else
        {
            IncrementPopup(-detectRate);
        }

        questionMark.SetActive(Investigating);
        

    }

    public void IncrementPopup(float value)
    {

        detectValue += value;
        if (detectValue > timeToDetect)
        {
            StartChase();
        }
        detectValue = Mathf.Clamp(detectValue, 0, timeToDetect);

        float barValue = detectValue / timeToDetect;
        exclamationPoint.ChangeValue(barValue);

    }


    public void StartChase()
    {
        if (Chasing) return;

        FindFirstObjectByType<AudioManager>().PlaySound("Spotted", transform.position, gameObject);
        Debug.Log("Starting Chase");
        exclamationPoint.ChangeValue(0);
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
        NavMeshHit hit;
        var path = new NavMeshPath();

        if (NavMesh.SamplePosition(pos, out hit, 1.0f, NavMesh.AllAreas) && NavMesh.CalculatePath(transform.position, pos, 1, path))
        {
            if (movementCoroutine != null) StopCoroutine(movementCoroutine);


            movementCoroutine = StartCoroutine(MoveToPos(pos));

            Investigating = true;
        }


        
    }



    public IEnumerator ResumePatrol()
    {
        Chasing = false;

        yield return new WaitForSeconds(1);
    
        Patrolling = true;

        agent.speed = normalSpeed;

        if (movementCoroutine != null) StopCoroutine(movementCoroutine);
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
                lastSeenPos = player.transform.position;
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

    public virtual IEnumerator Shoot()
    {
        Debug.Log("Shooting");
        if (shooting) yield break;

        animator.SetTrigger("Shoot");
        

        shooting = true;        
        agent.SetDestination(transform.position);

        yield return new WaitForSeconds(shootDelay);

        FindFirstObjectByType<AudioManager>().PlaySound("Gunshot", transform.position, gameObject);

        Vector3 dir = (player.transform.position - firePoint.position).normalized;

        RaycastHit hit;
        Ray ray = new Ray(firePoint.position, dir);

        
        if (Physics.Raycast(ray, out hit, 99, playerDetect))
        {
            BaseCharacter character = hit.collider.GetComponent<BaseCharacter>();
            if (character != null)
            {
                character.TakeDamage(bulletDamage);
            }

            SpawnTrail(hit.point);
        }
        else
        {
            SpawnTrail(ray.GetPoint(99));
        }

          

        shooting = false;
        agent.SetDestination(lastSeenPos);



    }

    public void SpawnTrail(Vector3 hitPoint)
    {
        if (bulletTrail == null)
        {
            Debug.Log("No Trail set! Returning...");
            return;
        }

        GameObject line = Instantiate(bulletTrail, firePoint.position, Quaternion.identity);
        LineRenderer lineRen = line.GetComponent<LineRenderer>();

        lineRen.useWorldSpace = true;
        lineRen.SetPosition(0, firePoint.position);
        lineRen.SetPosition(1, hitPoint);

        //Destroy(line, trailDuration);
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
