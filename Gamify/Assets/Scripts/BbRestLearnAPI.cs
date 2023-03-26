using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using TMPro;
using UnityEngine.UI;


public class BbRestLearnAPI : MonoBehaviour
{
    public string url = "https://ada-staging.blackboard.com";
    public string appKey = "fa5893df-6b24-49bd-aa0b-a37feabdeb0e";
    public string appSecret = "eCMvFEVmJTjAXgIBev34DefBW5RY0a4j";


    //Serializable Contents to send and store information
    TokenManager tm;
    BbLearnDelegation bbld;
    GroupList glist;


    //TextMeshPros
    public TMP_Text CourseName;
    //public TMP_Text availableText;
    public TMP_Text AnnnouncementName;
    public TMP_Text CourseContentName;
    public TMP_Text GroupName;
    public TMP_Text CalendarName;
    public TMP_Dropdown dropdown;
    public TMP_Text AttendanceName;
    public MeetingList meetingList1;

    //variables to hold needed details
    private string courseName;
    //private string available;
    private string groupName;
    private string calendarCName;



    private void Awake()
    {
        Debug.Log("Getting Token Manager.....");
        tm = GetComponent<TokenManager>();
        bbld = GetComponent<BbLearnDelegation>();
        TestGetToken();
    }

    public async void TestGetToken()
    {
        Debug.Log("Testing call.....");
        var auth = System.Convert.ToBase64String(
            System.Text.Encoding.Default.GetBytes(
                $"{appKey}:{appSecret}"
            )
        );
        var basicAuth = $"Basic {auth}";
        WWWForm form = new WWWForm();
        form.AddField("grant_type", "client_credentials");
        Debug.Log($"{url}/learn/api/public/v1/oauth2/token");
        Debug.Log(basicAuth);
        using var www = UnityWebRequest.Post($"{url}/learn/api/public/v1/oauth2/token", form);
        www.SetRequestHeader("Authorization", basicAuth);
        www.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");

        var operation = www.SendWebRequest();

        while (!operation.isDone)
        {
            await Task.Yield();
        }

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log($"Failed: {www.error}");
        }

        var jsonResponse = www.downloadHandler.text;
        Debug.Log(jsonResponse);
        BbAuthContext result = new BbAuthContext();
        try
        {
            result = JsonConvert.DeserializeObject<BbAuthContext>(jsonResponse);
            Debug.Log($"Success: {www.downloadHandler.text}");
        }
        catch (Exception ex)
        {
            Debug.Log($"{this} could not parse json {jsonResponse}. {ex.Message}");
        }

        //if (result.access_token != "")
        //{
        //    tm.setExpiresIn(result.expires_in);
        //}


        //GETTING ANNOUNCEMENTS
        using var www2 = UnityWebRequest.Get($"{url}/learn/api/public/v1/courses/courseId:GAME20/announcements");
        www2.SetRequestHeader("Authorization", $"Bearer {result.access_token}");
        www2.SetRequestHeader("Content-Type", "application/json");

        operation = www2.SendWebRequest();

        while (!operation.isDone)
        {
            await Task.Yield();
        }
        if (www2.result != UnityWebRequest.Result.Success)
        {
            Debug.Log($"Failed: {www.error}");
        }

        jsonResponse = www2.downloadHandler.text;

        try
        {
            AnnouncementList a = JsonUtility.FromJson<AnnouncementList>(jsonResponse);
            Debug.Log($"Announcement title: {a.results[0].title}");

            string allTitles = "";
            foreach (Announcement announcement in a.results)
            {
                string announcementText = "<b>" + announcement.title + "</b>" + "\n" + announcement.body + "\n";
                allTitles += announcementText;
            }

            AnnnouncementName.text = allTitles;

            var contentItems = JsonConvert.DeserializeObject<BbContentItems>(jsonResponse);
            foreach (var item in contentItems.results)
            {
                bbld.CreateSpawn();
            }


        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }

        //GETTING COURSE CONTENT
        using var www4 = UnityWebRequest.Get($"{url}/learn/api/public/v1/courses/courseId:GAME20/contents/_202502_1/children");
        www4.SetRequestHeader("Authorization", $"Bearer {result.access_token}");
        www4.SetRequestHeader("Content-Type", "application/json");

        operation = www4.SendWebRequest();

        while (!operation.isDone)
        {
            await Task.Yield();
        }

        if (www4.result != UnityWebRequest.Result.Success)
        {
            Debug.Log($"Failed: {www.error}");
        }

        jsonResponse = www4.downloadHandler.text;

        try
        {
            CourseContentList c = JsonUtility.FromJson<CourseContentList>(jsonResponse);

            //string allTitles1 = "";
            //foreach (CourseContent coursecontents in c.results)
            //{
            //    string courseText = "<b><color=blue>" + coursecontents.title + "</color></b>" + "\n";
            //    allTitles1 += courseText;
            //}
            //CourseContentName.text = allTitles1;

            CourseContentName.text = "";
            foreach (CourseContent coursecontents in c.results)
            {
                string courseText = $"<link={coursecontents.url}><b>{coursecontents.title}</b></link>\n";
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

        //GETTING COURSE NAME
        using var www3 = UnityWebRequest.Get($"{url}/learn/api/public/v1/courses/courseId:GAME20");
        www3.SetRequestHeader("Authorization", $"Bearer {result.access_token}");
        www3.SetRequestHeader("Content-Type", "application/json");

        operation = www3.SendWebRequest();

        while (!operation.isDone)
        {
            await Task.Yield();
        }

        if (www3.result != UnityWebRequest.Result.Success)
        {
            Debug.Log($"Failed: {www.error}");
        }

        jsonResponse = www3.downloadHandler.text;
        try
        {
            Course cn = JsonUtility.FromJson<Course>(jsonResponse);
            Debug.Log($"Course name: {cn.name}");
            //Debug.Log($"Course availability: {cn.availability.available}");
            //Debug.Log($"Announcement title: {a.title}");
            courseName = cn.name;
            CourseName.text = courseName;
            //available = cn.availability.available;


        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }



        //GETTING GROUPS
        using var www5 = UnityWebRequest.Get($"{url}//learn/api/public/v1/courses/courseId:GAME20/groups");
        www5.SetRequestHeader("Authorization", $"Bearer {result.access_token}");
        www5.SetRequestHeader("Content-Type", "application/json");

        operation = www5.SendWebRequest();

        while (!operation.isDone)
        {
            await Task.Yield();
        }

        if (www5.result != UnityWebRequest.Result.Success)
        {
            Debug.Log($"Failed: {www.error}");
        }

        jsonResponse = www5.downloadHandler.text;
        try
        {
            GroupList groupList = JsonUtility.FromJson<GroupList>(jsonResponse);

            groupName = groupList.results[0].name;
            GroupName.text = groupName;

        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }

        // CALENDAR ITEMS
        using var www6 = UnityWebRequest.Get($"{url}/learn/api/public/v1/calendars/items");
        www6.SetRequestHeader("Authorization", $"Bearer {result.access_token}");
        www6.SetRequestHeader("Content-Type", "application/json");

        operation = www6.SendWebRequest();



        while (!operation.isDone)
        {
            await Task.Yield();
        }
        if (www6.result != UnityWebRequest.Result.Success)
        {
            Debug.Log($"Failed: {www6.error}");
        }

        jsonResponse = www6.downloadHandler.text;
        Debug.Log(jsonResponse);

        try
        {
            CalendarList cl = JsonUtility.FromJson<CalendarList>(jsonResponse);

            //if yada while to check if there are items and if yes show if not dont show.

            string allTitles = "";
            foreach (CalendarEvent cal in cl.results)
            {
                string calendarText = "<b>" + cal.title + "</b>" + "\n" + cal.description + "\n";
                allTitles += calendarText;
            }
            CalendarName.text = allTitles;
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }

        // ATTENDANCE
        using var www7 = UnityWebRequest.Get($"{url}/learn/api/public/v1/courses/courseId:GAME20/meetings");
        www7.SetRequestHeader("Authorization", $"Bearer {result.access_token}");
        www7.SetRequestHeader("Content-Type", "application/json");

        operation = www7.SendWebRequest();



        while (!operation.isDone)
        {
            await Task.Yield();
        }
        if (www7.result != UnityWebRequest.Result.Success)
        {
            Debug.Log($"Failed: {www7.error}");
        }

        jsonResponse = www7.downloadHandler.text;
        Debug.Log(jsonResponse);
        meetingList1 = JsonUtility.FromJson<MeetingList>(jsonResponse);
        try
        {
            if (meetingList1 != null)
            {
                List<string> startTimes = new List<string>();
                foreach (Meeting meeting in meetingList1.results)
                {
                    Debug.Log($"Meeting Date: {meeting.start}");
                    startTimes.Add(meeting.start);
                    //Debug.Log($"Meeting Date: {meeting.start}");
                }
                dropdown.ClearOptions();
                dropdown.AddOptions(startTimes);
                // Select the first option by default
                dropdown.value = 0;
                // Set the dropdown's label text to the selected option
                dropdown.onValueChanged.AddListener(delegate
                {
                    UpdateStatusText(result, meetingList1);
                    SetLabel();
                });
                SetLabel();
            }
            void SetLabel()
            {
                if (dropdown.options.Count > 0)
                {
                    string selectedOption = dropdown.options[dropdown.value].text;
                    dropdown.GetComponentInChildren<TextMeshProUGUI>().text = selectedOption;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);

        }

    }

    private async void UpdateStatusText(BbAuthContext result, MeetingList meetingList1)
    {
        if (dropdown.options.Count == 0)
        {
            // The dropdown list is empty
            AttendanceName.text = "No meetings available";
            return;
        }

        int selectedIndex = dropdown.value;

        if (selectedIndex < 0 || selectedIndex >= dropdown.options.Count)
        {
            // The selected index is out of range
            AttendanceName.text = "Invalid selection";
            return;
        }

        string selectedMeeting = dropdown.options[selectedIndex].text.Trim();

        using var www = UnityWebRequest.Get($"{url}/learn/api/public/v1/courses/courseId:GAME20/meetings/users/_17978_1");
        www.SetRequestHeader("Authorization", $"Bearer {result.access_token}");
        www.SetRequestHeader("Content-Type", "application/json");

        var operation = www.SendWebRequest();

        while (!operation.isDone)
        {
            await Task.Yield();
        }

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log($"Failed: {www.error}");
            AttendanceName.text = "Failed to get status";
            return;
        }

        var jsonResponse = www.downloadHandler.text;
        Debug.Log(jsonResponse);

        AttendanceList statusResponse = JsonUtility.FromJson<AttendanceList>(jsonResponse);

        if (statusResponse == null || statusResponse.results.Count == 0)
        {
            // No status information available
            AttendanceName.text = "No status available";
            return;
        }

        if (meetingList1 != null && meetingList1.results != null)
        {
            foreach (Meeting meeting in meetingList1.results)
            {
                Debug.Log($"Meeting Date: {meeting.start}");
            }
        }
        else
        {
            Debug.Log("Meeting list is empty");
        }

        // Find the meeting ID for the selected meeting
        string selectedMeetingId = null;
        foreach (Meeting meeting in meetingList1.results)
        {
            if (meeting.start == selectedMeeting)
            {
                selectedMeetingId = meeting.id.ToString();
                break;
            }
        }
        Debug.Log($"Selected MeetingId is: {selectedMeetingId}");


        // Search for the user's attendance status
        Attendance userStatus = statusResponse.results.Find(status =>
        {
            bool idMatch = status.meetingId == selectedMeetingId;
            return idMatch;
        });

        if (userStatus != null)
        {
            Debug.Log($"UserStatus: {userStatus.status}");
            AttendanceName.text = $"User status: {userStatus.status}";
        }
        else
        {
            Debug.Log($"Selected meeting: {selectedMeeting}");
            Debug.Log("Meeting IDs in attendance list:");
            foreach (var status in statusResponse.results)
            {
                Debug.Log($"- {status.meetingId}");
            }
            AttendanceName.text = "User did not attend this meeting";
        }
    }



}

