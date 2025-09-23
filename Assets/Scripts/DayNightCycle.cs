using TMPro;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private Light sun;
    [SerializeField] private Light moon;

    [SerializeField] private float dayDuration = 1440f; // Seconds for full day
    [Range(0, 1)][SerializeField] private float timeOfDay = 0f;

    [SerializeField] private SkyboxBlender skyboxBlender;

    [SerializeField] private TextMeshProUGUI clockText;

    private float timeRate;

    void Start()
    {
        timeRate = 1f / dayDuration;
    }

    void Update()
    {
        timeOfDay += Time.deltaTime * timeRate;
        if (timeOfDay >= 1f) timeOfDay = 0f;

        UpdateLighting(timeOfDay);
        UpdateClock(timeOfDay);
    }

    private void UpdateLighting(float timePercent)
    {
        float sunRotation = timePercent * 360f - 90f;
        sun.transform.rotation = Quaternion.Euler(sunRotation, 170f, 0f);
        moon.transform.rotation = Quaternion.Euler(sunRotation + 180f, 170f, 0f);

        float sunDot = Mathf.Clamp01(Vector3.Dot(sun.transform.forward, Vector3.down));
        sun.intensity = Mathf.Lerp(0f, 1.2f, sunDot);
        moon.intensity = Mathf.Lerp(0.3f, 0f, sunDot);

        RenderSettings.ambientIntensity = Mathf.Lerp(0.2f, 1f, sunDot);

        if (skyboxBlender != null)
            skyboxBlender.SetBlend(1f - sunDot);
    }
    
    private void UpdateClock(float timePercent)
    {
        // Convert percentage of day into hours & minutes
        float totalMinutes = timePercent * 24f * 60f;
        int hours = Mathf.FloorToInt(totalMinutes / 60f);
        int minutes = Mathf.FloorToInt(totalMinutes % 60f);

        // Format GTA style (HH:MM)
        string formattedTime = string.Format("{0:00}:{1:00}", hours, minutes);

        if (clockText != null)
        {
            clockText.text = formattedTime;
            Debug.Log(formattedTime);
        }
    }
}
