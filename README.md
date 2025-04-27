Overview
This Unity project implements a grid-based tray movement system, where trays can be dragged and snapped to grid positions. The system also supports selecting trays, making them kinematic, and disabling gravity when selected, while preventing other trays from moving or passing through each other. The trays will snap to the nearest grid point once they stop moving.

Features
Grid-Based Movement: Trays automatically snap to a grid when they stop moving.

Selectable Trays: Trays can be selected, making them kinematic and disabling gravity.

Tray Interaction: Other trays stay still when one tray is selected, preventing collisions or movement.

Grid Manager: A grid manager script that defines the grid's layout and provides snapping functionality.

Rigidbody Integration: Trays use Rigidbody for physics-based movement and interaction.

Getting Started
Follow these steps to get the project up and running:

Prerequisites
Unity 2020.3 or higher.

Basic understanding of Unity's physics system and Rigidbody components.

Setup
Download the Project: Download the project folder and open it in Unity.

Scene Setup: The grid and tray setup should be handled automatically through the GridManager and TrayMover scripts. Ensure you have a floor with a Renderer attached to it in the scene to enable grid snapping.

Assigning Tray to Grid: Each tray object should have the TrayMover script attached. The trays will automatically snap to the grid defined in GridManager.

Key Scripts
GridManager.cs: Manages the grid's layout and provides functionality to snap trays to the nearest grid point.

Public Variables:

floorTransform: The floor object to which the grid is aligned.

rows and columns: Defines the number of rows and columns for the grid.

Methods:

GetNearestGridPoint(Vector3 worldPosition): Returns the nearest grid point based on the world position.

SnapAllTraysToGrid(): Snaps all trays to the grid.

IsWithinGrid(Vector3 worldPosition): Checks if a position is within the grid boundaries.

TrayMover.cs: Handles tray movement and selection.

Public Methods:

SetSelected(bool selected): Selects or deselects the tray, changing its physics behavior (kinematic and gravity).

Private Methods:

SnapToGrid(): Snaps the tray to the nearest grid point when it stops moving.

DisableOtherTraysPhysics(): Disables physics on other trays to prevent movement.

EnableOtherTraysPhysics(): Re-enables physics on all trays when the selected tray is deselected.

Usage
Selecting a Tray: You can call SetSelected(true) to make a tray kinematic and disable its gravity, preventing it from interacting with other trays. Other trays will remain stationary.

Deselecting a Tray: Call SetSelected(false) to re-enable physics for the tray, allowing it to move and interact with other trays again.

Example:

csharp
Copy
Edit
// Select a tray
trayMoverInstance.SetSelected(true);

// Deselect the tray
trayMoverInstance.SetSelected(false);
Snapping Trays to the Grid: Trays automatically snap to the grid once they stop moving, based on the nearest grid point.

Notes
The GridManager expects the floor object to have a Renderer component attached. The grid will be calculated based on the floor's size.

Physics interactions (collisions) are handled by Unity’s Rigidbody system. Other trays are prevented from moving when one tray is selected to avoid overlapping or passing through each other.

Example Scene
The example scene contains a grid layout with trays that can be moved around.

Trays will automatically snap to the nearest grid position when they stop moving.

You can select and deselect trays to see the kinematic and gravity behaviors in action.

Contributing
If you'd like to contribute to this project, feel free to fork the repository, make changes, and submit a pull request. Any improvements or bug fixes are welcome
