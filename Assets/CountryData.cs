using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CountryData : ScriptableObject
{
    public Country.Continent continent;
    public string countryName;
    public int stability;
    public bool isBattleground;
    public Sprite flag; 
}
