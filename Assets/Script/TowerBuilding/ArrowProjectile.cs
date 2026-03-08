
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.TowerBuilding
{
    public class ArrowProjectile : MonoBehaviour
    {
        private Transform target;
        public float speed = 15f;
        public float damage = 10f;
        public float targetOffset = 0.5f;// Thêm biến này để dễ chỉnh độ lệch trong Inspector nếu cần
        // Hàm để Tháp truyền mục tiêu cho mũi tên
        public void Seek(Transform _target, float _towerDamage)
        {
            target = _target;
            damage = _towerDamage; // Lấy sát thương chuẩn từ cục Data của Tháp
        }

        void Update()
        {
            if (target == null)
            {
                Destroy(gameObject); // Tự hủy nếu quái đã chết trước khi trúng
                return;
            }

            // Tính toán hướng bay
            Vector3 targetPos = target.position + Vector3.up * targetOffset;
            Vector3 dir = targetPos - transform.position;
            float distanceThisFrame = speed * Time.deltaTime;

            // Xoay mũi tên theo hướng bay
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // Kiểm tra nếu mũi tên đã chạm tới mục tiêu
            if (dir.magnitude <= distanceThisFrame)
            {
                HitTarget();
                return;
            }

            // Di chuyển mũi tên
            transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        }

        void HitTarget()
        {
            // Tìm component EnemyBase trên người con quái vật
            EnemyBase enemy = target.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // Trừ máu dựa trên số damage vừa nhận từ Tháp!
            }

            Debug.Log("Mũi tên trúng: " + target.name + " - Gây ra: " + damage + " sát thương");
            Destroy(gameObject); // Tự hủy mũi tên
        }

    }
}
