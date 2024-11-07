using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockUI : Singleton<BlockUI>
{
    public GameObject blockUIobj;

    public void Block()
    {
        blockUIobj.SetActive(true);
    }

    public void UnBlock()
    {
        blockUIobj.SetActive(false);
    }
}
