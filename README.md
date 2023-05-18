# King Of The Crown (KOTC): Multiplayer Mario Game

King Of The Crown is a reimagined version of the classic Mario game, transformed into a fun and exciting multiplayer experience! Players vie for control of the crown, in a game coded in C#, using the XNA and Monogame framework.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

- Visual Studio
- .NET Framework
- MonoGame Framework
- XNA Game Studio

### Installing

1. Clone the repository
    ```
    git clone https://github.com/Yannn001/King-Of-The-Crown-KOTC-Multiplayer-Mario-Game.git
    ```
2. Open the solution file (Sprint1.sln) with Visual Studio
3. Resolve any dependencies if necessary (mostly related to MonoGame and XNA)
4. Build and run the project.

## Project Structure

Below is a brief overview of the important directories in the project:

- **Audio:** Contains AudioManager.cs for managing audio files and sound effects.

- **Collisions:** Classes for managing object collisions in the game.

- **Commands:** Contains all commands related to gameplay, such as movement, interaction, and game states.

- **Content:** Contains the various graphical and audio resources used in the game.

- **Controllers:** Contains the different controller classes for managing user input.

- **Entities:** Contains the classes representing various objects/entities in the game like blocks, enemies, player etc.

- **Factories:** Classes for creating game entities and sprites are stored here.

- **Physics:** For all physics-related logic.

- **Sprites:** Graphical representation of game entities.

- **States:** For managing different game states.

- **Transformations:** All code related to game transformations should be put here.

- **Trackers:** Code that handles tracking of game metrics (scores, game progress, etc) can be found here.

- **Game1.cs:** Entry point of the game where all game components are initialized.

- **Program.cs:** The main entry point for the application.

## Contribution

All contributions are welcome! If you want to contribute, please follow these steps:

1. Fork the project.
2. Create your Feature branch (`git checkout -b feature/AmazingFeature`).
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`).
4. Push to the branch (`git push origin feature/AmazingFeature`).
5. Open a Pull Request.

## License

This project is licensed under the MIT License.

## Acknowledgements

- XNA Game Studio
- MonoGame Framework

This README is a work in progress and will be updated as the project evolves. If you have any questions or need further clarification, please don't hesitate to ask.
