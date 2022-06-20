using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Wall
{
    public float width;
    public float height;

    /// <summary>
    /// Углы стены
    /// </summary>
    Vector2[] points;

    /// <summary>
    /// Центр стены
    /// </summary>
    public Vector2 centerPoint;

    public Wall(float w, float h)
    {
        width = w;
        height = h;

        points = new Vector2[4]
        {
            new Vector2(-width / 2, -height / 2),
            new Vector2(-width / 2, height / 2),
            new Vector2(width / 2, height / 2),
            new Vector2(width / 2, -height / 2)
        };

        centerPoint = new Vector2(w / 2, h / 2);
    }

    /// <summary>
    /// Проверка принадлежности плитки стене
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public bool CheckPlate(Tile t)
    {
        int count = 0;

        foreach (var point in t.points)
        {
            if (CheckPoint(point))
                count++;
        }

        if (count == 0)
        {
            if (!CheckAngleTile(t, out Vector2 vector))
                return false;
        }

        if (count != 4)
        {
            t.Cut(this);
        }

        return true;
    }

    /// <summary>
    /// ПРоверки точки на принадлежность стене
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public bool CheckPoint(Vector2 point)
    {
        if (point.x < -width / 2 || point.x > width / 2)
            return false;
        if (point.y < -height / 2 || point.y > height / 2)
            return false;

        return true;
    }

    /// <summary>
    /// Точки пересечения лайна со стеной
    /// </summary>
    /// <param name="l"></param>
    /// <returns></returns>
    public List<Vector2> Cross(Line l)
    {
        List<Vector2> points = new List<Vector2>();
        Vector2 point = new Vector2();
        for (int i = 0; i < 4; i++)
        {
            if (l.Cross(new Line(this.points[i], this.points[i != 3 ? i + 1 : 0]), out point))
            {
                points.Add(point);
            }
        }

        return points;
    }

    /// <summary>
    /// Проверка принадлежности угловых точек стены плитке
    /// </summary>
    /// <param name="tile"></param>
    /// <param name="point"></param>
    /// <returns></returns>
    public bool CheckAngleTile(Tile tile, out Vector2 point)
    {
        foreach (var p in points)
        {
            bool result = false;
            int j = tile.points.Count - 1;
            for (int i = 0; i < tile.points.Count; i++)
            {
                if ((tile.points[i].y < p.y && tile.points[j].y >= p.y || tile.points[j].y < p.y && tile.points[i].y >= p.y) &&
                        (tile.points[i].x + (p.y - tile.points[i].y) / (tile.points[j].y - tile.points[i].y) * (tile.points[j].x - tile.points[i].x) < p.x))
                    result = !result;
                j = i;
            }

            if (result)
            {
                point = p;
                return true;
            }
        }
        point = new Vector2();
        return false;
    }
}

