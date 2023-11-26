using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // close, medium, long
    [SerializeField] public GameObject player;
    [SerializeField] public float delayEnemySpawn;
    [SerializeField] List<EnemyAI> enemyTypes;
    [SerializeField] WeaponSO weapons;
    [SerializeField] List<Sprite> enemyColor;

    [SerializeField] Vector2 spawnRateRange = new Vector2(1f, 5f);

    [SerializeField] List<EnemyAI> spawnedEnemies;

    public bool hasLost = false;

    float nextTimeToSpawn;
    float counter;

    [SerializeField] public DuelManager duelManager;
    public bool onStart = true;

    [SerializeField] FullScreenEffectManager effectManager;
    [SerializeField] private List<DificultySO> listOfDificultInOrder;
    public DificultySO currentDifficulty;
    [SerializeField] public Vector2 rangeAmmountToIncreaseDifficulty = new Vector2(10f, 15f);
    private int difficultyAmmount = 0;
    private int difficultyIndex = -1;
    private void Start()
    {
        if (onStart)
            StartSpawningHeros();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            effectManager.TotalCount = 0;
            duelManager.RestartDuels();
            StartSpawningHeros();
        }
    }
    public void StartSpawningHeros()
    {
        if (spawnedEnemies != null)
        {
            foreach (var enemy in spawnedEnemies)
            {
                if (enemy != null)
                {
                    enemy.gameObject.SetActive(false);
                    Destroy(enemy);
                }

            }
            spawnedEnemies.Clear();
        }
        StartCoroutine(StartSpawning());
    }

    public void SetDifficulty()
    {

        if (effectManager.TotalCount % difficultyAmmount == 0)
        {
            if (difficultyIndex < listOfDificultInOrder.Count - 1)
            {
                difficultyIndex++;
                difficultyAmmount = (int)Random.Range(rangeAmmountToIncreaseDifficulty.x, rangeAmmountToIncreaseDifficulty.y);
                switch (difficultyIndex)
                {
                    case 3:
                        duelManager.epicTimeHold = duelManager.epicTimeHold / 2;
                        break;

                    case 5:
                        duelManager.epicTimeHold = duelManager.epicTimeHold / 2;
                        break;

                    default:
                        break;
                }

            }
            currentDifficulty = listOfDificultInOrder[difficultyIndex];
        }
    }



    IEnumerator StartSpawning()
    {
        difficultyAmmount = (int)Random.Range(rangeAmmountToIncreaseDifficulty.x, rangeAmmountToIncreaseDifficulty.y);

        yield return new WaitForSeconds(delayEnemySpawn);
        while (duelManager.player.isAlive)
        {
            counter -= Time.deltaTime;
            // Debug.Log(counter);
            if (counter <= 0)
            {
                SpawnEnemy();
            }
            yield return null;
        }
    }


    public void SpawnEnemy()
    {
        SetDifficulty();
        nextTimeToSpawn = Random.Range(currentDifficulty.timeSpawnRange.x, currentDifficulty.timeSpawnRange.y);
        counter = nextTimeToSpawn;
        int enemyType = (int)currentDifficulty.typesOfEnemy;
        if (currentDifficulty.typesOfEnemy == DificultySO.TypesOfEnemy.Both)
            enemyType = Random.Range(0, enemyTypes.Count);
        EnemyAI newEnemy = Instantiate(enemyTypes[enemyType], this.transform);

        InitEnemy(newEnemy);
        spawnedEnemies.Add(newEnemy);
        duelManager.SubscribeEnemy(newEnemy);
    }

    public void InitEnemy(EnemyAI enemy)
    {
        enemy.gameObject.SetActive(true);
        enemy.mainCharacter = player.transform;
        enemy.myColor = enemyColor[Random.Range(0, enemyColor.Count - 1)];
        enemy.zigzag = currentDifficulty.shouldZigZag;
        enemy.zigzagCount = currentDifficulty.zigZagCount;
        enemy.zigzagWidth = currentDifficulty.zigZagWidth;
        GameObject newWeapon;
        switch (enemy.myRange)
        {
            case EnemyAI.AIRange.Close:

                newWeapon = Instantiate(weapons.closeRangeWeaponCollection[Random.Range(0, weapons.closeRangeWeaponCollection.Count)],
                   enemy.myWeapon.transform.position, Quaternion.identity, enemy.myWeapon.transform);
                break;
            case EnemyAI.AIRange.Medium:
                newWeapon = Instantiate(weapons.mediumRangeWeaponCollection[Random.Range(0, weapons.mediumRangeWeaponCollection.Count)],
                    enemy.myWeapon.transform.position, Quaternion.identity, enemy.myWeapon.transform);
                break;
            case EnemyAI.AIRange.Long:
                newWeapon = Instantiate(weapons.longRangeWeaponCollection[Random.Range(0, weapons.longRangeWeaponCollection.Count)],
                    enemy.myWeapon.transform.position, Quaternion.identity, enemy.myWeapon.transform);
                break;
            default:
                break;
        }
        enemy.enabled = true;
    }

}
