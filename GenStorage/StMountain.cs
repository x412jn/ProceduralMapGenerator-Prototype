using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StMountain
{
    public int mountCellIndex;
    public int mountCellExpanRate;
    public GameObject mountGobjectAttach;

    public StMountain(int cellIndex, int cellExpanRate, GameObject prefabAttach)
    {
        mountCellIndex = cellIndex;
        mountCellExpanRate = cellExpanRate;
        mountGobjectAttach = prefabAttach;
    }

}


