using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banner
{
    public string id;
    public string name;
    public Sprite image;
    public List<Card> cards = new();
    public AssetBundle bundle;
    public string error;
}
