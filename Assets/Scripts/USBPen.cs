using Unity.VisualScripting;
using UnityEngine;

public class USBPen : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) 
        {
            USBPenText.penCount += 1;
            Destroy(gameObject);
        }
    }
}
