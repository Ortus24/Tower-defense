using UnityEngine;
using TMPro;

public class SkillTooltipManager : MonoBehaviour
{
    public static SkillTooltipManager instance;

    [Header("UI References")]
    public GameObject tooltipPanel;
    public TMP_Text skillNameText;
    public TMP_Text skillDescriptionText;
    public TMP_Text currentLevelText;
    public TMP_Text requirementsText;
    public TMP_Text dateCurrent;
    public TMP_Text dataNext;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        CanvasGroup canvasGroup = tooltipPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = tooltipPanel.AddComponent<CanvasGroup>();
        }
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;

        HideTooltip(); 
    }

        // Cập nhật lại các tham số, nhận trực tiếp biến loại SkillSO
    public void ShowTooltip(SkillSO skillData, int currentLevel, int maxlevel , string requirements, Transform buttonTransform)
    {
        skillNameText.text = skillData.skillName;
        skillDescriptionText.text = skillData.skillDescription;

        currentLevelText.text = "Level: " + currentLevel + " / " + maxlevel;
        
        // Sử dụng Mathf.Min để giới hạn index, không bao giờ vượt qua độ dài mảng
        int safeAmountIdx = (skillData.amount != null && skillData.amount.Length > 0) ? Mathf.Min(currentLevel, skillData.amount.Length - 1) : 0;
        dateCurrent.text = skillData.amount != null && skillData.amount.Length > 0 ? skillData.amount[safeAmountIdx].ToString() : "0";
        
        if (currentLevel < maxlevel)
        {
            int safeNextAmountIdx = (skillData.amount != null && skillData.amount.Length > 0) ? Mathf.Min(currentLevel + 1, skillData.amount.Length - 1) : 0;
            dataNext.text = skillData.amount != null && skillData.amount.Length > 0 ? skillData.amount[safeNextAmountIdx].ToString() : "0";
            dataNext.fontSize = 30;
        }
        else
        {
            dataNext.text = "Max Level";
            dataNext.fontSize = 25;
        }

        if (string.IsNullOrEmpty(requirements))
        {
            int safeReqIdx = (skillData.pointReq != null && skillData.pointReq.Length > 0) ? Mathf.Min(currentLevel, skillData.pointReq.Length - 1) : 0;
            
            if (currentLevel >= maxlevel)
            {
                requirementsText.text = "Điều kiện: Đã Max";
            }
            else
            {
                requirementsText.text = "Điều kiện: " + (skillData.pointReq != null && skillData.pointReq.Length > 0 ? skillData.pointReq[safeReqIdx].ToString() : "0") + " exp";
            }
        }
        else
        {
            requirementsText.text = "Điều kiện: Mở khóa kỹ năng điều kiện";
        }

        tooltipPanel.SetActive(true);

        if (buttonTransform != null)
        {
            RectTransform buttonRect = buttonTransform.GetComponent<RectTransform>();
            if (buttonRect != null)
            {
                Vector3[] corners = new Vector3[4];
                buttonRect.GetWorldCorners(corners);
                
                Vector3 leftCenter = (corners[0] + corners[1]) / 2f; 
                
                tooltipPanel.transform.position = leftCenter - new Vector3(220, 0, 0);
            }
        }
    }


    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }
}
