using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynaCellSelected
{
    public int cellIndex;
    public float cellHeightBefore;
    //public Material cellMaterialBefore;
    public GameObject cellObjectAttachment;

    public GameObject cellObjectPawn;
    public List<GameObject> cellObjectMerchant;

    public DynaCellSelected(
        int index,
        float heightBefore,
        GameObject objectAttachment,
        GameObject objectPawn,
        List<GameObject> objectMerchant)
    {
        cellIndex = index;
        cellHeightBefore = heightBefore;
        //cellMaterialBefore = materialBefore;
        cellObjectAttachment = objectAttachment;
        cellObjectPawn = objectPawn;
        cellObjectMerchant = objectMerchant;
    }
}
