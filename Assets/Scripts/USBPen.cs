using Unity.VisualScripting;
using UnityEngine;

public class USBPen : MonoBehaviour
{
    public AudioClip pickupSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) 
        {
            USBPenText.penCount += 1;

            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            
            Destroy(gameObject);
        }
    }
}
