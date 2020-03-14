using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AStarView : MonoBehaviour
{
    private AStarMap map;
    void Start()
    {
        map = GetComponent<AStarMap>();
        var trigger = GetComponent<EventTrigger>();
        var entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback = new EventTrigger.TriggerEvent();
        entry.callback.AddListener(e =>
        {
            if (map.CanChange)
            {
                float X = Input.mousePosition.x - Screen.width / 2f + 40;
                float Y = Input.mousePosition.y - Screen.height / 2f + 40;
                var pos = AStarMap.Pos2Index(new Vector2(X, Y));
                map[pos.Item1, pos.Item2].Change();
            }
        });
        trigger.triggers.Add(entry);
    }
}
