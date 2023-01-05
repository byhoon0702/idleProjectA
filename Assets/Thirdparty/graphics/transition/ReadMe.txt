Thank you for choosing us!

Package provides 32 kinds of crossfade effects for you.
All shaders are in folder "Shader", all of them are camera effect shader.

Basically say, if you want use it in your own project you just need:
  1. Copy shader(s) to your own project.
  2. Attach "ScreenTransition.cs" to your main camera.
  3. Now you will see 4 variables on inspector.
     => "m_Mat" is the camera effect material working on your main camera. Just select a crossfade shader you want to use.
     => "m_Progress" control the crossfade progress from "m_PrevTex" to "m_NextTex".
Of course there is no limit how you use the mechanism.
For example, you can capture current scene and store it to a render texture before load the next scene.
Then you set the render texture as "m_PrevTex" and the new loaded scene view as "m_NextTex", change "m_Progress" from 0 to 1 will implement "fade in new level scene". 

Demo scene show all crossfade effects. Click the play button and effects will automatic run.
Please refer it as the usage example.

If you like it, please give us a good review on asset store. Thanks so much !
Any suggestion or improvement you want, please contact qq_d_y@163.com.
We'd like to help more and more unity3d developers.