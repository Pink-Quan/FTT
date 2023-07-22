using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class DBManager : MonoBehaviour
{
    [SerializeField] private string uri;
    [SerializeField] private bool isTest;

    private DateTime timePointer;
    void Start()
    {
        timePointer = DateTime.Now;
        UpdateDB();
        InvokeRepeating("DBU", 10, 10);
    }

    private void DBU()
    {
        UpdateDB();
    }

    public void UpdateDB(Action onDone = null)
    {
        UpdatePlayedTime();
        Login((isSuccess, res) =>
        {
            if (isSuccess)
            {
                //Debug.Log("LoginSuccess: " + res);
                UpdateUserData((iSuccess, res) =>
                {
                    //if (isSuccess)
                    //{
                    //    Debug.Log("Update Succes: " + res);
                    //}
                    //else
                    //{
                    //    Debug.Log("Update error: " + res);
                    //}
                });
            }
            //else
            //{
            //    Debug.Log("Loggin error: " + res);
            //}
            onDone?.Invoke();
        });
    }

    public void Login(Action<bool, string> OnDone)
    {
        var firstTimePlay = PlayerPrefs.GetString("First Time Play", "");
        if (firstTimePlay == "")
        {
            firstTimePlay = DateTime.Now.ToString("yyyy/MM/dd");
            PlayerPrefs.SetString("First Time Play", firstTimePlay);
        }
        StartCoroutine(Upload(uri + "/Login.php", OnDone,
            new PostData("userName", SystemInfo.deviceUniqueIdentifier),
            new PostData("Version", Application.version),
            new PostData("firstTimePlay", firstTimePlay)
            ));

    }

    public void UpdateUserData(Action<bool, string> OnDone)
    {
        StartCoroutine(Upload(uri + "/UpdateUserInfo.php", OnDone,
            new PostData("userName", SystemInfo.deviceUniqueIdentifier),
            new PostData("Version", Application.version),
            new PostData("firstTimePlay", PlayerPrefs.GetString("First Time Play")),
            new PostData("Progress", ((GameProgress)PlayerPrefs.GetInt("Progress")).ToString()),
            new PostData("PlayedTime", ((int)PlayerPrefs.GetFloat("Played Time", 0)).ToString()),
            new PostData("LastTimePlay", DateTime.Now.ToString("yyyy/MM/dd")),
            new PostData("Achievement", GetAchievement())
        ));
    }

    public void GetAnnouncement(Action<bool, string> OnDone)
    {
        StartCoroutine(Upload(uri + "/Announcement.php", OnDone,
            new PostData("Language", PlayerPrefs.GetString("Language", "Eng"))
        ));
    }

    IEnumerator GetRequest(string uri, Action<bool, string> OnDone)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                case UnityWebRequest.Result.ProtocolError:
                    OnDone?.Invoke(false, webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    OnDone?.Invoke(true, webRequest.downloadHandler.text);
                    break;
            }
        }
    }

    IEnumerator Upload(string uri, Action<bool, string> OnDone, params PostData[] postData)
    {
        WWWForm form = new WWWForm();
        for (int i = 0; i < postData.Length; i++)
            form.AddField(postData[i].fieldName, postData[i].value);

        UnityWebRequest www = UnityWebRequest.Post(uri, form);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Upload failed: " + www.error);
            OnDone?.Invoke(false, www.error);
        }
        else
        {
            Debug.Log("Upload Success, return: " + www.downloadHandler.text);
            OnDone?.Invoke(true, www.downloadHandler.text);

        }
    }

    private void OnApplicationQuit()
    {
        UpdatePlayedTime();
        Debug.Log(PlayerPrefs.GetFloat("Played Time", 0));
    }

    private void UpdatePlayedTime()
    {
        float playedTime = (float)((DateTime.Now - timePointer).TotalMinutes);
        timePointer = DateTime.Now;
        PlayerPrefs.SetFloat("Played Time", PlayerPrefs.GetFloat("Played Time", 0f) + playedTime);
    }

    private string GetAchievement()
    {
        StringBuilder achieve = new StringBuilder();

        achieve.Append(PlayerPrefs.GetInt("DONE DEMO", 0));
        achieve.Append(PlayerPrefs.GetInt("FAINT IN HOSPITAL", 0));

        string res = achieve.ToString();
        return res;
    }

    public struct PostData
    {
        public string fieldName;
        public string value;

        public PostData(string fiedlName, string value)
        {
            fieldName = fiedlName;
            this.value = value;
        }
    }

}
