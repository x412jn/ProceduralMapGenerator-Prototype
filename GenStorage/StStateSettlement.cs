using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StStateSettlement
{
    //GroupId
    public int settlementCellIndex;
    public int stateIndex;
    public int stateSettlementIndex;
    public string stateSettlementName;
    /// <summary>
    /// 0==castle|
    /// 1==city|
    /// 2==mill
    /// </summary>
    public int stateSettlementType;
    public GameObject settlementGobjectAttach;

    /// <summary>
    /// type:
    /// 0==castle|
    /// 1==city|
    /// 2==mill
    /// </summary>
    /// <param name="stateId"></param>
    /// <param name="settlementId"></param>
    /// <param name="settlementName"></param>
    /// <param name="type"></param>
    /// <param name="prefabAttach"></param>
    public StStateSettlement(int cellId,int stateId,int settlementId,string settlementName,int type,GameObject prefabAttach)
    {
        settlementCellIndex = cellId;
        stateIndex = stateId;
        stateSettlementIndex = settlementId;
        stateSettlementName = settlementName;
        stateSettlementType = type;
        settlementGobjectAttach = prefabAttach;
    }
}
