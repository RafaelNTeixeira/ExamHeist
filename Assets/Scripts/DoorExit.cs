using UnityEngine;

public class DoorExit : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    private bool doorOpened = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OpenExitDoor()
    {
        spriteRenderer.enabled = false; 
        doorOpened = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (doorOpened)
                Debug.Log("PLAYER WON");
        }
    }
}
