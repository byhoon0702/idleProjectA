Shader "Screen Transition/WindowSlice" {
	Properties {
		[HideInInspector] _MainTex ("Base (RGB)", 2D) = "white" {}
		[HideInInspector] _PrevTex ("Previous Scene", 2D) = "white" {}
		_Progress   ("Progress", Range(0, 1)) = 0
		_Cnt        ("Count", Float) = 10
		_Smoothness ("Smoothness", Float) = 0.5
	}
	CGINCLUDE
		#include "UnityCG.cginc"
		#include "ScreenTransition.cginc"
		float _Cnt, _Smoothness;
       	float4 frag (v2f_img i) : COLOR
       	{
			float pr = smoothstep(-_Smoothness, 0.0, i.uv.x - _Progress * (1.0 + _Smoothness));
			float s = step(pr, frac(_Cnt * i.uv.x));
			float4 c1 = SampleWithBorder(_PrevTex, i.uv, float4(0, 0, 0, 0));
			float4 c2 = tex2D(_MainTex, i.uv);
			return lerp(c1, c2, s);
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