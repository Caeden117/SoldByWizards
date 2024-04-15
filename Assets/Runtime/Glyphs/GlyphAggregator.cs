using System;
using JetBrains.Annotations;
using SoldByWizards.Maps;
using UnityEngine;

namespace SoldByWizards.Glyphs
{
    public class GlyphAggregator : MonoBehaviour
    {
        [SerializeField] private GlyphController[] _glyphControllers;
        [SerializeField] private MapStartButton _startButton;
        [SerializeField] private MeshRenderer[] _drawingStageIndicators;
        [SerializeField] private Material _glyphIndicatorOff;
        [SerializeField] private Material _glyphIndicatorOn;

        private int _glyphDrawingStage = 0;

        private void OnEnable()
        {
            for (var i = 0; i < _glyphControllers.Length; i++)
            {
                _glyphControllers[i].OnGlyphFinish += OnGlyphFinish;
            }

            ResetGlyphDrawingStage();
        }

        private void OnDisable()
        {
            for (var i = 0; i < _glyphControllers.Length; i++)
            {
                _glyphControllers[i].OnGlyphFinish -= OnGlyphFinish;
            }
        }

        private void OnGlyphFinish()
        {
            SetGlyphDrawingStage(++_glyphDrawingStage);
            if (_glyphDrawingStage >= _glyphControllers.Length) _startButton.ActivateButton(); // simulate pressing the start button. gamejam
        }

        [PublicAPI]
        public bool AllGlyphsValid()
        {
            for (var i = 0; i < _glyphControllers.Length; i++)
            {
                if (!_glyphControllers[i].IsValidGlyph) return false;
            }

            return true;
        }

        [PublicAPI]
        public int GetAggregateHash()
        {
            var hash = 0;

            unchecked
            {
                for (var i = 0; i < _glyphControllers.Length; i++)
                {
                    hash |= _glyphControllers[i].GetGlyphHash() << (i * 8);
                }
            }

            return hash;
        }

        [PublicAPI]
        public void ClearAllGlyphs()
        {
            for (var i = 0; i < _glyphControllers.Length; i++)
            {
                _glyphControllers[i].Clear();
            }
        }

        [PublicAPI]
        public void ResetGlyphDrawingStage()
        {
            SetGlyphDrawingStage(0);
        }

        private void SetGlyphDrawingStage(int drawingStage)
        {
            _glyphDrawingStage = drawingStage;
            for (var i = 0; i < _glyphControllers.Length; i++)
            {
                _glyphControllers[i].gameObject.SetActive(i == _glyphDrawingStage);
                _drawingStageIndicators[i].material = i < _glyphDrawingStage ? _glyphIndicatorOn : _glyphIndicatorOff;
            }
            _startButton.gameObject.SetActive(_glyphDrawingStage == _glyphControllers.Length);
        }
    }
}
