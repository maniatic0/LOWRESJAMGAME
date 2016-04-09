using UnityEngine;
using System.Collections;

public class GeneradorMapa : MonoBehaviour {


    public int columnas = 100;                                 // The number of columns on the board (how wide it will be).
    public int filas = 100;                                    // The number of rows on the board (how tall it will be).
    public RangoInt numeroCuartos = new RangoInt(15, 20);         // The range of the number of rooms there can be.
    public RangoInt anchoCuarto = new RangoInt(3, 10);         // The range of widths rooms can have.
    public RangoInt alturaCuarto = new RangoInt(3, 10);        // The range of heights rooms can have.
    public RangoInt longitudPasillo = new RangoInt(6, 10);    // The range of lengths corridors between rooms can have.
    public RangoInt anchoPasillo = new RangoInt(6, 10);    // The range of widths corridors between rooms can have.

    public float Borde;
    public Material texturas;
    public GameObject player;

    private TileType[][] tiles;                               // A jagged array of tile types representing the board, like a grid.
    private Cuarto[] cuartos;                                     // All the rooms that are created for this board.
    private Pasillo[] pasillos;                             // All the corridors that connect the rooms.
    private GameObject boardHolder;                           // GameObject that acts as a container for all other tiles.

    private Mesh mesh;
    private Vector3[] vertices;
    private Vector3[] triangulos;
    private Vector3[] uv;

    void SetupArregloTiles()
    {
        // Set the tiles jagged array to the correct width.
        tiles = new TileType[columnas][];

        // Go through all the tile arrays...
        for (int i = 0; i < tiles.Length; i++)
        {
            // ... and set each tile array is the correct height.
            tiles[i] = new TileType[filas];
        }
    }

    void CrearCuartosYPasillos()
    {
        // Create the rooms array with a random size.
        cuartos = new Cuarto[numeroCuartos.generar];

        // There should be one less corridor than there is rooms.
        pasillos = new Pasillo[cuartos.Length - 1];

        // Create the first room and corridor.
        cuartos[0] = new Cuarto();
        pasillos[0] = new Pasillo();

        // Setup the first room, there is no previous corridor so we do not use one.
        cuartos[0].Setup(anchoCuarto, alturaCuarto, columnas, filas);

        // Setup the first corridor using the first room.
        pasillos[0].Setup(cuartos[0], longitudPasillo, anchoPasillo, columnas, filas, true);

        for (int i = 1; i < cuartos.Length; i++)
        {
            // Create a room.
            cuartos[i] = new Cuarto();

            // Setup the room based on the previous corridor.
            cuartos[i].Setup(anchoCuarto, alturaCuarto, columnas, filas, pasillos[i - 1]);

            // If we haven't reached the end of the corridors array...
            if (i < pasillos.Length)
            {
                // ... create a corridor.
                pasillos[i] = new Pasillo();

                // Setup the corridor based on the room that was just created.
                pasillos[i].Setup(cuartos[i], longitudPasillo, anchoPasillo, columnas, filas, false);
            }
        }

    }

    void MarcarValoresTileDeCuartos()
    {
        // Go through all the rooms...
        for (int i = 0; i < cuartos.Length; i++)
        {
            Cuarto currentRoom = cuartos[i];

            // ... and for each room go through it's width.
            for (int j = 0; j < currentRoom.ancho; j++)
            {
                int xCoord = currentRoom.PosX + j;

                // For each horizontal tile, go up vertically through the room's height.
                for (int k = 0; k < currentRoom.alto; k++)
                {
                    int yCoord = currentRoom.PosY + k;
                    // The coordinates in the jagged array are based on the room's position and it's width and height.
                    tiles[xCoord][yCoord] = TileType.Floor;
                }
            }
        }
    }

    void MarcarValoresDeTileDePasillos()
    {
        Pasillo currentPasillo;
        int xCoord;
        int yCoord;
        // Go through every corridor...
        for (int i = 0; i < pasillos.Length; i++)
        {
            currentPasillo = pasillos[i];

            for (int r = 0; r < currentPasillo.cantidad; r++)
            {
                Corredor currentCorridor = currentPasillo.corredores[r];

                // Start the coordinates at the start of the corridor.
                xCoord = currentCorridor.startXPos;
                yCoord = currentCorridor.startYPos;

                switch (currentCorridor.direccion)
                {
                    case Direccion.Norte:
                        for (int j = 0; j < currentCorridor.longitud; j++)
                        {
                            for (int k = 0; k < currentCorridor.ancho; k++)
                            {
                                // Set the tile at these coordinates to Floor.
                                tiles[xCoord + k][yCoord + j] = TileType.Floor;
                            }
                        }
                        break;
                    case Direccion.Este:
                        for (int j = 0; j < currentCorridor.longitud; j++)
                        {
                            for (int k = 0; k < currentCorridor.ancho; k++)
                            {
                                // Set the tile at these coordinates to Floor.
                                tiles[xCoord + j][yCoord + k] = TileType.Floor;
                            }
                        }
                        break;
                    case Direccion.Sur:
                        for (int j = 0; j < currentCorridor.longitud; j++)
                        {
                            for (int k = 0; k < currentCorridor.ancho; k++)
                            {
                                // Set the tile at these coordinates to Floor.
                                tiles[xCoord + k][yCoord - j] = TileType.Floor;
                            }
                        }
                        break;
                    case Direccion.Oeste:
                        for (int j = 0; j < currentCorridor.longitud; j++)
                        {
                            for (int k = 0; k < currentCorridor.ancho; k++)
                            {
                                // Set the tile at these coordinates to Floor.
                                tiles[xCoord - j][yCoord + k] = TileType.Floor;
                            }
                        }
                        break;
                }
            }
        }
    }


    void Mostrar()
    {
        string texto;
        for (int i = 0; i < columnas; i++)
        {
            texto = "";
            for (int j = 0; j < filas; j++)
            {
                texto += tiles[i][j].ToString();
                if (j < filas - 1) { texto += " "; }
            }
            Debug.Log(texto);
        }
    }

    public static void InstanciarMatrizDeEnumTileTypes(TileType[][] Matriz, int Columnas, int Filas, float borde, Material Texturas, GameObject Parent, Vector3 Centro)
    {
        int centroX = (Columnas - 1) / 2;
        int centroY = (Filas - 1) / 2;

        float alto = borde / 100f;
        float ancho = borde / 100f;

        int numeroTiles = Columnas * Filas;
        int numeroTriangulos = numeroTiles * 6;
        int numeroVertices = numeroTiles * 4;

        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[numeroVertices];
        int[] triangulos = new int[numeroTriangulos];
        Vector2[] uv = new Vector2[numeroVertices];

        int iterVertices = 0;
        int iterTriangulos = 0;

        Vector3 inicio = new Vector3(Centro.x - ancho * ((float)centroX + 0.5f), Centro.y + alto * ((float)centroY + 0.5f), 0);
        Vector3 fila;
        for (int i = 0; i < Columnas; i++)
        {
            fila = inicio;
            for (int j = 0; j < Filas; j++)
            {
                vertices[iterVertices + 0] = fila;
                vertices[iterVertices + 1] = fila + new Vector3(ancho, 0, 0);
                vertices[iterVertices + 2] = fila + new Vector3(ancho, -alto, 0);
                vertices[iterVertices + 3] = fila + new Vector3(0, -alto, 0);

                triangulos[iterTriangulos + 0] = iterVertices + 0;
                triangulos[iterTriangulos + 1] = iterVertices + 1;
                triangulos[iterTriangulos + 2] = iterVertices + 2;
                triangulos[iterTriangulos + 3] = iterVertices + 0;
                triangulos[iterTriangulos + 4] = iterVertices + 2;
                triangulos[iterTriangulos + 5] = iterVertices + 3;

                switch (Matriz[i][j])
                {
                    case TileType.Wall:
                        uv[iterVertices + 0] = new Vector2(0, 0.5f);
                        uv[iterVertices + 1] = new Vector2(0.5f, 0.5f);
                        uv[iterVertices + 2] = new Vector2(0.5f, 1f);
                        uv[iterVertices + 3] = new Vector2(0, 1f);
                        break;
                    case TileType.Floor:
                        uv[iterVertices + 0] = new Vector2(0, 0);
                        uv[iterVertices + 1] = new Vector2(0.5f, 0);
                        uv[iterVertices + 2] = new Vector2(0.5f, 0.5f);
                        uv[iterVertices + 3] = new Vector2(0, 0.5f);
                        break;
                }


                iterVertices += 4;
                iterTriangulos += 6;

                /*
                GameObject instancia = (GameObject)Instantiate(Matriz[i][j].Transformar(Walls, Floors), fila, Quaternion.identity);
                instancia.transform.SetParent(Parent.transform); */
                fila.x += ancho;
            }
            inicio.y -= alto;
        }
        mesh = Parent.AddComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangulos;
        mesh.uv = uv;
        mesh.Optimize();
        mesh.RecalculateNormals();
        Parent.AddComponent<MeshRenderer>().material = Texturas;
    }

    private void Start()
    {
        // Create the board holder.
        boardHolder = new GameObject("BoardHolder");

        SetupArregloTiles();

        CrearCuartosYPasillos();

        MarcarValoresTileDeCuartos();
        MarcarValoresDeTileDePasillos();

        //Mostrar();

        InstanciarMatrizDeEnumTileTypes(tiles, columnas, filas, Borde, texturas, boardHolder, Vector3.zero);
        /*
        InstantiateTiles();
        InstantiateOuterWalls(); */
    }
}
