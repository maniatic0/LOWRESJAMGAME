using System;
using UnityEngine;

public static class Utilidades
{
    /// <summary>
    /// Utilidad para saber si un rectangulo esta dentro de otro
    /// </summary>
    /// <param name="minX">Coordenada X de Punto Izquierdo de Rectangulo Exterior</param>
    /// <param name="maxX">Coordenada X de Punto Derecho de Rectangulo Exterior</param>
    /// <param name="minY">Coordenada Y de Punto Izquierdo de Rectangulo Exterior</param>
    /// <param name="maxY">Coordenada Y de Punto Derecho de Rectangulo Exterior</param>
    /// <param name="posXmin">Coordenada X de Punto Izquierdo de Rectangulo Interior</param>
    /// <param name="posXmax">Coordenada X de Punto Derecho de Rectangulo Interior</param>
    /// <param name="posYmin">Coordenada Y de Punto Izquierdo de Rectangulo Interior</param>
    /// <param name="posYmax">Coordenada Y de Punto Derecho de Rectangulo Interior</param>
    /// <returns>Si rectangulo interior esta totalmente contenido en rectangulo exterior</returns>
    public static bool enRango(int minX, int maxX, int minY, int maxY, int posXmin, int posXmax, int posYmin, int posYmax)
    {
        return minX <= posXmin && posXmin <= maxX && minX <= posXmax && posXmax <= maxX && minY <= posYmin && posYmin <= maxY && minY <= posYmax && posYmax <= maxY;
    }
}