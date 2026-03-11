using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillOne : MonoBehaviour
{
    [Header("Skill Prefabs")]
    public GameObject lightningPrefab;   // prefab sét
    public GameObject rangeCircle;        // vòng tròn chọn vùng
    public GameObject LightCircle;

    [Header("Rain Lightning Settings")]
    public int lightningCount = 15;       // số tia sét
    public float spawnInterval = 0.15f;   // thời gian giữa mỗi tia
    public float circleRadius = 3f;       // bán kính vòng tròn

    [Header("Cooldown")]
    public float cooldownTime = 70f;

    [Header("Damage")]
    public int damage = 10;
    public float damageDelay = 100f;
    public int manaCost = 20;


    [Header("LockSkill")]
    public Image backgroundLock;
    public Image backgroundIconlock;

    private bool canUseSkill = true;
    private bool isSelectingPosition = false;

    public SkillCooldownUI cooldownUI;

    private bool isOnpenSkill = false;

    private GameObject magicCircleInstance;

    void Start()
    {
        // ❗ KHÔNG HIỆN VÒNG TRÒN KHI MỚI CHẠY GAME
        rangeCircle.SetActive(false);
    }

    void Update()
    {
        // ẤN E → BẬT CHẾ ĐỘ CHỌN VỊ TRÍ
        if (Input.GetKeyDown(KeyCode.E) && canUseSkill && isOnpenSkill)
        {
            if (PlayerController.Instance != null && PlayerController.Instance.GetCurrentMana() >= manaCost)
            {
                ActivateSkill();
            }
            else if (PlayerController.Instance == null)
            {
                ActivateSkill();
            }
            else
            {
                Debug.Log("Không đủ mana!");
            }
        }

        // ĐANG CHỌN VỊ TRÍ
        if (isSelectingPosition)
        {
            FollowMouse();

            // CLICK CHUỘT TRÁI → SPAWN MƯA SÉT
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(RainLightning());
            }

            // CLICK CHUỘT PHẢI → HỦY
            if (Input.GetMouseButtonDown(1))
            {
                CancelSkill();
            }
        }
    }

    public void OpenSkill()
    {
        backgroundIconlock.enabled = false;
        backgroundLock.enabled = false;
        isOnpenSkill = true;
    }

    // =========================
    // BẬT SKILL
    // =========================  
    void ActivateSkill()
    {
        isSelectingPosition = true;
        rangeCircle.SetActive(true);
    }

    // =========================
    // VÒNG TRÒN ĐI THEO CHUỘT
    // =========================
    void FollowMouse()
    {
        if (Camera.main == null) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        rangeCircle.transform.position = mousePos;
    }

    // =========================
    // MƯA SÉT
    // =========================
    IEnumerator RainLightning()
    {
        if (PlayerController.Instance != null)
        {
            if (PlayerController.Instance.GetCurrentMana() < manaCost)
            {
                CancelSkill();
                yield break;
            }
            PlayerController.Instance.TakeMana(manaCost);
        }

        canUseSkill = false;
        isSelectingPosition = false;

        Vector3 center = rangeCircle.transform.position;
        rangeCircle.SetActive(false);

        // BẮT ĐẦU COOLDOWN NGAY KHI DÙNG SKILL
        if (cooldownUI != null)
        {
            cooldownUI.StartCooldown(cooldownTime);
        }
        StartCoroutine(CooldownTimer());

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f; // rất quan trọng trong 2D

        magicCircleInstance = Instantiate(LightCircle, mousePos, Quaternion.identity);

        float searchDuration = 5f;
        float elapsedTimer = 0f;
        int spawnedCount = 0;
        System.Collections.Generic.HashSet<Transform> struckEnemies = new System.Collections.Generic.HashSet<Transform>();

        // TÌM KIẾM TRONG KHOẢNG 5 GIÂY VÀ SPAWN TỐI ĐA SỐ SÉT CHO PHÉP
        while (elapsedTimer < searchDuration && spawnedCount < lightningCount)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(center, circleRadius);
            System.Collections.Generic.List<Transform> enemies = new System.Collections.Generic.List<Transform>();

            foreach (Collider2D col in colliders)
            {
                if (col.CompareTag("Enemy") && !struckEnemies.Contains(col.transform))
                {
                    enemies.Add(col.transform);
                }
            }

            if (enemies.Count > 0)
            {
                // CHỌN NGẪU NHIÊN 1 ENEMY CHƯA BỊ ĐÁNH VÀ SPAWN GẮN LÊN ĐẦU
                Transform target = enemies[Random.Range(0, enemies.Count)];
                
                // CỤ THỂ LÀ GÁN CHA CHO PREFAB NGAY KHI SPAWN:
                GameObject lightning = Instantiate(lightningPrefab, target.position, Quaternion.identity, target);

                // NẾU CẦN CHỈNH HIGHER HAY THẾ NÀO THÌ CÓ THỂ CHỈNH localPosition (MẶC ĐỊNH BẠN MUỐN SPAWN LÊN ĐẦU THÌ CÓ THỂ ĐỂ ZERO NGHĨA LÀ TRÙNG VỚI VỊ TRÍ)
                lightning.transform.localPosition = new Vector3(0, 0.7f, 0); // Hoặc new Vector3(0, 1f, 0) nếu muốn cao hơn 1 chút so với tâm enemy.
                
                // GÂY SÁT THƯƠNG CHO ENEMY (Được xử lý dựa trên Coroutine để delay theo animation)
                EnemyHealth enemyHealth = target.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    StartCoroutine(ApplyDamageAfterDelay(enemyHealth, damageDelay));
                }

                // ĐÁNH DẤU ENEMY ĐÃ BỊ ĐÁNH THEO YÊU CẦU: MỖI CON 1 SÉT
                struckEnemies.Add(target);

                spawnedCount++;
                
                yield return new WaitForSeconds(spawnInterval);
                elapsedTimer += spawnInterval;
            }
            else
            {
                // KIỂM TRA XEM CÓ ENEMY TRONG VÙNG NHƯNG ĐỀU ĐÃ BỊ ĐÁNH CHƯA
                bool hasEnemyInRange = false;
                foreach (Collider2D col in colliders)
                {
                    if (col.CompareTag("Enemy"))
                    {
                        hasEnemyInRange = true;
                        break;
                    }
                }

                if (hasEnemyInRange && struckEnemies.Count > 0)
                {
                    // CÒN QUÁI TRONG VÙNG NHƯNG ĐỀU ĐÃ BỊ ĐÁNH → XÓA DANH SÁCH ĐỂ CÓ THỂ ĐÁNH LẠI
                    struckEnemies.Clear();
                }
                else
                {
                    // KHÔNG CÓ MỤC TIÊU NÀO TRONG VÙNG → CHỜ FRAME KẾ TIẾP ĐỂ TÌM TIẾP
                    yield return null;
                    elapsedTimer += Time.deltaTime;
                }
            }
        }

        // HỦY VÒNG TRÒN PHÉP SAU KHI SÉT ĐÁNH XONG
        if (magicCircleInstance != null)
        {
            magicCircleInstance.GetComponent<DestroyWhenCall>().DestroyNow();
        }
    }

    // COROUTINE CHỜ CHO ĐẾN KHI ANIMATION SÉT ĐÁNH XUỐNG RỒI MỚI TRỪ MÁU
    IEnumerator ApplyDamageAfterDelay(EnemyHealth enemyHealth, float delay)
    {
        yield return new WaitForSeconds(delay);
        
        // Kiểm tra xem quái có bị chết đi trong lúc chờ delay không
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
        }
    }

    //------------------------

    // =========================
    // COOLDOWN TIMER (CHẠY SONG SONG VỚI HIỆU ỨNG SÉT)
    // =========================
    IEnumerator CooldownTimer()
    {
        yield return new WaitForSeconds(cooldownTime);
        canUseSkill = true;
    }

    // =========================
    // HỦY CHỌN
    // =========================
    void CancelSkill()
    {
        isSelectingPosition = false;
        rangeCircle.SetActive(false);
    }

    public void UpdateDamage(int amount)
    {
        damage += amount;
    }
}
