using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashableEmpty : Slashable
{
    override public void TriggerEffect()
    {
        Debug.Log("slashed");
    }
}

