using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefab")]
    [SerializeField] private EnemyController enemyPrefab;
    [SerializeField] private int spawnAmmount = 100;

    [SerializeField] private ObjectPool<EnemyController> enemyPool;
    // Start is called before the first frame update
    void Start()
    {
        enemyPool = new ObjectPool<EnemyController>(() => {
            return Instantiate(enemyPrefab);
        }, enemy => { enemy.gameObject.SetActive(true);
        }, enemy => { enemy.gameObject.SetActive(false);
        }, enemy => { Destroy(enemy.gameObject);
        });

        InvokeRepeating(nameof(Spawn), 0.2f, 2f);
    }

    void Spawn() {
        for (int i = 0; i < spawnAmmount; i++) {
            var enemy = enemyPool.Get();
            enemy.transform.position = transform.position + Random.onUnitSphere * 10;
        }
    }
}
