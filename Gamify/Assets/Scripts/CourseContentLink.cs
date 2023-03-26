using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class CourseContentLink : MonoBehaviour, IPointerClickHandler
{
    public string linkUrl;

    public void OnPointerClick(PointerEventData eventData)
    {
        Application.OpenURL(linkUrl);
    }

    public void SetLinkData(string linkID, string linkText, string linkUrl)
    {
        this.linkUrl = linkUrl;
        TextMeshProUGUI tmp = GetComponent<TextMeshProUGUI>();
        tmp.text = linkText;
        tmp.color = Color.blue;
        tmp.fontStyle = FontStyles.Bold;
        tmp.richText = true;
        tmp.rectTransform.sizeDelta = new Vector2(tmp.preferredWidth, tmp.preferredHeight);
    }
}
