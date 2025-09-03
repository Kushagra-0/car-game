using Unity.Cinemachine;
using UnityEngine;

public class CarSpawnManager : MonoBehaviour
{
    [SerializeField] private CinemachineCamera followCamera;
    [SerializeField] private GameObject[] carPrefabs;

    private GameObject currentCar;

    void Awake()
    {
        SpwainFirstCar(0, new Vector3(260, 0.3f, -208), Quaternion.identity);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SpawnCar(0);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            SpawnCar(1);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            SpawnCar(2);
    }

    public void SpwainFirstCar(int index, Vector3 position, Quaternion rotation)
    {
        if (currentCar != null) Destroy(currentCar);

        currentCar = Instantiate(carPrefabs[index], position, rotation);

        followCamera.Follow = currentCar.transform;
    }

    public void SpawnCar(int index)
    {
        // Destroy old car if it exists
        Vector3 spawnPos = Vector3.zero;
        Quaternion spawnRot = Quaternion.identity;

        if (currentCar != null)
        {
            spawnPos = currentCar.transform.position;
            spawnRot = currentCar.transform.rotation;
            Destroy(currentCar);
        }

        // Spawn the new car
        currentCar = Instantiate(carPrefabs[index], spawnPos, spawnRot);

        // Assign camera to follow the car root
        followCamera.Follow = currentCar.transform;
        // followCamera.LookAt = currentCar.transform;
    }
}
