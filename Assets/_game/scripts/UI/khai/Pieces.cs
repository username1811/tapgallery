using AssetKits.ParticleImage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pieces : MonoBehaviour
{

    public ParticleImage _fx;
    public Sprite Icon;
    public int Amount;

 
    public void PlayFx(SpinItem item)
    {
        _fx.texture = item.icon.texture;
        _fx.Play();
    }

}
