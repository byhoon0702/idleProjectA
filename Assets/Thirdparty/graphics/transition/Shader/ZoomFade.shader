Shader "Screen Transition/Zoom Fade" {
	Properties {
		[HideInInspector] _MainTex ("Base (RGB)", 2D) = "white" {}
		[HideInInspector] _PrevTex ("Previous Scene", 2D) = "white" {}
		_Progress  ("Progress", Range(0, 1)) = 0
		_ZoomQuick ("Zoom Quick", Range(0.2, 1.0)) = 0.8
	}
	CGINCLUDE
		#include "UnityCG.cginc"
		#include "ScreenTransition.cginc"
		float _ZoomQuick;
       	float4 frag (v2f_img i) : COLOR
       	{
			float amount = smoothstep(0.0, _ZoomQuick, _Progress);
			float2 uv = 0.5 + ((i.uv - 0.5) * (1.0 - amount));
			float4 c1 = SampleWithBorder(_PrevTex, uv, float4(0, 0, 0, 0));
			float4 c2 = tex2D(_MainTex, i.uv);
			return lerp(c1, c2, smoothstep(_ZoomQuick - 0.2, 1.0, _Progress));
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