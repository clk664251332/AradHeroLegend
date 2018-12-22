using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapTitleUI : MonoSingleton<MapTitleUI> {

    public Text mapTitle;

    public void SetMapTitle(string title)
    {
        mapTitle.text = title;
    }
}
