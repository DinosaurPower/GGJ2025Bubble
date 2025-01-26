using UnityEngine;
using UnityEngine.UI;

public class LockChildPosition : MonoBehaviour
{
    [SerializeField] private RectTransform childToLock;

    private Vector3 lockedWorldPosition;

    private void Start()
    {
        // Record the child's initial position in world coordinates
        lockedWorldPosition = childToLock.position;
    }

    private void LateUpdate()
    {
        // Reapply the original world position so it doesn’t move with the parent
        childToLock.position = lockedWorldPosition;
    }
}
