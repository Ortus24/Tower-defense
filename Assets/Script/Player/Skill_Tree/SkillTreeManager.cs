using UnityEngine;
using TMPro;
public class SkillTreeManager : MonoBehaviour
{
    public ShillSlot[] skillSlots;
    public TMP_Text pointText;
    public int availablePoint;

    public static SkillTreeManager instance;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void OnEnable()
    {
        ShillSlot.OnAbilityPointSpent += HandleAbilityPointSpent;
        ShillSlot.OnSkillUnlocked += HanldeSkillMaxed;
    }

    public void OnDisable()
    {
        ShillSlot.OnAbilityPointSpent -= HandleAbilityPointSpent;
        ShillSlot.OnSkillUnlocked -= HanldeSkillMaxed;
    }

    private void Start()
    {
        foreach (ShillSlot slot in skillSlots)
        {
            slot.skillButton.onClick.AddListener(slot.TryUpgradeSkill);
        }
        UpdateAbilityPoints(100);
    }

    public void UpdateAbilityPoints(int points)
    {
        availablePoint += points;
        pointText.text = "Points: " + availablePoint;
    } 

    private void HandleAbilityPointSpent(ShillSlot shillSlot)
    {
        if(availablePoint > 0)
        {
            UpdateAbilityPoints(shillSlot.currentLevel);
            shillSlot.RefreshTooltip();
        }
    }

    private void HanldeSkillMaxed(ShillSlot shillSlot)
    {
        // shillSlot là skill vừa tăng max level
        // Kiểm tra null để tránh lỗi (Exception)
        if (shillSlot.prerequissiteSkillSlots == null) return;

        // Chỉ mở khóa những skill nằm trong danh sách "skill tiếp theo" của skill vừa max này
        foreach(ShillSlot nextSlot in shillSlot.prerequissiteSkillSlots)
        {
            if(nextSlot != null && !nextSlot.isUnlocked)
            {
                nextSlot.UnLock();
            }
        }
    }
}

