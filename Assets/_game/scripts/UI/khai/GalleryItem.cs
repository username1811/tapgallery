using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalleryItem : UICanvas
{

    public Image images;
    public override void Open()
    {
        base.Open();
       
    }

    public void SetImage(Sprite sprite)
    {
        images.sprite = sprite;
        ImageUtility.ResizeImgKeepHeight(images);
        ImageUtility.ResizeImgKeepWidth(images);
       
    }


    public void BackBtn()
    {
      
        UIManager.Ins.OpenUI<Gallery>();
     
        Close(0);
    }





}
