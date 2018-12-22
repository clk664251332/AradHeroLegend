using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxColiderSyn : MonoBehaviour {

    tk2dSprite sprite;
    BoxCollider2D colider;

    //private void OnDrawGizmos()
    //{
    //    sprite = GetComponent<tk2dSprite>();
    //    colider = GetComponent<BoxCollider2D>();
    //    var bounds = sprite.GetBounds();
    //    colider.size = bounds.size;
    //    colider.offset = new Vector2(bounds.center.x, bounds.center.y);
    //}

    private void Start()
    {
        sprite = GetComponent<tk2dSprite>();
        colider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        var bounds = sprite.GetBounds();
        colider.size = bounds.size;
        colider.offset = new Vector2(bounds.center.x, bounds.center.y);
    }
}
