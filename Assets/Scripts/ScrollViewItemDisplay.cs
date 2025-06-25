using TMPro;
using UnityEngine;

public class ScrollViewItemDisplay : MonoBehaviour
{
    public TextMeshProUGUI DisplayText;

    public void WriteText(string text)
    {
        DisplayText.text = text;
    }
}
