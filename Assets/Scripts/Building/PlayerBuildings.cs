using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuildings : Building
{
    public override void Start()
    {
        //base.Start();
        this.enabled = false;
    }
}
