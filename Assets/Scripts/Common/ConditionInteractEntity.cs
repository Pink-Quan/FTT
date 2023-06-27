using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionInteractEntity : InteractableEntity
{
    public string conditionItemPlayerGrabing;
    protected override void Update()
    {
        if (string.Compare(conditionItemPlayerGrabing,player.curItem.itemName)==0)
            base.Update();
    }
}
