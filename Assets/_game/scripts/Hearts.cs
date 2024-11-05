using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hearts : MonoBehaviour
{
    public RectTransform rectTransform;
    public List<Heart> hearts = new List<Heart>();
    public int remainHeart => hearts.Count(x => x.heartRedObj.gameObject.activeInHierarchy);


    public void OnLoadLevel(int amount)
    {
        InitHearts(amount);
        InitWidth();
    }

    public void InitHearts(int amount)
    {
        hearts.Clear();
        for (int i = 0; i < amount; i++)
        {
            Heart heart = PoolManager.Ins.Spawn<Heart>(PoolType.Heart);
            heart.transform.SetParent(this.transform);
            heart.transform.localScale = Vector3.one;
            heart.Show(true);
            hearts.Add(heart);
        }
    }

    public void InitWidth()
    {
        rectTransform.sizeDelta = new Vector2(63*hearts.Count + 10*(hearts.Count-1), 0);
    }

    public void AddHeart(int amount)
    {
        if (remainHeart >= hearts.Count) return;
        for (int i = remainHeart; i < Mathf.Min(remainHeart + amount, hearts.Count); i++)
        {
            Heart heart = hearts[i];
            heart.Show(true);
        }
    }

    public void SubtractHeart(int amount)
    {
        if (remainHeart <= 0) return;
        for (int i = remainHeart - 1; i >= Mathf.Max(remainHeart - amount, 0); i++)
        {
            Heart heart = hearts[i];
            heart.Show(false);
        }
    }

}
