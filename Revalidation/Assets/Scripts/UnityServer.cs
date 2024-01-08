using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnityServer : MonoBehaviour
{
    private static UnityServer instance;
    private const string SignalRHubUrl = "http://45.93.139.33:32836/gameHub";
    private HubConnection hubConnection;
    public ObjectSpawnerV2 objectSpawner;
    public HandSelector handSelector;
    private List<TouchObject> touchObject;
    public LevelManager levelManager;
    public int RedFish = 5;
    public int PinkFish = 5;
    public int GreenFish = 5;
    public int YellowFish = 5;

    void Awake()
    {
        if (levelManager == null)
            levelManager = FindObjectOfType<LevelManager>();

        RedFish = 5;
        PinkFish = 5;
        GreenFish = 5;
        YellowFish = 5;

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (objectSpawner == null)
        {
            objectSpawner = FindObjectOfType<ObjectSpawnerV2>();
        }

        if (objectSpawner != null)
        {
            touchObject = objectSpawner.Objects;
        }

        ConnectToSignalRHub();
    }

    void OnDestroy()
    {
        hubConnection?.StopAsync();
    }

    private async void ConnectToSignalRHub()
    {
        hubConnection = new HubConnectionBuilder().WithUrl(SignalRHubUrl).Build();

        hubConnection.On<ParameterChangeRequest>("ReceivedParameters", (request) =>
        {
            UpdateParameters(request);
        });

        hubConnection.On<SceneChange>("ReceivedSceneChange", (request) =>
        {
            ChangeScene(request);
        });

        await hubConnection.StartAsync();
    }

    void UpdateParameters(ParameterChangeRequest request)
    {
        try
        {
            switch (request.Script)
            {
                case "touchObject":
                    if (request.Parameter == "speed")
                    {
                        if (touchObject == null)
                            touchObject = objectSpawner.Objects;

                        foreach (TouchObject spawnObject in touchObject)
                        {
                            spawnObject.Speed = request.Value;
                        }
                    }
                    break;
                case "handSelector":
                    if (request.Parameter == "hand")
                    {
                        switch (request.Value)
                        {
                            case -1:
                                MainThreadDispatcher.Instance().Enqueue(() => handSelector.SelectHand("left"));
                                break;
                            case 0:
                                MainThreadDispatcher.Instance().Enqueue(() => handSelector.SelectHand("both"));
                                break;
                            case 1:
                                MainThreadDispatcher.Instance().Enqueue(() => handSelector.SelectHand("right"));
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case "objectSpawner":
                    switch (request.Parameter.ToLower())
                    {
                        //Default parameters
                        case "levellengthinsec":
                            objectSpawner.LevelLengthInSec = (int)request.Value;
                            objectSpawner.ProgressBar.TimeToComplete = request.Value;
                            break;
                        case "height":
                            objectSpawner.Height = request.Value;
                            break;
                        case "radius":
                            objectSpawner.SpawnRadius = request.Value;
                            break;
                        case "shoulderwidth":
                            objectSpawner.SpaceBetween = request.Value;
                            break;
                        case "ducking":
                            objectSpawner.IncludeDucking = request.Value != 0;
                            break;
                        case "movement":
                            objectSpawner.IncludeMovement = request.Value != 0;
                            break;
                        case "obstacles":
                            objectSpawner.IncludeObstacles = request.Value != 0;
                            break;
                        case "waitbetweenspawns":
                            objectSpawner.TimeBetweenSpawnsInSec = (int)request.Value;
                            break;
                        //Paremeters regarding fish
                        case "redfishamount":
                            RedFish = (int)request.Value;
                            objectSpawner.ProgressBar.UpdateAmountOfFish();
                            if (!objectSpawner.ColorsToSpawn.Contains("Red"))
                            {
                                objectSpawner.ColorsToSpawn.Add("Red");
                            }
                            GetSceneNameAndReload();
                            break;
                        case "pinkfishamount":
                            PinkFish = (int)request.Value;
                            objectSpawner.ProgressBar.UpdateAmountOfFish();
                            if (!objectSpawner.ColorsToSpawn.Contains("Pink"))
                            {
                                objectSpawner.ColorsToSpawn.Add("Pink");
                            }
                            GetSceneNameAndReload();
                            break;
                        case "greenfishamount":
                            GreenFish = (int)request.Value;
                            objectSpawner.ProgressBar.UpdateAmountOfFish();
                            if (!objectSpawner.ColorsToSpawn.Contains("Green"))
                            {
                                objectSpawner.ColorsToSpawn.Add("Green");
                            }
                            GetSceneNameAndReload();
                            break;
                        case "yellowfishamount":
                            YellowFish = (int)request.Value;
                            objectSpawner.ProgressBar.UpdateAmountOfFish();
                            if (!objectSpawner.ColorsToSpawn.Contains("Yellow"))
                            {
                                objectSpawner.ColorsToSpawn.Add("Yellow");
                            }
                            GetSceneNameAndReload();
                            break;
                        case "allowredfish":
                            GetSceneNameAndReload();
                            if (objectSpawner.ColorsToSpawn.Contains("Red"))
                            {
                                objectSpawner.ColorsToSpawn.Remove("Red");
                                RedFish = 0;
                            }
                            break;
                        case "allowpinkfish":
                            GetSceneNameAndReload();
                            if (objectSpawner.ColorsToSpawn.Contains("Pink"))
                            {
                                objectSpawner.ColorsToSpawn.Remove("Pink");
                                PinkFish = 0;
                            }
                            break;
                        case "allowgreenfish":
                            GetSceneNameAndReload();
                            if (objectSpawner.ColorsToSpawn.Contains("Green"))
                            {
                                objectSpawner.ColorsToSpawn.Remove("Green");
                                GreenFish = 0;
                            }
                            break;
                        case "allowyellowfish":
                            GetSceneNameAndReload();
                            if (objectSpawner.ColorsToSpawn.Contains("Yellow"))
                            {
                                objectSpawner.ColorsToSpawn.Remove("Yellow");
                                YellowFish = 0;
                            }
                            break;
                        default:
                            break;
                    }

                    break;
                default:
                    break;
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    void GetSceneNameAndReload()
    {
        try
        {
            MainThreadDispatcher.Instance().Enqueue(() =>
            {
                UpdateScene(levelManager.GetActiveScene());
            });
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }

    }

    void ChangeScene(SceneChange sceneChange)
    {
        try
        {
            UpdateScene(sceneChange.destinationScene.ToLower());
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    private void UpdateScene(string sceneName)
    {
        AsyncOperation status;
        switch (sceneName.ToLower())
        {
            case "level 1":
            case "level1":
                MainThreadDispatcher.Instance().Enqueue(() =>
                {
                    status = SceneManager.LoadSceneAsync("Level 1");
                    status.completed += (x) =>
                    {
                        objectSpawner = GameObject.FindGameObjectWithTag("SpawnPlane").GetComponent<ObjectSpawnerV2>();
                        touchObject = objectSpawner.Objects;
                    };
                }
                );
                break;
            case "level 2":
            case "level2":
                MainThreadDispatcher.Instance().Enqueue(() =>
                {
                    status = SceneManager.LoadSceneAsync("Level 2");
                    status.completed += (x) =>
                    {
                        objectSpawner = GameObject.FindGameObjectWithTag("SpawnPlane").GetComponent<ObjectSpawnerV2>();
                        touchObject = objectSpawner.Objects;
                    };
                });
                break;
            case "level 3":
            case "level3":
                MainThreadDispatcher.Instance().Enqueue(() =>
                {
                    status = SceneManager.LoadSceneAsync("Level 3");
                    status.completed += (x) =>
                    {
                        objectSpawner = GameObject.FindGameObjectWithTag("SpawnPlane").GetComponent<ObjectSpawnerV2>();
                        touchObject = objectSpawner.Objects;
                    };
                });
                break;
            case "mainmenu":
                MainThreadDispatcher.Instance().Enqueue(() => SceneManager.LoadSceneAsync("MainMenu"));
                break;
            case "quit":
                MainThreadDispatcher.Instance().Enqueue(() => levelManager.QuitLevel());
                break;
            default:
                break;
        }
    }
}

[Serializable]
public class ParameterChangeRequest
{
    [JsonProperty("script")]
    public string Script { get; set; }

    [JsonProperty("parameter")]
    public string Parameter { get; set; }

    [JsonProperty("value")]
    public float Value { get; set; }
}

[Serializable]
public class SceneChange
{
    [JsonProperty(PropertyName = "destinationScene")]
    public string destinationScene { get; set; }
}