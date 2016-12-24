using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TetrisCoordinate
{
    public TetrisCoordinate(int I = 0, int J = 0)
    {
        this.I = I;
        this.J = J;
    }

    /// <summary>
    /// 第I行
    /// </summary>
    public int I;

    /// <summary>
    /// 第J列
    /// </summary>
    public int J;
}

public enum ShapeRotation : int
{
    Up,
    Down,
    Left,
    Right
}

/// <summary>
/// 各种形状的基类
/// </summary>
public abstract class BaseShape
{
    protected TetrisCoordinate _coordinate;

    protected ShapeRotation _rotation;

    protected List<Element> _elementList;

    protected Color _color;

    public const int MaxChild = 4;

    public BaseShape()
    {
        _elementList = new List<Element>();

        for (int i = 0; i < MaxChild; i++)
        {
            _elementList.Add(new Element());
        }

        _coordinate = new TetrisCoordinate();

        int rand = UnityEngine.Random.Range(0, 4);
        _rotation = (ShapeRotation)rand;

        _color = Color.white;

        this.OnInit();
    }

    public Color Color
    {
        get
        {
            return _color;
        }
        set
        {
            _color = value;
        }
    }

    public ShapeRotation Rotation
    {
        get
        {
            return _rotation;
        }
        set
        {
            _rotation = value;
            this.UpdatePosition();
        }
    }

    public TetrisCoordinate Coordinate
    {
        get
        {
            return _coordinate;
        }
        set
        {
            _coordinate = value;
        }
    }

    protected abstract void OnInit();

    /// <summary>
    /// 旋转
    /// </summary>
    public void Rotate()
    {
        _rotation = this.GetNextRotation();
        this.UpdatePosition();
    }

    public ShapeRotation GetNextRotation()
    {
        ShapeRotation rotation = ShapeRotation.Left;
        if (_rotation == ShapeRotation.Up)
        {
            rotation = ShapeRotation.Right;
        }
        else if (_rotation == ShapeRotation.Right)
        {
            rotation = ShapeRotation.Down;
        }
        else if (_rotation == ShapeRotation.Down)
        {
            rotation = ShapeRotation.Left;
        }
        else if (_rotation == ShapeRotation.Left)
        {
            rotation = ShapeRotation.Up;
        }
        return rotation;
    }

    protected abstract void UpdatePosition();

    /// <summary>
    /// 获取所有子方格
    /// </summary>
    /// <returns></returns>
    public List<Element> GetElements()
    {
        return _elementList;
    }

    public int GetLeft()
    {
        int left = 999;
        for (int i = 0; i < _elementList.Count; i++)
        {
            int posI = _elementList[i].Position.I;
            if (posI < left)
            {
                left = posI;
            }
        }
        left = _coordinate.I + left;
        return left;
    }

    public int GetRight()
    {
        int right = -999;
        for (int i = 0; i < _elementList.Count; i++)
        {
            int posI = _elementList[i].Position.I;
            if (posI > right)
            {
                right = posI;
            }
        }
        right = _coordinate.I + right + 1;
        return right;
    }

    public int GetBottom()
    {
        int bottom = -999;
        for (int i = 0; i < _elementList.Count; i++)
        {
            int j = _elementList[i].Position.J;
            if(j > bottom)
            {
                bottom = j;
            }
        }
        bottom = _coordinate.J + bottom + 1;
        return bottom;
    }

    public int GetTop()
    {
        int top = 999;
        for (int i = 0; i < _elementList.Count; i++)
        {
            int j = _elementList[i].Position.J;
            if (j < top)
            {
                top = j;
            }
        }
        top = _coordinate.J + top;
        return top;
    }
}
