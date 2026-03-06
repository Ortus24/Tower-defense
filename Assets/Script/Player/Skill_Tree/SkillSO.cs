using UnityEngine;

[CreateAssetMenu(fileName = "SkillSO", menuName = "Skill Tree/SkillSO")]
public class SkillSO : ScriptableObject
{
    public string skillName;
    public string skillDescription;
    public int maxLevel;
    public int[] pointReq;
    public int[] amount;
}
