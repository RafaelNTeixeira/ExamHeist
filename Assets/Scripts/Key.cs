using UnityEngine;

public class Key : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) 
        {
            KeyText.keyCount += 1;
            Destroy(gameObject);
        }
    }
}
