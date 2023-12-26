using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnityServer : MonoBehaviour
{
    private const string prefix = "http://localhost:8085/";
    private static UnityServer instance;
    public ObjectSpawnerV2 objectSpawner;
    private List<TouchObject> touchObject;
    private HttpListener listener;
    private bool isRunning;
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

        if (objectSpawner != null)
        {
            touchObject = objectSpawner.Objects;
        }
    }

    void Start()
    {
        System.Threading.ThreadPool.QueueUserWorkItem(o => StartServer());
    }

    void OnApplicationQuit()
    {
        isRunning = false;
        StopServer();
    }

    void StartServer()
    {
        try
        {
            listener = new HttpListener();
            listener.Prefixes.Add(prefix);
            listener.Start();

            Debug.Log("Server is listening for requests...");
            isRunning = true;
            while (isRunning)
            {
                if (isRunning)
                {
                    HttpListenerContext context = listener.GetContext();
                    ProcessRequest(context);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error starting the server: {e.Message}");
        }
    }

    void StopServer()
    {
        if (listener != null && listener.IsListening)
        {
            listener.Stop();
            listener.Close();
            Debug.Log("Server stopped");
        }
    }

    void ProcessRequest(HttpListenerContext context)
    {
        try
        {
            HttpListenerRequest request = context.Request;

            if (request.HttpMethod == "OPTIONS")
            {
                SendOkResponse(context);
                return;
            }

            if (request.RawUrl == "/test")
            {
                SendOkResponse(context);
                return;
            }

            if (request.RawUrl == "/updateParameter")
            {
                using (Stream body = request.InputStream)
                {
                    using (StreamReader reader = new StreamReader(body, request.ContentEncoding))
                    {
                        string requestBody = reader.ReadToEnd();

                        ParameterChangeRequest parameterChangeRequest = JsonConvert.DeserializeObject<ParameterChangeRequest>(requestBody);
                        Debug.Log($"Deserialized: {parameterChangeRequest}");

                        UpdateParameters(parameterChangeRequest);

                        HttpListenerResponse response = context.Response;
                        response.Headers.Add("Access-Control-Allow-Origin", "*");
                        response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
                        response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
                        response.ContentType = "application/json";

                        string jsonResponse = "{\"message\": \"Succesful\"}";
                        byte[] buffer = Encoding.UTF8.GetBytes(jsonResponse);
                        response.ContentLength64 = buffer.Length;
                        Stream output = response.OutputStream;
                        output.Write(buffer, 0, buffer.Length);
                        output.Close();
                    }
                }
            }

            if (request.RawUrl == "/changeScene")
            {
                using (Stream body = request.InputStream)
                {
                    using (StreamReader reader = new StreamReader(body, request.ContentEncoding))
                    {
                        string requestBody = reader.ReadToEnd();

                        SceneChange sceneChange = JsonConvert.DeserializeObject<SceneChange>(requestBody);
                        //Debug.Log($"Deserialized: {sceneChange.destinationScene}");

                        ChangeScene(sceneChange);

                        HttpListenerResponse response = context.Response;
                        response.Headers.Add("Access-Control-Allow-Origin", "*");
                        response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
                        response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
                        response.ContentType = "application/json";

                        string jsonResponse = "{\"message\": \"Scene was changed\"}";
                        byte[] buffer = Encoding.UTF8.GetBytes(jsonResponse);
                        response.ContentLength64 = buffer.Length;
                        Stream output = response.OutputStream;
                        output.Write(buffer, 0, buffer.Length);
                        output.Close();
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error processing the request: {e.Message}");
        }
    }

    void UpdateParameters(ParameterChangeRequest request)
    {
        try
        {
            switch (request.scriptToChange)
            {
                case "touchObject":
                    if (request.parameterToChange == "speed")
                    {
                        foreach (TouchObject spawnObject in touchObject)
                        {
                            spawnObject.Speed = request.parameterValue;
                        }
                    }
                    break;
                case "objectSpawner":
                    switch (request.parameterToChange.ToLower())
                    {
                        //Default parameters
                        case "levellengthinsec":
                            objectSpawner.LevelLengthInSec = (int)request.parameterValue;
                            break;
                        case "height":
                            objectSpawner.Height = request.parameterValue;
                            break;
                        case "radius":
                            objectSpawner.SpawnRadius = request.parameterValue;
                            break;
                        case "shoulderwidth":
                            objectSpawner.SpaceBetween = request.parameterValue;
                            break;
                        case "ducking":
                            objectSpawner.IncludeDucking = request.parameterValue != 0;
                            break;
                        case "movement":
                            objectSpawner.IncludeMovement = request.parameterValue != 0;
                            break;
                        case "waitbetweenspawns":
                            objectSpawner.TimeBetweenSpawnsInSec = (int)request.parameterValue;
                            break;
                        //Paremeters regarding fish
                        case "redfishamount":
                            RedFish = (int)request.parameterValue;
                            objectSpawner.ProgressBar.UpdateAmountOfFish();
                            if (!objectSpawner.ColorsToSpawn.Contains("Red"))
                            {
                                objectSpawner.ColorsToSpawn.Add("Red");
                            }
                            GetSceneNameAndReload();
                            break;
                        case "pinkfishamount":
                            PinkFish = (int)request.parameterValue;
                            objectSpawner.ProgressBar.UpdateAmountOfFish();
                            if (!objectSpawner.ColorsToSpawn.Contains("Pink"))
                            {
                                objectSpawner.ColorsToSpawn.Add("Pink");
                            }
                            GetSceneNameAndReload();
                            break;
                        case "greenfishamount":
                            GreenFish = (int)request.parameterValue;
                            objectSpawner.ProgressBar.UpdateAmountOfFish();
                            if (!objectSpawner.ColorsToSpawn.Contains("Green"))
                            {
                                objectSpawner.ColorsToSpawn.Add("Green");
                            }
                            GetSceneNameAndReload();
                            break;
                        case "yellowfishamount":
                            YellowFish = (int)request.parameterValue;
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
                Debug.Log(levelManager.GetActiveScene());
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

    void SendOkResponse(HttpListenerContext context)
    {
        HttpListenerResponse response = context.Response;
        response.Headers.Add("Access-Control-Allow-Origin", "*");
        response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
        response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
        response.ContentType = "application/json";

        string jsonResponse = "{\"message\": \"OK\"}";
        byte[] buffer = Encoding.UTF8.GetBytes(jsonResponse);
        response.ContentLength64 = buffer.Length;
        Stream output = response.OutputStream;
        output.Write(buffer, 0, buffer.Length);
        output.Close();
    }
}

[Serializable]
public class ParameterChangeRequest
{
    [JsonProperty(PropertyName = "script")]
    public string scriptToChange;
    [JsonProperty(PropertyName = "parameter")]
    public string parameterToChange;
    [JsonProperty(PropertyName = "value")]
    public float parameterValue;
}

[Serializable]
public class SceneChange
{
    [JsonProperty(PropertyName = "scene")]
    public string destinationScene;
}
