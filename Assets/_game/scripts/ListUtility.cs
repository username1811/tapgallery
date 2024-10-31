using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListUtility
{
    // Hàm lấy ra x phần tử ngẫu nhiên từ một list
    public static List<T> GetRandomElements<T>(List<T> list, int x)
    {
        // Đảm bảo rằng số phần tử cần lấy không vượt quá số lượng phần tử trong list
        x = Mathf.Min(x, list.Count);

        // Tạo một list để lưu các phần tử ngẫu nhiên
        List<T> randomElements = new List<T>();

        // Tạo một danh sách chứa các chỉ số có thể lấy ra
        List<int> indices = new List<int>();
        for (int i = 0; i < list.Count; i++)
        {
            indices.Add(i);
        }

        // Lấy x phần tử ngẫu nhiên
        for (int i = 0; i < x; i++)
        {
            // Lấy ngẫu nhiên chỉ số từ danh sách các chỉ số
            int randomIndex = Random.Range(0, indices.Count);

            // Thêm phần tử ngẫu nhiên vào list kết quả
            randomElements.Add(list[indices[randomIndex]]);

            // Xóa chỉ số đã lấy để tránh lặp lại
            indices.RemoveAt(randomIndex);
        }

        return randomElements;
    }


    public static void Shuffle<T>(this List<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
