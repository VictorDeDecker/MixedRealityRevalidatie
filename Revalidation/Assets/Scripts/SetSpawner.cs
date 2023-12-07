using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSpawner : MonoBehaviour
{
    public List<TouchObject> Objects;

    public int SetWidth = 5;

    public int AmountOfSets = 45;

    public int LevelLengthInSec = 200;

    public float MaxPercentageOfMissingObjects = 0.5f;

    public int InfiniteSpawnWaitTime = 2;

    public bool ContainsDuckSets = false;

    public bool _spawning = false;
    public bool HeightRows = false;

    void Start()
    {
        StartCoroutine(StartSpawning());
    }

    private IEnumerator StartSpawning()
    {
        _spawning = true;
        while (_spawning)
        {
            SpawnSet();
            yield return new WaitForSeconds(InfiniteSpawnWaitTime);
        }
    }

    private void SpawnSet()
    {
        TouchObject[] spawnSet;
        if (ContainsDuckSets)
        {
            spawnSet = Random.Range(0, 10) == 0 ? GenerateFullSet() : GenerateSet();
        }
        else
        {
            spawnSet = GenerateSet();
        }

        var bottomSpawnSet = GenerateSet();

        var vertices = new List<Vector3>(GetComponent<MeshFilter>().sharedMesh.vertices);

        var leftTop = transform.TransformPoint(vertices[0]);
        var rightTop = transform.TransformPoint(vertices[10]);
        var leftBottom = transform.TransformPoint(vertices[110]);

        var xAxis = rightTop - leftTop;
        var zAxis = leftBottom - leftTop;

        var spawnLocation = leftTop + xAxis + zAxis / 2;
        var bottomSpawnLocation = leftTop + xAxis + zAxis / 2 - new Vector3(0, 1, 0);

        for (int i = 0; i < spawnSet.Length; i++)
        {
            spawnLocation = new Vector3(spawnLocation.x + 1, spawnLocation.y, spawnLocation.z);
            bottomSpawnLocation = new Vector3(bottomSpawnLocation.x + 1, bottomSpawnLocation.y, bottomSpawnLocation.z);
            if (spawnSet[i] is not null)
            {
                Instantiate(spawnSet[i].gameObject, spawnLocation, Quaternion.identity);
            }

            if (HeightRows)
            {

                if (bottomSpawnSet[i] is not null)
                {
                    Instantiate(bottomSpawnSet[i].gameObject, bottomSpawnLocation, Quaternion.identity);
                }
            }
        }
    }

    // Full set requires the user to duck in order to dodge the set
    private TouchObject[] GenerateFullSet()
    {
        var returnObject = new TouchObject[SetWidth];

        for (int i = 0; i < returnObject.Length; i++)
        {
            returnObject[i] = Objects[Random.Range(0, Objects.Count)];
        }

        return returnObject;
    }

    private TouchObject[] GenerateSet()
    {
        var returnObject = new TouchObject[SetWidth];

        var exclude = new List<int>();

        for (int i = 0; i < Mathf.Ceil(SetWidth * MaxPercentageOfMissingObjects); i++)
        {
            exclude.Add(Random.Range(0, SetWidth));
        }

        for (int i = 0; i < returnObject.Length; i++)
        {
            if (!exclude.Contains(i))
                returnObject[i] = Objects[Random.Range(0, Objects.Count)];
            else
                returnObject[i] = null;
        }

        return returnObject;
    }
}