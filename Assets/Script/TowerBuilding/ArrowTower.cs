using UnityEngine;

public class NewMonoBehaviourScript : BaseTower
{

    private float fireCountdown = 0f;

    protected override void OnBuildComplete()
    {
        //throw new System.NotImplementedException();
        { /* Hiệu ứng hoàn thành */ }
    }
    void Update()
    {
        if (!isBuilt) return;

        FindNearestTarget();
        if (target == null) return;

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / data.attackSpeed;
        }
        fireCountdown -= Time.deltaTime;
    }

    void Shoot()
    {
        // Instantiate Mũi tên và hướng về target
        Debug.Log("Arrow Tower bắn " + target.name);
    }
}
