using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace TwilightStruggle
{
    public class UIMapColors : SerializedMonoBehaviour
    {
        public Color usControl, ussrControl;
        public ColorBook africa, asia, middleEast, europe, southAmerica, centralAmerica;

        public ColorBook GetColorBook(Country country)
        {
            switch (country.continent)
            {
                case Country.Continent.Africa: return africa;
                case Country.Continent.Europe: return europe;
                case Country.Continent.MiddleEast: return middleEast;
                case Country.Continent.Asia: return asia;
                case Country.Continent.CentralAmerica: return centralAmerica;
                case Country.Continent.SouthAmerica: return southAmerica;
            }
            return new ColorBook();
        }
    }

    public struct ColorBook
    {
        public Color background, outline, influenceBG, stabilityText;
    }
}