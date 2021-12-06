using System.Collections;
using System.Collections.Generic;


public class DynaCellTerrain
{
    public int groupId;
    public int groupCellIndex;
    public int groupCell_GrowthRate;

    public DynaCellTerrain(int _groupId, int _groupCellIndex, int _groupCellGrowthRate)
    {
        groupId = _groupId;
        groupCellIndex = _groupCellIndex;
        groupCell_GrowthRate = _groupCellGrowthRate;
    }

}

