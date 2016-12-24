using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TetrisPanel : MonoBehaviour
{
    public UISprite BgCellRes;

    public GameObject bgContent;

    public UITable table;

    public List<UISprite> speedList;

    public UILabel Score;

    public GameObject NextParent;

    [HideInInspector]
    private NextSprite _nextShape;


    public void Init()
    {
        for (int i = 0; i < TetrisManager.Instance.GetSizeX(); i++)
        {
            for (int j = 0; j < TetrisManager.Instance.GetSizeY(); j++)
            {
                NGUITools.AddChild(table.gameObject, BgCellRes.gameObject);
            }    
        }
        BgCellRes.gameObject.SetActive(false);
        table.Reposition();
    }

    public void SetSpeed(TetrisSpeed speed)
    {
        for (int i = 0; i < speedList.Count; i++)
        {
            if(i == (int)speed)
            {
                speedList[i].spriteName = "ButtonCommon01";
                speedList[i].color = Color.green;
            }
            else
            {
                speedList[i].spriteName = "ButtonCommon01-Dis";
                speedList[i].color = Color.white;
            }
        }
    }

    public void SetNextShape(BaseShape shape)
    {
        if(_nextShape == null)
        {
            _nextShape = new NextSprite();
        }
        _nextShape.Data = shape;
    }

    public void OnClick(GameObject obj)
    {
        switch(obj.name)
        {
            case "Start":
                TetrisManager.Instance.StartGame();
                break;
            case "Speed1":
                TetrisManager.Instance.SetSpeed(TetrisSpeed.Slow);
                break;
            case "Speed2":
                TetrisManager.Instance.SetSpeed(TetrisSpeed.Medium);
                break;
            case "Speed3":
                TetrisManager.Instance.SetSpeed(TetrisSpeed.Fast);
                break;
        }
    }
}
