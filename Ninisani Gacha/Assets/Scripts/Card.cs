using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    public string id;
    public string name;
    public string description;
    public Rarity rarity;
    public Sprite artwork;
    public Sprite background;
    public string error;

    public enum Rarity {
        Common,
        Uncommon,
        Rare,
        SR,
        UR
    }
}
