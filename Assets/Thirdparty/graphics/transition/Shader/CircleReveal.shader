Shader "Screen Transition/Circle Reveal" {
	Properties {
		[HideInInspector] _MainTex ("Base (RGB)", 2D) = "white" {}
		[HideInInspector] _PrevTex ("Previous Scene", 2D) = "white" {}
		_Progress ("Progress", Range(0, 1)) = 0
		_FuzzyAmount ("Fuzzy Amount", Range(0, 1)) = 0.5
	}
	CGINCLUDE
		#include "UnityCG.cginc"
		#include "ScreenTransition.cginc"
		float _FuzzyAmount;
		fixed4 frag (v2f_img i) : COLOR
		{
			float radius = -_FuzzyAmount + _Progress * (0.70710678 + 2.0 * _FuzzyAmount);
			float len = length(i.uv - float2(0.5, 0.5));
			float lfc = len - radius;
	
			float4 c1 = tex2D(_PrevTex, i.uv);
			float4 c2 = tex2D(_MainTex, i.uv);
    
			float p = saturate((lfc + _FuzzyAmount) / (2.0 * _FuzzyAmount));
			return lerp(c2, c1, p);
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