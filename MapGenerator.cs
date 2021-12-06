using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexasphereGrid;

public class MapGenerator : MonoBehaviour
{

    Hexasphere hexa;
    public GameCore gameCore;

    [Header("接驳区")]
    /// <summary>
    /// 0=sea|
    /// 1=land
    /// </summary>
    [SerializeField] public Material[] tileMaterials;
    /// <summary>
    /// 0=sea|
    /// 1=land
    /// </summary>
    [SerializeField] public Material[] tileMaterialsSelected;
    /// <summary>
    /// 0=sea|
    /// 1=land
    /// </summary>
    [SerializeField] public Texture2D[] tileTextures;
    /// <summary>
    /// 0=sea|
    /// 1=land
    /// </summary>
    [SerializeField] public Texture2D[] tileTexturesIsSelected;
    [SerializeField] public GameObject[] mountainPrefabs;
    [SerializeField] public GameObject[] forestPrefabs;
    /// <summary>
    /// 0=castle|
    /// 1=city|
    /// 2=mill|
    /// </summary>
    [SerializeField] public GameObject[] buildingPrefabs;
    [SerializeField] public GameObject[] spawnPrefabs;

    [Header("预设数值")]
    public int genExpanMaxRate;
    public int genExpanMaxRootCells;
    public int genExpanDeviationRate;

    [Space]
    [Header("父子级定位")]
    public Transform parentPlayer;
    public Transform parentMountain;
    public Transform parentForest;
    public Transform parentSettlement;

    [Space]
    public List<StCellOverallIndex> listCellOverallIndex;
    public List<StCellSeed> listCellSeed;
    public List<StMountain> listMountain;
    public List<StForest> listForest;
    public List<StStateSettlement> listStateSettlement;
    public List<DynaCellTerrain> listDynaCellTerrain;

    //unit 结构策划
    //需要写一个虚基类来储存不同类型的单位的代码
    //需要写一个哈希表或数组来储存虚基类
    //通过tile index来检索哈希表或数组，如果绑定的虚基类不是null就说明这地方有东西
    
    public List<StUnitOverall> listUnitOverall;

    //TEMP VAR

    GameObject tempForInstantiateAttachment;

    //TEMP VAR



    //int maxTiles = hexa.

    // Start is called before the first frame update
    void Start()
    {
        hexa = gameCore.hexa;
        listCellOverallIndex = new List<StCellOverallIndex>();
        listCellSeed = new List<StCellSeed>();
        listMountain = new List<StMountain>();
        listForest = new List<StForest>();
        listStateSettlement = new List<StStateSettlement>();
        listDynaCellTerrain = new List<DynaCellTerrain>();

        listUnitOverall = new List<StUnitOverall>();

        WorldShaper();
        hexa.smartEdges = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void WorldShaper()
    {
        PlateGenerator();
        SeedGenerator();
        ContinentGenerator();
    }

    void PlateGenerator()
    {
        int maxTiles = hexa.tiles.Length;
        for (int i = 0; i < maxTiles; i++)
        {
            //hexa.SetTileMaterial(i, tileMaterials[0], false);
            hexa.SetTileTexture(i, tileTextures[0], false);
            hexa.SetTileTextureRotationToNorth(i);
            hexa.SetTileTag(i, "SeaTile");
            hexa.tiles[i].isWater = true;
            hexa.SetTileCanCross(i, false);
            listCellOverallIndex.Add(new StCellOverallIndex(i, 0, -1, false, false, 4, null));
            hexa.SetTileCrossCost(i, 4f);
            listUnitOverall.Add(new StUnitOverall(null, -1, null, null));
        }
    }

    void SeedGenerator()
    {
        int _genMaxExpansionRate = genExpanMaxRate;
        int _genMaxRootCells = genExpanMaxRootCells;
        int currentGroupIndex = 0;

        int _assignExpanRate;

        while (_genMaxExpansionRate > 0)
        {
            //随机分配一部分数值出来
            //Assign expansion rate through random value

            //如果当前的总增长数小于Deviation值，就已总增长数作为随机数天花板
            //if current expantion rate lower than deviation rate
            //choose current expansion rate as maximum number of deviation
            if (_genMaxExpansionRate < genExpanDeviationRate)
            {
                _assignExpanRate = Random.Range(1, _genMaxExpansionRate + 1);
            }
            //反之，就已deviation值作为随机数天花板
            //else, choose deviation rate as masimum random number
            else
            {
                _assignExpanRate = Random.Range(1, genExpanDeviationRate + 1);
            }

            //---------------------------
            //确认了当前轮的分离增长点数后
            //先从总的增长点数里减去当前的分离指数
            //remove assignment value from current expansion rate
            _genMaxExpansionRate -= _assignExpanRate;

            //种子上限
            //maximum root cells
            if (_genMaxRootCells > 0)
            {
                int _assignCellIndex = Random.Range(0, hexa.tiles.Length);
                bool _isAssigning = true;

                while (_isAssigning)
                {
                    _assignCellIndex = Random.Range(0, hexa.tiles.Length);
                    int[] _assignCellNeighbourCheck = hexa.GetTileNeighbours(_assignCellIndex);
                    if (hexa.tiles[_assignCellIndex].isWater == true)
                    {
                        int _check = 0;
                        for (int i = 0; i < _assignCellNeighbourCheck.Length; i++)
                        {
                            if (hexa.tiles[_assignCellNeighbourCheck[i]].isWater)
                            {
                                _check++;
                            }
                        }
                        if (_check >= _assignCellNeighbourCheck.Length)
                        {
                            _isAssigning = false;
                        }
                    }
                }

                listCellSeed.Add(new StCellSeed(_assignCellIndex, currentGroupIndex));
                listCellOverallIndex[_assignCellIndex].cellTerrainType = 1;
                listCellOverallIndex[_assignCellIndex].cellLandGroupId = currentGroupIndex;
                listCellOverallIndex[_assignCellIndex].cellIsCellSeed = true;
                listCellOverallIndex[_assignCellIndex].cellMovableCost = 4;
                listDynaCellTerrain.Add(new DynaCellTerrain(currentGroupIndex, _assignCellIndex, _assignExpanRate));

                hexa.SetTileCanCross(_assignCellIndex, true);
                hexa.SetTileCrossCost(_assignCellIndex, 4f);
                //hexa.SetTileMaterial(_assignCellIndex, tileMaterials[1], false);
                hexa.SetTileTexture(_assignCellIndex, tileTextures[1], false);
                hexa.SetTileTextureRotationToNorth(_assignCellIndex);
                hexa.SetTileTag(_assignCellIndex, "LandTile");
                hexa.tiles[_assignCellIndex].isWater = false;
                hexa.SetTileExtrudeAmount(_assignCellIndex, 0.5f);

                currentGroupIndex++;
                _genMaxRootCells--;

            }
            else
            {
                int _assignRegrouping = Random.Range(0, listDynaCellTerrain.Count);
                int _assignCellIndex = listDynaCellTerrain[_assignRegrouping].groupCellIndex;

                listDynaCellTerrain[_assignRegrouping].groupCell_GrowthRate += _assignExpanRate;

            }
        }

    }

    void ContinentGenerator()
    {
        int _genMaxExpansionRate = genExpanMaxRate;
        while (_genMaxExpansionRate > 0)
        {
            int _tempLength = listDynaCellTerrain.Count;
            if (_tempLength <= 0)
            {
                for (int i = 0; i < hexa.tiles.Length; i++)
                {
                    //如果选中的区块是陆地，它的邻居也存在水
                    if (listCellOverallIndex[i].cellTerrainType == 1
                        && FuncNeighbourCheck(i, 0, -1) == true)
                    {
                        int _tempTransferGroupId = listCellOverallIndex[i].cellLandGroupId;
                        listDynaCellTerrain.Add(new DynaCellTerrain(_tempTransferGroupId, i, _genMaxExpansionRate));
                        i = hexa.tiles.Length;
                    }
                }
            }
            else
            {
                int _tempRnd = Random.Range(0, listDynaCellTerrain.Count);
                int _tempSelectedIndex = listDynaCellTerrain[_tempRnd].groupCellIndex;
                int _tempSelectedGroup = listDynaCellTerrain[_tempRnd].groupId;
                int _tempSelectedRate = listDynaCellTerrain[_tempRnd].groupCell_GrowthRate;
                //如果方向没有问题
                //临时变动
                //不用改了，看样子效果还行，但如果要用道路联通各个区块的话，还是要检索并变更山脉为陆地才行
                //不然极端的情况也可能会出现
                //备份：
                //方案1：少山脉
                //FuncNeighbourCheck(_tempSelectedIndex, 0, -1)
                //方案2：多山脉
                //FuncNeighbourCheck(_tempSelectedIndex, 1, _tempSelectedGroup)
                if (FuncNeighbourCheck(_tempSelectedIndex, 0, -1))
                {
                    //查看是否depleted或是否还有剩余增长点数,或者它是否depleted以及Rate本身是否大于零
                    if (_tempSelectedRate > 0)
                    {
                        //选择一个随机方向，
                        int _tempTargetIndex = FuncNeighbourSelect(_tempSelectedIndex, 1, _tempSelectedGroup);
                        //首先要确认是不是-1
                        if (_tempTargetIndex != -1)
                        {
                            //如果那个方向是海，
                            if (hexa.tiles[_tempTargetIndex].isWater)
                            {
                                //那就变成陆地，分随机的一段扩张值过去，
                                int _tempAssignRate = Random.Range(1, _tempSelectedRate + 1);

                                listCellOverallIndex[_tempTargetIndex].cellTerrainType = 1;
                                listCellOverallIndex[_tempTargetIndex].cellLandGroupId = _tempSelectedGroup;
                                listCellOverallIndex[_tempTargetIndex].cellMovableCost = 4;
                                hexa.SetTileCrossCost(_tempTargetIndex, 4f);

                                listDynaCellTerrain.Add(new DynaCellTerrain(_tempSelectedGroup, _tempTargetIndex, _tempAssignRate));

                                //修改地貌
                                hexa.SetTileCanCross(_tempTargetIndex, true);
                                //hexa.SetTileMaterial(_tempTargetIndex, tileMaterials[1], false);
                                hexa.SetTileTexture(_tempTargetIndex, tileTextures[1], false);
                                hexa.SetTileTag(_tempTargetIndex, "LandTile");
                                hexa.tiles[_tempTargetIndex].isWater = false;
                                hexa.SetTileExtrudeAmount(_tempTargetIndex, 0.5f);

                                //本扩张值--
                                listDynaCellTerrain[_tempRnd].groupCell_GrowthRate -= _tempAssignRate;

                                //总值--
                                _genMaxExpansionRate--;

                                //查看本cell扩张值是否耗尽，如果是，就删掉list
                                if (listDynaCellTerrain[_tempRnd].groupCell_GrowthRate <= 0)
                                {
                                    listDynaCellTerrain.RemoveAt(_tempRnd);
                                }
                            }
                            //如果那个方向是陆地，但属于不同扩张组的，同时扩张源不是种子
                            else if (listCellOverallIndex[_tempTargetIndex].cellTerrainType == 1
                                && listCellOverallIndex[_tempTargetIndex].cellLandGroupId != _tempSelectedGroup
                                && listCellOverallIndex[_tempSelectedIndex].cellIsCellSeed != true)
                            {
                                //那就把本cell变成山，从list中移除，

                                //Debug.Log("Cell Index:" + _tempTargetIndex + "是山");

                                listCellOverallIndex[_tempSelectedIndex].cellTerrainType = 4;
                                listCellOverallIndex[_tempSelectedIndex].cellLandGroupId = -1;
                                listCellOverallIndex[_tempSelectedIndex].cellMovableCost = 999;
                                hexa.SetTileCrossCost(_tempSelectedIndex, 999f);
                                //改变地貌
                                hexa.SetTileCanCross(_tempSelectedIndex, false);
                                int _temp_tempRnd = Random.Range(0, mountainPrefabs.Length);
                                FuncSpawnPrefab(mountainPrefabs[_temp_tempRnd], _tempSelectedIndex,1.4f);
                                listCellOverallIndex[_tempSelectedIndex].cellObjectAttachment = tempForInstantiateAttachment;
                                tempForInstantiateAttachment.transform.SetParent(parentMountain);
                                hexa.SetTileTag(_tempSelectedIndex, "MountainTile");
                                hexa.tiles[_tempSelectedIndex].isWater = false;
                                listMountain.Add(new StMountain(_tempSelectedIndex, 0, tempForInstantiateAttachment));                                tempForInstantiateAttachment = null;
                                //REMOVE KEBAB
                                listDynaCellTerrain.RemoveAt(_tempRnd);

                                //剩余的rate扔给list里的其他组
                                //查看当前还有没有剩余区块

                                //再一次查看当前list还有多少剩余元素
                                _tempLength = listDynaCellTerrain.Count;
                                //如果大于零
                                if (_tempLength > 0)
                                {
                                    _tempRnd = Random.Range(0, _tempLength);
                                    //把tempSelectedRate扔过去
                                    listDynaCellTerrain[_tempRnd].groupCell_GrowthRate += _tempSelectedRate;
                                }
                                //反之则遍历全图找到合适的区块，把tempSelectedRate扔给它
                                else
                                {
                                    for (int i = 0; i < hexa.tiles.Length; i++)
                                    {
                                        if (listCellOverallIndex[i].cellTerrainType == 1
                                            && FuncNeighbourCheck(i, 0, -1))
                                        {
                                            int _tempTransferGroupId = listCellOverallIndex[i].cellLandGroupId;
                                            listDynaCellTerrain.Add(new DynaCellTerrain(_tempTransferGroupId, i, _tempSelectedRate));

                                            //终止循环
                                            i = hexa.tiles.Length;
                                        }
                                    }
                                }
                            }
                            else
                            {

                            }
                        }
                        else
                        {

                        }
                    }
                    //如果任何一项不是，那就把这个list删了，这轮循环结束
                    else
                    {
                        listDynaCellTerrain.RemoveAt(_tempRnd);
                    }
                }
                //如果方向都有问题，那就清空本区块的增长点数，扔掉本list，把增长点数扔到其他地方
                else
                {
                    listDynaCellTerrain.RemoveAt(_tempRnd);
                    //再一次查看当前list还有多少剩余元素
                    _tempLength = listDynaCellTerrain.Count;
                    //如果大于零
                    if (_tempLength > 0)
                    {
                        _tempRnd = Random.Range(0, _tempLength);
                        //把tempSelectedRate扔过去
                        listDynaCellTerrain[_tempRnd].groupCell_GrowthRate += _tempSelectedRate;
                    }
                    //反之则遍历全图找到合适的区块，把tempSelectedRate扔给它
                    else
                    {
                        for (int i = 0; i < hexa.tiles.Length; i++)
                        {
                            if (listCellOverallIndex[i].cellTerrainType == 1
                                && FuncNeighbourCheck(i, 0, -1) == true)
                            {
                                int _tempTransferGroupId = listCellOverallIndex[i].cellLandGroupId;
                                listDynaCellTerrain.Add(new DynaCellTerrain(_tempTransferGroupId, i, _tempSelectedRate));
                                //终止循环
                                i = hexa.tiles.Length;
                            }
                        }
                    }
                }
            }
        }
    }

    //ARMOURY!

    /// <summary>
    /// type:
    /// 0=陆地扩张（不撞山）
    /// 1=板块扩张（撞山）|
    /// 如果true，说明这个区块可用，反正则说明输入语法有问题或区块不可用|
    /// Type=0时，groupID打-1即可
    /// </summary>
    /// <param name="inquireIndex"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    bool FuncNeighbourCheck(int inquireIndex, int type, int groupId)
    {
        int[] _tempCheckNeighbour = hexa.GetTileNeighbours(inquireIndex);
        int _tempCheckIndex;
        //int _tempCheckCount = 0;
        for (int i = 0; i < _tempCheckNeighbour.Length; i++)
        {
            _tempCheckIndex = _tempCheckNeighbour[i];
            switch (type)
            {
                case 0:
                    //如果隔壁区块不是水，那么
                    if (hexa.tiles[_tempCheckIndex].isWater == true)
                    {
                        return true;
                    }
                    break;

                case 1:
                    if (groupId < 0)
                    {
                        return false;
                    }
                    //如果隔壁区块不是水，那么
                    if (hexa.tiles[_tempCheckIndex].isWater == true)
                    {
                        return true;
                    }
                    //检查是陆地的情况，同时检查扩张源和目标扩张组是否相同，还要检查扩张源是不是种子
                    else if (listCellOverallIndex[_tempCheckIndex].cellTerrainType == 1
                        && listCellOverallIndex[_tempCheckIndex].cellLandGroupId != groupId
                        && listCellOverallIndex[inquireIndex].cellIsCellSeed != true)
                    {
                        return true;
                    }
                    break;

                default:
                    return false;
            }
        }
        return false;
    }

    /// <summary>
    /// type:
    /// 0=陆地扩张（不撞山）
    /// 1=板块扩张（撞山）|
    /// 如果true，说明这个区块可用，反正则说明输入语法有问题或区块不可用|
    /// GroupId是扩张源的扩张组id：
    /// Type==0时只需要输入-1即可
    /// </summary>
    /// <param name="inquireIndex"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    int FuncNeighbourSelect(int inquireIndex, int type, int groupId)
    {
        int[] _tempCheckNeighbour = hexa.GetTileNeighbours(inquireIndex);
        int _tempCheckIndex;
        for (int i = 0; i < _tempCheckNeighbour.Length; i++)
        {
            _tempCheckIndex = _tempCheckNeighbour[i];
            switch (type)
            {
                case 0:
                    //如果隔壁区块不是水，那么返回该区块的index
                    if (hexa.tiles[_tempCheckIndex].isWater == true)
                    {
                        return _tempCheckIndex;
                    }
                    break;

                case 1:
                    if (groupId < 0)
                    {
                        return -1;
                    }
                    //如果隔壁区块不是水，那么返回该区块的index
                    if (hexa.tiles[_tempCheckIndex].isWater == true)
                    {
                        return _tempCheckIndex;
                    }
                    //或者区块是陆地，且groupID不同，同时扩张源不是种子
                    else if (listCellOverallIndex[_tempCheckIndex].cellTerrainType == 1
                        && listCellOverallIndex[_tempCheckIndex].cellLandGroupId != groupId
                        && listCellOverallIndex[inquireIndex].cellIsCellSeed != true)
                    {
                        return _tempCheckIndex;
                    }
                    break;

                default:
                    return -1;
            }
        }
        return -1;
    }


    void FuncSpawnPrefab(GameObject spawnPrefab, int tileIndex, float adjustScale)
    {
        // To apply a proper scale, get as a reference the length of a diagonal in tile 0 (note the "false" argument which specifies the position is in local coordinates)
        float size = Vector3.Distance(hexa.GetTileVertexPosition(0, 0, false), hexa.GetTileVertexPosition(0, 3, false));
        Vector3 scale = new Vector3(size, size, size);

        // Make it 50% smaller so it does not occupy entire tile
        scale *= adjustScale;

        //Spawn Object
        GameObject obj = Instantiate<GameObject>(spawnPrefab);
        tempForInstantiateAttachment = obj;
        // Move object to center of tile (GetTileCenter also takes into account extrusion)
        obj.transform.position = hexa.GetTileCenter(tileIndex);

        // Parent it to hexasphere, so it rotates along it
        obj.transform.SetParent(hexa.transform);

        // Align with surface
        obj.transform.LookAt(hexa.transform.position);
        obj.transform.Rotate(-90, 0, 0);

        // Set scale
        obj.transform.localScale = scale;
        
    }

}

