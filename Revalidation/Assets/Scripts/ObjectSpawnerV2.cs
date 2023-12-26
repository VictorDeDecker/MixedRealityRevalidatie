using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;
using UnityEngine;

public class ObjectSpawnerV2 : MonoBehaviour
{
    public List<TouchObject> Objects;
    public MaterialStorage Storage;
    public UpdateProgressBar ProgressBar;
    //public Task
    public int LevelLengthInSec = 180;
    public int TimeBetweenSpawnsInSec = 2;
    public bool InfiniteSpawn = false;
    public float Height;
    public float SpawnRadius;
    public float SpaceBetween;
    public bool IncludeDucking = false;
    public bool IncludeMovement = false;
    public bool IncludeObstacles = true;
    public bool IsSpawning = true;

    //The colors that you want to spawn
    public List<string> ColorsToSpawn = new List<string>();

    void Start()
    {
        if (ProgressBar == null)
            ProgressBar = FindObjectOfType<UpdateProgressBar>();

        if (!IncludeObstacles)
        {
            for (int i = 0; i < Objects.Count; i++)
            {
                if (!Objects[i].CompareTag("Fish"))
                    Objects.Remove(Objects[i]);
            }

        }
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
            (Vector3)Random.insideUnitCircle.normalized * Random.Range(0, SpawnRadius) + new Vector3(SpaceBetween / 2, Height, 0) :
            (Vector3)Random.insideUnitCircle.normalized * Random.Range(0, SpawnRadius) + new Vector3(-SpaceBetween / 2, Height, 0);
        if (IncludeMovement)
            point = new Vector3(Random.Range(-SpawnRadius + 1.5f, SpawnRadius + 1.5f), Random.Range(Height - SpawnRadius, Height + SpawnRadius));
        if (IncludeDucking)
            point = new Vector3(Random.Range(-SpawnRadius, SpawnRadius), Random.Range(0, Height + SpawnRadius));
        if(IncludeMovement && IncludeDucking)
            point = new Vector3(Random.Range(-SpawnRadius + 1.5f, SpawnRadius + 1.5f), Random.Range(0, Height + SpawnRadius));

        return point + this.gameObject.transform.position;
    }

    private TouchObject UpdateMaterial(TouchObject touchObject)
    {
        ProgressBar.CheckColorAmount();
        if (Random.Range(0, 3) == 0 && ColorsToSpawn.Count > 0)
        {
            touchObject.IsTargetFish = true;
            var spawnObjectRenderer = touchObject.GetComponentInChildren<SkinnedMeshRenderer>();
            var material = GetAllowedColorMaterial();
            spawnObjectRenderer.material = material;
            touchObject.Color = material.name.ToLower();
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
        CheckColorsToSpawn();
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

    private void CheckColorsToSpawn()
    {
        if (ColorsToSpawn.Count < 0) return;

        if (ProgressBar.AllRedFishesCaught)
        {
            if (ColorsToSpawn.Contains("Red"))
                ColorsToSpawn.Remove("Red");
        }
        else
        {
            if (!ColorsToSpawn.Contains("Red"))
                ColorsToSpawn.Add("Red");
        }

        if (ProgressBar.AllPinkFishesCaught)
        {
            if (ColorsToSpawn.Contains("Pink"))
                ColorsToSpawn.Remove("Pink");
        }
        else
        {
            if (!ColorsToSpawn.Contains("Pink"))
                ColorsToSpawn.Add("Pink");
        }

        if (ProgressBar.AllGreenFishesCaught)
        {
            if (ColorsToSpawn.Contains("Green"))
                ColorsToSpawn.Remove("Green");
        }
        else
        {
            if (!ColorsToSpawn.Contains("Green"))
                ColorsToSpawn.Add("Green");
        }

        if (ProgressBar.AllYellowFishescaught)
        {
            if (ColorsToSpawn.Contains("Yellow"))
                ColorsToSpawn.Remove("Yellow");
        }
        else
        {
            if (!ColorsToSpawn.Contains("Yellow"))
                ColorsToSpawn.Add("Yellow");
        }
    }
}