using Assets.Script.TowerBuilding;
using Assets.Script.TowerBuilding.EconomyTower;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.LowLevel;

public class NewMonoBehaviourScript : BaseTower
{

    [Header("Cài đặt Bắn")]
    public GameObject arrowPrefab;
    public Transform[] firePoints;

    [Header("Cài đặt UI (Banner)")]
    public BuildingBanner bannerScript; // Kéo Canvas con (chứa script Banner) vào đây



    private Animator archerAnim;
    private float fireCountdown = 0f;

    protected override void Start()
    {
        base.Start();
        // 1. Setup Animator
        archerAnim = GetComponentInChildren<Animator>();

        // 2. Setup Banner (Giống hệt GoldMine)
        if (bannerScript != null)
        {
            // Truyền bản thân (gameObject) và Data vào để Banner biết nó đang quản lý ai
            bannerScript.Setup(gameObject, data);

            // Mặc định ẩn Banner đi
            bannerScript.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!isBuilt) return; // Nếu chưa xây xong thì không làm gì

        // --- Logic Bắn (Giữ nguyên) ---
        FindNearestTarget();

        if (target != null)
        {
            if (archerAnim != null) archerAnim.SetBool("isAttacking", true);

            // Kiểm tra tốc độ bắn
            if (fireCountdown <= 0f)
            {
                //Shoot();
                ShootMulti();
                // Công thức: 1 giây / số phát bắn mỗi giây (Ví dụ Spd=2 -> 0.5s bắn 1 lần)
                fireCountdown = 1f / data.attackSpeed;
            }
        }
        else
        {
            if (archerAnim != null) archerAnim.SetBool("isAttacking", false);
        }

        // Đếm ngược thời gian bắn
        if (fireCountdown > 0) fireCountdown -= Time.deltaTime;
    }

    // --- XỬ LÝ CLICK CHUỘT (THÊM MỚI) ---
    private void OnMouseDown()
    {
        // 1. Chặn nếu đang click vào nút UI (để không bị click xuyên)
       // if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;
        bool newState = true;
        // 2. Bật / Tắt Banner
        if (bannerScript != null)
        {
            // Nếu đang tắt -> Bật
            // Nếu đang bật -> Tắt
            bool isActive = bannerScript.gameObject.activeSelf;
            bannerScript.gameObject.SetActive(!isActive);
        }
        // 3. ĐỒNG BỘ THANH MÁU (Bật/Tắt cùng lúc với Banner)
        // Biến healthBarScript và currentHP được kế thừa sẵn từ BaseTower
        if (healthBarScript != null)
        {
            healthBarScript.Toggle(newState);

            // Nếu đang bật lên thì cập nhật lại giao diện máu cho chắc chắn
            if (newState == true)
            {
                healthBarScript.UpdateHealthUI(currentHP, data.maxHP);
            }
        }
    }
    void ShootMulti()
    {
        // Duyệt qua tất cả các điểm bắn đang có trong mảng
        foreach (Transform point in firePoints)
        {
            if (point != null)
            {
                CreateArrow(point);
            }
        }
    }

    void CreateArrow(Transform spawnPoint)
    {
        if (arrowPrefab != null)
        {
            // Tạo mũi tên tại vị trí của spawnPoint (trái hoặc phải)
            GameObject arrowGO = Instantiate(arrowPrefab, spawnPoint.position, Quaternion.identity);
            ArrowProjectile arrow = arrowGO.GetComponent<ArrowProjectile>();

            if (arrow != null)
            {
                arrow.Seek(target, data.damage);
            }
        }
    }
}
