# Documentation

## Inhouds opgave

- [Documentation](#documentation)
  - [Inhouds opgave](#inhouds-opgave)
  - [Post processing](#post-processing)
  - [Angular Frontend](#angular-frontend)
  - [Scripts](#scripts)
    - [Fish generator (SetSpawner and TouchObject scripts)](#fish-generator-setspawner-and-touchobject-scripts)
    - [Unity server](#unity-server)

## Post processing

We used Post Processing to achieve the underwater look in the game. All of the information below was provided by a [tutorial](https://www.youtube.com/watch?v=JStFXTZMCv0) on youtube by the [maker](https://syntystore.com/) of the [asset pack](https://syntystore.com/products/polygon-tropical-jungle-nature-biome?_pos=20&_sid=86dccdbcf&_ss=r) that we are using in the project.

First of you should download the Post Processing package in Unity. This package includes all of the necessary files and scripts to achieve the underwater look.

**Important: the Post Processing package (v2 which is the standard installation) does not work with the Universal Render Pipeline (URP)**

Create a layer called Post that you will later assign to the Post-process volumes. Use a Post-process layer component on the camera. This will enable the camera to interact with the volumes. In the layer component, assign the layer variable to the 'Post' layer you created in the beginning. The Anti-aliasing mode should be set to FXAA.

Create 2 cube objects which will represent the surface and underwater. Name and size them accordingly. Add the Post-process volume component to both of these cubes. Disable their mesh renderers, toggle the `IsTrigger` variable in the box collider and give them the Post layer you created in the beginning.

If you haven't already, create 2 Post-process files. Give one of them a light blue color and assign them to the underwater cube. Assign the other one to the surface cube. Set the blend distance to 0.5.

Now you will have the underwater look, but you can also add some fog to improve the visual. To do this, go to Window > Rendering > Lighting. At the top go to environment and under 'Other settings', click 'Fog'. Change the `Mode` to linear and you can set the End to 150 if you want the same look as in our project. If you want to make it so the fog dissapears when you go above water, follow the tutorial on top.

## Angular Frontend

We have created an angular frontend website so that the occupational therapist can access and influence different parameters of the Unity game. For example: the speed of objects, length of the level, amount of rows and more can be changed. The level can also be switched on the site. All of these changes are accomplished using a websocket connection to communicate with the websocket on a remote server that communicates with the unity backend server script.

## Scripts

### Fish generator (SetSpawner and TouchObject scripts)

The fish generator/spawner works as follows. It is put on a gameobject and it will spawn a set/row of fish for the user to dodge. It puts the set in the middle of the object and spawns them all next to each other. With a chance of skipping over one so there's empty spots the player can dodge into. This works by generating a list of integers to exclude from the set list. Once the exclude list is made the set gets looped over and the objects get added, apart from when the index is in the exclude list.

The level length is decided by a variable and the amount of sets is also set by a variable. With a bit of math you can calculate the time between the spawning of different sets. If infinite spawn is enabled the time between sets can also be changed with a variable.

The objects that get spawned all have a touchObject script attached to them that makes them move and detects if they have collided with the Target tag. Once they collide with an object that has the Target tag the object destroys itself.

### Unity server

This script starts a websocket hub connection using signalR to connect to the remote websocket. This websocket can listen to updateParameter requests and gives these parameters to all of its listening clients. This way the occupational therapist can change the different parameters of the project.

### UpdateProgressBar

This is the largest script which handles and centralizes all of the gamelogic. It is used to store all of the different variables such as amount of times patient has hit an obstacle, amount of time patient has hit an object with their head, amount of time a patient has hit a not target fish, amount of times a patient has hit a target fish with their right and left controller and so on.

It is also necessary for showing the progress of the patient using the progress bar in the top left corner of the level. It also shows the patient how many of each fish color they still have to catch.

It has multiple methods for handling certain events.

1. MissedObject(bool IsTargetFish, string color): This method is called when a touchObject hits the target at the end of the path. If the object is a target fish, then it will use the color parameter to add a missed fish of the corresponding color. This information (Amount of missed fish for each color) gets shown at the end of the level in the stats screen.

2. HitNotTargetFish(): This method is called when a patient hit a fish which wasn't a target fish. When this happens, it will add a missed fish of a random color. This way a patient has to hit an extra fish to complete the level.

3. HitObstacle(): This method is called when a patient hits an obstacle with their fishnet or with the vr headset. Just like the previous method will it randomly add a missed fish of a random color.

4. HitObjectWithHead(): This method gets called when a patient hits a fish with their head. It will do the same as the previous 2 methods.

5. HitObject(string hand, string color): This method is called when a patient catches a target fish. It uses the hand parameter to give the corresponding hand an extra point so that it can be shown at the end of the level at the stats screen. It will also update which color of fish was hit. Afterwards it checks if the player has hit all of the necessary fish. If this is the case, the objectspawnerV2 will then stop spawning fish and all of the current objects in the scene get removed. It will then show the winning screen.

6. SetStatsScreenText(bool lost): This method gets called when the player has won or lost. It will show all of the details and information that has been collected throughout the duration of the level.

### Fishnet collision

This script is used for checking when a fishnet collides with an obstacle, fish or target fish. When it hits either of those 3, a corresponding method of the updateProgressbar script is called. It is important to exclude the post processing volumes in the collission, because it will otherwise delete these volumes and your underwater effect will dissappear.

When the fishnet collides with a fish, it will check if it is of type 'touchObject' (which is another script) and then it will check if this fish has already been hit by a fishnet. This check is necessary because otherwise there is a chance that the corresponding method gets called multiple times. This is because the fishnet has multiple colliders and if you move fast enough, most of them get triggered.

If the fish has not been hit yet and it is a target fish, it will call the HitObject() method from the updateProgressbar script. It will give the hand and the color of the target fish as parameters. All of this is necessary for the stats screen at the end of the level.

If the fish is not a target fish, the HitNotTargetFish() script will be called so that it can also be shown at the end of the level on the stats screen.

If it is an obstacle, it will call the HitObstacle() method so that it can be shown at the end of the level like the other methods.

---

### Small scripts

We wrote a few small scripts that had a single purpose.

- #### **Close pause menu**

  This script is used to open and close the pause menu were a patient can switch levels.

- #### **Close task menu**

  This script is used to close the task menu that appears at the beginning of a level. After 3 seconds, the menu dissapears again so that the level can start.

- #### **HandSelector**

  This script is used so that the physical therapist can choose which hand is used for catching the fish. He/She can choose between left, right and both hands. This script uses the XRInteractionManager to make the XRGrabInteracteable be selected when it gets activated.

- #### **Levelmanager**

  This script is used to exit the game, go to a different level, or get the active level.

- #### **MainThreadDispatcher**

  This script is from [github, made by PimDeWitte](https://github.com/PimDeWitte/UnityMainThreadDispatcher/blob/master/Runtime/UnityMainThreadDispatcher.cs). It is used to access the main thread of the game runtime. This is necessary because the unity server is running on a seperate thread where certain actions are not possible such as changing a scene, getting objects in a scene etc.

- #### **MaterialStorage**

  This scripts is used to store all of the available materials. These materials can be accessed in other scripts to apply to the necessary fish objects. This script also has a method to get the materials that are related to the fish that a patient has to touch.

- #### **Rotate Rock**

  This script is used to rotate the rock obstacle.

- #### **TaskMenu**

  This script is used to set the text of the task menu that appears at the beginning of a level. It has a link to the updateProgressBar script so that it can see how many of each fish a patient has to catch.

- #### **TouchObject**

  This script is attached to every object that gets spawned in the ObjectSpawnerV2, ObjectSpawner and SetSpawner. In this script, you can change the speed of the object, set the color that it gets spawned with, if it has gotten hit yet and if it is a target fish or not.

  This script is also used to rotate every object that gets out of the spawner because they are sometimes turned around. It also has an onTriggerEnter method were it will hit the targetPlane at the end of a level. If it hits this plane, the MissedObject() method of the updateProgressBar script is called.

- #### **VR headset Collision**

  This script is used to count and destroy all of the objects that a patient hits with the vr headset. The amount gets stored in another script where, at the end of a level, a stats menu gets shown where the patient can see how many times he/she hit an object with their head.

- #### **Xr Handler**

  This script is used to bind the close task menu script to the right controller. When a player presses a button, the menu appears/dissappears.

## Known bugs
- #### **Multiview not available in project**
  Multiview is not supported when using the post processing v2 package. [unity issue tracker](https://issuetracker.unity3d.com/issues/quest-stutter-slash-frame-pacing-with-oculusruntime-dot-waittobeginframe-when-the-player-is-built)

- #### **Build project has random stutters**
  When the project is build, it is pretty laggy/stuttery/jittery. This is because of a known issue which involves deltaTime. There is not a straightforward fix. [unity issue tracker](https://issuetracker.unity3d.com/issues/quest-stutter-slash-frame-pacing-with-oculusruntime-dot-waittobeginframe-when-the-player-is-built)
