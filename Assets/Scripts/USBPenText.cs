using UnityEngine;
using UnityEngine.UI;

public class USBPenText : MonoBehaviour
{
    Text text;
    public static int penCount = 0;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    private void Update()
    {
        text.text = penCount + "";
    }
}
