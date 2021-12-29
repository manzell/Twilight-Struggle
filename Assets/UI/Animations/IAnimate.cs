using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwilightStruggle.UI
{
    public interface IAnimate
    {
        public abstract void Animate(GameCommand command);
    }
}
