using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;
 
// Class to manage the eletric panel
// It disables the security cameras, lights and alarm for a certain amount of time
public class EletricPanel : MonoBehaviour
{
    private bool isPlayerInside = false;
    [SerializeField] private SecurityCamera[] securityCameras;
    [SerializeField] private Light2D[] lamps;
    [SerializeField] private AlarmLamp[] alarmLamps;
    [SerializeField] private Stairs[] stairs;

    private void Update()
    {
        if (isPlayerInside && Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (GameManager.instance.currentState == GameManager.GameState.Paused) return;

            else if (GameManager.instance.currentState == GameManager.GameState.Playing)
            {
                ToggleEletricPanel(40); // Activate the eletric panel for 40 seconds
            }

            else if (GameManager.instance.currentState == GameManager.GameState.Tutorial)
            {
                ToggleEletricPanel(5); // Activate the eletric panel for 5 seconds
            }
        }
    }

    // Function to disable the security cameras, lights, alarm for a certain amount of time and unlock the stairs
    private void ToggleEletricPanel(int seconds)
    {
        StartCoroutine(DisableCamerasForSeconds(seconds));
        StartCoroutine(DisableLightsForSeconds(seconds));
        StartCoroutine(DisableAlarmForSeconds(seconds));
        UnlockStairs();
    }

    // Coroutine to disable the security cameras for a certain amount of time
    private IEnumerator DisableCamerasForSeconds(float seconds)
    {
        foreach (SecurityCamera camera in securityCameras)
        {
            camera.DisableCamera();
        }
        yield return new WaitForSeconds(seconds); // Wait for the seconds to pass before enabling the cameras again
        foreach (SecurityCamera camera in securityCameras)
        {
            camera.EnableCamera();
        }
    }

    // Coroutine to disable the lights for a certain amount of time
    private IEnumerator DisableLightsForSeconds(float seconds)
    {
        foreach (Light2D lamp in lamps)
        {
            lamp.enabled = false;
        }
        yield return new WaitForSeconds(seconds); // Wait for the seconds to pass before enabling the lights again
        foreach (Light2D lamp in lamps)
        {
            lamp.enabled = true;
        }
    }

    // Coroutine to disable the alarm for a certain amount of time
    private IEnumerator DisableAlarmForSeconds(float seconds)
    {
        AlarmManager.instance.StopAlarm();
        foreach (var alarmLamp in alarmLamps)
        {
            if (alarmLamp != null)
            {
                alarmLamp.ActivateAlarmLight(false);
            }
        }
        yield return new WaitForSeconds(seconds); // Wait for the seconds to pass before enabling the alarm again
        foreach (var alarmLamp in alarmLamps)
        {
            if (alarmLamp != null)
            {
                alarmLamp.ActivateAlarmLight(true);
            }
        }
        AlarmManager.instance.ResumeAlarm();
    }

    // Unlock the stairs
    private void UnlockStairs()
    {
        foreach (Stairs stair in stairs)
        {
            stair.BlockStairs(false);
        }
    }

    // Function to detect if the player is inside the interaction range of electric panel
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            isPlayerInside = true; 
        }
    }

    // Function to detect if the player exits the interaction range of electric panel
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            isPlayerInside = false;
        }
    }

}