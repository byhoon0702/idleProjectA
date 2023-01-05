Shader "Screen Transition/Dissolve" {
	Properties {
		[HideInInspector] _MainTex ("Base (RGB)", 2D) = "white" {}
		[HideInInspector] _PrevTex ("Previous Scene", 2D) = "white" {}
		_NoiseTex ("Noise Texture", 2D) = "white" {}
		_Progress ("Progress", Range(0, 1)) = 0
	}
	CGINCLUDE
		#include "UnityCG.cginc"
		#include "ScreenTransition.cginc"
		sampler2D _NoiseTex;
       	float4 frag (v2f_img i) : COLOR
       	{
			float noise = tex2D(_NoiseTex, i.uv).x;
			if (noise > _Progress)
				return tex2D(_PrevTex, i.uv);
			else
				return tex2D(_MainTex, i.uv);
       	}
	ENDCG
	SubShader {
		ZTest Off Cull Off ZWrite Off Blend Off
	  	Fog { Mode off }
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			ENDCG
		}
	}
	FallBack Off
}