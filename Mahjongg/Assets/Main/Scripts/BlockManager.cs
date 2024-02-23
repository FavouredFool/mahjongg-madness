using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    [SerializeField] Transform BlockPositionParent;

    // At the moment the code only works with GridSubdivisions == 2!!
    public static int GridSubdivisions => 2;
    public static float BlockHeight => 0.23f;
    public static float VerticalGridSize => 0.39f / GridSubdivisions;
    public static float HorizontalGridSize => 0.30f / GridSubdivisions;
    public BlockPosition[] BlockPositions { get; private set; }

    public void Start()
    {
        BlockPositions = BlockPositionParent.GetComponentsInChildren<BlockPosition>();

        foreach (BlockPosition blockPosition in BlockPositions)
        {
            blockPosition.SetLocking(this);
        }
    }

    public void RemoveBlockFromLocks(BlockPosition positionToRemove)
    {
        foreach (BlockPosition blockPosition in BlockPositions)
        {
            blockPosition.VerticalLocks.Remove(positionToRemove);
            blockPosition.HorizontalLocks.Remove(positionToRemove);
        }
    }
}
