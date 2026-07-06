using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    public GameObject roadPrefab;

    public float maxDistance = 100000f;
    public float segmentLength = 10f;

    void Start()
    {
        GenerateRoad();
    }

    void GenerateRoad()
    {
        int count = Mathf.CeilToInt(maxDistance / segmentLength);

        for (int i = 0; i < count; i++)
        {
            Vector3 pos = new Vector3(0f, 0f, i * segmentLength);

            Instantiate(roadPrefab, pos, Quaternion.identity);
        }
    }
}