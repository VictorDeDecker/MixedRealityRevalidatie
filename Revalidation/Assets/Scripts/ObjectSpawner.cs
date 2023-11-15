using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public List<TouchObject> SpawnObjects;
    public uint AmountOfObjects = 10;
    public uint SpawnDurationInSec = 60;
    private bool _spawning = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!_spawning)
        {
            StartCoroutine(StartSpawner());
        }

    }

    private IEnumerator StartSpawner()
    {
        _spawning = true;
        for (int i = 0; i <= AmountOfObjects; i++)
        {
            SpawnObject();
            yield return new WaitForSeconds(SpawnDurationInSec / AmountOfObjects);
        }
    }

    private void SpawnObject()
    {
        var spawnLocation = RandomPointOnPlane();
        var spawnObject = SpawnObjects[Mathf.FloorToInt(SpawnObjects.Count * Random.value)];

        var newObject = Instantiate(spawnObject, spawnLocation, Quaternion.identity);
    }

    private Vector3 RandomPointOnPlane()
    {
        List<Vector3> verticeList = new List<Vector3>(GetComponent<MeshFilter>().sharedMesh.vertices);
        var leftTop = transform.TransformPoint(verticeList[0]);
        var rightTop = transform.TransformPoint(verticeList[10]);
        var leftBottom = transform.TransformPoint(verticeList[110]);

        var xAxis = rightTop - leftTop;
        var zAxis = leftBottom - leftTop;

        var rndPoint = leftTop + xAxis * Random.value + zAxis * Random.value;

        return rndPoint;
    }
}
