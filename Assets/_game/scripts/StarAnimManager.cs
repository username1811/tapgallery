using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarAnimManager : Singleton<StarAnimManager>
{
    public Vector3[] points = new Vector3[] {};
    public Transform starWorldTF;
    public float duration;
    public RectTransform starRectTF;



    public void Follow()
    {
        StartCoroutine(IEFollow()); 
        IEnumerator IEFollow()
        {
            float timer = duration+1f;
            while (timer > 0f)
            {
                starRectTF.position = CameraManager.Ins.cam.WorldToScreenPoint(starWorldTF.position);
                timer -= Time.deltaTime;
                yield return null;  
            }
            yield return null;
        }
    }

    public void OnOpenHome()
    {
        return;
        starRectTF = UIManager.Ins.GetUI<Home>().starRectTF;
        starRectTF.gameObject.SetActive(false);
        DOVirtual.DelayedCall(0.5f, () =>
        {
            Move(() =>
            {
                UIManager.Ins.GetUI<Home>().ScaleButtonStar();
            });
        });
    }

    [Button]
    public void Move(Action OnComplete=null)
    {
        Follow();
        starRectTF.gameObject.SetActive(true);
        starWorldTF.position = Vector3.zero;
        starRectTF.rotation = Quaternion.identity;
        starRectTF.localScale = Vector3.one*4f;
        Vector3 targetMove = CameraManager.Ins.cam.ScreenToWorldPoint(UIManager.Ins.GetUI<Home>().buttonStarRectTF.position);

        starRectTF.localScale = Vector3.one * 0.1f;
        starRectTF.DOScale(4f, 0.5f).SetEase(Ease.OutBack);

        DOVirtual.DelayedCall(1f, () => {
            starWorldTF.DOLocalMoveX(targetMove.x, duration).SetEase(Ease.InSine).OnComplete(() =>
            {
                starRectTF.gameObject.SetActive(false);
                OnComplete?.Invoke();
            });
            starWorldTF.DOLocalMoveY(targetMove.y, duration).SetEase(Ease.InBack).OnComplete(() =>
            {
            });
            starRectTF.DOScale(1f, duration);
            starRectTF.DORotate(new Vector3(0, 0, -359f), duration, RotateMode.FastBeyond360).SetEase(Ease.InSine);
        });
        
    }
}
