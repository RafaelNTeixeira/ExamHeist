using UnityEngine;

// Class responsible for the SpeedBoost object
// It increases the player's speed when picked up
public class SpeedBoost : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) 
        {
            if (!other.gameObject.GetComponent<Player>().isBoosted)
                other.gameObject.GetComponent<Player>().BoostSpeed(2.0f);

            else{
                other.gameObject.GetComponent<Player>().ApplySpeed(1/2.0f);
                other.gameObject.GetComponent<Player>().BoostSpeed(2.0f);
            }
            
            Destroy(gameObject);
        }
    }
}
