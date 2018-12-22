using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CursorState { Normal, Buy, Sell, Dialog }
public class CursorManager : MonoSingleton<CursorManager> {

    public Texture2D Normal, Buy, Sell, Dialog;
    public CursorState state;

    private void Awake()
    {
        state = CursorState.Normal;
    }

    public void SetNormal()
    {
        Cursor.SetCursor(Normal, Vector2.zero, CursorMode.Auto);
        state = CursorState.Normal;
    }
    public void SetBuy()
    {
        Cursor.SetCursor(Buy, Vector2.zero, CursorMode.Auto);
        state = CursorState.Buy;
    }
    public void SetSell()
    {
        Cursor.SetCursor(Sell, Vector2.zero, CursorMode.Auto);
        state = CursorState.Sell;
    }
    public void SetDialog()
    {
        Cursor.SetCursor(Dialog, Vector2.zero, CursorMode.Auto);
        state = CursorState.Dialog;
    }
}
