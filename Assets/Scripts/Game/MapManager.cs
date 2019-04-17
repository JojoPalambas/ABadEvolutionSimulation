using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager
{
    public enum TileType
    {
        none,
        floor,
        wall,
        lava,
        spikes
    }

    public readonly Tilemap tilemap;

    public MapManager(Tilemap tilemap)
    {
        this.tilemap = tilemap;
    }

    public TileType GetTileType(Vector2Int position)
    {
        Sprite sprite = tilemap.GetSprite(new Vector3Int(position.x, position.y, 0));
        if (sprite == null)
        {
            return TileType.none;
        }
        Debug.Log(sprite.name);
        if (sprite.name == "tileset_0")
        {
            return TileType.wall;
        }
        if (sprite.name == "tileset_1")
        {
            return TileType.floor;
        }
        if (sprite.name == "tileset_2")
        {
            return TileType.lava;
        }
        if (sprite.name == "tileset_3")
        {
            return TileType.spikes;
        }
        return TileType.none;
    }
}
