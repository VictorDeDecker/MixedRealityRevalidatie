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

    //The colors that you want to spawn
    public List<string> ColorsToSpawn = new List<string>();

    //Amount of objects to hit per color
    public Dictionary<string, int> AmountPerColor = new Dictionary<string, int>();

    private MaterialStorage Storage;

    void Start()
    {
        Storage = FindObjectOfType(typeof(MaterialStorage)) as MaterialStorage;
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

        if (spawnObject.gameObject.CompareTag("Fish"))
            UpdateMaterial(spawnObject);


        Instantiate(spawnObject.gameObject, RandomSpawnPoint(), Quaternion.identity);
    }

    private Vector3 RandomSpawnPoint()
    {
        Vector3 point = Random.value > 0.5 ?
            (Vector3)Random.insideUnitCircle.normalized * Random.Range(0, SpawnRadius) + new Vector3(SpaceBetween / 2, 0, 0) :
            (Vector3)Random.insideUnitCircle.normalized * Random.Range(0, SpawnRadius) + new Vector3(-SpaceBetween / 2, 0, 0);

        return point + this.gameObject.transform.position + new Vector3(0, Height, 0);
    }

    private TouchObject UpdateMaterial(TouchObject touchObject)
    {
        if (Random.Range(0, 3) == 0)
        {
            touchObject.IsTargetFish = true;
            var spawnObjectRenderer = touchObject.GetComponentInChildren<SkinnedMeshRenderer>();
            var material = GetAllowedColorMaterial();
            spawnObjectRenderer.material = material;
        }
        else
        {
            touchObject.IsTargetFish = false;
            var spawnObjectRenderer = touchObject.GetComponentInChildren<SkinnedMeshRenderer>();

            if (Random.Range(0, 2) == 0)
                spawnObjectRenderer.material = Storage.MaterialDictionary["Main"];
            else
                spawnObjectRenderer.material = Storage.MaterialDictionary["balloon"];
        }
        return touchObject;
    }

    private Material GetAllowedColorMaterial()
    {
        var material = Storage.GetHighlightedMaterials()[Random.Range(0, 4)];
        bool allowColor = false;
        for (int i = 0; i < ColorsToSpawn.Count; i++)
        {
            if (material.name.ToLower() == ColorsToSpawn[i].ToLower())
                allowColor = true;
        }

        if (allowColor)
        {
            return material;
        }
        else
        {
            return GetAllowedColorMaterial();
        }
    }
}
