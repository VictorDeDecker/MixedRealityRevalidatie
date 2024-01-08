using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectSpawnerV2 : MonoBehaviour
{
    public List<TouchObject> Objects;
    private List<TouchObject> objectsToUse;
    public MaterialStorage Storage;
    public UpdateProgressBar ProgressBar;
    public int LevelLengthInSec = 180;
    public int TimeBetweenSpawnsInSec = 2;
    public bool InfiniteSpawn = false;
    public float Height;
    public float SpawnRadius;
    public float SpaceBetween;
    public bool IncludeDucking = false;
    public bool IncludeMovement = false;
    public bool IncludeObstacles = false;
    public bool IsSpawning = true;
    public string Scene = "Level2";

    //The colors that you want to spawn
    public List<string> ColorsToSpawn = new List<string>();

    void Start()
    {
        if (ProgressBar == null)
            ProgressBar = FindObjectOfType<UpdateProgressBar>();

        objectsToUse = Objects;

        StartCoroutine(StartSpawning());
    }

    private IEnumerator StartSpawning()
    {

        while ((LevelLengthInSec > 0 || InfiniteSpawn) && IsSpawning)
        {
            if (!IncludeObstacles)
            {
                objectsToUse = Objects.Where(obj => obj.CompareTag("Fish")).ToList();
            }
            else
            {
                objectsToUse = Objects;
            }
            LevelLengthInSec--;
            SpawnObject();
            yield return new WaitForSeconds(TimeBetweenSpawnsInSec);
        }
    }

    private void SpawnObject()
    {
        var spawnObject = objectsToUse[Mathf.FloorToInt(objectsToUse.Count * Random.value)];

        if (spawnObject.gameObject.CompareTag("Fish"))
            UpdateMaterial(spawnObject);

        spawnObject.Scene = Scene;

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
        if (IncludeMovement && IncludeDucking)
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
            if (Scene == "Level2")
            {
                if (Random.Range(0, 2) == 0)
                    spawnObjectRenderer.material = Storage.MaterialDictionary["Main"];
                else
                    spawnObjectRenderer.material = Storage.MaterialDictionary["balloon"];
            }
            else if (Scene == "Level3")
            {
                switch (Random.Range(0, 3))
                {
                    case 0:
                        spawnObjectRenderer.material = Storage.MaterialDictionary["Main"];
                        break;
                    case 1:
                        spawnObjectRenderer.material = Storage.MaterialDictionary["balloon"];
                        break;
                    case 2:
                        spawnObjectRenderer.material = Storage.MaterialDictionary["glow"];
                        break;
                }
            }
            else
            {
                if (Random.Range(0, 2) == 0)
                    spawnObjectRenderer.material = Storage.MaterialDictionary["Main"];
                else
                    spawnObjectRenderer.material = Storage.MaterialDictionary["balloon"];
            }


            touchObject.Color = "";
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
        if (ColorsToSpawn.Count <= 0) return;

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