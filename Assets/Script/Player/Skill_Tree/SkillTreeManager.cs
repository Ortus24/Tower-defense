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
        skillSlotStatsUpdate();
        pointText.text = "Points: " + availablePoint;
    }

    private void skillSlotStatsUpdate()
    {
        int lastAppliedHPLevel = skillSlots[0].currentLevel;
        skillSlots[0].skillButton.onClick.AddListener(() => 
        {
            // Kiểm tra xem level có thực sự tăng lên không (nếu max level hoặc không đủ điểm thì level sẽ không đổi)
            if (skillSlots[0].currentLevel > lastAppliedHPLevel)
            {
                int hpToAdd = skillSlots[0].skillSO.amount[skillSlots[0].currentLevel] - skillSlots[0].skillSO.amount[lastAppliedHPLevel];
                StatesManager.Instance.IncreaseHP(hpToAdd);
                lastAppliedHPLevel = skillSlots[0].currentLevel; // cập nhật lại level hiện tại
            }
        });

        int lastAppliedHealthHpLevel = skillSlots[1].currentLevel;
        skillSlots[1].skillButton.onClick.AddListener(() => 
        {
            if (skillSlots[1].currentLevel > lastAppliedHealthHpLevel)
            {
                int healthHpToAdd = skillSlots[1].skillSO.amount[skillSlots[1].currentLevel] - skillSlots[1].skillSO.amount[lastAppliedHealthHpLevel];
                StatesManager.Instance.IncreaseHPHealth(healthHpToAdd);
                lastAppliedHealthHpLevel = skillSlots[1].currentLevel;
            }
        });

        int lastAppliedManaLevel = skillSlots[2].currentLevel;
        skillSlots[2].skillButton.onClick.AddListener(() => 
        {
            if (skillSlots[2].currentLevel > lastAppliedManaLevel)
            {
                int manaToAdd = skillSlots[2].skillSO.amount[skillSlots[2].currentLevel] - skillSlots[2].skillSO.amount[lastAppliedManaLevel];
                StatesManager.Instance.IncreaseMana(manaToAdd);
                lastAppliedManaLevel = skillSlots[2].currentLevel;
            }
        });

        int lastAppliedHoiManaLevel = skillSlots[3].currentLevel;
        skillSlots[3].skillButton.onClick.AddListener(() => 
        {
            if (skillSlots[3].currentLevel > lastAppliedHoiManaLevel)
            {
                int hoiManaToAdd = skillSlots[3].skillSO.amount[skillSlots[3].currentLevel] - skillSlots[3].skillSO.amount[lastAppliedHoiManaLevel];
                StatesManager.Instance.IncreaseHoiMana(hoiManaToAdd);
                lastAppliedHoiManaLevel = skillSlots[3].currentLevel;
            }
        });

        int lastAppliedDamageLevel = skillSlots[4].currentLevel;
        skillSlots[4].skillButton.onClick.AddListener(() => 
        {
            if (skillSlots[4].currentLevel > lastAppliedDamageLevel)
            {
                int damageToAdd = skillSlots[4].skillSO.amount[skillSlots[4].currentLevel] - skillSlots[4].skillSO.amount[lastAppliedDamageLevel];
                StatesManager.Instance.IncreaseDamage(damageToAdd);
                lastAppliedDamageLevel = skillSlots[4].currentLevel;
            }
        });

        int lastAppliedAttackSpeedLevel = skillSlots[5].currentLevel;
        skillSlots[5].skillButton.onClick.AddListener(() => 
        {
            if (skillSlots[5].currentLevel > lastAppliedAttackSpeedLevel)
            {
                int attackSpeedToAdd = skillSlots[5].skillSO.amount[skillSlots[5].currentLevel] - skillSlots[5].skillSO.amount[lastAppliedAttackSpeedLevel];
                StatesManager.Instance.IncreaseAttackSpeed(attackSpeedToAdd);
                lastAppliedAttackSpeedLevel = skillSlots[5].currentLevel;
            }
        });

        int lastAppliedSpeedLevel = skillSlots[6].currentLevel;
        skillSlots[6].skillButton.onClick.AddListener(() => 
        {
            if (skillSlots[6].currentLevel > lastAppliedSpeedLevel)
            {
                int speedToAdd = skillSlots[6].skillSO.amount[skillSlots[6].currentLevel] - skillSlots[6].skillSO.amount[lastAppliedSpeedLevel];
                StatesManager.Instance.IncreaseSpeed(speedToAdd);
                lastAppliedSpeedLevel = skillSlots[6].currentLevel;
            }
        });

        int lastAppliedNeLevel = skillSlots[7].currentLevel;
        skillSlots[7].skillButton.onClick.AddListener(() => 
        {
            if (skillSlots[7].currentLevel > lastAppliedNeLevel)
            {
                int neToAdd = skillSlots[7].skillSO.amount[skillSlots[7].currentLevel] - skillSlots[7].skillSO.amount[lastAppliedNeLevel];
                StatesManager.Instance.IncreaseNe(neToAdd);
                lastAppliedNeLevel = skillSlots[7].currentLevel;
            }
        });

        int lastAppliedCriticalChanceLevel = skillSlots[8].currentLevel;
        skillSlots[8].skillButton.onClick.AddListener(() => 
        {
            if (skillSlots[8].currentLevel > lastAppliedCriticalChanceLevel)
            {
                int criticalChanceToAdd = skillSlots[8].skillSO.amount[skillSlots[8].currentLevel] - skillSlots[8].skillSO.amount[lastAppliedCriticalChanceLevel];
                StatesManager.Instance.IncreaseCriticalChance(criticalChanceToAdd);
                lastAppliedCriticalChanceLevel = skillSlots[8].currentLevel;
            }
        });
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
            UpdateAbilityPoints(-shillSlot.skillSO.pointReq[shillSlot.currentLevel-1]);
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

