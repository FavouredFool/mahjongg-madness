using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] Camera _camera;
    [SerializeField] BlockManager _blockManager;

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

            Debug.Log(blockPosition);

            if (blockPosition.IsLocked())
            {
                return;
            }

            _blockManager.RemoveBlockFromLocks(blockPosition);
            blockPosition.DestroyBlockPosition();
        }
    }
}
