using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private CarConfigSO carConfig;
    // [SerializeField] private float accleration = 500f;
    // [SerializeField] private float steering = 200f;
    // [SerializeField] private float maxSpeed = 20f;
    // [SerializeField] private float minSpeedForSteering = 0.1f;
    // [SerializeField] private float downforce = 100f;
    // [SerializeField] private float dragCoefficient = 0.95f;

    private Rigidbody rb;
    private CarInputHandler carInputHandler;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.centerOfMass = new Vector3(0, -0.5f, 0);
        carInputHandler = GetComponent<CarInputHandler>();
    }

    void FixedUpdate()
    {
        Vector2 moveInput = carInputHandler.GetMoveInput();

        Vector3 force = transform.forward * moveInput.y * carConfig.acceleration * Time.fixedDeltaTime;

        if (rb.linearVelocity.magnitude < carConfig.maxSpeed)
        {
            rb.AddForce(force, ForceMode.Acceleration);
        }

        if (rb.linearVelocity.magnitude > carConfig.minSpeedForSteering)
        {
            float speedRatio = rb.linearVelocity.magnitude / carConfig.maxSpeed;
            speedRatio = Mathf.Clamp01(speedRatio);


            float steerDirection = Vector3.Dot(rb.linearVelocity, transform.forward) >= 0 ? 1f : -1f;
            float steerInput = moveInput.x * steerDirection;


            float turn = steerInput * carConfig.steering * Time.fixedDeltaTime;
            // transform.Rotate(0, turn, 0, Space.World);
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0, turn, 0));
        }

        rb.linearVelocity *= 0.99f;

        Vector3 sidewaysVelocity = Vector3.Project(rb.linearVelocity, transform.right);
        rb.AddForce(-sidewaysVelocity * 5f, ForceMode.Acceleration);
    }

    private void LateUpdate()
    {
        float tiltX = Mathf.Abs(transform.eulerAngles.x);
        float tiltZ = Mathf.Abs(transform.eulerAngles.z);

        if (tiltX > 180) tiltX = 360 - tiltX;
        if (tiltZ > 180) tiltZ = 360 - tiltZ;

        if (tiltX > 1f || tiltZ > 1f)
        {
            Quaternion targetRotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }
}
