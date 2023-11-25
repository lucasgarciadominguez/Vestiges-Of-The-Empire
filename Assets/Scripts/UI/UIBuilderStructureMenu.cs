using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AnimationTypes
{
    None,
    BuildMenu,
    ExtractiveMenu,
    HouseMenu
}
public class UIBuilderStructureMenu : MonoBehaviour
{
    [SerializeField]
    float duration;
    [SerializeField]
    float delay;
    [SerializeField]
    AnimationCurve curve;
    [SerializeField]
    RectTransform target;
    [SerializeField]
    Vector2 startingPoint;
    [SerializeField]
    Vector2 finalPoint;
    Coroutine currentAnimation;

    public bool isPlaying => currentAnimation != null;
    public void FadeIn(string animType)
    {
        if (currentAnimation==null)
            currentAnimation=StartCoroutine(FadeInCoroutine(startingPoint, finalPoint));
    }
    public void FadeOut(string animType)
    {
        AnimationTypes animationTypes=GetAnimationTypes(animType);
        if(currentAnimation == null)
        currentAnimation = StartCoroutine(FadeInCoroutine(finalPoint, startingPoint));
    }
    IEnumerator FadeInCoroutine(Vector2 a, Vector2 b)
    {
        Vector2 startPoint=a;
        Vector2 endingPoint=b;
        float elapsed = 0;
        while (elapsed <= delay)
        {
            elapsed+=Time.deltaTime;
            yield return null;
        }

        elapsed = 0;
        while(elapsed <= duration)
        {
            float percentage=elapsed/duration;
            float curvePercentage= curve.Evaluate(percentage);
            elapsed += Time.deltaTime;
            Vector2 currentPosition =Vector2.LerpUnclamped(startPoint, endingPoint, curvePercentage);
            target.localPosition = currentPosition;
            yield return null;
        }
        target.localPosition = endingPoint;
        currentAnimation=null;
    }
    public AnimationTypes GetAnimationTypes(string animType)
    {
        switch (animType)
        {
            case "BuildMenu":
                return AnimationTypes.BuildMenu;
            case "ExtractiveMenu":
                return AnimationTypes.ExtractiveMenu;
            case "HouseMenu":
                return AnimationTypes.HouseMenu;
            default:
                break;
        }
        return AnimationTypes.None;
    }

}
