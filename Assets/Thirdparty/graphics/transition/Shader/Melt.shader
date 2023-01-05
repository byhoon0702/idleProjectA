Shader "Screen Transition/Melt" {
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
       	fixed4 frag (v2f_img i) : COLOR
       	{
			float offset = min(_Progress + _Progress * tex2D(_NoiseTex, float2(i.uv.x, 0.3)).r, 1);
			float2 uv = i.uv;
			uv.y -= offset;
			if (uv.y > 0.0)
				return tex2D(_PrevTex, i.uv);
			else
				return tex2D(_MainTex, frac(uv));
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