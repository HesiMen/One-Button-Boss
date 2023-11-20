using UnityEngine;
using DG.Tweening;
using System.Collections;


//Base enemy AI
public class EnemyAI : MonoBehaviour, IDamageable
{

    public enum AIState { Creep, PreAttack, Attack }
    public enum AIRange { Close = 1, Medium = 2, Long = 3 }
    public AIState currentState;
    public AIRange myRange = AIRange.Close;

    [SerializeField] int health = 1;
    private int currHealth;





    [Header("General Settings")]
    [SerializeField] Transform mainCharacter;
    [SerializeField] float spawnRadius = 2.0f;

    [Header("Creep Settings")]
    [SerializeField] float creepDuration = 5.0f;
    private Tween creepTween;

    [Header("PreAttack Settings")]
    [SerializeField] float preAttachDistance = 2.0f;
    [SerializeField] float preAttackDuration = 2.0f;
    private Sequence preAtkTween = null;

    //[Header("Attack Settings")]
    private int attackDistance;


    public delegate void OnHeroAtk(bool atkNow, EnemyAI self);
    public OnHeroAtk myAtk;

    private bool hasFinishedAttack = false;

    [SerializeField] bool zigzag = false;
    [SerializeField] float zigzagWidth = 1.0f; // Width of the zigzag movement
    [SerializeField] int zigzagCount = 3; // Number of zigzags
    private void Start()
    {
        Initiate();
        SpawnAtRadiusFromPointA();
        currentState = AIState.Creep;
        StartCoroutine(BehaviorCoroutine());
    }

    private void Initiate()
    {
        currHealth = health;
        attackDistance = (int)myRange;
    }
    private void Update()
    {
        LookAtPointA();
    }

    IEnumerator BehaviorCoroutine()
    {
        while (!hasFinishedAttack)
        {
            float distanceToPointA = Vector2.Distance(transform.position, mainCharacter.position);

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
                    if (preAtkTween != null && preAtkTween.IsPlaying())
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
        Vector2 directionToTarget = mainCharacter.position - transform.position;
        float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }

    private void SpawnAtRadiusFromPointA()
    {
        float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector2 offset = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * spawnRadius;
        transform.position = (Vector2)mainCharacter.position + offset;
    }

    private void Creep()
    {
        Debug.Log("Creeping");
        creepTween = transform.DOMove(mainCharacter.position, creepDuration).SetEase(Ease.Linear);
    }




    public virtual void PreAttack()
    {

        if (zigzag)
        {
            Vector3 startPosition = transform.position;
            Vector3 endPosition = mainCharacter.position;


            preAtkTween = DOTween.Sequence();

            for (int i = 0; i < zigzagCount; i++)
            {
                // Calculate intermediate points for zigzag
                Vector3 direction = (endPosition - startPosition).normalized;
                Vector3 right = new Vector3(-direction.y, direction.x, direction.z); // Right vector perpendicular to direction
                Vector3 zigzagPoint = Vector3.Lerp(startPosition, endPosition, (i + 0.5f) / zigzagCount) + right * zigzagWidth * ((i % 2 == 0) ? 1 : -1);

                // Append movement to the zigzag point
                preAtkTween.Append(transform.DOMove(zigzagPoint, preAttackDuration / (zigzagCount * 2)).SetEase(Ease.Linear));
            }

            // Finally move to the end position
            preAtkTween.Append(transform.DOMove(endPosition, preAttackDuration / (zigzagCount * 2)).SetEase(Ease.Linear));
        }
        else
        {
            preAtkTween.Append(transform.DOMove(mainCharacter.position, preAttackDuration).SetEase(Ease.InOutQuad));
        }




    }

    public virtual void Attack()
    {
        DOTween.Kill(transform); // Stop any ongoing tween
        //myAtk.Invoke(true, this);
        hasFinishedAttack = true;
        // Additional attack behavior here
    }

    public void Damage(int damageHit)
    {
        currHealth = currHealth - damageHit;
        if (currHealth <= 0)
        {
            Kill();
        }
    }

    public virtual void Kill()
    {

    }

}
