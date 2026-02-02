using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Unity.AppUI.Redux;
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
        public void Seek(Transform _target)
        {
            target = _target;
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
            // Gây sát thương cho Enemy tại đây (nếu bạn đã có script máu)
            // target.GetComponent<EnemyHealth>().TakeDamage(damage);

            Debug.Log("Mũi tên trúng: " + target.name);
            Destroy(gameObject); // Hủy mũi tên sau khi trúng
        }
    }
}
