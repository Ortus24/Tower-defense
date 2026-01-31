using UnityEngine;

public class BarrackTower : BaseTower
{
    public GameObject knightPrefab;
    public Transform[] spawnPoints;
    [SerializeField] private int numberOfKnights = 2;
    protected override void OnBuildComplete()
    {
        SpawnKnights();
    }

    void SpawnKnights()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            GameObject knightGO = Instantiate(knightPrefab, spawnPoints[i].position, Quaternion.identity);
            KnightAI knightScript = knightGO.GetComponent<KnightAI>();

            if (knightScript != null)
            {
                knightScript.parentBarrack = this; // Gán nhà lính làm chủ thể quản lý
            }
        }
    }
}
