using System.Net.NetworkInformation;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class PlacementVidual : MonoBehaviour
{
    [Header("Gán đối tượng Vòng tròn vào đây")]
    public GameObject rangeCircleObject;

    public void Start()
    {
        // Quan trọng: Mặc định luôn ẩn khi khởi tạo
        // (Trừ khi code bên ngoài gọi ToggleRange(true) ngay sau đó)
        if (rangeCircleObject != null) rangeCircleObject.SetActive(false);
    }

    public void SetRange(float range)
    {
        if (rangeCircleObject != null)
        {
            // Scale = Range * 2 (Vì Range là bán kính)
            float diameter = range * 2f;
            rangeCircleObject.transform.localScale = new Vector3(diameter, diameter, 1f);
        }
    }

    public void ToggleRange(bool show)
    {
        if (rangeCircleObject != null) rangeCircleObject.SetActive(show);
    }
}
