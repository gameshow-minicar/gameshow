using UnityEngine;

public class StageGenerator : MonoBehaviour
{
    public GameObject roadPrefab;
    public GameObject cliffPrefab;
    public GameObject pointPrefab;
    public Transform player;
    public Vector3 lastRoadPos;

    public int maxDistance = 2000;
    public int minDistance = 2000;
    public float chunkSize = 10f;

    void Start()
    {
        GenerateStage();
    }

    void GenerateStage()
    {
        int Dist = Random.Range(minDistance, maxDistance+1);
        int count = Mathf.CeilToInt(Dist / chunkSize);

        Vector3 startPos = player.position + new Vector3(chunkSize * 0.5f, 0f, 0f);

        for (int i = 0; i < count; i++)
        {
            Vector3 pos = startPos + new Vector3(i * chunkSize, 0f, 0f);
            Instantiate(roadPrefab, pos, Quaternion.identity);
            if (i == count - 1)
            {
                lastRoadPos = pos + new Vector3(chunkSize * 0.5f, 1f, 0f);
                Instantiate(pointPrefab, lastRoadPos, Quaternion.identity);
            }
        }

        // 崖を最後に配置
        Vector3 cliffPos = startPos + new Vector3(count * chunkSize, 0f, 0f);
        Instantiate(cliffPrefab, cliffPos, Quaternion.identity);
    }
}