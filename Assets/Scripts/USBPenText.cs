using UnityEngine;
using UnityEngine.UI;

// Class responsible for the USB pen count on the screen
public class USBPenText : MonoBehaviour
{
    Text text;
    public static int penCount = 1;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    private void Update()
    {
        text.text = penCount + "";
    }
}
