using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using DG.Tweening; 

namespace TwilightStruggle.UI
{
    public class Animations : SerializedMonoBehaviour
    {
        public void FadeSwapText(TextMeshProUGUI text, string newText, float time)
        {
            text.DOFade(0f, time * .45f).OnComplete(() => {
                text.text = newText;
                text.DOFade(1f, time * .45f).SetDelay(time * .1f);
            });
        }
    }
}
