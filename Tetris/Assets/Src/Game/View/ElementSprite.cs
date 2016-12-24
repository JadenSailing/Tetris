using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ElementSprite
{
    public UISprite Sprite;

    private bool _isValid;

    private Color _color;

    private TetrisCoordinate _pos;

    public void Init()
    {
        GameObject obj = NGUITools.AddChild(TetrisManager.Instance.panel.bgContent, TetrisManager.Instance.ElementSpriteRes.gameObject);
        obj.SetActive(true);
        Sprite = obj.GetComponent<UISprite>();
    }

    public bool IsValid
    {
        get
        {
            return _isValid;
        }
        set
        {
            _isValid = value;

            this.UpdateDisplay();
        }
    }

    public Color32 Color
    {
        get
        {
            return _color;
        }
        set
        {
            _color = value;
            Sprite.color = _color;
        }
    }

    public TetrisCoordinate Position
    {
        get
        {
            return _pos;
        }
        set
        {
            _pos = value;

            int halfX = TetrisManager.Instance.GetSizeX() / 2;
            int halfY = TetrisManager.Instance.GetSizeY() / 2;

            Sprite.gameObject.transform.localPosition = new Vector3((_pos.I - halfX) * TetrisManager.ElementSize, -(_pos.J - halfY) * TetrisManager.ElementSize);
        }
    }

    public void UpdateDisplay()
    {
        Sprite.gameObject.SetActive(_isValid);
    }

    public void Destroy()
    {
        GameObject.Destroy(Sprite.gameObject);
    }
}
