using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StUnitOverall
{
    public GameObject cellObjectTroop;

    /// <summary>
    /// 0=neutral|
    /// 1=reble|
    /// 2=player|
    /// so on
    /// </summary>
    public int cellFactionTroop;

    public List<GameObject> cellObjectMerchant;
    //TODO
    //public int MerchantType;

    /// <summary>
    /// 0=neutral|
    /// 1=reble|
    /// 2=player|
    /// so on
    /// </summary>
    public List<int> cellFactionMerchant;
    
    /// <summary>
    /// universal unit index
    /// </summary>
    /// <param name="objectTroop"></param>
    /// <param name="troopFaction">-1=null|0=neutral|1=reble|2=player|</param>
    /// <param name="objectMerchant"></param>
    /// <param name="merchantFaction">-1=null|0=neutral|1=reble|2=player|</param>
    public StUnitOverall(
        GameObject objectTroop,
        int troopFaction, 
        List<GameObject> objectMerchant,
        List<int> merchantFaction)
    {
        cellObjectTroop = objectTroop;
        cellFactionTroop = troopFaction;
        cellObjectMerchant = objectMerchant;
        cellFactionMerchant = merchantFaction;
    }

}
