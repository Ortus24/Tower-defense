using Assets.Script.TowerBuilding;
using UnityEngine;

public class NewMonoBehaviourScript : BaseTower
{

    private Animator archerAnim;
    private float fireCountdown = 0f;

    [Header("Cài đặt bắn")]
    public GameObject arrowPrefab; // Kéo Prefab mũi tên vào đây
    public Transform firePoint;   // Điểm bắn (vị trí tay cung thủ)

    void Start()
    {
        // Lấy Animator từ đối tượng con Archer_Blue_34
        archerAnim = GetComponentInChildren<Animator>();
    }

    protected override void OnBuildComplete() { }

    void Update()
    {
        if (!isBuilt) return;

        FindNearestTarget();

        if (target != null)
        {
            // Cập nhật Animator
            if (archerAnim != null) archerAnim.SetBool("isAttacking", true);

            if (fireCountdown <= 0f)
            {
                Shoot();
                fireCountdown = 1f / data.attackSpeed;
            }
        }
        else
        {
            if (archerAnim != null) archerAnim.SetBool("isAttacking", false);
        }

        if (fireCountdown > 0) fireCountdown -= Time.deltaTime;
    }

    void Shoot()
    {
        if (arrowPrefab != null && firePoint != null)
        {
            // Sinh ra mũi tên tại vị trí firePoint
            GameObject arrowGO = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
            ArrowProjectile arrow = arrowGO.GetComponent<ArrowProjectile>();

            if (arrow != null)
            {
                arrow.Seek(target); // Ra lệnh cho mũi tên đuổi theo quái
            }
        }
    }
}
