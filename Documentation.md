# Documentation

## Inhouds opgave

- [Post Processing](#post-processing)
- [Scripts](#scripts)
  - [Fish generator](#fish-generator)
  - [Submarine movement](#submarine-movement)

## Post processing

We used Post Processing to achieve the underwater look in the game. All of the information below was provided by a [tutorial](https://www.youtube.com/watch?v=JStFXTZMCv0) on youtube by the [maker](https://syntystore.com/) of the [asset pack](https://syntystore.com/products/polygon-tropical-jungle-nature-biome?_pos=20&_sid=86dccdbcf&_ss=r) that we are using in the project.

First of you should download the Post Processing package in Unity. This package includes all of the necessary files and scripts to achieve the underwater look.

**Important: the Post Processing package (v2 which is the standard installation) does not work with the Universal Render Pipeline (URP)**

Create a layer called Post that you will later assign to the Post-process volumes. Use a Post-process layer component on the camera. This will enable the camera to interact with the volumes. In the layer component, assign the layer variable to the 'Post' layer you created in the beginning. The Anti-aliasing mode should be set to FXAA.

Create 2 cube objects which will represent the surface and underwater. Name and size them accordingly. Add the Post-process volume component to both of these cubes. Disable their mesh renderers, toggle the `IsTrigger` variable in the box collider and give them the Post layer you created in the beginning.

If you haven't already, create 2 Post-process files. Give one of them a light blue color and assign them to the underwater cube. Assign the other one to the surface cube. Set the blend distance to 0.5.

Now you will have the underwater look, but you can also add some fog to improve the visual. To do this, go to Window > Rendering > Lighting. At the top go to environment and under 'Other settings', click 'Fog'. Change the `Mode` to linear and you can set the End to 150 if you want the same look as in our project. If you want to make it so the fog dissapears when you go above water, follow the tutorial on top.

## Scripts

### Fish generator

### Submarine movement
