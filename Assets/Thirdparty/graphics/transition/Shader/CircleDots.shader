Shader "Screen Transition/Circle Dots" {
	Properties {
		[HideInInspector] _MainTex ("Base (RGB)", 2D) = "white" {}
		[HideInInspector] _PrevTex ("Previous Scene", 2D) = "white" {}
		_Progress ("Progress", Range(0, 1)) = 0
		_Size     ("Size", Float) = 20
		_Center   ("Center", Vector) = (0.5, 0.5, 0, 0)
	}
	CGINCLUDE
		#include "UnityCG.cginc"
		#include "ScreenTransition.cginc"
		float _Size;
		float4 _Center;
       	float4 frag (v2f_img i) : COLOR
       	{
			bool f = distance(frac(i.uv * _Size), float2(0.5, 0.5)) < (_Progress / distance(i.uv, _Center.xy));
			float4 c1 = SampleWithBorder(_PrevTex, i.uv, float4(0, 0, 0, 0));
			float4 c2 = tex2D(_MainTex, i.uv);
			return lerp(c1, c2, f);
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