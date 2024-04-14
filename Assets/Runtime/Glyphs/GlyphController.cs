using JetBrains.Annotations;
using System;
using SoldByWizards.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SoldByWizards.Glyphs
{
    public class GlyphController : MonoBehaviour, WizardInput.IGlyphsActions
    {
        [SerializeField] private InputController _inputController;
        [SerializeField] private Camera _raycastCamera;
        [Space, SerializeField] private LineRenderer _glyphLineRenderer;
        [SerializeField] private GlyphPoint[] _points;
        [SerializeField] private LayerMask _glyphLayerMask;
        [SerializeField] private LayerMask _glyphPointLayerMask;

        private int[] _selectedPoints = new int[4];
        private int _numPoints = 0;

        private bool _isDrawing;
        private Vector3 _currentGlyphPosition;

        [PublicAPI]
        public bool IsValidGlyph => _numPoints is >= 2 and <= 4;

        [PublicAPI]
        public int GetGlyphHash()
        {
            if (!IsValidGlyph) return 0;

            Span<int> hashSpan = stackalloc int[_numPoints];
            _selectedPoints[.._numPoints].CopyTo(hashSpan);

            var isReversed = false;

            // Find an edge by going in a consistent manner
            for (var i = 0; i < _points.Length; i++)
            {
                var pointIdx = Array.IndexOf(_selectedPoints, _points[i].GlyphID);

                if (pointIdx == 0) break;
                if (pointIdx != _numPoints - 1) continue;

                isReversed = true;
                break;
            }

            if (isReversed)
            {
                hashSpan.Reverse();
            }

            var hash = 0;

            for (var i = 0; i < _numPoints; i++)
            {
                hash |= hashSpan[i] << (i * 2);
            }

            return hash;
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                var middlePoint = 0.5f * new Vector2(_raycastCamera.scaledPixelWidth, _raycastCamera.scaledPixelHeight);
                var ray = _raycastCamera.ScreenPointToRay(middlePoint);

                var raycastHit = Physics.Raycast(ray, out var glyphHit, float.MaxValue, _glyphLayerMask);

                if (!raycastHit || glyphHit.transform.GetComponentInParent<GlyphController>() != this) return;

                _numPoints = 0;
                _isDrawing = true;
            }
            else if (context.canceled)
            {
                CancelDraw();
            }
        }

        public void Clear()
        {
            _numPoints = 0;
            CancelDraw();
        }

        private void Start() => _inputController.Input.Glyphs.AddCallbacks(this);

        private void Update()
        {
            if (!_isDrawing) return;

            var middlePoint = 0.5f * new Vector2(_raycastCamera.scaledPixelWidth, _raycastCamera.scaledPixelHeight);
            var ray = _raycastCamera.ScreenPointToRay(middlePoint);

            var raycastHit = Physics.Raycast(ray, out var glyphHit, float.MaxValue, _glyphLayerMask);

            if (!raycastHit || glyphHit.transform.GetComponentInParent<GlyphController>() != this)
            {
                CancelDraw();
                return;
            }

            _currentGlyphPosition = _glyphLineRenderer.transform.InverseTransformPoint(glyphHit.point).WithZ(0);

            ApplyPointsToLineRenderer();

            var snapPointHit = Physics.Raycast(ray, out var glyphSnapHit, float.MaxValue, _glyphPointLayerMask);

            if (!snapPointHit) return;

            var pointIdx = glyphSnapHit.transform.GetComponent<GlyphPoint>().GlyphID;

            if (GlyphContainsPoint(pointIdx)) return;

            _selectedPoints[_numPoints] = pointIdx;
            _numPoints++;

            ApplyPointsToLineRenderer();
        }

        private bool GlyphContainsPoint(int pointIdx)
        {
            for (var i = 0; i < Mathf.Min(_numPoints, _selectedPoints.Length); i++)
            {
                if (_selectedPoints[i] == pointIdx) return true;
            }

            return false;
        }

        private void CancelDraw()
        {
            _isDrawing = false;
            if (!IsValidGlyph) _numPoints = 0;
            ApplyPointsToLineRenderer();
        }

        private void ApplyPointsToLineRenderer()
        {
            var points = new Vector3 [_numPoints + (_isDrawing ? 2 : 0)];

            for (var i = 0; i < _numPoints; i++)
            {
                points[i] = _glyphLineRenderer.transform
                    .InverseTransformPoint(_points[_selectedPoints[i]].transform.position).WithZ(0);
            }

            if (_isDrawing)
            {
                points[^2] = _currentGlyphPosition;
                points[^1] = _currentGlyphPosition;
            }

            _glyphLineRenderer.positionCount = points.Length;
            _glyphLineRenderer.SetPositions(points);
        }
    }
}
