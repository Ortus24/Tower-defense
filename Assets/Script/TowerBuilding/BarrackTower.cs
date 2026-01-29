using UnityEngine;

public class BarrackTower : BaseTower
{
    public GameObject knightPrefab;
    public Transform[] spawnPoints;
    protected override void OnBuildComplete()
    {
        SpawnKnights();
    }

    void SpawnKnights()
    {
        int count = Random.Range(2, 4); // Sinh ra 2-3 lính
        for (int i = 0; i < count; i++)
        {
            Instantiate(knightPrefab, spawnPoints[i].position, Quaternion.identity);
        }
    }
}
