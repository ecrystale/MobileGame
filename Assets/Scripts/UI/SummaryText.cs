using UnityEngine;
using UnityEngine.UI;

public class SummaryText : MonoBehaviour
{
    public Text ValueText;

    public void UpdateValue(string value)
    {
        ValueText.text = value;
    }
}
