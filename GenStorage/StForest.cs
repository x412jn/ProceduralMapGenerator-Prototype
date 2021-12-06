using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StForest
{
    public int forestCellIndex;
    public int forestCellExpanRate;
    public GameObject forestGobjectAttach;

    public StForest(int cellIndex, int cellExpanRate, GameObject prefabAttach)
    {
        forestCellIndex = cellIndex;
        forestCellExpanRate = cellExpanRate;
        forestGobjectAttach = prefabAttach;
    }

}

