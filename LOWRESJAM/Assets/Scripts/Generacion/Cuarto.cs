using UnityEngine;
using System.Collections;
/// <summary>
/// Cuarto de Dungeon
/// </summary>
public class Cuarto
{
    /// <summary>
    /// Direccion de pasillo antes de cuarto
    /// </summary>
    public Direccion direccionEntrada;

    /// <summary>
    /// Posicion inicial en X, medido desde izquierda
    /// </summary>
    public int PosX;

    /// <summary>
    /// Posicion inicial en Y, medido desde izquierda
    /// </summary>
    public int PosY;

    /// <summary>
    /// Altura del cuarto
    /// </summary>
    public int alto;
    /// <summary>
    /// Anchura del cuarto
    /// </summary>
    public int ancho;

    /// <summary>
    /// Generar el primer Cuarto
    /// </summary>
    /// <param name="ancho_">Posibles anchuras del cuarto</param>
    /// <param name="alto_">Posibles alturas del cuarto</param>
    /// <param name="columnas">Cantidad de Columnas en matriz de mundo</param>
    /// <param name="filas">Cantidad de Filas en matriz de mundo</param>
    public void Setup(RangoInt ancho_, RangoInt alto_, int columnas, int filas)
    {
        ancho = ancho_.generar;
        alto = alto_.generar;
        PosX = Mathf.RoundToInt(columnas / 2f - ancho / 2f);
        PosY = Mathf.RoundToInt(filas / 2f - alto / 2f);
    }

    public void Setup(RangoInt ancho_, RangoInt alto_, int columnas, int filas, Pasillo pasillo)
    {
        direccionEntrada = pasillo.direccion;
        ancho = ancho_.generar;
        alto = alto_.generar;
        PosX = Mathf.RoundToInt(columnas / 2f - ancho / 2f);
        PosY = Mathf.RoundToInt(filas / 2f - alto / 2f);

        switch (direccionEntrada)
        {
            case Direccion.Norte:
                alto = Mathf.Clamp(alto, 1, filas - pasillo.finalYPos);
                PosY = pasillo.finalYPos;
                PosX = Random.Range(pasillo.finalXPos - pasillo.ancho + 1, pasillo.finalXPos - pasillo.ancho + ancho);
                PosX = Mathf.Clamp(PosX, 0, columnas - ancho);
                break;
            case Direccion.Este:
                ancho = Mathf.Clamp(ancho, 1, columnas - pasillo.finalXPos);
                PosX = pasillo.finalXPos;
                PosY = Random.Range(pasillo.finalYPos - pasillo.ancho + 1, pasillo.finalYPos - pasillo.ancho + alto);
                PosY = Mathf.Clamp(PosY, 0, filas - alto);
                break;
            case Direccion.Sur:
                alto = Mathf.Clamp(pasillo.finalYPos - alto, 1, pasillo.finalYPos);
                PosY = pasillo.finalYPos - alto + 1;
                PosX = Random.Range(pasillo.finalXPos - pasillo.ancho + 1, pasillo.finalXPos - pasillo.ancho + ancho);
                PosX = Mathf.Clamp(PosX, 0, columnas - ancho);
                break;
            case Direccion.Oeste:
                ancho = Mathf.Clamp(ancho, 1, pasillo.finalXPos);
                PosX = pasillo.finalXPos - ancho + 1;
                PosY = Random.Range(pasillo.finalYPos - pasillo.ancho + 1, pasillo.finalYPos - pasillo.ancho + alto);
                PosY = Mathf.Clamp(PosY, 0, filas - alto);
                break;
        }
    }
}
