using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public enum AIState { Creep, PreAttack, Attack }
    public AIState currentState;

    private Rigidbody2D rb;
    private float creepTimer = 0f;
    private float preAttackTimer = 0f;

    [Header("General Settings")]
    public Transform pointA;
    public float spawnRadius = 2.0f; 

    [Header("Creep Settings")]
    public AnimationCurve creepSpeedCurve;
    public float creepSpeedMultiplier = 1.0f;
   

    [Header("PreAttack Settings")]
    public float preAttackDistance = 1.0f;
    public AnimationCurve preAttackSpeedCurve;
    public float preAttackSpeedMultiplier = 2.0f;

    [Header("Attack Settings")]
    public float attackDistance = 1.0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SpawnAtRadiusFromPointA();
        currentState = AIState.Creep; 
        StartCoroutine(BehaviorCoroutine());
    }

    private void Update()
    {
        LookAtPointA();
    }

    IEnumerator BehaviorCoroutine()
    {
        while (true)
        {
            float distanceToPointA = Vector2.Distance(transform.position, pointA.position);

            switch (currentState)
            {
                case AIState.Creep:
                    Creep();
                    break;
                case AIState.PreAttack:
                    PreAttack();
                    break;
                case AIState.Attack:
                    Attack();
                    break;
            }

            if (distanceToPointA <= attackDistance)
            {
                currentState = AIState.Attack;
            }
            else if (distanceToPointA <= preAttackDistance && currentState != AIState.Attack)
            {
                currentState = AIState.PreAttack;
            }

            yield return null;
        }
    }

    private void LookAtPointA()
    {
        Vector2 directionToTarget = pointA.position - transform.position;
        float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90)); 
    }

    private void SpawnAtRadiusFromPointA()
    {
        float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad; 
        Vector2 offset = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * spawnRadius;
        transform.position = (Vector2)pointA.position + offset;
    }

    private void Creep()
    {
        creepTimer += Time.deltaTime;
        float speed = creepSpeedCurve.Evaluate(Mathf.InverseLerp(0, spawnRadius, creepTimer)) * creepSpeedMultiplier;
        Debug.Log(speed);
        MoveTowardsPointA(speed);
    }

    private void PreAttack()
    {
        preAttackTimer += Time.deltaTime;
        float speed = preAttackSpeedCurve.Evaluate(preAttackTimer / preAttackDistance) * preAttackSpeedMultiplier;
        MoveTowardsPointA(speed);
    }

    private void Attack()
    {
        rb.velocity = Vector2.zero;
     
    }

    private void MoveTowardsPointA(float speed)
    {
        Vector2 direction = ((Vector2)pointA.position - rb.position).normalized;
        rb.velocity = direction * speed;
    }
}
