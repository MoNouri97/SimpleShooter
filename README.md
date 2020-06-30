<h1 align="center"> 🎮 Top-Down Shooter</h1>
<div align="center">

`Unity3d and C#`

</div>

---

<p align="center"> A simple top down shooter with level generation and power-ups
    <br> 
</p>

## 📝 Table of Contents

- [About](#about)
- [Interesting Parts](#interesting)
- [TODO](#todo)
- [gameplay](#gameplay)
- [Built Using](#built_using)
- [Authors](#authors)
- [Acknowledgments](#acknowledgement)

## 🧐 About <a name = "about"></a>

this game was developed as a project to test my unity and c# knowledge (and gamedev is fun 😄)

## 🏁 Interesting Parts <a name = "interesting"></a>

- **Map Generation** : this was done using a **[Flood fill ](https://en.wikipedia.org/wiki/Flood_fill) algorithm** to insure that the randomly placed objects doesn't render parts of the map unreachable
- **Enemy Spawner** : the enemies spawn in each level at constant rate selecting each time a random tile that doesn't contain an obstacle **however** when the player stays still for a while the enemies will spawn at the same tile as the player to encourage fast (and fun) gameplay
- **Pickups** : these are some power-ups to introduce some variety.There is 3 types :
  - health
  - speed
  - attack (**grenades**)
- **Shooting**
  - **RayCasting** : a technique used for shooting consists of drawing a ray from the player's gun in the direction of the mouse cursor and if this ray intersects with a _damageable_ entity then it takes damage
  - **RigidBodies** : creating rigid body for each bullet and using unity's physics to simulate a shot, when the bullet hits a _damageable_ entity then it takes damage
  - ⏩ At the end i used rigidbody approach to get better bullet interactions and ricochet

## 📌 TODO <a name="todo"></a>

- infinite level generation
- level ( map and difficulty) editor
- better UI
- models and textures

## 🚀 Gameplay <a name = "gameplay"></a>

- **Movement** WASD
- **Shooting** Mouse1
- **Grenades** G
- **DevMode(Skip Level)** mouse2

## ⛏️ Built Using <a name = "built_using"></a>

- [Unity3d](https://unity.com/) - GameEngine ( using **C#**)
- [Gimp](https://www.gimp.org/) - Textures
- [Audacity](https://www.audacityteam.org/) - Audio

## ✍️ Authors <a name = "authors"></a>

- [@Monouri97](https://github.com/monouri97) - coding and visuals

See also the list of [contributors](https://github.com/kylelobo/The-Documentation-Compendium/contributors) who participated in this project.

## 🎉 Acknowledgements <a name = "acknowledgement"></a>

- Special thanks to Sebastian Lague for his 'Introduction to game-development' and 'Create a Game' Courses and the inspiration and the idea behind this game
