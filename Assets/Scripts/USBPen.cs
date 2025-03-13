using Unity.VisualScripting;
using UnityEngine;

// Class responsible for the USBPen object
// It increases the pen count when picked up
public class USBPen : MonoBehaviour
{
    public AudioClip pickupSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) 
        {
            USBPenText.penCount += 1; // Increment the pen count

            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            
            Destroy(gameObject);
        }
    }
}
