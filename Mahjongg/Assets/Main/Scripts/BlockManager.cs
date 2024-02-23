using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System.Linq;
using System;

public class BlockManager : MonoBehaviour
{
    [SerializeField] Transform _blockPositionParent;
    [SerializeField] SpriteManager _spriteManager;
    [SerializeField] GameObject _youWonObject;

    // At the moment the code only works with GridSubdivisions == 2!!
    public static int GridSubdivisions => 2;
    public static float BlockHeight => 0.23f;
    public static float VerticalGridSize => 0.39f / GridSubdivisions;
    public static float HorizontalGridSize => 0.30f / GridSubdivisions;
    public List<BlockPosition> BlockPositions { get; private set; }

    public void Start()
    {
        BlockPositions = _blockPositionParent.GetComponentsInChildren<BlockPosition>().ToList();

        Assert.IsTrue(BlockPositions.Count % 2  == 0);

        List<(Sprite, Sprite)> sprites = _spriteManager.LoadImages();

        sprites = sprites.OrderBy(e => UnityEngine.Random.value).ToList();
        BlockPositions = BlockPositions.OrderBy(e => UnityEngine.Random.value).ToList();

        // Set IDs
        List<(Guid, Guid)> idsForPair = new();

        for (int i = 0; i < sprites.Count; i++)
        {
            Guid id0 = Guid.NewGuid();
            Guid id1 = Guid.NewGuid();

            idsForPair.Add((id0, id1));
        }

        for (int i = 0, j = 0; i < BlockPositions.Count; i += 2, j++)
        {
            BlockPositions[i].OwnGuid = idsForPair[j % sprites.Count].Item1;
            BlockPositions[i+1].OwnGuid = idsForPair[j % sprites.Count].Item2;

            BlockPositions[i].GoalGuid = idsForPair[j % sprites.Count].Item2;
            BlockPositions[i+1].GoalGuid = idsForPair[j % sprites.Count].Item1;

            BlockPositions[i].SetSprite(sprites[j % sprites.Count].Item1);
            BlockPositions[i+1].SetSprite(sprites[j % sprites.Count].Item2);

            BlockPositions[i].SetLocking(this);
            BlockPositions[i+1].SetLocking(this);
        }
    }

    public void Update()
    {
        if (BlockPositions.Count == 0)
        {
            _youWonObject.SetActive(true);
        }
    }

    public void RemoveBlockFromLocks(BlockPosition positionToRemove)
    {
        foreach (BlockPosition blockPosition in BlockPositions)
        {
            blockPosition.VerticalLocks.Remove(positionToRemove);
            blockPosition.HorizontalLocks.Remove(positionToRemove);
        }

        BlockPositions.Remove(positionToRemove);
    }
}
