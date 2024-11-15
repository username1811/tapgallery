using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelPassItem : UICanvas
{
    public List<Image> ImageFrame;
    public Image ImageLevel;

    public void InitImage(LevelInfooo levelInfooo)
    {

        /*if (ImageFrame.Count > 0)
        {
            int randomIndex = Random.Range(0, ImageFrame.Count);
            ImageFrame[randomIndex].gameObject.SetActive(true);
        }*/
        ImageFrame[1].gameObject.SetActive(true);
        ImageLevel.sprite = SpriteUtility.GetSpriteFromTexture2D(levelInfooo.texture2d);
        if(levelInfooo.texture2d.width > levelInfooo.texture2d.height)
        {
            ImageLevel.ResizeImgKeepWidth();
        }
        else
        {
            ImageLevel.ResizeImgKeepHeight();
        }
    }

    public void ShowImageLevel()
    {
       
        var galleryItem = UIManager.Ins.OpenUI<GalleryItem>();
        galleryItem.SetImage(ImageLevel.sprite); 
    }


}
