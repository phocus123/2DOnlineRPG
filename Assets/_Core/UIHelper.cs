using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIHelper
{
    public static IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup)
    {
        float rate = 1.0f / 0.25f;
        float progress = 0.0f;

        while (progress <= 1.0)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, progress);
            progress += rate * Time.deltaTime;
            yield return null;
        }
    }

    public static void ToggleCanvasGroup(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }

    public static void CloseAllCanvasGroups(List<CanvasGroup> canvasGroups)
    {
        foreach (CanvasGroup canvasGroup in canvasGroups)
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
        }
    }
}
