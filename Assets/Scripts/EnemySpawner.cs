using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [Header("Player Information")]
    [SerializeField] private Transform player;

    [Header("Enemy Prefab")]
    [SerializeField] private EnemyController enemyPrefab;
    [SerializeField] private int spawnAmmount = 10;
    [SerializeField] private int currentSpawn;
    [SerializeField] private int maxSpawnAmmount = 50;

    [Header("Spawner Range")]
    public float maxDistance;

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

        // get player position
        player = GameObject.FindGameObjectWithTag("Player").transform;

        InvokeRepeating(nameof(Spawn), 0.2f, 5f);
    }

    void Spawn() {

        // cap the max amount of mushrooms on screen at a time
        if(currentSpawn >= maxSpawnAmmount) {
            currentSpawn = 0;
            return;
        }

        // check if the player is nearby
        if((transform.position - player.position).magnitude >= maxDistance) {
            return;
        }

        for (int i = 0; i < spawnAmmount; i++) {
            var enemy = enemyPool.Get();
            AudioManager.instance.Play("Shroom Spawns", true);
            enemy.transform.position = transform.position; //transform.position + Random.onUnitSphere * 100;

            enemy.Init(Kill);
            currentSpawn++;
        }
    }

    void Kill(EnemyController enemy) {
        AudioManager.instance.Play("Shroom Dies", true);
        enemyPool.Release(enemy);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }
}
