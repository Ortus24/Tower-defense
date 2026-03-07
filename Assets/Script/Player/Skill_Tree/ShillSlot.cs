using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class ShillSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public List<ShillSlot> prerequissiteSkillSlots;
    public SkillSO skillSO;

    public int currentLevel;
    public bool isUnlocked;

    public TMP_Text skillLevelText;
    public Image backgroundImage;
    public Image skillIcon;
    public Button skillButton;
    public Image braking;

    public static event Action<ShillSlot> OnAbilityPointSpent;
    public static event Action<ShillSlot> OnSkillUnlocked;

    public void OnValidate()
    {
        if(skillSO != null && skillLevelText != null && backgroundImage != null)
        {
            UpdateUI();
        }
    }

    public void UnLock()
    {
        isUnlocked = true;
        UpdateUI();
    }
    
    public void TryUpgradeSkill()
    {
        // Thêm điều kiện kiểm tra còn điểm nâng cấp hay không
        if(isUnlocked && currentLevel < skillSO.maxLevel && SkillTreeManager.instance != null && SkillTreeManager.instance.availablePoint > 0)
        {
            currentLevel++;
            OnAbilityPointSpent?.Invoke(this);

            if (currentLevel >= skillSO.maxLevel)
            {
                OnSkillUnlocked?.Invoke(this);
            }

            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        if(isUnlocked)
        {
            skillButton.interactable = true;
            skillLevelText.text = currentLevel.ToString() + "/" + skillSO.maxLevel.ToString();
            skillLevelText.fontSize = 20;
            backgroundImage.color = new Color32(255, 255, 255, 0);
            skillIcon.color = new Color32(255, 255, 255, 255);
            braking.color = new Color32(238, 238, 238, 50);
        }
        else
        {
            skillButton.interactable = false;
            skillLevelText.text = "";
            skillLevelText.fontSize = 11;
            backgroundImage.color = new Color32(50, 50, 50, 255);
            skillIcon.color = new Color32(255, 255, 255, 130);
            braking.color = new Color32(238, 238, 238, 0);
        }
    }

    public void RefreshTooltip()
    {
        if (skillSO != null && SkillTooltipManager.instance != null)
        {
            string levelInfo = currentLevel + "/" + skillSO.maxLevel;
            string requirements = "";
            
            if (!isUnlocked)
            {
                requirements = "[Chưa đủ điều kiện kích hoạt]\n[Hãy nâng cấp Max kỹ năng trước đó]";
            }

            SkillTooltipManager.instance.ShowTooltip(
                skillSO, 
                currentLevel,
                skillSO.maxLevel,
                requirements,
                this.transform
            );
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        RefreshTooltip();
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        if (SkillTooltipManager.instance != null)
        {
            SkillTooltipManager.instance.HideTooltip();
        }
    }
}
