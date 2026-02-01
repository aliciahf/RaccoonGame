using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class CombinableItem : PickupItems
{
    public List<PickupItems> combinors;
    public List<CombinableItem> branches;

    //private void Start()
    //{
    //    foreach (PickupItems pickup in combinors)
    //    {
    //        this.Value += pickup.Value;
    //    }
    //}

    public override void OnMouseDown()
    {
        base.OnMouseDown();
        foreach (PickupItems pickup in combinors)
        {
            if(pickup != null) pickup.gameObject.SetActive(false);
        }
        foreach (CombinableItem branch in branches)
        {
            if (branch != null) branch.gameObject.SetActive(false);
        }
    }

    public override void Reset()
    {
        foreach (PickupItems pickup in combinors)
        {
            if (pickup != null && pickup is not CombinableItem) pickup.gameObject.SetActive(true);
        }
        this.gameObject.SetActive(false);
    }

}
