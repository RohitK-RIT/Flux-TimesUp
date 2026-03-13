using UnityEngine;

namespace _Project.Scripts.Gameplay.PCG {
    /// <summary>
    /// Represents a grid system used for managing cell-based placement within the dungeon.
    /// </summary>
    public class GridSystem { 
        /// <summary>
        /// Gets the width of the grid.
        /// </summary>
        public int GridWidth { get; }
        /// <summary>
        /// Gets the height of the grid.
        /// </summary>
        public int GridHeight { get; }
        /// <summary>
        /// Gets the size of each cell in the grid.
        /// </summary>
        public float CellSize { get; }
        /// <summary>
        /// Array to track occupied cells within the grid.
        /// </summary>
        private readonly bool[,] _occupiedCells;

        /// <summary>
        /// Visited Array to track visited cells within the grid.
        /// </summary>
        public bool[,] VisitedCells { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GridSystem"/> class with the specified dimensions and cell size.
        /// </summary>
        /// <param name="width">The width of the grid.</param>
        /// <param name="height">The height of the grid.</param>
        /// <param name="cellSize">The size of each cell in the grid.</param>
        public GridSystem(int width, int height, float cellSize) {
            GridWidth = width;
            GridHeight = height;
            this.CellSize = cellSize;
            _occupiedCells = new bool[width, height];
            VisitedCells = new bool[width, height];
        }
        
        /// <summary>
        /// Resets the visited cells array.
        /// </summary>
        public void ResetVisitedCells()
        {
            VisitedCells = new bool[GridWidth, GridHeight];
        }
        
        /// <summary>
        /// Gets the Vector2 grid cell position from the specified world position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="gridOrigin"></param>
        /// <returns></returns>
        public Vector2 GetGridCellPositionFromWorldPosition(int x, int y, Vector3 gridOrigin)
        {
            var startX = gridOrigin.x;
            var endX = gridOrigin.x + (GridWidth * CellSize);
            var startY = gridOrigin.z;
            var endY = gridOrigin.z + (GridHeight * CellSize);
            if (x < startX || x > endX || y < startY || y > endY) {
                Debug.LogWarning($"Cannot mark cell outside of grid bounds: {x}, {y}");
                return Vector2.zero;
            }
            x = Mathf.FloorToInt((x - gridOrigin.x) / CellSize);
            y = Mathf.FloorToInt((y - gridOrigin.z) / CellSize);
            
            x = Mathf.Clamp(x, 0, _occupiedCells.GetLength(0) - 1);
            y = Mathf.Clamp(y, 0, _occupiedCells.GetLength(1) - 1);
            return new Vector2(x, y);
        }

        /// <summary>
        /// Marks the specified cell as occupied.
        /// </summary>
        /// <param name="x">The x-coordinate of the cell.</param>
        /// <param name="y">The y-coordinate of the cell.</param>
        /// <param name="gridOrigin"></param>
        public void MarkCellOccupied(int x, int y, Vector3 gridOrigin) {
            var newVec2 = GetGridCellPositionFromWorldPosition(x, y, gridOrigin);
            _occupiedCells[(int)newVec2.x, (int)newVec2.y] = true;
        }
        
        /// <summary>
        /// Checks if the specified cell is occupied.
        /// </summary>
        /// <param name="x">The x-coordinate of the cell.</param>
        /// <param name="y">The y-coordinate of the cell.</param>
        /// <returns>True if the cell is occupied, false otherwise.</returns>
        public bool IsCellOccupied(int x, int y) {
            // Clamp the indices to ensure they are within bounds
            x = Mathf.Clamp(x, 0, GridWidth - 1);
            y = Mathf.Clamp(y, 0, GridHeight - 1);
            return _occupiedCells[x, y];
        }
        
        /// <summary>
        /// Gets the world position of the specified cell.
        /// </summary>
        /// <param name="x">The x-coordinate of the cell.</param>
        /// <param name="y">The y-coordinate of the cell.</param>
        /// <param name="gridOrigin">The origin point of the grid in the world.</param>
        /// <returns>The world position of the cell.</returns>
        public Vector3 GetCellWorldPosition(float x, float y, Vector3 gridOrigin) {
            return gridOrigin + new Vector3(x * CellSize, 0, y * CellSize);
        }
        
        /// <summary>
        /// Gizmos drawing method to visualize occupied cells.
        /// </summary>
        /// <param name="gridOrigin"></param>
        public void DrawOccupiedCellsGizmos(Vector3 gridOrigin) {
            Gizmos.color = Color.red;
            for (int x = 0; x < GridWidth; x++) {
                for (int y = 0; y < GridHeight; y++) {
                    if (IsCellOccupied(x, y)) {
                        var cellPos = GetCellWorldPosition(x, y, gridOrigin);
                        Gizmos.DrawCube(cellPos, new Vector3(CellSize, 0.1f, CellSize));
                    }
                }
            }
        }
    }
}