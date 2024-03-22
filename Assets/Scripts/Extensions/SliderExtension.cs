using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
namespace SwiKinGsStudio.UI
{
    public static class SliderExtension
    { 
        public static void SetSmoothlyValue(this Slider slider, float endValue, float duration)
        {
            slider.DOValue(endValue, duration);
        }

        public static void FillBlink(this Slider slider, float duration)
        {
            Image fillImage = slider.fillRect.GetComponent<Image>();
            if (DOTween.IsTweening(fillImage)) return;

            Sequence seqeunce = DOTween.Sequence();
            seqeunce.Append(fillImage.DOFade(0, duration));
            seqeunce.Append(fillImage.DOFade(1, duration));
        }

        public static void StartFillBlinking(this Slider slider, float duration)
        {
            Image fillImage = slider.fillRect.GetComponent<Image>();
            if (DOTween.IsTweening(fillImage)) return;
            Sequence seqeunce = DOTween.Sequence();
            seqeunce.Append(fillImage.DOFade(0, duration));
            seqeunce.Append(fillImage.DOFade(1, duration));
            seqeunce.SetLoops(-1);
        }
        public static void StartFillBlinking(this Slider slider, Color endColor, float duration)
        {
            Image fillImage = slider.fillRect.GetComponent<Image>();
            if (DOTween.IsTweening(fillImage)) return;

            Sequence seqeunce = DOTween.Sequence();
            Color startFillColor = fillImage.color;

            seqeunce.Append(fillImage.DOColor(endColor, duration));
            seqeunce.Append(fillImage.DOColor(startFillColor, duration));
            seqeunce.SetLoops(-1);
        }
        public static void StopFillBlinking(this Slider slider)
        {
            Image fillImage = slider.fillRect.GetComponent<Image>();
            fillImage.DOKill();
            fillImage.DOFade(1, 0);
        }

        public static void SetSmoothlyColor(this Slider slider, Color neededColor, float duration)
        {
            Image fillImage = slider.fillRect.GetComponent<Image>();
            fillImage.DOColor(neededColor, duration);
        }
        public static void SetSmoothlyColor(this Slider slider, Color neededColor, float duration, out Color previousColor)
        {
            Image fillImage = slider.fillRect.GetComponent<Image>();

            previousColor = fillImage.color;
            fillImage.DOColor(neededColor, duration);
        }

        public static Color GetFillColor(this Slider slider)
        {
            Image fillImage = slider.fillRect.GetComponent<Image>();
            return fillImage.color;
        }

        public static void SmoothlyDisable(this Slider slider, float duration)
        {
            if (slider.gameObject.activeSelf == false) return;

            CanvasGroup canvasGroup = slider.TryGetComponent(out CanvasGroup _) ? slider.GetComponent<CanvasGroup>() : slider.gameObject.AddComponent<CanvasGroup>();
            canvasGroup.DOFade(0, duration).OnComplete(() => slider.gameObject.SetActive(false));
        }

        public static void SmoothlyActivate(this Slider slider, float duration)
        {
            if (slider.gameObject.activeSelf) return;

            CanvasGroup canvasGroup = slider.TryGetComponent(out CanvasGroup _) ? slider.GetComponent<CanvasGroup>() : slider.gameObject.AddComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
            slider.gameObject.SetActive(true);
            canvasGroup.DOFade(1, duration);
        }
        public static void SmoothlyActivate(this Slider slider, float endValue, float duration)
        {
            if (slider.gameObject.activeSelf) return;

            CanvasGroup canvasGroup = slider.TryGetComponent(out CanvasGroup _) ? slider.GetComponent<CanvasGroup>() : slider.gameObject.AddComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
            slider.gameObject.SetActive(true);
            canvasGroup.DOFade(endValue, duration);
        }
    }
}