using UnityEngine;
using System.Collections;

public class GeneradorMapa : MonoBehaviour {
    // The type of tile that will be laid in a specific position.
    public enum TileType
    {
        Wall, Floor,
    }


    public int columnas = 100;                                 // The number of columns on the board (how wide it will be).
    public int filas = 100;                                    // The number of rows on the board (how tall it will be).
    public RangoInt numeroCuartos = new RangoInt(15, 20);         // The range of the number of rooms there can be.
    public RangoInt anchoCuarto = new RangoInt(3, 10);         // The range of widths rooms can have.
    public RangoInt alturaCuarto = new RangoInt(3, 10);        // The range of heights rooms can have.
    public RangoInt longitudPasillo = new RangoInt(6, 10);    // The range of lengths corridors between rooms can have.
    public RangoInt anchoPasillo = new RangoInt(6, 10);    // The range of widths corridors between rooms can have.
    public GameObject[] floorTiles;                           // An array of floor tile prefabs.
    public GameObject[] wallTiles;                            // An array of wall tile prefabs.
    public GameObject[] outerWallTiles;                       // An array of outer wall tile prefabs.
    public GameObject player;

    private TileType[][] tiles;                               // A jagged array of tile types representing the board, like a grid.
    private Cuarto[] cuartos;                                     // All the rooms that are created for this board.
    private Pasillo[] pasillos;                             // All the corridors that connect the rooms.
    private GameObject boardHolder;                           // GameObject that acts as a container for all other tiles.

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
            /*

            if (i == rooms.Length * .5f)
            {
                Vector3 playerPos = new Vector3(rooms[i].xPos, rooms[i].yPos, 0);
                Instantiate(player, playerPos, Quaternion.identity);
            } */
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
        // Go through every corridor...
        for (int i = 0; i < pasillos.Length; i++)
        {
            Pasillo currentCorridor = pasillos[i];
            
            // Start the coordinates at the start of the corridor.
            int xCoord = currentCorridor.startXPos;
            int yCoord = currentCorridor.startYPos;

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
    private void Start()
    {
        // Create the board holder.
        boardHolder = new GameObject("BoardHolder");

        SetupArregloTiles();

        CrearCuartosYPasillos();

        MarcarValoresTileDeCuartos();
        MarcarValoresDeTileDePasillos();

        Mostrar();
        /*
        InstantiateTiles();
        InstantiateOuterWalls(); */
    }
}
