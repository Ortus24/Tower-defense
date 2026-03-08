#if UNITY_EDITOR
using UnityEditor.ShaderGraph;
#endif
using UnityEngine;

[CreateAssetMenu(fileName = "New Item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    [TextArea] public string itemDescription;
    public Sprite itemIcon;
    public int stackSize = int.MaxValue;

    [Header("Stats Recovery")]
    public int healAmount; // Lượng máu hồi
    public int manaAmount; // Lượng năng lượng hồi
}
