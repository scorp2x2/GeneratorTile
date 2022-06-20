using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Line
{
    public Vector2 pointA;
    public Vector2 pointB;

    public Line(Vector2 a, Vector2 b)
    {
        pointA = a;
        pointB = b;
    }

    public Line(float x1, float y1, float x2, float y2) : this(new Vector2(x1, y1), new Vector2(x2, y2))
    {

    }

    public bool Cross(Vector2 a, Vector2 b, out Vector2 cross)
    {
        return Cross(new Line(a, b), out cross);
    }

    public bool Cross(float x1, float y1, float x2, float y2, out Vector2 cross)
    {
        return Cross(new Line(x1, y1, x2, y2),out cross);
    }

    /// <summary>
    /// Пересечение двух отрезков
    /// </summary>
    /// <param name="line"></param>
    /// <param name="cross">точка пересечения</param>
    /// <returns></returns>
    public bool Cross(Line line, out Vector2 cross)
    {
        cross = new Vector2();
        float n;
        if (pointB.y - pointA.y != 0)
        {  
            float q = (pointB.x - pointA.x) / (pointA.y - pointB.y);
            float sn = (line.pointA.x - line.pointB.x) + (line.pointA.y - line.pointB.y) * q;
            if (sn == 0)
                return false;
            float fn = (line.pointA.x - pointA.x) + (line.pointA.y - pointA.y) * q;  
            n = fn / sn;
        }
        else
        {
            if (line.pointA.y - line.pointB.y == 0)
                return false;  
            n = (line.pointA.y - pointA.y) / (line.pointA.y - line.pointB.y);   
        }
        cross.x = line.pointA.x + (line.pointB.x - line.pointA.x) * n;  
        cross.y = line.pointA.y + (line.pointB.y - line.pointA.y) * n;  

        cross.x = (float)Math.Round(cross.x, 3);
        cross.y = (float)Math.Round(cross.y, 3);

        if (Range(pointA.x, pointB.x, cross.x) && Range(line.pointA.x, line.pointB.x, cross.x))
            if (Range(pointA.y, pointB.y, cross.y) && Range(line.pointA.y, line.pointB.y, cross.y)) 
            	return true;

        return false;
    }

    /// <summary>
    /// Находится ли число между двух значений
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    bool Range(float a, float b, float value)
    {
        return value >= Mathf.Min(a, b) && value <= Mathf.Max(a, b);
    }
}

