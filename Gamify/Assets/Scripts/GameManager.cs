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

    public List<Course> courses = new List<Course>();
    public TMP_Text CourseContentName;
    public List<TMP_Text> CourseContentLinks;
    Content courseContent;
    [SerializeField]
    private List<Transform> Weeks;

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
            for (int i = 0; i < courses.Count; i++)
            {
                Enviroment.Instance.portals[i].gameObject.SetActive(true);
                Enviroment.Instance.portals[i].GetComponentInChildren<TextMeshProUGUI>().text = courses[i].name;
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            GetCourseContent(courses[0].courseId);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            GetCourseContentChildren(courses[0].courseId, courseContent.id);
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


    public async void GetCourseInfo(string id)
    {
        using var www3 = UnityWebRequest.Get($"{BASE_URL}/learn/api/public/v3/courses/" + id); ;
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
            courses.Add(cn);
            Debug.Log($"Course name: {cn.name}");
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public async void GetCourseContent(string courseId)
    {
        using var www4 = UnityWebRequest.Get($"{BASE_URL}/learn/api/public/v1/courses/courseId:" + courseId + "/contents"); //ToDo
        www4.SetRequestHeader("Authorization", $"Bearer {LoginManager.Instance.accessToken}");
        www4.SetRequestHeader("Content-Type", "application/json");

        var operation = www4.SendWebRequest();

        while (!operation.isDone)
        {
            await Task.Yield();
        }

        if (www4.result != UnityWebRequest.Result.Success)
        {
            Debug.Log($"Failed: {www4.error}");
        }

        var jsonResponse = www4.downloadHandler.text;

        try
        {
            Debug.Log(jsonResponse);
            Contents contents = JsonConvert.DeserializeObject<Contents>(jsonResponse);
            foreach (Content content in contents.results)
            {
                if (content.title == "Course Content")
                {
                    Debug.Log(content.id);
                    courseContent = content;
                }
            }

        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public async void GetCourseContentChildren(string courseId, string contentId)
    {
        using var www4 = UnityWebRequest.Get($"{BASE_URL}/learn/api/public/v1/courses/courseId:" + courseId + "/contents/" + contentId + "/children");
        www4.SetRequestHeader("Authorization", $"Bearer {LoginManager.Instance.accessToken}");
        www4.SetRequestHeader("Content-Type", "application/json");

        var operation = www4.SendWebRequest();

        while (!operation.isDone)
        {
            await Task.Yield();
        }

        if (www4.result != UnityWebRequest.Result.Success)
        {
            Debug.Log($"Failed: {www4.error}");
        }

        var jsonResponse = www4.downloadHandler.text;

        try
        {
            CourseContentList c = JsonUtility.FromJson<CourseContentList>(jsonResponse);
            Debug.Log(jsonResponse);
            CourseContentName.text = "";
            foreach (CourseContent coursecontent in c.results)
            {
                string courseText = $"<link={coursecontent.url}><b>{coursecontent.title}</b></link>\n";
                CourseContentName.text += courseText;
            }

            // Attach the CourseContentLink component to each link
            foreach (TMP_LinkInfo linkInfo in CourseContentName.textInfo.linkInfo)
            {
                GameObject linkGO = new GameObject("CourseContentLink");
                var link = linkGO.AddComponent<CourseContentLink>();
                link.SetLinkData(linkInfo.GetLinkID(), linkInfo.GetLinkText(), linkInfo.GetLinkID());
                linkGO.transform.SetParent(CourseContentName.transform, false);
                RectTransform rect = linkGO.GetComponent<RectTransform>();
            }

        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    private void SetWeeksAndLectures()
    {

    }




}