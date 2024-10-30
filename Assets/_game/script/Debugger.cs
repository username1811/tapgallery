using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    public static void DrawCircle(Vector3 center, float radius, Color color, float duration=9999f)
    {
        int segments = 90;
        float angleStep = 360f / segments;
        for (int i = 0; i < segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            float nextAngle = (i + 1) * angleStep * Mathf.Deg2Rad;

            // Tính toán vị trí của các điểm
            Vector3 point1 = center + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
            Vector3 point2 = center + new Vector3(Mathf.Cos(nextAngle) * radius, Mathf.Sin(nextAngle) * radius, 0);

            // Vẽ đường từ tâm đến viền để tạo thành đường tỏa
            Debug.DrawLine(center, point1, color, duration);
            // Vẽ đoạn thẳng giữa hai điểm liên tiếp
            Debug.DrawLine(point1, point2, color, duration);
        }
    }
}
