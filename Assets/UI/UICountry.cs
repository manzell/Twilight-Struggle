using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector; 
using TMPro;
using DG.Tweening;

namespace TwilightStruggle.UI
{
    public class UICountry : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] TextMeshProUGUI _countryName, _stability, _usInfluence, _ussrInfluence;
        [SerializeField] Image _shadow, _background, _stabilityBG, _usInfluenceBG, _ussrInfluenceBG, _flag;
        [SerializeField] Outline _outline;
        public Country country;

        public static Dictionary<Country, UICountry> countryMap = new Dictionary<Country, UICountry>();
        public Image Shadow => _shadow;

        public UnityEvent<Country> clickEvent = new UnityEvent<Country>();
        public void OnPointerDown(PointerEventData eventData) => clickEvent.Invoke(country);

        void Awake()
        {
            Game.adjustInfluenceEvent.after.AddListener((c, f, i) => { if (c == country) Refresh(); });
            Game.gameStartEvent.AddListener(() => SetCountry(country));
        }

        [Button]
        void SetCountry(Country country)
        {
            if (country != null)
            {
                if (countryMap.ContainsKey(country))
                    countryMap[country] = this;
                else
                    countryMap.Add(country, this);

                this.country = country;
                gameObject.name = country.countryName + " Marker";
            }

            Refresh();
        }

        void Refresh()
        {
            UIMapColors UIMapColors = FindObjectOfType<UIMapColors>();

            // Set our Text Fields
            _countryName.text = country.countryName;
            _stability.text = country.stability.ToString();
            _usInfluence.text = country.influence[Game.Faction.USA].ToString();
            _ussrInfluence.text = country.influence[Game.Faction.USSR].ToString();

            _countryName.DOFade(1, .5f);
            _stability.DOFade(1, .5f);
            _usInfluence.DOFade(1, .5f);
            _ussrInfluence.DOFade(1, .5f);

            //// Image & Color settings
            _background.DOColor(UIMapColors.GetColorBook(country).background, 1f);
            _outline.DOColor(UIMapColors.GetColorBook(country).outline, 1f);

            _stabilityBG.DOColor(country.isBattleground ? Color.black : Color.white, 1f);
            _stability.DOColor(country.isBattleground ? Color.white : UIMapColors.GetColorBook(country).stabilityText, 1f);

            _usInfluenceBG.DOColor(country.control == Game.Faction.USA ? UIMapColors.usControl : UIMapColors.GetColorBook(country).influenceBG, 1f);
            _ussrInfluenceBG.DOColor(country.control == Game.Faction.USSR ? UIMapColors.ussrControl : UIMapColors.GetColorBook(country).influenceBG, 1f);

            if (country.influence[Game.Faction.USA] == 0)
                _usInfluence.DOColor(UIMapColors.GetColorBook(country).stabilityText, .75f).SetDelay(1.5f);
            else
                _usInfluence.DOColor(country.control == Game.Faction.USA ? Color.white : UIMapColors.usControl, .75f).SetDelay(.25f);

            if (country.influence[Game.Faction.USSR] == 0)
                _ussrInfluence.DOColor(UIMapColors.GetColorBook(country).stabilityText, .75f).SetDelay(1.5f);
            else
                _ussrInfluence.DOColor(country.control == Game.Faction.USSR ? Color.white : UIMapColors.ussrControl, .75f).SetDelay(.25f);

            if (country.countryData.flag != null)
                _flag.sprite = country.countryData.flag;
            else
                _flag.DOFade(0f, 1f);
        }

        public void ResetShadow()
        {
            _shadow.DOColor(Color.black, .3f);
            _shadow.DOFade(.3f, .3f);
        }
    }
}