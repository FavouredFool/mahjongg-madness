using System.Collections;
using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] Camera _camera;
    [SerializeField] BlockManager _blockManager;
    [SerializeField] Color _highlightColor;

    BlockPosition TouchedBlock { get; set; } = null;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TouchBlock();
        }
    }

    public void TouchBlock()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            BlockPosition blockPosition = hit.transform.GetComponentInParent<BlockPosition>();

            if (blockPosition == null )
            {
                Debug.LogWarning("Didn't touch a block");
                return;
            }

            if (blockPosition.IsLocked())
            {
                SetTouchedBlock(null);
                return;
            }

            if (TouchedBlock == null)
            {
                SetTouchedBlock(blockPosition);
                return;
            }

            if (!blockPosition.IsMatching(TouchedBlock))
            {
                SetTouchedBlock(blockPosition);
                return;
            }


            _blockManager.RemoveBlockFromLocks(TouchedBlock);
            TouchedBlock.DestroyBlockPosition();

            _blockManager.RemoveBlockFromLocks(blockPosition);
            blockPosition.DestroyBlockPosition();

            SetTouchedBlock(null);
        }
    }

    public void SetTouchedBlock(BlockPosition blockPosition)
    {
        if (TouchedBlock != null)
        {
            TouchedBlock.ResetColor();
        }
        
        TouchedBlock = blockPosition;

        if (TouchedBlock != null)
        {
            TouchedBlock.SetTouchedColor(_highlightColor);
        }
    }
}
