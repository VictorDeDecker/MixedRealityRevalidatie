# Documentation

## Inhouds opgave

- [Post Processing](#post-processing)
- [Angular Frontend](#angular-frontend)
- [Scripts](#scripts)
  - [Fish generator](#fish-generator-setspawner-and-touchobject-scripts)
  - [Submarine movement](#unity-server)

## Post processing

We used Post Processing to achieve the underwater look in the game. All of the information below was provided by a [tutorial](https://www.youtube.com/watch?v=JStFXTZMCv0) on youtube by the [maker](https://syntystore.com/) of the [asset pack](https://syntystore.com/products/polygon-tropical-jungle-nature-biome?_pos=20&_sid=86dccdbcf&_ss=r) that we are using in the project.

First of you should download the Post Processing package in Unity. This package includes all of the necessary files and scripts to achieve the underwater look.

**Important: the Post Processing package (v2 which is the standard installation) does not work with the Universal Render Pipeline (URP)**

Create a layer called Post that you will later assign to the Post-process volumes. Use a Post-process layer component on the camera. This will enable the camera to interact with the volumes. In the layer component, assign the layer variable to the 'Post' layer you created in the beginning. The Anti-aliasing mode should be set to FXAA.

Create 2 cube objects which will represent the surface and underwater. Name and size them accordingly. Add the Post-process volume component to both of these cubes. Disable their mesh renderers, toggle the `IsTrigger` variable in the box collider and give them the Post layer you created in the beginning.

If you haven't already, create 2 Post-process files. Give one of them a light blue color and assign them to the underwater cube. Assign the other one to the surface cube. Set the blend distance to 0.5.

Now you will have the underwater look, but you can also add some fog to improve the visual. To do this, go to Window > Rendering > Lighting. At the top go to environment and under 'Other settings', click 'Fog'. Change the `Mode` to linear and you can set the End to 150 if you want the same look as in our project. If you want to make it so the fog dissapears when you go above water, follow the tutorial on top.

## Angular Frontend

We have created an angular frontend website so that the occupational therapist can access and influence different parameters of the Unity game. For example: the speed of objects, length of the level, amount of rows and more can be changed. The level can also be switch on the site. All of these changes are accomplished using http requests to communicate with the unity server.

## Scripts

### Fish generator (SetSpawner and TouchObject scripts)

The fish generator/spawner works as follows. It is put on a gameobject and it will spawn a set/row of fish for the user to dodge. It puts the set in the middle of the object and spawns them all next to each other. With a chance of skipping over one so there's empty spots the player can dodge into. This works by generating a list of integers to exclude from the set list. Once the exclude list is made the set gets looped over and the objects get added, apart from when the index is in the exclude list.

The level length is decided by a variable and the amount of sets is also set by a variable. With a bit of math you can calculate the time between the spawning of different sets. If infinite spawn is enabled the time between sets can also be changed with a variable.

The objects that get spawned all have a touchObject script attached to them that makes them move and detects if they have collided with the Target tag. Once they collide with an object that has the Target tag the object destroys itself.

### Unity server

This script creates and starts a unity server on a seperate thread. This server can be accessed by our Angular frontend with http requests so that the occupational therapist can change the different parameters of the Setspawner and touchObject scripts. While the application is running.
