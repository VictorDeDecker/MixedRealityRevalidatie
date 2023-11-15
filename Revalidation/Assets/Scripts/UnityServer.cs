using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

public class UnityServer : MonoBehaviour
{
    private const string prefix = "http://localhost:8085/"; // Update with your desired URL
    public ObjectSpawner objectSpawner;
    private List<TouchObject> touchObject;

    void Start()
    {
        // Start the server on a separate thread
        touchObject = objectSpawner.SpawnObjects;
        System.Threading.ThreadPool.QueueUserWorkItem(o => StartServer());
    }

    void StartServer()
    {
        try
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(prefix);
            listener.Start();

            Debug.Log("Server is listening for requests...");

            while (true)
            {
                // Wait for a request
                HttpListenerContext context = listener.GetContext();

                // Process the request
                ProcessRequest(context);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error starting the server: {e.Message}");
        }
    }

    void ProcessRequest(HttpListenerContext context)
    {
        try
        {
            // Get the request
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
                        Debug.Log(requestBody);

                        ParameterChangeRequest parameterChangeRequest = JsonConvert.DeserializeObject<ParameterChangeRequest>(requestBody);
                        Debug.Log($"Deserialized: {parameterChangeRequest}");

                        UpdateParameters(parameterChangeRequest);

                        // Send a response (if needed)
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
        }
        catch (Exception e)
        {
            Debug.LogError($"Error processing the request: {e.Message}");
        }
    }

    void UpdateParameters(ParameterChangeRequest request)
    {
        Debug.Log(request.parameterToChange);
        Debug.Log(request.parameterValue);
        Debug.Log(request.scriptToChange);
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
                if (request.parameterToChange == "amountOfObjects")
                {
                    objectSpawner.AmountOfObjects = (uint)request.parameterValue;
                }
                else if (request.parameterToChange == "timeInSeconds")
                {
                    objectSpawner.SpawnDurationInSec = (uint)request.parameterValue;
                }
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

// Define a class to represent the expected request format (adjust based on your needs)
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
