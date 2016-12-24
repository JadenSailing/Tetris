using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ShapeSprite
{
    private BaseShape _Data;

    public List<UISprite> SpriteList = null;

    public void Init()
    {
        
    }

    public BaseShape Data
    {
        get
        {
            return _Data;
        }
        set
        {
            if(SpriteList != null)
            {
                if(SpriteList.Count > 0)
                {
                    for (int i = 0; i < SpriteList.Count; i++)
                    {
                        GameObject.Destroy(SpriteList[i].gameObject);
                    }
                    SpriteList.Clear();
                }
            }
            else
            {
                SpriteList = new List<UISprite>();
            }

            _Data = value;

            for (int i = 0; i < BaseShape.MaxChild; i++)
            {
                GameObject obj = NGUITools.AddChild(TetrisManager.Instance.panel.bgContent, TetrisManager.Instance.ElementSpriteRes.gameObject);
                obj.SetActive(true);
                UISprite spr = obj.GetComponent<UISprite>();
                spr.color = _Data.Color;
                SpriteList.Add(spr);
            }
            this.UpdateDisplay();
        }
    }

    public void MoveDown()
    {
        Data.Coordinate.J = Data.Coordinate.J + 1;
        this.UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        int halfX = TetrisManager.Instance.GetSizeX() / 2;
        int halfY = TetrisManager.Instance.GetSizeY() / 2;

        for (int i = 0; i < BaseShape.MaxChild; i++)
        {
            Element e = Data.GetElements()[i];
            SpriteList[i].gameObject.SetActive(true);
            ElementPosition pos = e.Position;
            SpriteList[i].gameObject.transform.localPosition = new Vector3((pos.I + Data.Coordinate.I - halfX) * TetrisManager.ElementSize, -(pos.J + Data.Coordinate.J - halfY) * TetrisManager.ElementSize);
        }
    }

    public void Destroy()
    {
        _Data = null;
        for (int i = 0; i < SpriteList.Count; i++)
        {
            GameObject.Destroy(SpriteList[i].gameObject);
        }
        SpriteList.Clear();
        SpriteList = null;
    }
}
