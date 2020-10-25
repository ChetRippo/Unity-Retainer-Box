# Unity-Retainer-Box
An optimization-focused canvas element for Unity uGUI. Based on Unreal's UMG Retainer Box. Contents are rendered to a texture and swapped out for it, saving draw calls in exchange for texture memory.

# Description
Optimizing complex UIs can be difficult, especially on mobile hardware where draw calls and complex shaders quickly become too expensive to render every frame. Instead of sacrifing time and visual quality to simplify them, a retainer box can be a better solution. A simple, bareboness implementation of UE4's UMG Retainer Box, this element will render itself on demand to a texture, then disable its hierarchy and instead enable a UI Image with the rendered texture in its place, resulting in only 1 draw call.

# Setup
Provided is an example scene with a simple text retainer box. The general setup is as follows:
* Create a new camera and canvas, with the canvas set to the same scaling properties as the canvas that owns the objects you want to render to texture.
* Set the new canvas to Screen space - camera, pointed at the new camera, give the new camera negative depth and a solid clear color with a color of your choice
* Add a RetainerBoxCapture component to the camera, pointed at the new canvas.
* Copy the objects you want to render onto the new canvas. You'll likely want to prefab these objects so you don't have to modify them twice from this point on.
* Add a Raw Image with the same size and position as the content you want to render to the "real" canvas- this will be rendered instead of the actual content. Set its material to the RetainerBoxChromaKey material, with its color set to the color you used on the camera earlier.
* Add a RetainerBox component to your content parent, with TargetCapture set to the new camera, and Img set to the Raw Image you just added. If Auto Enable is checked, on start it should swap the UI content for the Raw Image instead.

# Implementation Notes
* If the UI in question is not always static and occasionally needs to change, the retainer box can be simply disabled by calling RetainerBox.DisableImage() and re-enabling the content. When ready to re-enable it, call EnableImage().
* Whenever the game's resolution is changed, Rerender() must be called on all RetainerBoxCaptures, or else they may have the wrong size. Easiest way to implement this is to broadcast an event on resolution change, and have RetainerBoxCaptures listen for this event on start.
* The RetainerBoxCapture assumes all involved canvases have a CanvasScalar with referenceResolution set to 1920 x 1080. This logic can be tweaked if necessary.
* Only 16:9, 16:10, 3:2, and 21:9 are supported. Other ratios can be added in RetainerBoxCapture.GetCanvasTargetRes().
