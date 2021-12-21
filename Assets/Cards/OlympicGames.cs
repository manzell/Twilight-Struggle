using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OlympicGames : Card
{
    public override void CardEvent(GameAction.Command command)
    {
        UIManager uiManager = FindObjectOfType<UIManager>();

        // TODO: Present this to the Enemy Player rather than the playing player
        Message($"The Olympics are being hosted in {SelectHost()}. Participate or Boycott?");

        uiManager.SetButton(uiManager.confirmButton, "Participate", Participate);
        uiManager.SetButton(uiManager.cancelButton, "Boycott", Boycott);

        void Participate()
        {
            Dictionary<Game.Faction, int> rolls = new Dictionary<Game.Faction, int> { { Game.Faction.USA, 0 }, { Game.Faction.USSR, 0 } };

            while (rolls[Game.Faction.USA] == rolls[Game.Faction.USSR])
            {
                rolls[Game.Faction.USA] = Random.Range(0, 6) + 1;
                rolls[Game.Faction.USSR] = Random.Range(0, 6) + 1;
                rolls[Game.phasingPlayer] += 2;
            }

            Message($"{(rolls[Game.Faction.USA] > rolls[Game.Faction.USSR] ? Game.Faction.USA : Game.Faction.USSR)} wins the Olympics and 2 VPs!");
            Game.AdjustVPs.Invoke(rolls[Game.Faction.USA] > rolls[Game.Faction.USSR] ? 2 : -2);

            Finish();
        }

        void Boycott()
        {
            Message($"{command.enemyPlayer} Boycotts!");
            Game.AdjustDEFCON.Invoke(Game.phasingPlayer, -1);
            // Play a generic 4-op Card with Finish as the call back
            Finish();
        }

        void Finish()
        {
            uiManager.UnsetButton(uiManager.confirmButton);
            uiManager.UnsetButton(uiManager.cancelButton);
            command.callback.Invoke(); 
        }

        string SelectHost()
        {
            List<Country> hosts = new List<Country>();
            List<string> hostCities = new List<string>();

            foreach (Country country in FindObjectsOfType<Country>())
                if (country.control == Game.actingPlayer)
                    hosts.Add(country);

            switch (hosts[Random.Range(0, hosts.Count)].countryName)
            {
                case "Canada":
                    hostCities.Add("Montreal");
                    break;
                case "Japan":
                    hostCities.Add("Tokyo");
                    break;
                case "France":
                    hostCities.Add("Paris");
                    break;
                case "West Germany":
                    hostCities.Add("Frankfurt");
                    hostCities.Add("Berlin");
                    hostCities.Add("Frankfurt");
                    break;
                case "Italy":
                    hostCities.Add("Rome");
                    break;
                case "Spain/Portugal":
                    hostCities.Add("Madrid");
                    hostCities.Add("Barcelona");
                    hostCities.Add("Lisbon");
                    break;
                case "Greece":
                    hostCities.Add("Athens");
                    break;
                case "Turkey":
                    hostCities.Add("Istanbul");
                    break;
                case "Denmark":
                    hostCities.Add("Copenhagen");
                    break;
                case "Norway":
                    hostCities.Add("Oslo");
                    break;
                case "Sweden":
                    hostCities.Add("Stockholm");
                    break;
                case "Austria":
                    hostCities.Add("Vienna");
                    break;
                case "Poland":
                    hostCities.Add("Warsaw");
                    hostCities.Add("Krakow");
                    break;
                case "Hungary":
                    hostCities.Add("Budapest");
                    break;
                case "Czechoslovakia":
                    hostCities.Add("Prague");
                    break;
                case "Bulgaria":
                    hostCities.Add("Sofia");
                    break;
                case "Romania":
                    hostCities.Add("Bucharest");
                    break;
                case "Yugoslavia":
                    hostCities.Add("Sarajevo");
                    break;
                case "Syria":
                    hostCities.Add("Damascus");
                    break;
                case "Lebanon":
                    hostCities.Add("Beirut");
                    break;
                case "Israel":
                    hostCities.Add("Jerusalem");
                    break;
                case "Egypt":
                    hostCities.Add("Cairo");
                    break;
                case "Libya":
                    hostCities.Add("Tripoli");
                    break;
                case "Jordan":
                    hostCities.Add("Sarajevo");
                    break;
                case "Saudi Arabia":
                    hostCities.Add("Riyadh");
                    break;
                case "Gulf States":
                    hostCities.Add("Sarajevo");
                    break;
                case "Iraq":
                    hostCities.Add("Baghdad");
                    break;
                case "Iran":
                    hostCities.Add("Tehran");
                    break;
                case "Afghanistan":
                    hostCities.Add("Kabul");
                    break;
                case "Pakistan":
                    hostCities.Add("Karachi");
                    break;
                case "India":
                    hostCities.Add("Bombay");
                    hostCities.Add("Calcutta");
                    hostCities.Add("New Dehlin");
                    break;
                case "Thailand":
                    hostCities.Add("Bangkok");
                    break;
                case "Malaysia":
                    hostCities.Add("Kuala Lampus");
                    break;
                case "Indonesia":
                    hostCities.Add("Jakarta");
                    break;
                case "Australia":
                    hostCities.Add("Sydney");
                    hostCities.Add("Melbourne");
                    break;
                case "Phillipines":
                    hostCities.Add("Manila");
                    break;
                case "South Korea":
                    hostCities.Add("Seoul");
                    break;
                case "North Korea":
                    hostCities.Add("Pyongyang");
                    break;
                case "Mexico":
                    hostCities.Add("Mexico City");
                    break;
                case "Guatemala":
                    hostCities.Add("Guatemala City");
                    break;
                case "El Salvador":
                    hostCities.Add("San Salvador");
                    break;
                case "Nicaragua":
                    hostCities.Add("Manuage");
                    break;
                case "Cuba":
                    hostCities.Add("Havana");
                    break;
                case "Haiti":
                    hostCities.Add("Port-au-Prince");
                    break;
                case "Dominican Republic":
                    hostCities.Add("Santo Domingo");
                    break;
                case "Costa Rica":
                    hostCities.Add("San Jose");
                    break;
                case "Panama":
                    hostCities.Add("Panama City");
                    break;
                case "Colombia":
                    hostCities.Add("Bogota");
                    break;
                case "Venzuela":
                    hostCities.Add("Maracaibo");
                    break;
                case "Brazil":
                    hostCities.Add("Sao Paulo");
                    hostCities.Add("Rio de Janeiro");
                    break;
                case "Uruguay":
                    hostCities.Add("Montevideo");
                    break;
                case "Argentina":
                    hostCities.Add("Buenos Aires");
                    break;
                case "Paraguay":
                    hostCities.Add("Asuncion");
                    break;
                case "Bolivia":
                    hostCities.Add("La Paz");
                    break;
                case "Peru":
                    hostCities.Add("Lima");
                    break;
                case "Ecuador":
                    hostCities.Add("Quito");
                    break;
                case "Algeria":
                    hostCities.Add("Algiers");
                    break;
                case "Tunisia":
                    hostCities.Add("Tunis");
                    break;
                case "Morocco":
                    hostCities.Add("Casablanca");
                    break;
                case "West African States":
                    hostCities.Add("Dakar");
                    break;
                case "Saharan States":
                    hostCities.Add("Timbuktu");
                    break;
                case "Ivory Coast":
                    hostCities.Add("Abidjan");
                    break;
                case "Nigeria":
                    hostCities.Add("Lagos");
                    break;
                case "Cameroon":
                    hostCities.Add("Yaounde");
                    break;
                case "Zaire":
                    hostCities.Add("Kinshasa");
                    break;
                case "Angola":
                    hostCities.Add("Luanda");
                    break;
                case "Zimbabwe":
                    hostCities.Add("Harare");
                    break;
                case "Botswana":
                    hostCities.Add("Gabarone");
                    break;
                case "South Africa":
                    hostCities.Add("Cape Town");
                    hostCities.Add("Johannesburg");
                    break;
                case "SE Africa States":
                    hostCities.Add("Dar es Salaam");
                    break;
                case "Kenya":
                    hostCities.Add("Nairobi");
                    break;
                case "Somalia":
                    hostCities.Add("Mogadishu");
                    break;
                case "Ethiopia":
                    hostCities.Add("Addis Ababa");
                    break;
                case "Sudan":
                    hostCities.Add("Khartoum");
                    break;
            }

            return hostCities[Random.Range(0, hostCities.Count)];
        }
    }


}
