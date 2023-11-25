using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using UnityEngine;

public class UnityServer : MonoBehaviour
{
    private const string angularEndpoint = "http://localhost:4200/textureUpdate";
    private const string prefix = "http://localhost:8085/";
    private static UnityServer instance;
    public SetSpawner objectSpawner;
    private List<TouchObject> touchObject;
    private HttpListener listener;
    private bool isRunning;
    private readonly LevelManager levelManager = new();
    public RenderTextureController renderController;
    private const float frameRate = 24f;
    private float frameInterval;

    void Awake()
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

        objectSpawner = FindObjectOfType<SetSpawner>();

        if (objectSpawner == null)
        {
            objectSpawner = new SetSpawner();
        }

        if (renderController == null)
        {
            renderController = FindObjectOfType<RenderTextureController>();
        }

        frameInterval = 1f / frameRate;
        //StartCoroutine(SendTexturePeriodically());
        if (objectSpawner != null)
        {
            touchObject = objectSpawner.Objects;
        }
    }

    void Start()
    {
        objectSpawner = FindObjectOfType<SetSpawner>();

        if (objectSpawner == null)
        {
            objectSpawner = new SetSpawner();
        }

        if (renderController == null)
        {
            renderController = FindObjectOfType<RenderTextureController>();
        }

        frameInterval = 1f / frameRate;
        //StartCoroutine(SendTexturePeriodically());
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
                MainThreadDispatcher.Instance().Enqueue(() => levelManager.LoadLevel("Level 1"));
                break;
            case "level2":
                MainThreadDispatcher.Instance().Enqueue(() => levelManager.LoadLevel("Level 2"));
                break;
            case "level3":
                MainThreadDispatcher.Instance().Enqueue(() => levelManager.LoadLevel("Level 3"));
                break;
            case "mainmenu":
                MainThreadDispatcher.Instance().Enqueue(() => levelManager.LoadLevel("MainMenu"));
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
    private IEnumerator SendTexturePeriodically()
    {
        while (true)
        {
            SendTextureToAngular();
            yield return new WaitForSeconds(frameInterval);
        }
    }

    void SendTextureToAngular()
    {
        try
        {
            byte[] textureData = renderController.capturedTexture.EncodeToPNG();

            // Create a custom class to hold the texture data and any additional information
            TextureUpdateRequest textureUpdateRequest = new TextureUpdateRequest
            {
                TextureData = Convert.ToBase64String(textureData), // Convert to Base64 for easy transmission
                Width = renderController.capturedTexture.width,
                Height = renderController.capturedTexture.height
            };

            HttpClient client = new HttpClient();
            string jsonRequest = JsonConvert.SerializeObject(textureUpdateRequest);
            StringContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(angularEndpoint, content).Result;

            if (response.IsSuccessStatusCode)
            {
                Debug.Log("Texture data sent successfully to Angular application.");
            }
            else
            {
                Debug.LogError($"Failed to send texture data to Angular application. Status Code: {response.StatusCode}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error sending data to Angular application: {e.Message}");
        }
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

[Serializable]
public class TextureUpdateRequest
{
    public string TextureData;
    public int Width;
    public int Height;
}
