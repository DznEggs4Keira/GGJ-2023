using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefab")]
    [SerializeField] private EnemyController enemyPrefab;
    [SerializeField] private int spawnAmmount = 10;
    [SerializeField] private int currentSpawn;
    [SerializeField] private int maxSpawnAmmount = 50;

    [SerializeField] private ObjectPool<EnemyController> enemyPool;
    // Start is called before the first frame update
    void Start()
    {
        enemyPool = new ObjectPool<EnemyController>(() => {
            return Instantiate(enemyPrefab);
        }, enemy => { enemy.gameObject.SetActive(true);
        }, enemy => { enemy.gameObject.SetActive(false);
        }, enemy => { Destroy(enemy.gameObject);
        },true, 10, maxSpawnAmmount);

        InvokeRepeating(nameof(Spawn), 0.2f, 5f);
    }

    void Spawn() {

        if(currentSpawn >= maxSpawnAmmount) {
            currentSpawn = 0;
            return;
        }

        for (int i = 0; i < spawnAmmount; i++) {
            var enemy = enemyPool.Get();
            enemy.transform.position = transform.position; //transform.position + Random.onUnitSphere * 100;

            enemy.Init(Kill);
            currentSpawn++;
        }
    }

    void Kill(EnemyController enemy) {
        enemyPool.Release(enemy);
    }
}
