using UnityEngine;
using System.Collections;

/// <summary>
/// The type of tile that will be laid in a specific position.
/// </summary>
public enum TileType
{
    Wall, Floor,
}

/// <summary>
/// Clase que tiene que existir porque sí, Transforma el enum en gameobject
/// </summary>
public static class TransformacionTileType
{
    /// <summary>
    /// Transforma un enum TileType a un GameObject
    /// </summary>
    /// <param name="self">Instancia quien lo llama</param>
    /// <param name="Walls">GameObjects que son tipo Wall</param>
    /// <param name="Floors">GameObjects que son tipo Floor</param>
    /// <returns>Un GameAbject elegido aleatoreamente que encaja con el enum self</returns>
    public static GameObject Transformar(this TileType self, GameObject[] Walls, GameObject[] Floors)
    {
        switch (self)
        {
            case TileType.Wall:
                return Walls[Random.Range(0, Walls.Length - 1)];
            case TileType.Floor:
                return Floors[Random.Range(0, Floors.Length - 1)];
            default:
                return null;
        }
    }
}
