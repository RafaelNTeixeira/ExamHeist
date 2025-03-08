using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) 
        {
            other.gameObject.GetComponent<Player>().BoostSpeed();
            
            Destroy(gameObject);
        }
    }
}
