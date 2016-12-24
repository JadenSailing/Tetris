using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public struct ElementPosition
{
    public int I;
    public int J;
}

/// <summary>
/// 定义每个小格子
/// </summary>
public class Element
{
    public ElementPosition Position = new ElementPosition();
}