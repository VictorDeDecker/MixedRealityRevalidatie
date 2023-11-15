using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class WebsiteInterface : MonoBehaviour
{
    public string serverUrl = "http://localhost:4200"
        ;
    public void ChangeParameter(string parameterName, float parameterValue)
    {
        StartCoroutine(SendParameterRequest(parameterName, parameterValue));
    }

    IEnumerator SendParameterRequest(string parameterName, float parameterValue)
    {
        string url = $"{serverUrl}/changeParameter?name={parameterName}&value={parameterValue}";
        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"Failed to send parameter request: {request.error}");
        }
    }
}
