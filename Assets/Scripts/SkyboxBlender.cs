using UnityEngine;

public class SkyboxBlender : MonoBehaviour
{
    [SerializeField] private Material blendedSkybox;
    [Range(0f, 1f)][SerializeField] private float blend = 0f;

    void Start()
    {
        if (blendedSkybox == null)
        {
            Debug.LogError("Assign the blended panoramic skybox material!");
            return;
        }

        RenderSettings.skybox = blendedSkybox;
    }

    void Update()
    {
        if (blendedSkybox != null)
        {
            blendedSkybox.SetFloat("_Blend", blend);
            DynamicGI.UpdateEnvironment();
        }
    }

    public void SetBlend(float value)
    {
        blend = Mathf.Clamp01(value);
    }
}
