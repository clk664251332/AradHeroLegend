using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawTest : MonoBehaviour {

    public GameObject Break;
    public void Test()
    {
        Instantiate(Break, new Vector2(457, 100), Quaternion.identity);
    }
}
