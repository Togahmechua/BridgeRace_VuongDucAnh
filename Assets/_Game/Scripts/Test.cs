using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Renderer rdr;
    public void ChangeColor()
    {
        rdr.material.color = Player.Ins.GetLastBrickColor();
    }
}
