using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PositionDirection
{
    public Vector2Int position;
    public MapManager.Direction direction;

    public PositionDirection(Vector2Int position, MapManager.Direction direction)
    {
        this.position = position;
        this.direction = direction;
    }
}

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

    public enum Direction
    {
        none,
        up,
        down,
        left,
        right
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
        };
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

    public bool IsTileValid(Vector2Int position)
    {
        TileType type = GetTileType(position);
        return type != TileType.wall && type != TileType.none;
    }

    public Vector3 mapPositionToWorldPosition(Vector2Int mapPosition)
    {
        return tilemap.GetCellCenterWorld(new Vector3Int(mapPosition.x, mapPosition.y, 0));
    }

    public PositionDirection GetNextPosition(Vector2Int position, Direction lastDirection)
    {
        Direction direction = GetNextDirection(position, lastDirection);

        return new PositionDirection(MovePosition(position, direction), direction);
    }

    public Direction GetNextDirection(Vector2Int position, Direction direction)
    {
        List<Direction> possibleDirections = GetPossibleDirections(position, direction);

        if (possibleDirections.Count <= 0)
        {
            return Direction.none;
        }

        int rand = (int) Random.Range(0, possibleDirections.Count);
        return possibleDirections[rand];
    }

    public List<Direction> GetPossibleDirections(Vector2Int position, Direction direction)
    {
        List<Direction> possibleDirections = new List<Direction>(); ;

        if (GetTileType(position + new Vector2Int(0, 1)) != TileType.wall && direction != Direction.down)
        {
            possibleDirections.Add(Direction.up);
        }
        if (GetTileType(position + new Vector2Int(0, -1)) != TileType.wall && direction != Direction.up)
        {
            possibleDirections.Add(Direction.down);
        }
        if (GetTileType(position + new Vector2Int(-1, 0)) != TileType.wall && direction != Direction.right)
        {
            possibleDirections.Add(Direction.left);
        }
        if (GetTileType(position + new Vector2Int(1, 0)) != TileType.wall && direction != Direction.left)
        {
            possibleDirections.Add(Direction.right);
        }

        return possibleDirections;
    }

    public bool IsMoveValid(Vector2Int position, Direction direction)
    {
        return IsTileValid(MovePosition(position, direction));
    }

    public static Vector2Int MovePosition(Vector2Int position, Direction direction)
    {
        if (direction == Direction.up)
            return position + new Vector2Int(0, 1);
        if (direction == Direction.down)
            return position + new Vector2Int(0, -1);
        if (direction == Direction.left)
            return position + new Vector2Int(-1, 0);
        if (direction == Direction.right)
            return position + new Vector2Int(1, 0);

        Debug.LogWarning("MapManager.MovePosition failed");
        return position;
    }
}
