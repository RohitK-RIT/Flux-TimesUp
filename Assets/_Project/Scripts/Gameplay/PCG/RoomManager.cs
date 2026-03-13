using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Gameplay.PCG {
    /// <summary>
    /// Manages the placement and tracking of rooms within the dungeon.
    /// </summary>
    public class RoomManager : MonoBehaviour {
        
        /// <summary>
        /// List of all rooms in the dungeon.
        /// </summary>
        public List<Room> rooms = new();
        private DungeonGenerator _dungeonGenerator;
        
        /// <summary>
        /// Initializes the RoomManager by getting references to the DungeonGenerator and CorridorManager components.
        /// </summary>
        private void Awake()
        {
            _dungeonGenerator = GetComponent<DungeonGenerator>();
        }
        
        /// <summary>
        /// Places a room at the specified position and marks the corresponding grid cells as occupied.
        /// </summary>
        /// <param name="roomPrefab">The room prefab to instantiate.</param>
        /// <param name="position">The position to place the room.</param>
        public void PlaceRoom(Room roomPrefab, Vector3 position) {
            if(roomPrefab.roomType is RoomType.Start or RoomType.Boss)
            {
                rooms.Add(roomPrefab);
                MarkRoomCellsOccupied(roomPrefab, position);
                return;
            }
            var roomInstance = Instantiate(roomPrefab, position, Quaternion.identity);
            rooms.Add(roomInstance);
            MarkRoomCellsOccupied(roomInstance, position);
        }
        
        /// <summary>
        /// Marks the grid cells occupied by the specified room.
        /// </summary>
        /// <param name="room">The room instance.</param>
        /// <param name="position">The position of the room.</param>
        private void MarkRoomCellsOccupied(Room room, Vector3 position) {
            // Adjust the starting indices based on the grid origin
            var startX = Mathf.FloorToInt((position.x - room.size.x / 2));
            var startY = Mathf.FloorToInt((position.z - room.size.z / 2));
            
            // Ensure indices are within bounds
            for (var x = startX; x < startX + room.size.x + _dungeonGenerator.GridSystem.CellSize; x+=5) {
                for (var y = startY; y <  startY + room.size.z + _dungeonGenerator.GridSystem.CellSize; y+=5) {
                    _dungeonGenerator.GridSystem.MarkCellOccupied(x, y,_dungeonGenerator.GridOrigin);
                }
            }
        }
        
        /// <summary>
        /// Finds a valid position for the specified room within the grid.
        /// </summary>
        /// <param name="room">The room to place.</param>
        /// <returns>A valid position for the room, or Vector3.zero if no valid position is found.</returns>
        public Vector3 FindValidPosition(Room room) {
            var grid = _dungeonGenerator.GridSystem;
            
            var maxX = _dungeonGenerator.GridSystem.VisitedCells.GetLength(0);
            var maxY = _dungeonGenerator.GridSystem.VisitedCells.GetLength(1);
            
            var roomWidthInCells = Mathf.CeilToInt(room.size.x / _dungeonGenerator.GridSystem.CellSize);
            var roomHeightInCells = Mathf.CeilToInt(room.size.z / _dungeonGenerator.GridSystem.CellSize);
            
            maxX -= roomWidthInCells;
            maxY -= roomHeightInCells;
            
            const int attempts = 100;
            for(var i = 0; i < attempts; i++) {
                var x = Random.Range(0, maxX);
                var y = Random.Range(0, maxY);
                if (IsAreaFree(x, y, roomWidthInCells, roomHeightInCells)) {
                    var position = grid.GetCellWorldPosition(x, y,_dungeonGenerator.GridOrigin);
                    var roomPosition = new Vector3(position.x + room.size.x / 2, position.y, position.z + room.size.z / 2);
                    return roomPosition;
                }
            }
            Debug.LogWarning("Failed to find a valid position for the room.");
            return Vector3.zero;
        }
        /// <summary>
        /// Checks if the specified area within the grid is free of occupied cells.
        /// </summary>
        /// <param name="startX">The starting X index.</param>
        /// <param name="startY">The starting Y index.</param>
        /// <param name="width">The width of the area in cells.</param>
        /// <param name="height">The height of the area in cells.</param>
        /// <returns>True if the area is free, false otherwise.</returns>
        private bool IsAreaFree(int startX, int startY, int width, int height) {
            for (var x = startX; x < startX + width + 2; x++) {
                for (var y = startY; y < startY + height + 2; y++) {
                    if (_dungeonGenerator.GridSystem.IsCellOccupied(x, y)) {
                        return false; // Found an occupied cell
                    }
                }
            }
            return true; // All cells are free
        }
        /// <summary>
        /// Finds the closest unconnected exit to the specified exit.
        /// </summary>
        /// <param name="currentExit">The current exit.</param>
        /// <returns>The closest unconnected exit, or null if none is found.</returns>
        public Exit FindClosestUnconnectedExit(Exit currentExit) {
            Exit closestExit = null;
            var closestDistance = float.MaxValue;
            
            foreach (var room in rooms) {
                if (room.Exits.Contains(currentExit)) continue; // Skip exits in the same room

                foreach (var exit in room.Exits) {
                    if (exit.isConnected) continue;

                    var distance = Vector3.Distance(currentExit.worldPosition, exit.worldPosition);
                    if (!(distance < closestDistance))
                        continue;
                    closestDistance = distance;
                    closestExit = exit;
                }
            }
            return closestExit;
        }
        
        /// <summary>
        /// Finds the closest unconnected exit to the specified exit from a list of unconnected rooms.
        /// </summary>
        /// <param name="unconnectedRooms"></param>
        /// <param name="closestExit"></param>
        /// <returns>The pair of Closest Unconnected Exit and the Room it belongs to.</returns>
        public (Exit, Room) FindClosestUnconnectedExitFromUnconnectedRooms(List<Room> unconnectedRooms, Exit closestExit)
        {
            Room closestRoom = null;
            Exit closestExitFromUnconnectedRooms = null;
            var closestDistance = float.MaxValue;
            
            foreach (var room in unconnectedRooms)
            {
                if (room.Exits.Contains(closestExit)) continue; // Skip exits in the same room
                foreach (var exit in room.Exits)
                {
                    if (exit.isConnected) continue;

                    var distance = Vector3.Distance(closestExit.worldPosition, exit.worldPosition);
                    if (!(distance < closestDistance))
                        continue;
                    closestDistance = distance;
                    closestExitFromUnconnectedRooms = exit;
                    closestRoom = room;
                }
            }

            return closestExitFromUnconnectedRooms != null ?    (closestExitFromUnconnectedRooms, closestRoom) : (null,
                null);
        }
    }
}
