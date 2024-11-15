using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalleryItem : UICanvas
{

    public Image img;
    public Vector2 oldImgSize;
    public override void Open()
    {
        base.Open();
       
    }
    public override void Awake()
    {
        base.Awake();
        oldImgSize = img.rectTransform.sizeDelta;    
    }

    public void SetImage(Sprite sprite)
    {
        img.rectTransform.sizeDelta = oldImgSize;
        img.sprite = sprite;
        if (sprite.rect.width > sprite.rect.height)
        {
            img.ResizeImgKeepWidth();
        }
        else
        {
            img.ResizeImgKeepHeight();
        }

    }


    public void BackBtn()
    {
        UIManager.Ins.CloseUI<GalleryItem>();
    }





}
