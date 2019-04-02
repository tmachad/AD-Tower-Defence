# Yet Another Tower Defense Game
### How to Play
Blobs are coming to steal your star, build towers and upgrade them to fend them off! When you start the game you will have access to 5 identical tower templates that you can build towers from. Every 5 waves you'll unlock a powerful upgrade to improve your towers with. Combine upgrades in different ways to create the most powerful combinations you can find, and try to last as long as possible.

### Building Towers
To build a tower, open the towers menu by clicking on the tab in the top right corner of the screen labelled "Towers", then click the tower template you want to place, then click the place you want to build it. If you decide you don't want to build a tower after clicking one of the templates, press Escape to deselect the template. If you click to place a tower on a non-buildable tile, a blocked tile, or if you don't have enough money for it, an error message will appear. Want to get rid of an already built tower? Click on the tower, then click the "Sell" button in the tower info panel that appears in the bottom left corner of the screen, and you'll remove the tower while recovering half of its cost.

### Upgrading Towers
Every 5 waves you will unlock a new upgrade to use; a message will appear in the center of the screen telling you when a new upgrade is unlocked and its name.
To upgrade towers, open the tower menu and drag the upgrade you want to add onto one of the three upgrade slots on the tower template you want to upgrade. You can only have one copy of each upgrade on a given template. This won't upgrade already built towers though: to upgrade already built towers to match their template click on the tower, then on the "Upgrade" button in the tower info panel that appears in the bottom left corner of the screen. Upgrades aren't free though, you'll have to pay for any new upgrades being added to the tower.

### Waves
After you choose a level, the first wave will wait to start until you click the "Next Wave" button, so take your time familiarizing yourself with the level and building your defenses. However, once you start the first wave each subsequent wave will start a short time after the previous one has finished appearing, although you can speed this up by clicking "Next Wave" to skip the brief waiting time. The waves get longer and longer as time goes on, and the enemies get stronger as well, so watch out!

### Game Over
Once 10 blobs have made it to the end of the level, you lose! Don't worry though, if you made it further than anyone else has on that level, the wave you reached will be saved for all to see on the main menu. And besides, you can always try again, although you will have to start gathering your upgrades all over again.

### Moving the Camera
Need to adjust your view? Use the mousewheel to zoom the camera in and out, and the arrow keys (or WASD) to pan the camera around.

## Upgrades
There are 6 different upgrades available in the game, and you can put any combination of 3 onto a single tower template. They are:
1. **Minigun**: Grants bonus attack speed to the tower that increases each time the tower attacks. The bonus decays over time though, so if the tower doesn't get to attack very often it will lose the bonus.
2. **Aura Attack**: Instead of firing a homing projectile, the tower instantly hits all enemies in range when it attacks, but its damage is reduced by 30%.
3. **Explosive**: Every time the tower hits an enemy it creates 1-tile radius explosion that deals 25% of the original attack's damage.
4. **Laser Beam**: Instead of firing a homing projectile, the tower fires a laser that hits every enemy between it and the target.
5. **Overcharger**: While not attacking, the tower gains bonus damage without limit. All of this bonus damage is unleashed in its next attack, after which the bonus resets to 0.
6. **Sniper**: Doubles the tower's damage and range, but halves its firerate.

Upgrades are applied to the tower from left to right, and in some combinations the order of upgrades may matter. To reorder upgrades, use the same method you would use to apply them in the first place. Don't worry, you'll only be charged money to upgrade your already built towers if you add new upgrades, simply moving them around will cost nothing.

## Enemies
There are 4 different varieties of blob to watch out for, each with varying stats.
1. **Normal**: These are your standard blobs, and can be recognized by their green colour. They have moderate health and speed, and grant a modest bounty when killed. As time goes on, their health will increase a bit each round.
2. **Tank**: These are much tougher blobs, and are dark red in colour. They have high health and lower speed, and grant a bit more money when killed. Their health scales with time, although it grows faster than that of a normal blob.
3. **Speedy**: These are weak but fast, and are sky blue in colour. They have very low health and higher speed, and grant a very small amount of money when killed. Their health and speed both grow in later waves.
4. **VIP**: Normal blobs with important jobs, these blobs are royal purple in colour. They have similar health and speed to normal blobs, but grant a much higher bounty when killed. Their health increases in later waves, but much faster than normal blobs.

## Tips for Testing
 - Don' want to wait 30 waves to unlock all 6 upgrades? Go to the "Game Manager" object in the scene, find the "Tower Manager" component, and check the "Unlock All" value (third from the bottom) to start the game will all upgrades unlocked.
 - Need more money or lives? Go to the "Game Manager" object in the scene, find the "Game Manager" component, and then change the "Starting Money" and "Starting Lives" values.