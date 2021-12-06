using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StCellOverallIndex
{
    //这个只用来作为terrain的生成器的list静态储存用

    /// <summary>
    /// To find certain tile in hexasphere.tile[]
    /// </summary>
    public int cellTerrainIndex;

    /// <summary>
    /// 0=sea|
    /// 1=land|
    /// 2=road|
    /// 3=forest|
    /// 4=mountain|
    /// 5=salted
    /// </summary>
    public int cellTerrainType;

    public int cellLandGroupId;

    public bool cellIsCellSeed;

    public bool cellIsSettlement;

    /// <summary>
    /// 1==RailRoad|
    /// 2==Road|
    /// 4==Maritime|
    /// 4==Land|
    /// 8==Forest|
    /// 8==Hill|
    /// 999==Mountain
    /// </summary>
    public int cellMovableCost;

    public GameObject cellObjectAttachment;

    /// <summary>
    /// 由tiles.length检索|
    /// cellType:
    /// 0=sea,cost4|
    /// 1=land,cost4|
    /// 2=road,cost2,|
    /// 3=forest,cost8|
    /// 4=mountain,cost999|
    /// 5=salted,cost8|
    /// [Group Id 默认为-1]
    /// </summary>
    /// <param name="cellIndex"></param>
    /// <param name="cellType"></param>
    public StCellOverallIndex(
        int cellIndex, 
        int cellType, 
        int cellLandId, 
        bool isCellSeed,
        bool isSettlement,
        int MoveCost,
        GameObject objectAttachment)
    {
        cellTerrainIndex = cellIndex;
        cellTerrainType = cellType;
        cellLandGroupId = cellLandId;
        cellIsCellSeed = isCellSeed;
        cellIsSettlement = isSettlement;
        cellMovableCost = MoveCost;
        cellObjectAttachment = objectAttachment;
    }

}


