using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System;
using TMPro;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class LoginManager : MonoBehaviour
{
    public static LoginManager Instance { get; private set; }


    public TextMeshProUGUI txt;
    private const string CLIENT_ID = "fa5893df-6b24-49bd-aa0b-a37feabdeb0e";
    private const string CLIENT_SECRET = "eCMvFEVmJTjAXgIBev34DefBW5RY0a4j";
    private const string REDIRECT_URI = "http:localhost:8080";
    private const string AUTHORIZATION_ENDPOINT = "https://ada-staging.blackboard.com/learn/api/public/v1/oauth2/authorizationcode";
    private const string TOKEN_ENDPOINT = "https://ada-staging.blackboard.com/learn/api/public/v1/oauth2/token";

    public string authorizationCode;
    public string accessToken;
    public string TestCode;
    public string user_id;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {

            string code = Application.absoluteURL.Substring(Application.absoluteURL.IndexOf("=") + 1);
            authorizationCode = code;
            GetToken();
        }
    }

    public void Login()
    {
        // Redirect user to the authorization endpoint
        string authorizationUrl = AUTHORIZATION_ENDPOINT + "?redirect_uri=" + "http:localhost:8080" /*REDIRECT_URI + SceneManager.GetActiveScene()*/ + "&response_type=code&client_id=" + CLIENT_ID + "&scope=read";
        Application.OpenURL(authorizationUrl);
    }

    public async void GetToken()
    {

        var auth = System.Convert.ToBase64String(
            System.Text.Encoding.Default.GetBytes(
                $"{CLIENT_ID}:{CLIENT_SECRET}"
            )
        );
        var basicAuth = $"Basic {auth}";
        WWWForm form = new WWWForm();
        form.AddField("grant_type", "authorization_code");
        form.AddField("code", TestCode);
        form.AddField("redirect_uri", "http:localhost:8080");
        using var www = UnityWebRequest.Post(TOKEN_ENDPOINT, form);
        //using var www = UnityWebRequest.Post($"{url}/learn/api/public/v1/oauth2/token?grant_type=authorization_code&code=[code]&redirect_uri=[your_application_url]", form);
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
        BbAuthContext result = new BbAuthContext();
        try
        {
            result = JsonConvert.DeserializeObject<BbAuthContext>(jsonResponse);
            Debug.Log($"Success: " + result.access_token);
            accessToken = result.access_token;
            user_id = result.user_id;

        }
        catch (Exception ex)
        {
            Debug.Log($"{this} could not parse json {jsonResponse}. {ex.Message}");
        }
        // if (www.result == UnityWebRequest.Result.Success)
        // {
        //     Debug.Log("Hello: " + result.access_token);
        // }
    }

    /*public IEnumerator RequestAccessToken()
    {
        using (UnityWebRequest www = UnityWebRequest.Post(TOKEN_ENDPOINT + "?grant_type=client_credentials", ""))
        {
            www.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            www.SetRequestHeader("Accept", "application/json");
            www.SetRequestHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(CLIENT_ID + ":" + CLIENT_SECRET)));
            yield return www.SendWebRequest();


            if (www.result == UnityWebRequest.Result.Success)
            {
                // Parse the response
                string responseText = www.downloadHandler.text;
                TokenResponse token = JsonUtility.FromJson<TokenResponse>(responseText);

                // Do something with the token
                Debug.Log("Access token: " + token.access_token);
                txt.text = "Token = " + token.access_token;
                accessToken = token.access_token;
            }
            else
            {
                txt.text = "Error";
            }
        }
    }*/




    [Serializable]
    public class AccessTokenResponse
    {
        public string access_token;
        public string token_type;
        public int expires_in;
        public string scope;
        public string user_id;
    }


    [System.Serializable]
    public class TokenResponse
    {
        public string access_token;
        public string token_type;
        public int expires_in;
    }






}
