using UnityEngine;
using DG.Tweening;
using System.Collections;

public class EnemyAI : MonoBehaviour
{

    public enum AIState { Creep, PreAttack, Attack }
    public AIState currentState;

    [Header("General Settings")]
    public Transform pointA;
    public float spawnRadius = 2.0f;

    [Header("Creep Settings")]
    public float creepDuration = 5.0f;
    private Tween creepTween;

    [Header("PreAttack Settings")]
    public float preAttachDistance = 2.0f;
    public float preAttackDuration = 2.0f;
    private Sequence preAtkTween = null;

    [Header("Attack Settings")]
    public float attackDistance = 1.0f;


    public delegate void OnHeroAtk(bool atkNow, EnemyAI self);
    public OnHeroAtk myAtk;

    private bool hasFinishedAttack = false;
    private void Start()
    {
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
        while (!hasFinishedAttack)
        {
            float distanceToPointA = Vector2.Distance(transform.position, pointA.position);

            switch (currentState)
            {
                case AIState.Creep:
                    if (!DOTween.IsTweening(transform))
                        Creep();
                    break;
                case AIState.PreAttack:
                    if (creepTween.IsPlaying())
                        creepTween.Kill();
                    if (preAtkTween == null)
                        PreAttack();
                    break;
                case AIState.Attack:
                    if (preAtkTween.IsPlaying())
                        preAtkTween.Kill();
                    Attack();
                    break;
            }

            if (distanceToPointA <= attackDistance)
            {
                currentState = AIState.Attack;
            }
            else if (distanceToPointA <= preAttachDistance && currentState != AIState.Attack)
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
        Debug.Log("Creeping");
        creepTween = transform.DOMove(pointA.position, creepDuration).SetEase(Ease.Linear);
    }


    //private void PreAttack()
    //{
    //    Debug.Log("Pre attack");
    //    Vector3 startPosition = transform.position;
    //    Vector3 endPosition = pointA.position;
    //    float zigzagWidth = 1.0f; // Width of the zigzag movement
    //    int zigzagCount = 3; // Number of zigzags

    //    preAtkTween = DOTween.Sequence();

    //    for (int i = 0; i < zigzagCount; i++)
    //    {
    //        // Calculate intermediate points for zigzag
    //        Vector3 direction = (endPosition - startPosition).normalized;
    //        Vector3 right = new Vector3(-direction.y, direction.x, direction.z); // Right vector perpendicular to direction
    //        Vector3 zigzagPoint = Vector3.Lerp(startPosition, endPosition, (i + 0.5f) / zigzagCount) + right * zigzagWidth * ((i % 2 == 0) ? 1 : -1);

    //        // Append movement to the zigzag point
    //        preAtkTween.Append(transform.DOMove(zigzagPoint, preAttackDuration / (zigzagCount * 2)).SetEase(Ease.Linear));
    //    }

    //    // Finally move to the end position
    //    preAtkTween.Append(transform.DOMove(endPosition, preAttackDuration / (zigzagCount * 2)).SetEase(Ease.Linear));
    //}

    private void PreAttack()
    {

        preAtkTween = DOTween.Sequence();

        preAtkTween.Append(transform.DOMove(pointA.position, preAttackDuration).SetEase(Ease.InOutQuad));




    }

    private void Attack()
    {
        DOTween.Kill(transform); // Stop any ongoing tween
        //myAtk.Invoke(true, this);
        hasFinishedAttack = true;
        // Additional attack behavior here
    }



    //public enum AIState { Creep, PreAttack, Attack }
    //public AIState currentState;

    //private Rigidbody2D rb;
    //private float creepTimer = 0f;
    //private float preAttackTimer = 0f;

    //[Header("General Settings")]
    //public Transform pointA;
    //public float spawnRadius = 2.0f; 

    //[Header("Creep Settings")]
    //public AnimationCurve creepSpeedCurve;
    //public float creepSpeedMultiplier = 1.0f;


    //[Header("PreAttack Settings")]
    //public float preAttackDistance = 1.0f;
    //public AnimationCurve preAttackSpeedCurve;
    //public float preAttackSpeedMultiplier = 2.0f;

    //[Header("Attack Settings")]
    //public float attackDistance = 1.0f;

    //private void Start()
    //{
    //    rb = GetComponent<Rigidbody2D>();
    //    SpawnAtRadiusFromPointA();
    //    currentState = AIState.Creep; 
    //    StartCoroutine(BehaviorCoroutine());
    //}

    //private void Update()
    //{
    //    LookAtPointA();
    //}

    //IEnumerator BehaviorCoroutine()
    //{
    //    while (true)
    //    {
    //        float distanceToPointA = Vector2.Distance(transform.position, pointA.position);

    //        switch (currentState)
    //        {
    //            case AIState.Creep:
    //                Creep();
    //                break;
    //            case AIState.PreAttack:
    //                PreAttack();
    //                break;
    //            case AIState.Attack:
    //                Attack();
    //                break;
    //        }

    //        if (distanceToPointA <= attackDistance)
    //        {
    //            currentState = AIState.Attack;
    //        }
    //        else if (distanceToPointA <= preAttackDistance && currentState != AIState.Attack)
    //        {
    //            currentState = AIState.PreAttack;
    //        }

    //        yield return null;
    //    }
    //}




    //private void Creep()
    //{
    //    creepTimer += Time.deltaTime;
    //    float speed = creepSpeedCurve.Evaluate(Mathf.InverseLerp(0, spawnRadius, creepTimer)) * creepSpeedMultiplier;
    //    Debug.Log(speed);
    //    MoveTowardsPointA(speed);
    //}

    //private void PreAttack()
    //{
    //    preAttackTimer += Time.deltaTime;
    //    float speed = preAttackSpeedCurve.Evaluate(preAttackTimer / preAttackDistance) * preAttackSpeedMultiplier;
    //    MoveTowardsPointA(speed);
    //}

    //private void Attack()
    //{
    //    rb.velocity = Vector2.zero;

    //}

    //private void MoveTowardsPointA(float speed)
    //{
    //    Vector2 direction = ((Vector2)pointA.position - rb.position).normalized;
    //    rb.velocity = direction * speed;
    //}
}
