using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoadGenerator : MonoBehaviour
{
    public static RoadGenerator instance;
    public GameObject RoadPrefab;
    private List<GameObject> roads = new List<GameObject>();
    public float maxSpeed = 10.0f;
    private float speed = 0.0f;
    public int maxRoadCount = 5;

    void Awake() => instance = this;

    void Start()
    {
        ResetLevel();
        //StartLevel();
    }

    void Update()
    {
        if (speed == 0) return;

        foreach (GameObject road in roads)
            road.transform.position -= new Vector3(0, 0, speed * Time.deltaTime);
        
        if (roads[0].transform.position.z < -15)
        {
            Destroy(roads[0]);
            roads.RemoveAt(0);

            CreateNextRoad();
        }

    }

    private void CreateNextRoad()
    {
        Vector3 position = Vector3.zero;
        if (roads.Count > 0)
            position = roads[roads.Count - 1].transform.position + new Vector3(0, 0, 15);
        GameObject go = Instantiate(RoadPrefab, position, Quaternion.identity);
        go.transform.SetParent(transform);
        roads.Add(go);
    }


    public void StartLevel()
    {
       speed = maxSpeed;
       SwipeManager.instance.enabled = true;
    }

    public void ResetLevel()
    {
        speed = 0.0f;
        while (roads.Count > 0)
        {
            Destroy(roads[0]);
            roads.RemoveAt(0);
        }
        for (int i = 0; i < maxRoadCount; i++)
            CreateNextRoad();

        SwipeManager.instance.enabled = false;
    }


}
