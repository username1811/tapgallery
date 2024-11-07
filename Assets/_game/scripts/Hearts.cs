using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hearts : MonoBehaviour
{
    public RectTransform rectTransform;
    public List<Heart> hearts = new List<Heart>();
    public int remainHeart => hearts.Count(x => x.heartRedObj.gameObject.activeInHierarchy);
    public GameObject heartInfObj;


    public void OnLoadLevel(int amount)
    {
        InitHearts(amount);
        InitWidth();
    }

    public void InitHearts(int amount)
    {
        if(amount == 999)
        {
            heartInfObj.gameObject.SetActive(true);
            return;
        }
        heartInfObj.gameObject.SetActive(false);
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
        rectTransform.sizeDelta = new Vector2(Mathf.Max(63 * hearts.Count + 10 * (hearts.Count - 1), 360f), 0);
    }

    public void AddHeart(int amount)
    {
        if (remainHeart >= hearts.Count) return;
        int maxx = Mathf.Min(remainHeart + amount, hearts.Count);
        for (int i = remainHeart; i < maxx; i++)
        {
            Heart heart = hearts[i];
            heart.Show(true);
        }
    }

    public void SubtractHeart(int amount)
    {
        if (remainHeart <= 0) return;
        int minn = Mathf.Max(remainHeart - amount, 0);
        for (int i = remainHeart - 1; i >= minn; i--)
        {
            Heart heart = hearts[i];
            heart.Show(false);
        }
    }

}
