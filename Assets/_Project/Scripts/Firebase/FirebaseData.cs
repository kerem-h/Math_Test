using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class TestResult
{
    public int blank;
    public int correct;
    public int wrong;
}

[System.Serializable]
public class AllResults : Dictionary<string, TestResult>{}

public class FirebaseData : MonoBehaviour
{
    private const string url = "https://plane-game-280c1-default-rtdb.firebaseio.com/.json";
    private AllResults _results;

    public static IEnumerator GetJsonData(string url, Action<AllResults> callback)
    {
        using (var request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                var jsonText = request.downloadHandler.text;
                var result = JsonConvert.DeserializeObject<AllResults>(jsonText);  // Change this line
                if (result != null)
                {
                    callback?.Invoke(result);
                }
            }
            else
            {
                Debug.LogError("Error Getting Json File");
            }
        }
    }
}
