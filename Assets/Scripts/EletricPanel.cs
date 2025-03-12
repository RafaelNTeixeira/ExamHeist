using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class EletricPanel : MonoBehaviour
{
    private bool isPlayerInside = false;
    [SerializeField] private SecurityCamera[] securityCameras;
    [SerializeField] private Light2D[] lamps;
    [SerializeField] private AlarmLamp[] alarmLamps;

    private void Update()
    {
        if (isPlayerInside && Input.GetKeyDown(KeyCode.UpArrow))
        {
            ToggleEletricPanel(5);
        }
    }

    private void ToggleEletricPanel(int seconds)
    {
        StartCoroutine(DisableCamerasForSeconds(seconds));
        StartCoroutine(DisableLightsForSeconds(seconds));
        StartCoroutine(DisableAlarmForSeconds(seconds));
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
        yield return new WaitForSeconds(seconds);
        foreach (var alarmLamp in alarmLamps)
        {
            if (alarmLamp != null)
            {
                alarmLamp.ActivateAlarmLight(true);
            }
        }
        AlarmManager.instance.ResumeAlarm();
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