using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class CourseContentLink : MonoBehaviour, IPointerClickHandler
{
    public string linkUrl;

    public void OnPointerClick(PointerEventData eventData)
    {
        Application.OpenURL(linkUrl);
    }

    internal void SetLinkData(string v1, string v2, string v3)
    {
        throw new NotImplementedException();
    }
}
