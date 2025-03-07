using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class EletricPanel : MonoBehaviour
{
    private bool isPlayerInside = false;
    [SerializeField] private SecurityCamera[] securityCameras;
    [SerializeField] private Light2D[] lamps;

    private void Update()
    {
        if (isPlayerInside && Input.GetKeyDown(KeyCode.UpArrow))
        {
            ToggleEletricPanel();
        }
    }

    private void ToggleEletricPanel()
    {
        // For 20 seconds Camaras are disabled
        StartCoroutine(DisableCamerasForSeconds(5));

        // For 20 seconds lights are disabled
        StartCoroutine(DisableLightsForSeconds(5));
    }

    private IEnumerator DisableCamerasForSeconds(float seconds)
    {
        foreach (SecurityCamera camera in securityCameras)
        {
            camera.DisableCamera();
        }
        yield return new WaitForSeconds(seconds);
        foreach (SecurityCamera camera in securityCameras)
        {
            camera.EnableCamera();
        }
    }

    private IEnumerator DisableLightsForSeconds(float seconds)
    {
        foreach (Light2D lamp in lamps)
        {
            lamp.enabled = false;
        }
        yield return new WaitForSeconds(seconds);
        foreach (Light2D lamp in lamps)
        {
            lamp.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            isPlayerInside = true; 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            isPlayerInside = false;
        }
    }

}