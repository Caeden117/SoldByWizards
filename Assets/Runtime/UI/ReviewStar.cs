using UnityEngine;
using UnityEngine.UI;

namespace SoldByWizards.UI
{
    public class ReviewStar : MonoBehaviour
    {
        [SerializeField] private Image _outlineImage = null!;
        [SerializeField] private Image _fillImage = null!;
        [SerializeField] private Color _fullColor;
        [SerializeField] private Color _emptyColor;
    }
}
