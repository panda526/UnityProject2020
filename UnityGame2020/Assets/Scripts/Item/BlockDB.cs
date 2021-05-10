using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct BlockData
{
	public BlockBase block;
	public List<Vector3> posList;
}
[CreateAssetMenu(fileName = "BlockDB", menuName = "DB/BlockDB", order = 4)]
public class BlockDB : ScriptableObject
{//賦予給一道門
	public List<BlockData> blockData;
}
