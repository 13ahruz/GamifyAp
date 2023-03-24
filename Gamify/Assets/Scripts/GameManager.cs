using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using TMPro;

public class GameManager : MonoBehaviour
{
    private const string CLIENT_ID = "fa5893df-6b24-49bd-aa0b-a37feabdeb0e";
    private const string CLIENT_SECRET = "eCMvFEVmJTjAXgIBev34DefBW5RY0a4j";
    private const string REDIRECT_URI = "unitydl://";
    private const string AUTHORIZATION_ENDPOINT = "https://ada-staging.blackboard.com/learn/api/public/v1/oauth2/authorizationcode";
    private const string TOKEN_ENDPOINT = "https://ada-staging.blackboard.com/learn/api/public/v1/oauth2/token";
    private const string BASE_URL = "https://ada-staging.blackboard.com";

    public List<string> courseIds = new List<string>();

    public TMP_Text CourseName;
    [SerializeField]
    private GameObject courseInScreen;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GetCourseID();
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            foreach (string courseId in courseIds)
            {
                Debug.Log("getting course info");
                GetCourseInfo(courseId);
            }
        }
    }

    public async void GetCourseID()
    {
        using var www3 = UnityWebRequest.Get($"{BASE_URL}/learn/api/public/v1/users/uuid:" + LoginManager.Instance.user_id + "/courses/");
        www3.SetRequestHeader("Authorization", $"Bearer {LoginManager.Instance.accessToken}");
        www3.SetRequestHeader("Content-Type", "application/json");
        var operation = www3.SendWebRequest();

        while (!operation.isDone)
        {
            await Task.Yield();
        }

        if (www3.result != UnityWebRequest.Result.Success)
        {
            Debug.Log($"Failed: {www3.error}");
        }

        var jsonResponse = www3.downloadHandler.text;

        try
        {

            MyCourses myCourses = JsonConvert.DeserializeObject<MyCourses>(jsonResponse);

            foreach (Course course in myCourses.results)
            {
                courseIds.Add(course.courseId);
            }

            foreach (string courseId in courseIds)
            {
                Debug.Log(courseId);
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }


    public async void GetCourseInfo(string courseId)
    {
        using var www3 = UnityWebRequest.Get($"{BASE_URL}/learn/api/public/v3/courses/" + courseId); ;
        www3.SetRequestHeader("Authorization", $"Bearer {LoginManager.Instance.accessToken}");
        www3.SetRequestHeader("Content-Type", "application/json");
        var operation = www3.SendWebRequest();

        while (!operation.isDone)
        {
            await Task.Yield();
        }

        if (www3.result != UnityWebRequest.Result.Success)
        {
            Debug.Log($"Failed: {www3.error}");
        }

        var jsonResponse = www3.downloadHandler.text;

        try
        {
            Debug.Log(jsonResponse);
            Course cn = JsonUtility.FromJson<Course>(jsonResponse);
            Debug.Log($"Course name: {cn.name}");
            Enviroment.Instance.portals[0].gameObject.SetActive(true);
            Enviroment.Instance.portals[0].GetComponentInChildren<TextMeshProUGUI>().text = cn.name;
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
}