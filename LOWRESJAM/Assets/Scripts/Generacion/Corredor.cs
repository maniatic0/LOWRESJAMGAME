using UnityEngine;

/// <summary>
/// Direcciones validas
/// </summary>
public enum Direccion
{
    Norte, Este, Sur, Oeste,
}


/// <summary>
/// Unidad Basica de un Pasillo, es una linea Recta
/// </summary>
public class Corredor {

    /// <summary>
    /// Posicion inicial en eje X, medido de abajo a la izquierda
    /// </summary>
    public int startXPos;
    /// <summary>
    /// Posicion inicial en eje Y, medido de abajo a la izquierda
    /// </summary>
    public int startYPos;
    /// <summary>
    /// Longitud del Corredor
    /// </summary>
    public int longitud;
    /// <summary>
    /// Anchura del Corredor
    /// </summary>
    public int ancho;
    /// <summary>
    /// Direccion del corredor
    /// </summary>
    public Direccion direccion;
    /// <summary>
    /// Posicion X final del corredor
    /// </summary>
    public int posXFinal
    {
        get
        {
            if (direccion == Direccion.Norte || direccion == Direccion.Norte) // Corridor vertical
            {
                return startXPos + ancho - 1;
            }
            else if (direccion == Direccion.Este)  // Corridor Horizontal
            {
                return startXPos + longitud - 1;
            }
            return startXPos - longitud + 1;  // Corridor Horizontal
        }
    }
    /// <summary>
    /// Posicion Y final del corredor
    /// </summary>
    public int posYFinal
    {
        get
        {
            if (direccion == Direccion.Este || direccion == Direccion.Oeste) // Corridor Horizontal
            {
                return startYPos + ancho - 1;
            }
            else if (direccion == Direccion.Norte)  // Corridor Vertical
            {
                return startYPos + longitud - 1;
            }
            return startYPos - longitud + 1;  // Corridor Vertical
        }
    }

    /// <summary>
    /// Genera el primer corredor 
    /// </summary>
    /// <param name="cuarto">Del cuarto donde partimos</param>
    /// <param name="longitud_">Longitud del corredor</param>
    /// <param name="ancho_">Ancho del corredor</param>
    /// <param name="columnas">Cantidad de columas de matriz que representa el mundo</param>
    /// <param name="filas">Cantidad de filas de matriz que representa el mundo</param>
    /// <param name="primero">Primer pasillo</param>
    /// <returns></returns>
    public bool Setup(Cuarto cuarto, int longitud_, int ancho_, int columnas, int filas, bool primero = false)
    {
        bool generado = false; // Si se genero correctamente
        direccion = (Direccion)Random.Range(0, 4); // Aprovechar que es enum para generar direccion aleatorea

        int dado = Random.Range(0, 2);
        if (!primero && direccion == cuarto.direccionEntrada) // Vamos mal, estamos retrocediendo
        {
            if (dado == 1)
            {
                direccion = (Direccion)(((int)direccion - 1) % 4);
            }
            else
            {
                direccion = (Direccion)(((int)direccion + 1) % 4);
            }
        }
        // Colocar Longitud y Ancho
        longitud = longitud_;
        ancho = ancho_;
        if (ancho < 1) { Debug.Log("Error"); }

        for (int i = 0; i < 2; i++)  // Tratar de conseguir posicion Valida
        {
            switch (direccion)  // Casos dependiendo de Direccion
            {
                case Direccion.Norte:
                    // Si vamos al norte generar el punto izquierdo en cualquier parte norte de cuarto,
                    // tomando en cuenta que el corredor tiene ancho
                    startXPos = Random.Range(cuarto.PosX, cuarto.PosX + cuarto.ancho - ancho);
                    startYPos = cuarto.PosY + cuarto.alto - 1;
                    break;
                case Direccion.Este:
                    startXPos = cuarto.PosX + cuarto.ancho - 1;
                    startYPos = Random.Range(cuarto.PosY, cuarto.PosY + cuarto.alto - ancho);
                    break;
                case Direccion.Sur:
                    startXPos = Random.Range(cuarto.PosX, cuarto.PosX + cuarto.ancho - ancho);
                    startYPos = cuarto.PosY + cuarto.alto - 1;
                    break;
                case Direccion.Oeste:
                    startXPos = cuarto.PosX + cuarto.ancho - 1;
                    startYPos = Random.Range(cuarto.PosY, cuarto.PosY + cuarto.alto - ancho);
                    break;
                default:
                    break;
            }
            // Probar si estamos en cuadricula del mundo
            if (Utilidades.enRango(0, columnas - 1, 0, filas - 1, startXPos, posXFinal, startYPos, posYFinal))
            {
                generado = true;
                break;
            }
            else
            {
                // Tratar otra direccion
                if (dado == 1)
                {
                    direccion = (Direccion)(((int)direccion - 1) % 4);
                }
                else
                {
                    direccion = (Direccion)(((int)direccion + 1) % 4);
                }
            }
        }
        return generado;
    }

    /// <summary>
    /// Genera el primer corredor 
    /// </summary>
    /// <param name="anterior">Corredor anterior</param>
    /// <param name="cuarto">Del cuarto donde partimos</param>
    /// <param name="longitud_">Longitud del corredor</param>
    /// <param name="ancho_">Ancho del corredor</param>
    /// <param name="columnas">Cantidad de columas de matriz que representa el mundo</param>
    /// <param name="filas">Cantidad de filas de matriz que representa el mundo</param>
    /// <param name="primero">Primer pasillo</param>
    /// <returns></returns>
    public bool Setup(Corredor anterior, Cuarto cuarto, int longitud_, int ancho_, int columnas, int filas, bool primero = false)
    {
        bool generado = false; // Si se genero correctamente
        direccion = (Direccion)Random.Range(0, 4); // Aprovechar que es enum para generar direccion aleatorea

        int dado = Random.Range(0, 2);
        if (!primero && direccion == cuarto.direccionEntrada) // Vamos mal, estamos retrocediendo
        {
            if (dado == 1)
            {
                direccion = (Direccion)(((int)direccion - 1) % 4);
            }
            else
            {
                direccion = (Direccion)(((int)direccion + 1) % 4);
            }
        }
        // Colocar Longitud y Ancho
        longitud = longitud_;
        ancho = ancho_;
        if (ancho < 1) { Debug.Log("Error"); }

        for (int i = 0; i < 2; i++)  // Tratar de conseguir posicion Valida
        {
            switch (direccion)  // Casos dependiendo de Direccion
            {
                case Direccion.Norte:
                    switch (anterior.direccion)
                    {
                        case Direccion.Norte:  // Vamos ambos al norte
                            startXPos = anterior.posXFinal - anterior.ancho + 1;
                            startYPos = anterior.posYFinal;
                            break;
                        case Direccion.Este:  // Anterior va a este
                            startXPos = anterior.posXFinal;
                            startYPos = anterior.posYFinal - anterior.ancho + 1;
                            break;
                        case Direccion.Sur:
                            // Este Caso deberia ser imposible
                            break;
                        case Direccion.Oeste:  // Anterior va a oeste
                            startXPos = anterior.posXFinal + ancho - 1;
                            startYPos = anterior.posYFinal - anterior.ancho + 1;
                            break;
                    }
                    break;
                case Direccion.Este:
                    switch (anterior.direccion)
                    {
                        case Direccion.Norte:  // Anterior va a norte
                            startXPos = anterior.posXFinal;
                            startYPos = anterior.posYFinal - anterior.ancho + 1;
                            break;
                        case Direccion.Este:  // Vamos ambos al este
                            startXPos = anterior.posXFinal;
                            startYPos = anterior.posYFinal - anterior.ancho + 1;
                            break;
                        case Direccion.Sur:  // Anterior va a Sur
                            startXPos = anterior.posXFinal;
                            startYPos = anterior.posYFinal;
                            break;
                        case Direccion.Oeste:  // Anterior va a oeste
                            // Este Caso deberia ser imposible
                            break;
                    }
                    break;
                case Direccion.Sur:
                    switch (anterior.direccion)
                    {
                        case Direccion.Norte:  // Anterior va a norte
                            // Caso Imposible
                            break;
                        case Direccion.Este:  // Anterior va a este
                            startXPos = anterior.posXFinal;
                            startYPos = anterior.posYFinal;
                            break;
                        case Direccion.Sur:  // Anterior va a Sur
                            startXPos = anterior.posXFinal - anterior.ancho + 1;
                            startYPos = anterior.posYFinal;
                            break;
                        case Direccion.Oeste:  // Anterior va a oeste
                            startXPos = anterior.posXFinal + ancho - 1;
                            startYPos = anterior.posYFinal;
                            break;
                    }
                    break;
                case Direccion.Oeste:
                    switch (anterior.direccion)
                    {
                        case Direccion.Norte:  // Anterior va a norte
                            startXPos = anterior.posXFinal;
                            startYPos = anterior.posYFinal + ancho - 1;
                            break;
                        case Direccion.Este:  // Anterior va a este
                            // Caso Imposible
                            break;
                        case Direccion.Sur:  // Anterior va sur
                            startXPos = anterior.posXFinal;
                            startYPos = anterior.posYFinal + ancho - 1;
                            break;
                        case Direccion.Oeste:  // Anterior va a oeste
                            startXPos = anterior.posXFinal;
                            startYPos = anterior.posYFinal - anterior.ancho + 1;
                            break;
                    }
                    break;
            }
            // Probar si estamos en cuadricula del mundo
            if (Utilidades.enRango(0, columnas - 1, 0, filas - 1, startXPos, posXFinal, startYPos, posYFinal))
            {
                generado = true;
                break;
            }
            else
            {
                // Tratar otra direccion
                if (dado == 1)
                {
                    direccion = (Direccion)(((int)direccion - 1) % 4);
                }
                else
                {
                    direccion = (Direccion)(((int)direccion + 1) % 4);
                }
            }
        }
        return generado;
    }
}
