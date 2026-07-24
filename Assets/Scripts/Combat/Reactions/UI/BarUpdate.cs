using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Bar : ReactionBase
{
    [SerializeField] private Scrollbar _scrollbar;
    [SerializeField] private TextMeshProUGUI _updateText;
    [SerializeField] private float _lerpDuration = 0.15f;
    [SerializeField] private float _maxStep = 0.05f;

    private Coroutine _coroutine;

    public override void Execute(EventContext ctx)
    {
        if (ctx is IBarContext barContext)
        {
            float targetSize = barContext.Current / barContext.Max;

            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }

            _coroutine = StartCoroutine(LerpBar(targetSize));

            _updateText.text = $"{barContext.Current} / {barContext.Max}";
        }
    }

    private IEnumerator LerpBar(float targetSize)
    {
        float startSize = _scrollbar.size;
        float elapsed = 0f;

        while (elapsed < _lerpDuration)
        {
            elapsed += Mathf.Min(Time.deltaTime, _maxStep);
            _scrollbar.size = Mathf.Lerp(startSize, targetSize, elapsed / _lerpDuration);
            yield return null;
        }

        _scrollbar.size = targetSize;
    }
}
