using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelPassItem : MonoBehaviour
{
    public List<Image> ImageFrame;
    public Image ImageLevel;

    public void InitImage(LevelInfooo levelInfooo)
    {

        if (ImageFrame.Count > 0)
        {
            int randomIndex = Random.Range(0, ImageFrame.Count);
            ImageFrame[randomIndex].gameObject.SetActive(true);
        }
        ImageLevel.sprite = SpriteUtility.GetSpriteFromTexture2D(levelInfooo.texture2d);
    }
}
