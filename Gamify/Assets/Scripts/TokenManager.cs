using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenManager : MonoBehaviour
{
    public float expires_in = 3600;

    // Update is called once per frame
    void Update()
    {
        // we want to call the refresh to when it expires so we accommodate for the
        // time it takes to refresh the frame by a second
        if (expires_in > 1)
        {
            expires_in -= Time.deltaTime;
            // Debug.Log(expires_in);
        }
        else
        {
            // call to refresh token and assign it back to the timer
        }
    }

    public void setExpiresIn(float ei)
    {
        expires_in = ei;
        Debug.Log($"Set Expires In: {expires_in}");
    }
}