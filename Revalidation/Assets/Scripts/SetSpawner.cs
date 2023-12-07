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

    public bool _spawning = false;

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
        var spawnSet = GenerateSet();
        var vertices = new List<Vector3>(GetComponent<MeshFilter>().sharedMesh.vertices);

        var leftTop = transform.TransformPoint(vertices[0]);
        var rightTop = transform.TransformPoint(vertices[10]);
        var leftBottom = transform.TransformPoint(vertices[110]);

        var xAxis = rightTop - leftTop;
        var zAxis = leftBottom - leftTop;

        var spawnLocation = leftTop + xAxis + zAxis / 2;

        for (int i = 0; i < spawnSet.Length; i++)
        {
            spawnLocation = new Vector3(spawnLocation.x + 1, spawnLocation.y, spawnLocation.z);
            if (spawnSet[i] != null)
            {
                Instantiate(spawnSet[i].gameObject, spawnLocation, Quaternion.identity);
            }

        }
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