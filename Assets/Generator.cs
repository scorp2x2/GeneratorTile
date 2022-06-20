using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Generator : MonoBehaviour
{
    public Transform panelTiles;
    public GameObject tileMeshPrefub;

    public InputField widthIF;
    public InputField heightIF;
    public InputField stitchIF;
    public InputField angleIF;
    public InputField shiftIF;
    public Text squareTB;

    Wall wall = new Wall(1000, 800);
    Tile patternTile = new Tile(100, 50, new Vector2());

    public Tile[,] tiles;

    float angle = 15;
    float stitch = 2;
    float shift = 15;

    // Start is called before the first frame update
    void Start()
    {
        Calculate();
    }

    public void ReadUI()
    {
        if (widthIF.text != "")
        {
            try
            {
                patternTile.width = float.Parse(widthIF.text);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
        }
        else
            patternTile.width = 0;

        if (heightIF.text != "")
        {
            try
            {
                patternTile.height = float.Parse(heightIF.text);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
        }
        else
            patternTile.height = 0;
        if (angleIF.text != "")
        {
            try
            {
                angle = -Mathf.Min(float.Parse(angleIF.text), 89);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
        }
        else
            angle = 0;
        if (stitchIF.text != "")
        {
            try
            {
                stitch = float.Parse(stitchIF.text) / 10;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
        }
        else
            stitch = 0;
        if (shiftIF.text != "")
        {
            try
            {
                shift = Mathf.Min(float.Parse(shiftIF.text) / 10, patternTile.width);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
        }
        else
            shift = 0;
    }

    public void Calculate(string a = "")
    {
        ReadUI();
        Main();
    }

    void Main()
    {
        List<List<Tile>> tiles = new List<List<Tile>>();

        int countWidth = Mathf.RoundToInt(Mathf.Max(wall.width, wall.height) * 2 / patternTile.width);
        int countHeight = Mathf.RoundToInt(Mathf.Max(wall.width, wall.height) * 2 / patternTile.height);

        Debug.Log($"Кoличество: {countWidth} x {countHeight}");

        for (int i = 0; i < countHeight; i++)
        {
            tiles.Add(new List<Tile>());
            for (int j = 0; j < countWidth; j++)
            {
                float x = j * patternTile.width + (j) * stitch + (i * shift) % patternTile.width - countWidth * patternTile.width / 2;
                float y = i * patternTile.height + (i) * stitch - countHeight * patternTile.height / 2 + 10;

                var t = new Tile(patternTile.width, patternTile.height, new Vector2(x, y));
                t.Rotate(angle, new Vector2()); //Поворот плитки

                if (wall.CheckPlate(t) && t.points.Count > 2) //Проверка на принадлежность стене и обрезание плитки
                    tiles.Last().Add(t);
            }
        }

        float sq = 0;
        foreach (var tileRow in tiles)
            foreach (var item in tileRow)
            {
                if (item != null)
                {
                    sq += item.Square();
                }
            }
        squareTB.text = $"{Mathf.Round(sq / 10000)} м2";

        Draw(tiles, angle);
    }

    public void Draw(List<List<Tile>> tiles, float angle)
    {
        for (int j = 0; j < panelTiles.childCount; j++)
        {
            Destroy(panelTiles.GetChild(j).gameObject);
        }

        foreach (var tileRow in tiles)
            foreach (var item in tileRow)
            {
                if (item != null)
                {
                    {
                        Instantiate(tileMeshPrefub, panelTiles).GetComponent<UnityMeshCreator>().Draw(item, angle);

                        if (item.points.Count == 3)
                        {
                            item.points.Reverse();
                            Instantiate(tileMeshPrefub, panelTiles).GetComponent<UnityMeshCreator>().Draw(item, angle);
                        }
                    }
                }
            }
    }
}
