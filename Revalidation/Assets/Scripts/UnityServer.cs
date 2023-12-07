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
    public SetSpawner objectSpawner;
    private List<TouchObject> touchObject;
    private HttpListener listener;
    private bool isRunning;
    private readonly LevelManager levelManager;

    void Awake()
    {
        gameObject.AddComponent<LevelManager>();
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
                        Debug.Log($"Deserialized: {sceneChange.destinationScene}");

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
                    if (request.parameterToChange.ToLower() == "AmountOfSets".ToLower())
                    {
                        objectSpawner.AmountOfSets = (int)request.parameterValue;
                    }
                    else if (request.parameterToChange.ToLower() == "LevelLengthInSec".ToLower())
                    {
                        objectSpawner.LevelLengthInSec = (int)request.parameterValue;
                    }
                    else if (request.parameterToChange.ToLower() == "MaxPercentageOfMissingObjects".ToLower())
                    {
                        objectSpawner.MaxPercentageOfMissingObjects = request.parameterValue;
                    }
                    else if (request.parameterToChange.ToLower() == "SetWidth".ToLower())
                    {
                        objectSpawner.SetWidth = (int)request.parameterValue;
                    }
                    else if (request.parameterToChange.ToLower() == "InfiniteSpawnWaitTime".ToLower())
                    {
                        objectSpawner.InfiniteSpawnWaitTime = (int)request.parameterValue;
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

    void ChangeScene(SceneChange sceneChange)
    {
        AsyncOperation status;
        try
        {
            switch (sceneChange.destinationScene.ToLower())
            {
                case "level1":
                    MainThreadDispatcher.Instance().Enqueue(() =>
                    {
                        status = SceneManager.LoadSceneAsync("Level 1");
                        status.completed += (x) =>
                        {
                            objectSpawner = GameObject.FindGameObjectWithTag("SpawnPlane").GetComponent<SetSpawner>();
                            touchObject = objectSpawner.Objects;
                        };
                    }
                    );

                    break;
                case "level2":
                    MainThreadDispatcher.Instance().Enqueue(() =>
                    {
                        status = SceneManager.LoadSceneAsync("Level 2");
                        status.completed += (x) =>
                        {
                            objectSpawner = GameObject.FindGameObjectWithTag("SpawnPlane").GetComponent<SetSpawner>();
                            touchObject = objectSpawner.Objects;
                        };
                    });
                    break;
                case "level3":
                    MainThreadDispatcher.Instance().Enqueue(() =>
                    {
                        status = SceneManager.LoadSceneAsync("Level 3");
                        status.completed += (x) =>
                        {
                            objectSpawner = GameObject.FindGameObjectWithTag("SpawnPlane").GetComponent<SetSpawner>();
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
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    private void Status_completed(AsyncOperation obj)
    {
        throw new NotImplementedException();
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
