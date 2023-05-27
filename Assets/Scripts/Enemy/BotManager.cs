using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotManager : MonoBehaviour
{
    public GameObject playerObject;
    public GameObject ChaseBotPrefab;
    public int NumberofEnemies;
    public float SpawnCooldown;
    public GameObject[] SpawnPoints;


    private List<GameObject> ActiveEnemies;


    public void KillMe(GameObject obj) {
        ActiveEnemies.Remove(obj);
        Destroy(obj);
    }

    // Start is called before the first frame update
    void Start()
    {
        ActiveEnemies = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        //spawn enemies if not enough
        if (ActiveEnemies.Count < NumberofEnemies) {
            Debug.Log("SPAWNING");
            Vector3 spawnPoint;
            if (SpawnPoints.Length < 1) {
                spawnPoint = Vector3.zero;
            } else {
                spawnPoint= SpawnPoints[Random.Range(0, SpawnPoints.Length)].transform.position;
            }
            GameObject newEnemey = Instantiate(ChaseBotPrefab, spawnPoint, Quaternion.identity);
            ActiveEnemies.Add(newEnemey);
            newEnemey.GetComponent<BotController>().SetTarget(playerObject);
            newEnemey.GetComponent<Killable>().SetController(this);
        }
    }
}
