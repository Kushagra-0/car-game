using UnityEngine;

[CreateAssetMenu(menuName = "Car/CarConfig", fileName = "NewCarConfig")]
public class CarConfigSO : ScriptableObject
{
    [Header("Core stats")]
    public float acceleration = 500f;
    public float steering = 200f;
    public float maxSpeed = 20f;
    public float minSpeedForSteering = 0.1f;

    [Header("Misc (kept for parity with your controller)")]
    public float downforce = 100f;
    public float dragCoefficient = 0.95f;
}
