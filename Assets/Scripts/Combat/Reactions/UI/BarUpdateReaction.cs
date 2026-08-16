using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Updates a scrollbar based on received context.
public class BarUpdateReaction : ReactionBase
{
    [SerializeField] private Scrollbar _scrollbar;
    [SerializeField] private TextMeshProUGUI _updateText;
    [SerializeField] private float _lerpDuration = 0.15f;
    [SerializeField] private float _maxStep = 0.05f;

    private Coroutine _coroutine;

    public override void Execute(EventContext ctx)
    {
        // Ignores if game state isn't playing.
        if (GameManager.Instance.GetGameState() != GameState.Playing) return;
        
        // If event context implements bar context, use the current and max amounts to animate the bar.
        if (ctx is IBarContext barContext)
        {
            // Size is 0 - 1, 0 being empty and 1 being max. Find the target size by finding the fraction between current and max.
            float targetSize = barContext.Current / barContext.Max;

            // Stops previous lerp coroutine
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }

            // Starts a coroutine which lerps the bar to the target size.
            _coroutine = StartCoroutine(LerpBar(targetSize));

            // Unity string interpolation, Sets health text to something like 30 / 100
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
            
            // Lerps current scrollbar size towards targetsize. Lerping is basically what percent from start size to target size.
            // lerp t value = 0.5 = 50% of the way to targetSize from startSize if 
            _scrollbar.size = Mathf.Lerp(startSize, targetSize, elapsed / _lerpDuration);
            yield return null;
        }

        // Sets the scroll bar to the target size as elapsed will likely not be perfectly 1:1 with _lerpDuration.
        _scrollbar.size = targetSize;
    }
}
