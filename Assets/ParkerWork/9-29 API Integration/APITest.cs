using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class APITest : MonoBehaviour
{
    const string API_FACTS = "https://meowfacts.herokuapp.com/";

    void Start()
    {
        StartCoroutine(FetchFact(API_FACTS));
    }

    IEnumerator FetchFact(string url)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            // Send the request and wait for the response
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Raw JSON: " + request.downloadHandler.text);
            }
            else
            {
                Debug.Log("Error: " + request.error);
            }
        }
    }
}
