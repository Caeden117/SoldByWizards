using System;
using JetBrains.Annotations;
using UnityEngine;

namespace SoldByWizards.Glyphs
{
    public class GlyphAggregator : MonoBehaviour
    {
        [SerializeField] private GlyphController[] _glyphControllers;

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
    }
}
