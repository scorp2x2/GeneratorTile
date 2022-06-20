using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class Tile
{
    public float width;
    public float height;

    public List<Vector2> points;

    public Vector2 startPoint;

    public Tile(Tile tile)
    {
        width = tile.width;
        height = tile.height;
    }

    public Tile(float w, float h, Vector2 point)
    {
        width = w;
        height = h;

        points = new List<Vector2>()
        {
            point,
            point + Vector2.up * h,
            point + Vector2.up * h + Vector2.right * w,
            point + Vector2.right * w
        };

        startPoint = points[0];
    }

    /// <summary>
    /// Индекс точки не выходящий за пределы массива
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public int GetIndexPoints(int index)
    {
        if (index < 0) return points.Count - 1;
        if (index > points.Count - 1) return 0;
        return index;
    }

    /// <summary>
    /// ПОрезать плитку относительно стены
    /// </summary>
    /// <param name="wall"></param>
    public void Cut(Wall wall)
    {
        int indexOne = 0;

        List<Vector2> newPoints = new List<Vector2>();
        foreach (var item in points)
        {
            newPoints.Add(item);
        }

        if (wall.CheckAngleTile(this, out Vector2 point))
        {
            for (int i = 0; i < points.Count; i++)
            {
                if (!wall.CheckPoint(points[GetIndexPoints(i)]) && !wall.CheckPoint(points[GetIndexPoints(i + 1)]))
                {
                    newPoints.Insert(GetIndexPoints(i + 1), point);
                    break;
                }
            }
        }

        while (true)
        {

            Line l1 = new Line(points[GetIndexPoints(indexOne)], points[GetIndexPoints(indexOne + 1)]);

            var p1 = wall.Cross(l1);

            foreach (var item in p1)
                if (!newPoints.Any(a => Mathf.Approximately(a.x, item.x) && Mathf.Approximately(a.y, item.y)))
                {
                    newPoints.Insert(newPoints.IndexOf(points[GetIndexPoints(indexOne + 1)]), item);
                }

            Line l2 = new Line(points[GetIndexPoints(indexOne)], points[GetIndexPoints(indexOne - 1)]);

            var p2 = wall.Cross(l2);

            foreach (var item in p2)
                if (!newPoints.Any(a => Mathf.Approximately(a.x, item.x) && Mathf.Approximately(a.y, item.y)))
                {
                    newPoints.Insert(newPoints.IndexOf(points[GetIndexPoints(indexOne)]), item);
                }


            indexOne = GetIndexPoints(indexOne + 1);

            if (indexOne == 0)
                break;
        }

        points = newPoints;

        for (int i = 0; i < points.Count;)
        {
            if (!wall.CheckPoint(points[i]))
                points.RemoveAt(i);
            else
                i++;
        }
    }

    /// <summary>
    /// Получить площадь плитки
    /// </summary>
    /// <returns></returns>
    public float Square()
    {
        float s = 0;

        for (int i = 1; i < points.Count - 1; i++)
        {
            s += SquareTree(points[GetIndexPoints(0)], points[GetIndexPoints(i)], points[GetIndexPoints(i + 1)]);
        }
        s = Mathf.Abs(s);

        return s;
    }

    /// <summary>
    /// ПЛощадь треугольника
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <returns></returns>
    float SquareTree(Vector2 a, Vector2 b, Vector2 c)
    {
        float l1 = Vector2.Distance(a, b);
        float l2 = Vector2.Distance(b, c);
        float l3 = Vector2.Distance(c, a);

        float p = (l1 + l2 + l3) / 2;
        float s = Mathf.Sqrt(p * (p - l1) * (p - l2) * (p - l3));

        return s;
    }

    /// <summary>
    /// Повернуть плитку на угол относительно точки
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="vector"></param>
    public void Rotate(float angle, Vector2 vector)
    {
        for (int i = 0; i < points.Count; i++)
            points[i] = RotatePoint(points[i], angle, vector);

        startPoint = points[0];
    }

    /// <summary>
    /// Повернуть точку относительно точки
    /// </summary>
    /// <param name="point"></param>
    /// <param name="angle"></param>
    /// <param name="vector"></param>
    /// <returns></returns>
    public Vector2 RotatePoint(Vector2 point, float angle, Vector2 vector)
    {
        float newX = (point.x - vector.x) * Mathf.Cos(angle * Mathf.PI / 180) - (point.y - vector.y) * Mathf.Sin(angle * Mathf.PI / 180) + vector.x;
        float newY = (point.x - vector.x) * Mathf.Sin(angle * Mathf.PI / 180) + (point.y - vector.y) * Mathf.Cos(angle * Mathf.PI / 180) + vector.y;

        point = new Vector2(newX, newY);

        return point;
    }
}

