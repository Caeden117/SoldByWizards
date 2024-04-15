using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoldByWizards
{
    //[ExecuteInEditMode]
    public class GlyphTutorial : MonoBehaviour
    {
        private LineRenderer _lineRenderer;

        [SerializeField]
        public float Opacity;
        [SerializeField]
        public int Points;
        [SerializeField]
        public Vector2 Point0;
        [SerializeField]
        public Vector2 Point1;
        [SerializeField]
        public Vector2 Point2;
        [SerializeField]
        public Vector2 Point3;

        private void Start()
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }

        private void Update()
        {
            Color col = Color.white;
            col.a = Opacity;
            _lineRenderer.startColor = col;
            _lineRenderer.endColor = col;
            _lineRenderer.positionCount = Points;
            if (Points <= 0) return;
            _lineRenderer.SetPosition(0, Point0);
            if (Points <= 1) return;
            _lineRenderer.SetPosition(1, Point1);
            if (Points <= 2) return;
            _lineRenderer.SetPosition(2, Point2);
            if (Points <= 3) return;
            _lineRenderer.SetPosition(3, Point3);
        }
    }
}
