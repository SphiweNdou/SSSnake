# Snake Game – WPF Desktop App (C#)

A fully functional Snake game built with C# and Windows Presentation Foundation (WPF). The game features smooth movement, grid-based gameplay, and real-time player input handling through arrow keys.

## 🐍 Features

- 🎮 Classic Snake gameplay
- 🎯 Collect food and grow the snake
- 💥 Collision detection with walls or self
- ⌨️ Arrow key controls
- 🧱 Grid-based rendering with XAML canvas
- 🧩 Clean modular architecture with enums and logic separation

## 🧰 Tech Stack

- **Language:** C#
- **Framework:** .NET WPF
- **UI:** XAML
- **IDE:** Visual Studio 2022

## 🛠️ Setup Instructions

### 1. Clone the Repository
```bash
git clone https://github.com/SphiweNdou/wpf-snake-game.git
cd wpf-snake-game
```

### 2. Open the Solution in Visual Studio
- Open the `SSSnake.sln` file.
- Set the `SSSnake` project as the startup project.
- Press `F5` to run the application.

## 🧩 Database Setup

> This application does **not require a database**. All game state is managed in memory during runtime for performance and simplicity.

## 🖼️ User Interface

- Built using **WPF and XAML**
- Grid rendered inside a dynamic canvas in `MainWindow.xaml`
- Visual updates triggered through game logic in `MainWindow.xaml.cs`
- Snake segments and food are drawn in real time as the snake moves

## 🎮 Controls

- **← / → / ↑ / ↓** – Move the snake
- Game ends if the snake hits the wall or itself

## 📁 Project Structure

- `Snake.cs` – Main logic for snake movement and growth
- `Direction.cs` – Enum for movement direction
- `GridCell.cs` – Class representing a cell in the playfield
- `MainWindow.xaml` – UI layout with canvas and visual components
- `MainWindow.xaml.cs` – Event handling and game loop

## 👨‍💻 Author

**Sphiwe Ndou**  
Desktop App Developer | .NET Enthusiast  
GitHub: [@SphiweNdou](https://github.com/SphiweNdou)  
LinkedIn: [linkedin.com/in/yourprofile](https://linkedin.com/in/ndivhuho-ndou-39246515a)


