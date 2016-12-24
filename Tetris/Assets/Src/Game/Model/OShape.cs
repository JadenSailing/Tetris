using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 如下形状
/// □□
/// □□
/// </summary>
public class OShape : BaseShape
{
    protected override void OnInit()
    {
        this._color = new UnityEngine.Color(1, 0, 1, 1);
        this.UpdatePosition();
    }

    protected override void UpdatePosition()
    {
        Element e1 = _elementList[0];
        Element e2 = _elementList[1];
        Element e3 = _elementList[2];
        Element e4 = _elementList[3];
        
        e1.Position.I = -1;
        e1.Position.J = -1;

        e2.Position.I = 0;
        e2.Position.J = -1;

        e3.Position.I = -1;
        e3.Position.J = 0;

        e4.Position.I = 0;
        e4.Position.J = 0;
    }
}