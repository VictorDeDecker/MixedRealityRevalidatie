using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawnerV2 : MonoBehaviour
{
    public List<TouchObject> Objects;
    public int LevelLengthInSec = 180;
    public int TimeBetweenSpawnsInSec = 2;
    public bool InfiniteSpawn = false;
    public float Height;
    public float SpawnRadius;
    public float SpaceBetween;
    public bool IncludeDucking = false;
    public bool IncludeMovement = false;
    public bool IsSpawning = true;


    void Start()
    {
        StartCoroutine(StartSpawning());
    }

    private IEnumerator StartSpawning()
    {

        while ((LevelLengthInSec > 0 || InfiniteSpawn) && IsSpawning)
        {
            LevelLengthInSec--;
            SpawnObject();
            yield return new WaitForSeconds(TimeBetweenSpawnsInSec);
        }
    }

    private void SpawnObject()
    {
        var spawnObject = Objects[Mathf.FloorToInt(Objects.Count * Random.value)];

        Instantiate(spawnObject.gameObject, RandomSpawnPoint(), Quaternion.identity);
    }

    private Vector3 RandomSpawnPoint()
    {
        Vector3 point = Random.value > 0.5 ?
            (Vector3)Random.insideUnitCircle.normalized * Random.Range(0, SpawnRadius) + new Vector3(SpaceBetween / 2, 0, 0) :
            (Vector3)Random.insideUnitCircle.normalized * Random.Range(0, SpawnRadius) + new Vector3(-SpaceBetween / 2, 0, 0);

        return point + this.gameObject.transform.position + new Vector3(0, Height, 0);
    }
}
