using UnityEngine;

public class SkillOneController : MonoBehaviour
{
    public GameObject rangeCircle;      // vòng tròn AOE
    public GameObject skillEffectPrefab; // hiệu ứng skill
    public float radius = 3f;

    private bool isCasting = false;

    void Update()
    {
        if (!isCasting) return;

        FollowMouse();

        if (Input.GetMouseButtonDown(0))
        {
            CastSkill();
        }

        if (Input.GetMouseButtonDown(1))
        {
            CancelSkill();
        }
    }

    public void ActivateSkill()
    {
        isCasting = true;
        rangeCircle.SetActive(true);
    }

    void FollowMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        rangeCircle.transform.position = mousePos;
    }

    void CastSkill()
    {
        Instantiate(skillEffectPrefab, rangeCircle.transform.position, Quaternion.identity);
        CancelSkill();
    }

    void CancelSkill()
    {
        isCasting = false;
        rangeCircle.SetActive(false);
    }
}
