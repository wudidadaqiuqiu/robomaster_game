
using UnityEngine;

namespace RoboticItems {
    public class SpeedCaculator  : MonoBehaviour {
        public Vector3 smoothedVelocity;
        private Vector3 lastPosition;
        public float smoothingFactor = 0.1f;
        void Update() {
            // 计算瞬时速度
            Vector3 currentVelocity = (transform.position - lastPosition) / Time.deltaTime;

            // 使用平滑算法
            smoothedVelocity = Vector3.Lerp(smoothedVelocity, currentVelocity, smoothingFactor);

            // 更新上一帧的位置
            lastPosition = transform.position;
        }
    }
}