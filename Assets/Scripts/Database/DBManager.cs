using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DBManager : MonoBehaviour
{
    [SerializeField] string uri;
    void Start()
    {
        string userName = SystemInfo.deviceUniqueIdentifier + DateTime.Now.ToString("dd/MM/yyyy");
        Login();
    }

    public void Login()
    {
        StartCoroutine(GetRequest(uri, null));
    }



    IEnumerator GetRequest(string uri, Action<string> OnSucces)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    string res = webRequest.downloadHandler.text;
                    Debug.Log("Received: " + res);
                    OnSucces?.Invoke(res);
                    break;
            }
        }
    }
}
