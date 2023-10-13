using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectScroller : MonoBehaviour
{
    public List<Vector3> SpawnLocations;
    public List<GameObject> SpawnObjects;
    private GameObject _currentObject;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!_currentObject || _currentObject.IsDestroyed())
            SpawnObject();

    }

    private void SpawnObject()
    {
        var spawnLocation = SpawnLocations[Mathf.FloorToInt(SpawnLocations.Count * Random.value)];
        var spawnObject = SpawnObjects[Mathf.FloorToInt(SpawnObjects.Count * Random.value)];

        _currentObject = Instantiate(spawnObject, spawnLocation, Quaternion.identity);
    }
}
