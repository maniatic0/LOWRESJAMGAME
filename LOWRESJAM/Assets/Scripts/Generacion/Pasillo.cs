using UnityEngine;
using System.Collections;

public class Pasillo {
    /// <summary>
    /// Altura del Pasillo
    /// </summary>
    public int longitud;
    /// <summary>
    /// Anchura del Pasillo
    /// </summary>
    public int ancho;
    /// <summary>
    /// Cantidad de Corredores
    /// </summary>
    public int cantidad;
    /// <summary>
    /// Array de Corredores
    /// </summary>
    public Corredor[] corredores;
    /// <summary>
    /// Posicion inicial X del primer corredor
    /// </summary>
    public int startXPos
    {
    get {return corredores[0].startXPos;}
    }
    /// <summary>
    /// Posicion final Y del ultimo corredor
    /// </summary>
    public int startYPos
    {
        get { return corredores[0].startYPos; }
    }
    public int finalXPos
    {
        get { return corredores[0].posXFinal; }
    }
    /// <summary>
    /// Posicion final Y del ultimo corredor
    /// </summary>
    public int finalYPos
    {
        get { return corredores[cantidad-1].posYFinal; }
    }
    /// <summary>
    /// Direccion del ultimo corredor
    /// </summary>
    public Direccion direccion
    {
        get { return corredores[cantidad - 1].direccion; }
    }

    /// <summary>
    /// Generar un pasillo
    /// </summary>
    /// <param name="cuarto">Cuarto de donde sale pasillo</param>
    /// <param name="longitud_">Longitud del pasillo</param>
    /// <param name="ancho_">Ancho del pasillo</param>
    /// <param name="columnas">Cantidad de Columnas de matriz del mundo</param>
    /// <param name="filas">Cantidad de Filas de matriz del mundo</param>
    /// <param name="primero">Si es el primer pasillo</param>
    public void Setup(Cuarto cuarto, RangoInt longitud_, RangoInt ancho_, int columnas, int filas, bool primero)
    {
        // Generar ancho y largo de pasillo
        longitud = longitud_.generar;
        ancho = ancho_.generar;

        // Cantidad de corredores
        if (longitud < ancho)
        {
            cantidad = 1;
        }
        else
        {
            cantidad = Random.Range(1, ancho);
        }
        corredores = new Corredor[cantidad];
        corredores[0] = new Corredor(); // Primer Corredor

        int longitudCorredor = Mathf.FloorToInt((float)longitud / (float)cantidad);
        // Generacion de Pasillo
        bool generadoCorrecto = false;
        // Inicializacion
        for (int i = 1; i < cantidad; i++)
        {
            corredores[i] = new Corredor();
        }

        while (!generadoCorrecto)
        {
            generadoCorrecto = true;
            corredores[0].Setup(cuarto, longitudCorredor + 1, ancho, columnas, filas, primero);
            for (int i = 1; i < cantidad && generadoCorrecto; i++)
            {
                generadoCorrecto = corredores[i].Setup(corredores[i - 1], cuarto, longitudCorredor, ancho, columnas, filas, primero);
            }
        }
    }

}
