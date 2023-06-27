using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField]private GameObject[] monsterArray;
    [SerializeField] private GameObject enemyPin;
    [SerializeField] private GameObject Player;
    [SerializeField] private Transform[] spawnpoints;
    [SerializeField] private bool[] attendance;
    [SerializeField] private int enemyAttendance;
    [SerializeField] private int maxCount = 5;
    [SerializeField] private int index = 0;
    public static EnemySpawnManager instance;
    // Start is called before the first frame update
    public void Awake(){
        instance = this;
    }
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Spawning GameObj every 10 seconds
    //creat a corouting of type iEnumerator -- Yield Events
    //while loop

    [System.Obsolete]
    IEnumerator SpawnRoutine()
    {
        while(true)
        {
            if(enemyPin.transform.childCount < maxCount){
                index = Random.Range(0, spawnpoints.Length);
                Vector3 location = spawnpoints[index].position;
                enemyAttendance[index] = true;
                GameObject monster = Instantiate(monsterArray[Random.Range(0, monsterArray.Length)]);
                monster.transform.Translate(location);
                monster.transform.parent = enemyPin.transform;
            }
            
            yield return new WaitForSeconds(5.0f);
        }
        //while loop
            //instante mutilable enemies
            //yeild wait
    }

    public void PlayersDead(){
        StopCoroutine(SpawnRoutine());
    }
}
