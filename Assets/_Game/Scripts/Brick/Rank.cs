using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rank : MonoBehaviour
{
    [SerializeField] private Renderer rend;
    public ColorByEnum CurrentColorEnum { get; private set; }
    [SerializeField] ColorData colorData;

    public virtual void ChangeColor(ColorByEnum  color)
    {
        CurrentColorEnum = color;
        rend.material = colorData.GetMaterial(color);
    }

    private void OnCollisionEnter(Collision other)
    {
        Character character = Cache.GetCharacter(other.collider);
        if(character != null)
        {
            this.ChangeColor(character.CurrentColorEnum);
        }
    }
}
