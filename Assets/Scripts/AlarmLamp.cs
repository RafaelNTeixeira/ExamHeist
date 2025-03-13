using UnityEngine;
using UnityEngine.Rendering.Universal;

// Class to manage the alarm lamp
// This class is responsible for the flickering effect of the alarm lamp
public class AlarmLamp : MonoBehaviour
{
    private Light2D alarmLight;
    private float flickerSpeed = 2.0f;
    public bool isActive = false;
    private float targetIntensity = 0f;
    private float currentIntensity = 0f;
    private float intensityLerpSpeed = 3.0f;

    private void Awake()
    {
        alarmLight = GetComponent<Light2D>();
        alarmLight.intensity = 0f; 
    }

    private void Update()
    {
        if (alarmLight != null)
        {
            // Flickering effect
            float alpha = (Mathf.Sin(Time.time * flickerSpeed) + 1) / 2;
            targetIntensity = isActive ? alpha * 3 : 0f;

            // Smoothly interpolate light intensity
            currentIntensity = Mathf.Lerp(currentIntensity, targetIntensity, Time.deltaTime * intensityLerpSpeed);
            alarmLight.intensity = currentIntensity;

            alarmLight.enabled = currentIntensity > 0.01f; // Prevents flickering when close to 0
        }
    }

    // Function to activate the alarm light
    public void ActivateAlarmLight(bool state)
    {
        isActive = state;
    }
}
