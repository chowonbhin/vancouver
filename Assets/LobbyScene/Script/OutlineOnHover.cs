using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;

public class OutlineManager : MonoBehaviour
{
    public List<Outline> outlines;  // 여러 개의 Outline 컴포넌트들을 저장할 리스트

    private Dictionary<Outline, Color> originalColors = new();

    private void Awake()
    {
        foreach (var outline in outlines)
        {
            if (outline != null)
            {
                originalColors[outline] = outline.OutlineColor;
                outline.enabled = false;
            }
        }
    }

    public void EnableOutline(Outline target)
    {
        if (target != null)
        {
            target.enabled = true;
            target.OutlineColor = Color.blue;
        }
    }

    public void DisableOutline(Outline target)
    {
        if (target != null && originalColors.ContainsKey(target))
        {
            target.enabled = false;
            target.OutlineColor = originalColors[target];
        }
    }
}
