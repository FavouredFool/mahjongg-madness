using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class BlockPosition : MonoBehaviour
{
    [Header("Child Dependencies")]
    [SerializeField] Image image;

    [Header("Configuration")]
    [SerializeField][Range(1, 10)] int _height = 1;
    [SerializeField][Range(-20, 20)] int _xPos = 0;
    [SerializeField][Range(-20, 20)] int _zPos = 0;

    List<BlockPosition> _verticalLocks = new();
    List<BlockPosition> _horizontalLocks = new();

    public int Height { get => _height; }
    public int XPos { get => _xPos; }
    public int ZPos { get => _zPos; }
    public List<BlockPosition> VerticalLocks => _verticalLocks;
    public List<BlockPosition> HorizontalLocks => _horizontalLocks;

    public void SetLocking(BlockManager blockManager)
    {
        SetLockingBlocks(blockManager);
        ValidateLockingBlocks();
    }

    public void OnValidate()
    {
        SetPosition();
    }

    public void SetSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }

    public bool IsLocked()
    {
        return _verticalLocks.Count > 0 || _horizontalLocks.Count > 1;
    }

    void SetLockingBlocks(BlockManager blockManager)
    {
        BlockPosition[] blockPositions = blockManager.BlockPositions;

        _verticalLocks.Clear();
        _horizontalLocks.Clear();

        foreach (BlockPosition referencePosition in blockPositions)
        {
            if (this == referencePosition)
            {
                continue;
            }

            Assert.IsTrue(!PositionIntersectsMe(referencePosition), $"Intersects {this}");

            // Vertical Locking
            if (PositionOnMyXZ(referencePosition) && referencePosition.Height > _height)
            {
                _verticalLocks.Add(referencePosition);
            }

            // Horizontal Locking
            if (referencePosition.Height == _height)
            {
                if (PositionNextToMeX(referencePosition))
                {
                    _horizontalLocks.Add(referencePosition);
                }
            }
        }
    }

    bool PositionIntersectsMe(BlockPosition otherPosition)
    {
        bool yIntersects = _height == otherPosition.Height;
        bool xzIntersects = PositionOnMyXZ(otherPosition);

        return yIntersects && xzIntersects;
    }

    bool PositionOnMyXZ(BlockPosition otherPosition)
    {
        bool xIntersects = Mathf.Abs(otherPosition.XPos - _xPos) < BlockManager.GridSubdivisions;
        bool zIntersects = Mathf.Abs(otherPosition.ZPos - _zPos) < BlockManager.GridSubdivisions;

        return xIntersects && zIntersects;
    }

    bool PositionNextToMeX(BlockPosition otherPosition)
    {
        bool xIntersects = Mathf.Abs(otherPosition.XPos - _xPos) < BlockManager.GridSubdivisions;
        bool zIntersects = Mathf.Abs(otherPosition.ZPos - _zPos) < BlockManager.GridSubdivisions;
        bool xNextTo = Mathf.Abs(otherPosition.XPos - _xPos) < BlockManager.GridSubdivisions + 1;

        return !xIntersects && zIntersects && xNextTo;
    }

    void ValidateLockingBlocks()
    {
        int sameHeightBlocks = 0;

        foreach (BlockPosition block in _verticalLocks)
        {
            Assert.IsTrue(block.Height >= _height);

            if (block.Height == _height)
            {
                sameHeightBlocks++;
            }
        }

        Assert.IsTrue(sameHeightBlocks <= 2);
    }

    void SetPosition()
    {
        Vector3 positionTemp = transform.position;
        positionTemp.x = _xPos * BlockManager.HorizontalGridSize;
        positionTemp.y = _height * BlockManager.BlockHeight;
        positionTemp.z = _zPos * BlockManager.VerticalGridSize;
        transform.position = positionTemp;

        gameObject.name = ToString();
    }

    public void DestroyBlockPosition()
    {
        Destroy(gameObject);
    }

    public override string ToString()
    {
        return $"[BlockPosition (Locked: {IsLocked()}, X: {_xPos}, Height: {_height}, Z: {_zPos})]";
    }
}
