using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardComponent : MonoBehaviour
{
    public TMP_Text title;
    public SpriteRenderer artwork;
    public SpriteRenderer background;
    public SpriteRenderer frame;
    public SpriteRenderer back;

    public Card card;
    public Card Card {
        get { return Card; }
        set {
            card = value;
            SetCard(card);
        }
    }

    private void Update() {
        if (artwork != null) {
            if(title.TryGetComponent<MeshRenderer>(out var renderer)) {
                renderer.material.SetVector("_Up", transform.up);
                //JavascriptPlugin.Log(transform.up.ToString());
            }
        }
    }

    private void SetCard(Card card) {
        artwork.sprite = card.artwork;
        background.sprite = card.background;
        title.text = card.name;
    }
}
