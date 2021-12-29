using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TwilightStruggle
{
    public interface IDragBehavior
    {
        public void OnDragStart(Transform t, PointerEventData data);
        public void OnDrag(Transform t, PointerEventData data);
        public void OnDragEnd(Transform t, PointerEventData data);
    }
}
