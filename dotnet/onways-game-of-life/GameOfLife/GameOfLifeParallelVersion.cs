namespace GameOfLife;

/// <summary>
/// Represents Conway's Game of Life in a parallel version.
/// The class provides methods to simulate the game's evolution based on simple rules.
/// </summary>
public sealed class GameOfLifeParallelVersion
{
    private readonly bool[,] initialGrid;
    private readonly int rows;
    private readonly int columns;

    private bool[,] currentGrid;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameOfLifeParallelVersion"/> class with the specified number of rows and columns of the grid. The initial state of the grid is randomly set with alive or dead cells.
    /// </summary>
    /// <param name="rows">The number of rows in the grid.</param>
    /// <param name="columns">The number of columns in the grid.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the number of rows or columns is less than or equal to 0.</exception>
    public GameOfLifeParallelVersion(int rows, int columns)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(rows);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(columns);

        this.rows = rows;
        this.columns = columns;

        this.initialGrid = new bool[rows, columns];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                this.initialGrid[i, j] = Random.Shared.Next(2) == 1;
            }
        }

        this.currentGrid = (bool[,])this.initialGrid.Clone();
        this.Generation = 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GameOfLifeParallelVersion"/> class with the given grid.
    /// </summary>
    /// <param name="grid">The 2D array representing the initial state of the grid.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <param name="grid"/> is null.</exception>
    public GameOfLifeParallelVersion(bool[,] grid)
    {
        ArgumentNullException.ThrowIfNull(grid);

        this.rows = grid.GetLength(0);
        this.columns = grid.GetLength(1);
        this.initialGrid = (bool[,])grid.Clone();
        this.currentGrid = (bool[,])grid.Clone();
        this.Generation = 0;
    }

    /// <summary>
    /// Gets the current generation grid as a separate copy.
    /// </summary>
    public bool[,] CurrentGeneration
    {
        get
        {
            return (bool[,])this.currentGrid.Clone();
        }
    }

    /// <summary>
    /// Gets the current generation number.
    /// </summary>
    public int Generation { get; private set; }

    /// <summary>
    /// Restarts the game by resetting the current grid to the initial state.
    /// </summary>
    public void Restart()
    {
        this.currentGrid = (bool[,])this.initialGrid.Clone();
        this.Generation = 0;
    }

    /// <summary>
    /// Advances the game to the next generation based on the rules of Conway's Game of Life.
    /// </summary>
    public void NextGeneration()
    {
        bool[,] nextGrid = new bool[this.rows, this.columns];

        Parallel.For(0, this.rows, i =>
        {
            for (int j = 0; j < this.columns; j++)
            {
                int aliveNeighbors = this.CountAliveNeighbors(i, j);
                bool isAlive = this.currentGrid[i, j];

                nextGrid[i, j] = isAlive
                    ? aliveNeighbors == 2 || aliveNeighbors == 3
                    : aliveNeighbors == 3;
            }
        });

        this.currentGrid = nextGrid;
        this.Generation++;
    }

    /// <summary>
    /// Counts the number of alive neighbors for a given cell in the grid.
    /// </summary>
    /// <param name="row">The row index of the cell.</param>
    /// <param name="column">The column index of the cell.</param>
    /// <returns>The number of alive neighbors for the specified cell.</returns>
    private int CountAliveNeighbors(int row, int column)
    {
        int count = 0;

        for (int i = row - 1; i <= row + 1; i++)
        {
            for (int j = column - 1; j <= column + 1; j++)
            {
                if (i == row && j == column)
                {
                    continue;
                }

                if (i >= 0 && i < this.rows &&
                    j >= 0 && j < this.columns &&
                    this.currentGrid[i, j])
                {
                    count++;
                }
            }
        }

        return count;
    }
}
