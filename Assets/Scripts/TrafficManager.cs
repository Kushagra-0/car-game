using System.Collections.Generic;
using UnityEngine;

public class TrafficManager : MonoBehaviour
{
    [SerializeField] private GameObject[] aiCarPrefabs;
    [SerializeField] private int maxCars = 20;
    [SerializeField] private float spawnRadius = 100f;
    [SerializeField] private float despawnDistance = 150f;
    [SerializeField] private float spawnInterval = 2f;

    private Transform player;

    private List<GameObject> activeCars = new List<GameObject>();
    private Waypoint[] waypoints;
    private float spawnTimer;

    void Start()
    {
        waypoints = FindObjectsOfType<Waypoint>();
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;

        // Spawn cars
        if (spawnTimer >= spawnInterval && activeCars.Count < maxCars)
        {
            SpawnCarNearPlayer();
            spawnTimer = 0f;
        }

        // Despawn cars
        for (int i = activeCars.Count - 1; i >= 0; i--)
        {
            if (Vector3.Distance(player.position, activeCars[i].transform.position) > despawnDistance)
            {
                Destroy(activeCars[i]);
                activeCars.RemoveAt(i);
            }
        }
    }

    void SpawnCarNearPlayer()
    {
        if (waypoints.Length == 0) return;

        // Pick random waypoint near player
        Waypoint spawnPoint = waypoints[Random.Range(0, waypoints.Length)];
        if (Vector3.Distance(spawnPoint.transform.position, player.position) > spawnRadius)
            return; // skip if too far

        // Pick random car prefab
        GameObject prefab = aiCarPrefabs[Random.Range(0, aiCarPrefabs.Length)];

        // Spawn car
        GameObject newCar = Instantiate(prefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
        activeCars.Add(newCar);
    }

    public void SetPlayer(Transform playerTransform)
    {
        player = playerTransform;
    }

}
