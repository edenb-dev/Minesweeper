# Minesweeper
Android game developed in Xamarin. ( C# &amp; .Net based technology )

My take on the classic Minesweeper game.

![Minesweepers_Screens](https://user-images.githubusercontent.com/70844165/92663341-aa8f6180-f309-11ea-98da-f0e99291f4d6.jpg)

## Game

### Introduction

You are presented with a board of squares.<br>
Some squares contain mines (bombs), others don't.<br>
If you press a square containing a bomb, you lose.<br>
If you manage to press all squares ( without pressing any bombs ) you win.<br>

### How to play 

Pressing a square which doesn't have a bomb reveals the number of neighbouring squares containing bombs.<br>
Long press a square to flag it, if you think that the square contains a bomb.<br>
Long pressing a flagged square will remove the flag, and you will be able interact with it once more.<br>
When all squres have been revealed the smiley face will put his glasses indicating that you have won, once pressed you will be able to save your name and score.

#### Modes 

Easy   - 9 by 9 board with 15 bombs.<br>
Medium - 16 by 16 board with 35 bombs.<br>
Hard   - 20 by 25 board with 80 bombs.<br>
Custom - you can choose the width, height and the amount of bombs you would like to play with.

#### Features regarding the banner

* To reset the game click on the smiley face. ( the board and timer will be reset )
* To move the board, simply click on the hand icon, then click the board, and drag it to the desired place.
* To open the menu, long press on the menu button.
