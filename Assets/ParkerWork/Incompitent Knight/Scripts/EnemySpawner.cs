using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;       // empty GameObjects marking where enemies appear
    public float spawnInterval = 3f;
    public int maxAlive = 5;              // enemy cap

    private ObjectPool<GameObject> pool; // object pool!!!!!!!!!!!!!!!
    private int alive;
    private float nextSpawnTime;

    void Awake()
    {
        pool = new ObjectPool<GameObject>(
            createFunc: CreateEnemy,                 // runs only when the pool is empty
            actionOnGet: PrepareEnemy,               // runs every time enemy is taken out
            actionOnRelease: e => e.SetActive(false),
            actionOnDestroy: e => Destroy(e),
            collectionCheck: false,
            defaultCapacity: maxAlive,
            maxSize: maxAlive * 2);
    }

    void Update()
    {
        if (spawnPoints.Length == 0 || alive >= maxAlive) return;
        if (Time.time < nextSpawnTime) return;

        nextSpawnTime = Time.time + spawnInterval;
        alive++;
        pool.Get(); // grabs enemy from pool!!!!!!!!!! (if there are any in pool)
    }

    GameObject CreateEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, transform);

        // give enemy its "return me to queue" callback
        enemy.GetComponent<EnemyDeath>().returnToPool = ReturnEnemy;
        return enemy;
    }

    void PrepareEnemy(GameObject enemy)
    {
        enemy.GetComponent<Health>().ResetHealth();
        enemy.SetActive(true);

        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];

        //CharacterController overrides transform changes unless off while teleport
        CharacterController cc = enemy.GetComponent<CharacterController>();
        cc.enabled = false;
        enemy.transform.position = point.position;
        cc.enabled = true;
    }

    void ReturnEnemy(GameObject enemy)
    {
        alive--;
        pool.Release(enemy); // puts enemy back in pool!!!!!!!!!!!!!!!!
    }

    void OnDrawGizmos()
    {
        if (spawnPoints == null) return;
        Gizmos.color = Color.red;
        foreach (Transform p in spawnPoints)
            if (p != null) Gizmos.DrawWireSphere(p.position, 0.4f);
    }
}