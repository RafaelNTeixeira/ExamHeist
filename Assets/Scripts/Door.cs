using UnityEngine;

public class Door : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Collider2D doorCollider;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        doorCollider = GetComponent<Collider2D>();
    }

    public void OpenDoor()
    {
        spriteRenderer.enabled = false; 
        doorCollider.enabled = false;  
    }
}
