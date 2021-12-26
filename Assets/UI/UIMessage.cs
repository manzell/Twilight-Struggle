using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TwilightStruggle.UI
{
    public class UIMessage : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI message;
        public void Message(string str) => message.text = str;
    }
}