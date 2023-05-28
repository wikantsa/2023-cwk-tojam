using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotManager : MonoBehaviour
{
    public GameObject playerObject;
    public GameObject ZombieBotPrefab;
    public GameObject FlyBotPrefab;
    public float FlyBotSpawnHeight;
    public GameObject BigBotPrefab; //O Lawd he comin
    public int NumberofEnemies;
    public float SpawnCooldown;
    public GameObject[] SpawnPoints;



    private List<GameObject> ActiveEnemies;
    private float timer;

    public void KillMe(GameObject obj) {
        ActiveEnemies.Remove(obj);
        Destroy(obj);
    }

    // Start is called before the first frame update
    void Start()
    {
        ActiveEnemies = new List<GameObject>();
        timer = SpawnCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        //spawn enemies if not enough
        if (timer < 0 && ActiveEnemies.Count < NumberofEnemies) {
            Vector3 spawnPoint = GetSpawnPoint();
            int EnemyTableRoll = Random.Range(0, 10);
            if (EnemyTableRoll > 1) {
                SpawnZombieBot(spawnPoint);
            } else if (EnemyTableRoll == 1){
                SpawnFlyBot(spawnPoint);
            } else {
                SpawnBigBot(spawnPoint);
            }
        }
    }

    Vector3 GetSpawnPoint(){
        Vector3 spawnPoint;
        if (SpawnPoints.Length < 1) {
            spawnPoint = Vector3.zero;
        } else {
            spawnPoint= SpawnPoints[Random.Range(0, SpawnPoints.Length)].transform.position;
        }
        return spawnPoint;
    }

    void SpawnZombieBot(Vector3 spawnPoint) {
        GameObject newEnemey = Instantiate(ZombieBotPrefab, spawnPoint, Quaternion.identity);
        ActiveEnemies.Add(newEnemey);
        newEnemey.GetComponent<ITargetSettable>().SetTarget(playerObject);
        newEnemey.GetComponent<Killable>().SetController(this);
    }
     void SpawnBigBot(Vector3 spawnPoint) {
        GameObject newEnemey = Instantiate(BigBotPrefab, spawnPoint, Quaternion.identity);
        ActiveEnemies.Add(newEnemey);
        newEnemey.GetComponent<ITargetSettable>().SetTarget(playerObject);
        newEnemey.GetComponent<Killable>().SetController(this);
    }

    void SpawnFlyBot(Vector3 spawnPoint) {
        GameObject newEnemey = Instantiate(FlyBotPrefab, spawnPoint + (Vector3.up*FlyBotSpawnHeight), Quaternion.identity);
        ActiveEnemies.Add(newEnemey);
        newEnemey.GetComponent<ITargetSettable>().SetTarget(playerObject);
        newEnemey.GetComponent<Killable>().SetController(this);
    }
}
