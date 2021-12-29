using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TwilightStruggle
{
    public interface IDragBehavior
    {
        public void OnDragStart(PointerEventData data);
        public void OnDrag(PointerEventData data);
        public void OnDragEnd(PointerEventData data);
    }
}
