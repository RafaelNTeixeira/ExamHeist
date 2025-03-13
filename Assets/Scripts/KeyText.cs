using UnityEngine;
using UnityEngine.UI;

// Class responsible for the key count on the screen
public class KeyText : MonoBehaviour
{
    Text text;
    public static int keyCount = 0;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    private void Update()
    {
        text.text = keyCount + "";
    }
}
