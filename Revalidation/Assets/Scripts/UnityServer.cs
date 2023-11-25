using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

public class UnityServer : MonoBehaviour
{
    private const string prefix = "http://localhost:8085/";
    private static UnityServer instance;
    public SetSpawner objectSpawner;
    private List<TouchObject> touchObject;
    private HttpListener listener;
    private bool isRunning;
    private readonly LevelManager levelManager = new();

    private void Awake()
    {
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
        objectSpawner = FindObjectOfType<SetSpawner>();

        if (objectSpawner == null)
        {
            objectSpawner = new SetSpawner();
        }
        touchObject = objectSpawner.Objects;
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
                        Debug.Log($"Deserialized: {sceneChange}");

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
                else if (request.parameterToChange.ToLower() == "InfiniteSpawn".ToLower())
                {
                    if ((int)request.parameterValue == 0)
                        objectSpawner.InfiniteSpawn = false;
                    else if ((int)request.parameterValue == 1)
                        objectSpawner.InfiniteSpawn = true;
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

    void ChangeScene(SceneChange sceneChange)
    {
        switch (sceneChange.destinationScene.ToLower())
        {
            case "level1":
                levelManager.LoadLevel("Level 1");
                break;
            case "level2":
                levelManager.LoadLevel("Level 2");
                break;
            case "level3":
                levelManager.LoadLevel("Level 3");
                break;
            case "mainmenu":
                levelManager.LoadLevel("MainMenu");
                break;
            case "quit":
                levelManager.QuitLevel();
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
