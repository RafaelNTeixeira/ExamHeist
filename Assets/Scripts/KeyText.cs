using UnityEngine;
using UnityEngine.UI;

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
