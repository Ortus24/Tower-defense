using System.Collections;
using UnityEngine;

public class SkillOne : MonoBehaviour
{
    [Header("Skill Prefabs")]
    public GameObject lightningPrefab;   // prefab sét
    public GameObject rangeCircle;        // vòng tròn chọn vùng

    [Header("Rain Lightning Settings")]
    public int lightningCount = 15;       // số tia sét
    public float spawnInterval = 0.15f;   // thời gian giữa mỗi tia
    public float circleRadius = 3f;       // bán kính vòng tròn

    [Header("Cooldown")]
    public float cooldownTime = 5f;

    private bool canUseSkill = true;
    private bool isSelectingPosition = false;

    public SkillCooldownUI cooldownUI;

    void Start()
    {
        // ❗ KHÔNG HIỆN VÒNG TRÒN KHI MỚI CHẠY GAME
        rangeCircle.SetActive(false);
    }

    void Update()
    {
        // ẤN E → BẬT CHẾ ĐỘ CHỌN VỊ TRÍ
        if (Input.GetKeyDown(KeyCode.E) && canUseSkill)
        {
            ActivateSkill();
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
        canUseSkill = false;
        isSelectingPosition = false;

        Vector3 center = rangeCircle.transform.position;
        rangeCircle.SetActive(false);

        for (int i = 0; i < lightningCount; i++)
        {
            // RANDOM VỊ TRÍ TRONG VÒNG TRÒN
            Vector2 randomOffset = Random.insideUnitCircle * circleRadius;
            Vector3 spawnPos = center + new Vector3(randomOffset.x, randomOffset.y + 3f, 0);

            // SPAWN SÉT
            Instantiate(lightningPrefab, spawnPos, Quaternion.identity);

            yield return new WaitForSeconds(spawnInterval);
        }

        if (cooldownUI != null)
            cooldownUI.StartCooldown();

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
}
