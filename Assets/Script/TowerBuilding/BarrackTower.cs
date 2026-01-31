using UnityEngine;

public class BarrackTower : BaseTower
{
    public GameObject knightPrefab;
    public Transform[] spawnPoints;
    [SerializeField] private int numberOfPlayer = 2;
    protected override void OnBuildComplete()
    {
        SpawnKnights();
    }

    void SpawnKnights()
    {
        //for (int i = 0; i < numberOfPlayer; i++)
        //{
        //    Instantiate(knightPrefab, spawnPoints[i].position, Quaternion.identity);
        //}
    }
}
