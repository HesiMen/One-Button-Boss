using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // close, medium, long
    [SerializeField] public GameObject player;
    [SerializeField] List<EnemyAI> enemyTypes;
    [SerializeField] WeaponSO weapons;
    [SerializeField] List<Sprite> enemyColor;

    [SerializeField] Vector2 spawnRateRange = new Vector2(1f, 5f);

    [SerializeField] List<EnemyAI> spawnedEnemies;

    public bool hasLost = false;

    float nextTimeToSpawn;
    float counter;

    [SerializeField] public DuelManager duelManager;
    private void Start()
    {
        SpawnEnemy();
    }
    private void Update()
    {
        if (!hasLost)
        {
            counter -= Time.deltaTime;
            if (counter <= 0)
            {
                SpawnEnemy();
            }

        }
    }

    public void SpawnEnemy()
    {
        nextTimeToSpawn = Random.RandomRange(spawnRateRange.x, spawnRateRange.y);
        counter = nextTimeToSpawn;

        EnemyAI newEnemy = Instantiate(enemyTypes[Random.Range(0, enemyTypes.Count)], this.transform);

        InitEnemy(newEnemy);
        spawnedEnemies.Add(newEnemy);
        duelManager.SubscribeEnemy(newEnemy);
    }

    public void InitEnemy(EnemyAI enemy)
    {

        enemy.mainCharacter = player.transform;
        enemy.myColor = enemyColor[Random.Range(0, enemyColor.Count - 1)];
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
