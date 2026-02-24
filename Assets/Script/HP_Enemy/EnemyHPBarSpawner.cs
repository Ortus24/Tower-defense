using UnityEngine;

public class EnemyHPBarSpawner : MonoBehaviour
{
    public EnemyHealth health;
    public Transform headPoint;
    public HPBarUI hpBarPrefab;

    void Start()
    {
        var hp = Instantiate(hpBarPrefab);
        hp.Init(health, headPoint);
    }
}
