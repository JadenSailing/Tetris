using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum TetrisSpeed : int
{
    Slow,
    Medium,
    Fast,
}

public class TetrisManager
{
    public const int ElementSize = 32;

    public int _w = 10;
    public int _h = 20;

    /// <summary>
    /// 每秒下降x格
    /// </summary>
    private TetrisSpeed _speed = TetrisSpeed.Slow;

    private int _score = 0;

    public TetrisPanel panel;

    public UISprite ElementSpriteRes;

    private float _startTime = 0.0f;

    private float _lastMoveTime = 0.0f;

    private ShapeSprite _currentShape;

    private BaseShape _nextShape;

    private bool _isPlaying = false;

    private bool _isPausing = false;

    public static TetrisManager _instance;

    private List<BaseShape> _ShapeList = new List<BaseShape>();

    private List<Type> _ShapeType = new List<Type>();

    private List<List<ElementSprite>> _valueList = new List<List<ElementSprite>>();

    public static TetrisManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new TetrisManager();
            }
            return _instance;
        }
    }

    public void Init()
    {
        panel.Init();

        _ShapeType.Add(typeof(IShape));
        _ShapeType.Add(typeof(OShape));
        _ShapeType.Add(typeof(TShape));
        _ShapeType.Add(typeof(SShape));
        _ShapeType.Add(typeof(ZShape));
        _ShapeType.Add(typeof(LShape));
        _ShapeType.Add(typeof(JShape));

        TouchManager.Instance.Register(KeyCode.LeftArrow, OnKeyDown);
        TouchManager.Instance.Register(KeyCode.RightArrow, OnKeyDown);
        TouchManager.Instance.Register(KeyCode.UpArrow, OnKeyDown);
        TouchManager.Instance.Register(KeyCode.DownArrow, OnKeyDown);

        for (int i = 0; i < _h; i++)
        {
            List<ElementSprite> list = new List<ElementSprite>();
            for (int j = 0; j < _w; j++)
            {
                ElementSprite e = new ElementSprite();
                e.Init();
                e.Position = new TetrisCoordinate(j, i);
                e.Color = Color.white;
                e.IsValid = true;
                list.Add(e);
            }
            _valueList.Add(list);
        }

        this.Reset();

        this.SetSpeed(TetrisSpeed.Slow);
    }

    private void Reset()
    {
        for (int i = 0; i < _valueList.Count; i++)
        {
            for (int j = 0; j < _valueList[i].Count; j++)
            {
                _valueList[i][j].IsValid = false;
            }
        }

        if (_currentShape != null)
        {
            _currentShape.Destroy();
            _currentShape = null;
        }
        Score = 0;
        _isPausing = false;
        _isPlaying = false;
    }

    public int Score
    {
        get
        {
            return _score;
        }
        set
        {
            _score = value;
            panel.Score.text = _score.ToString();
        }
    }

    private void OnKeyDown(KeyCode key)
    {
        if(!_isPlaying)
        {
            return;
        }
        if(_isPausing)
        {
            return;
        }
        if(_currentShape == null)
        {
            return;
        }
        switch(key)
        {
            case KeyCode.LeftArrow:
                TryMoveLeft();
                break;
            case KeyCode.RightArrow:
                TryMoveRight();
                break;
            case KeyCode.UpArrow:
                if(_currentShape != null)
                {
                    BaseShape shape = _currentShape.Data;

                    BaseShape shape2 = System.Activator.CreateInstance(shape.GetType()) as BaseShape;

                    shape2.Coordinate = shape.Coordinate;
                    shape2.Rotation = shape.GetNextRotation();

                    if (shape2.GetLeft() < 0)
                    {
                        return;
                    }
                    if (shape2.GetRight() > _w)
                    {
                        return;
                    }
                    if (shape2.GetBottom() > _h)
                    {
                        return;
                    }
                    List<Element> eList = shape2.GetElements();
                    for (int i = 0; i < eList.Count; i++)
                    {
                        int y = eList[i].Position.J + shape2.Coordinate.J;
                        int x = eList[i].Position.I + shape2.Coordinate.I;
                        if(y < 0)
                        {
                            continue;
                        }
                        if(_valueList[y][x].IsValid)
                        {
                            return;
                        }
                    }

                    shape.Rotate();
                    _currentShape.UpdateDisplay();
                }
                break;
            case KeyCode.DownArrow:
                _lastMoveTime -= this.GetCostTimePerLine();
                this.Update();
                break;
        }
    }

    private void TryMoveLeft()
    {
        if(_currentShape.Data.GetLeft() <= 0)
        {
            return;
        }
        if (this.CheckRight2LeftCollision(_currentShape.Data))
        {
            return;
        }
        _currentShape.Data.Coordinate.I = _currentShape.Data.Coordinate.I - 1;
        _currentShape.UpdateDisplay();
    }

    private void TryMoveRight()
    {
        if (_currentShape.Data.GetRight() >= _w)
        {
            return;
        }
        if (this.CheckLeft2RightCollision(_currentShape.Data))
        {
            return;
        }
        _currentShape.Data.Coordinate.I = _currentShape.Data.Coordinate.I + 1;
        _currentShape.UpdateDisplay();
    }
    
    public int GetSizeX()
    {
        return _w;
    }

    public int GetSizeY()
    {
        return _h;
    }

    public void StartGame()
    {
        this.Reset();

        _startTime = Time.time;
        _lastMoveTime = _startTime;
        _isPlaying = true;
        
    }

    private float GetCostTimePerLine()
    {
        int rate = 1;
        switch(_speed)
        {
            case TetrisSpeed.Slow:
                rate = 1;
                break;
            case TetrisSpeed.Medium:
                rate = 2;
                break;
            case TetrisSpeed.Fast:
                rate = 4;
                break;
        }
        return 1.0f / rate / 2;
    }

    public void Update()
    {
        if(!_isPlaying)
        {
            return;
        }
        if(_currentShape == null)
        {
            CreateNext();
            return;
        }
        if(Time.time - _lastMoveTime > this.GetCostTimePerLine())
        {
            //如果触底或碰撞
            if (_currentShape.Data.GetBottom() >= _h || this.CheckUp2DownCollision(_currentShape.Data))
            {
                //合并块
                this.AddElement();

                //检测消除
                this.ClearLines();

                //检测触顶
                if (_currentShape.Data.GetTop() < 0)
                {
                    _isPlaying = false;
                    return;
                }
                _currentShape.Destroy();
                _currentShape = null;
            }
            else
            {
                _currentShape.MoveDown();
            }
            _lastMoveTime = Time.time;
        }
    }

    private void AddElement()
    {
        //加入当前值
        List<Element> curEList = _currentShape.Data.GetElements();
        for (int i = 0; i < curEList.Count; i++)
        {
            int y = curEList[i].Position.J + _currentShape.Data.Coordinate.J;
            int x = curEList[i].Position.I + _currentShape.Data.Coordinate.I;
            if(y < 0)
            {
                continue;
            }
            _valueList[y][x].Color = _currentShape.Data.Color;
            _valueList[y][x].IsValid = true;
        }
    }

    /// <summary>
    /// 检查是否有完成的行
    /// </summary>
    private void ClearLines()
    {
        int totalLine = 0;
        //检测消除
        for (int i = 0; i < _h; i++)
        {
            bool isLineOK = true;
            for (int j = 0; j < _w; j++)
            {
                if(!_valueList[i][j].IsValid)
                {
                    isLineOK = false;
                    break;
                }
            }
            if(isLineOK)
            {
                totalLine++;
                //i行可消除 之上的整体下移
                for (int j = i; j > 0; j--)
                {
                    for (int k = 0; k < _w; k++)
                    {
                        _valueList[j][k].Color = _valueList[j - 1][k].Color;
                        _valueList[j][k].IsValid = _valueList[j - 1][k].IsValid;
                    }
                }
                //首行清空
                for (int k = 0; k < _w; k++)
                {
                    _valueList[0][k].IsValid = false;
                }
            }
        }
        Score += this.CalculateScore(totalLine);
    }

    /// <summary>
    /// 分值
    /// </summary>
    /// <param name="lines"></param>
    /// <returns></returns>
    private int CalculateScore(int lines)
    {
        return (int)Mathf.Pow(2.0f, (float)(lines)) - 1;
    }


    private bool CheckUp2DownCollision(BaseShape shapeUp)
    {
        List<Element> eUpList = shapeUp.GetElements();
        for (int i = 0; i < eUpList.Count; i++)
        {
            int x = eUpList[i].Position.I + shapeUp.Coordinate.I;
            int y = eUpList[i].Position.J + shapeUp.Coordinate.J + 1;

            if(y < 0)
            {
                continue;
            }

            if (_valueList[y][x].IsValid)
            {
                return true;
            }
        }
        return false;
    }

    private bool CheckLeft2RightCollision(BaseShape shapeLeft)
    {
        List<Element> eLeftList = shapeLeft.GetElements();
        for (int i = 0; i < eLeftList.Count; i++)
        {
            int x = eLeftList[i].Position.I + shapeLeft.Coordinate.I + 1;
            int y = eLeftList[i].Position.J + shapeLeft.Coordinate.J;
            if(y < 0)
            {
                continue;
            }
            if (_valueList[y][x].IsValid)
            {
                return true;
            }
        }
        return false;
    }

    private bool CheckRight2LeftCollision(BaseShape shapeRight)
    {
        List<Element> eRightList = shapeRight.GetElements();
        for (int i = 0; i < eRightList.Count; i++)
        {
            int x = eRightList[i].Position.I + shapeRight.Coordinate.I - 1;
            int y = eRightList[i].Position.J + shapeRight.Coordinate.J;
            if (y < 0)
            {
                continue;
            }

            if(_valueList[y][x].IsValid)
            {
                return true;
            }
        }
        return false;
    }

    private void CreateNext()
    {
        BaseShape shape = _nextShape;
        if (shape == null)
        {
            int index = UnityEngine.Random.Range(0, _ShapeType.Count);
            Type t = _ShapeType[index];
            shape = System.Activator.CreateInstance(t) as BaseShape;
        }
        shape.Coordinate.I = _w / 2;
        shape.Coordinate.J = 0;

        _currentShape = new ShapeSprite();
        _currentShape.Data = shape;

        //同时创建下一个
        int nextIndex = UnityEngine.Random.Range(0, _ShapeType.Count);
        Type nextT = _ShapeType[nextIndex];
        _nextShape = System.Activator.CreateInstance(nextT) as BaseShape;
        panel.SetNextShape(_nextShape);
    }

    public void Pause()
    {
        _isPausing = true;
    }

    public void SetSpeed(TetrisSpeed speed)
    {
        _speed = speed;

        panel.SetSpeed(_speed);
    }
}