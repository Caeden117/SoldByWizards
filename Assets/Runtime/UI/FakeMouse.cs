using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SoldByWizards
{
    public class FakeMouse : MonoBehaviour
    {
        [SerializeField, CanBeNull] private Camera _camera;
        [SerializeField, CanBeNull] private RectTransform _mouseVisuals;
        [SerializeField, CanBeNull] private RectTransform _canvasRectTransform;
        [SerializeField] private Vector2 _canvasRectSize = new Vector2(1920, 1080);

        public void Update()
        {
            if (_camera == null || _mouseVisuals == null)
                return;

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRectTransform, Mouse.current.position.ReadValue(), _camera, out Vector2 localPoint))
                return;
            localPoint.x = Mathf.Clamp(localPoint.x, -_canvasRectSize.x / 2f, _canvasRectSize.x / 2f);
            localPoint.y = Mathf.Clamp(localPoint.y, -_canvasRectSize.y / 2f, _canvasRectSize.y / 2f);
            _mouseVisuals.anchoredPosition = localPoint;
        }
    }
}
