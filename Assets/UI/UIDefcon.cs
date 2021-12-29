using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace TwilightStruggle.UI
{
    public class UIDefcon : MonoBehaviour
    {
        [SerializeField] Image activeImage;
        [SerializeField] Sprite[] defconSprites;

        private void Awake()
        {
            DEFCONtrack.adjustDefconEvent.AddListener(UpdateDefcon);
        }

        void UpdateDefcon(int amount)
        {
            activeImage.DOCrossfadeImage(defconSprites[DEFCONtrack.status - 1], 2.5f).SetEase(Ease.Linear);
        }
    }
}