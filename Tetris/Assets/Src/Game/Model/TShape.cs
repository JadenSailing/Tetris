using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 如下形状
/// □□□
///   □
/// </summary>
public class TShape : BaseShape
{
    protected override void OnInit()
    {
        this._color = UnityEngine.Color.cyan;
        this.UpdatePosition();
    }

    protected override void UpdatePosition()
    {
        Element e1 = _elementList[0];
        Element e2 = _elementList[1];
        Element e3 = _elementList[2];
        Element e4 = _elementList[3];

        switch(_rotation)
        {
            case ShapeRotation.Up:
                e1.Position.I = -1;
                e1.Position.J = 0;

                e2.Position.I = 0;
                e2.Position.J = -1;

                e3.Position.I = 0;
                e3.Position.J = 0;

                e4.Position.I = 1;
                e4.Position.J = 0;
                break;
            case ShapeRotation.Down:
                e1.Position.I = -1;
                e1.Position.J = -1;

                e2.Position.I = 0;
                e2.Position.J = -1;

                e3.Position.I = 0;
                e3.Position.J = 0;

                e4.Position.I = 1;
                e4.Position.J = -1;
                break;
            case ShapeRotation.Left:
                e1.Position.I = -1;
                e1.Position.J = 0;

                e2.Position.I = 0;
                e2.Position.J = -1;

                e3.Position.I = 0;
                e3.Position.J = 0;

                e4.Position.I = 0;
                e4.Position.J = 1;
                break;
            case ShapeRotation.Right:
                e1.Position.I = 0;
                e1.Position.J = -1;

                e2.Position.I = 0;
                e2.Position.J = 0;

                e3.Position.I = 0;
                e3.Position.J = 1;

                e4.Position.I = 1;
                e4.Position.J = 0;
                break;
        }
    }
}