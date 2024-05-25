using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stair : MonoBehaviour
{
    public Renderer rdr;
    
    public void ChnageColor()
    {
        rdr.material.color = Player.Ins.GetLastBrickColor();
    }
}
