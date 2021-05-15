using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    public class TileUtils
    {
        public static List<Tuple<Vector2, int>> GetTiles(TileMap map, Rect2 box)
        {
            List<Tuple<Vector2, int>> tiles = new List<Tuple<Vector2, int>>();
            Vector2 pos = box.Position - (box.Size/2);
            Vector2 max = pos + box.Size;
            for (float x = pos.x; x <= max.x; x += box.Size.x)
            {
                for (float y = pos.y; y <= max.y; y += box.Size.y)
                {
                    var v = map.WorldToMap(map.ToLocal(new Vector2(x, y)));
                    var a = map.GetCellv(v);
                    if (a != -1) if (!tiles.Any((t) => t.Item1 == v)) tiles.Add(new Tuple<Vector2, int>(v, a));
                }
            }
            return tiles;
        }

        public static IEnumerable<Vector2> GetTileVectors(TileMap map, Rect2 box)
        {
            return GetTiles(map, box).Select(t => t.Item1);
        }

        public static IEnumerable<int> GetTileTypes(TileMap map, Rect2 box)
        {
            return GetTiles(map, box).Select(t => t.Item2);
        }
    }

    public class MathUtils
    {
        public static float Interpolate(float a, float b, float thru)
        {
            float diff = b - a;
            return a + diff * thru;
        }
    }
}