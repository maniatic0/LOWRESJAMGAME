using System;
/// <summary>
/// Dados un minimo y un maximo devuelve un entero aleatoreo entre ellos
/// </summary>
// Que se vea en interfaz de unity
[Serializable]
public class RangoInt {
    public int minimo;  // Minimo valor que puede tomar
    public int maximo;  // Maximo valor que puede tomar

    // Constructor
    public RangoInt(int minimo_, int maximo_)
    {
        minimo = minimo_;
        maximo = maximo_;
    }

    /// <summary>
    /// Generar numero aleatorio entre minimo y maximo
    /// </summary>
    public int generar
    {
        get { return UnityEngine.Random.Range(minimo, maximo); }
    }
}
