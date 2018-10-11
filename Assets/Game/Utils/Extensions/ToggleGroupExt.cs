using UnityEngine;
using UnityEngine.UI;

namespace Game.Utils
{
    public static class ToggleGroupExt
    {
        public static void SetInteractable(this ToggleGroup toggleGroup, bool interactable)
        {
            toggleGroup.gameObject.GetOrAddComponent<CanvasGroup>().interactable = interactable;
        }
    }
}