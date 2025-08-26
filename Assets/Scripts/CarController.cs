using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private float accleration = 500f;
    [SerializeField] private float steering = 200f;
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private float minSpeedForSteering = 0.1f;
    [SerializeField] private float downforce = 100f;
    [SerializeField] private float dragCoefficient = 0.95f;

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

        Vector3 force = transform.forward * moveInput.y * accleration * Time.fixedDeltaTime;

        if (rb.linearVelocity.magnitude < maxSpeed)
        {
            rb.AddForce(force, ForceMode.Acceleration);
        }

        if (rb.linearVelocity.magnitude > minSpeedForSteering)
        {
            float speedRatio = rb.linearVelocity.magnitude / maxSpeed;
            speedRatio = Mathf.Clamp01(speedRatio);


            float steerDirection = Vector3.Dot(rb.linearVelocity, transform.forward) >= 0 ? 1f : -1f;
            float steerInput = moveInput.x * steerDirection;


            float turn = steerInput * steering * Time.fixedDeltaTime;
            transform.Rotate(0, turn, 0, Space.World);
        }

        rb.linearVelocity *= 0.99f;
        
        Vector3 sidewaysVelocity = Vector3.Project(rb.linearVelocity, transform.right);
        rb.AddForce(-sidewaysVelocity * 5f, ForceMode.Acceleration);
    }
}
