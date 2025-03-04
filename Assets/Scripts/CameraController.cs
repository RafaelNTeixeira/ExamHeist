using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;


    private void Update()
    {
        if (player.position.x < 0)
        {
            transform.position = new Vector3(0, 0, transform.position.z);
            return;
        }
        else if (player.position.x > 17.7){
            transform.position = new Vector3(17.7f, 0, transform.position.z);
            return;
        }
        transform.position = new Vector3(player.position.x, 0, transform.position.z);
    }

}
