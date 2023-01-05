Shader "Screen Transition/Directional Warp" {
	Properties {
		[HideInInspector] _MainTex ("Base (RGB)", 2D) = "white" {}
		[HideInInspector] _PrevTex ("Previous Scene", 2D) = "white" {}
		_Progress   ("Progress", Range(0, 1)) = 0
		_Smoothness ("Smoothness", Float) = 0.5
		_Direction  ("Direction", Vector) = (-1, 1, 0, 0)
		_Center     ("Center", Vector) = (0.5, 0.5, 0, 0)
	}
	CGINCLUDE
		#include "UnityCG.cginc"
		#include "ScreenTransition.cginc"
		float _Smoothness;
		float2 _Direction, _Center;
       	float4 frag (v2f_img i) : COLOR
       	{
			float2 uv = i.uv;
			float2 v = normalize(_Direction);
			v /= abs(v.x) + abs(v.y);
			float d = v.x * _Center.x + v.y * _Center.y;
			float m = 1.0 - smoothstep(-_Smoothness, 0.0, v.x * uv.x + v.y * uv.y - (d - 0.5 + _Progress * (1.0 + _Smoothness)));
			float4 c1 = SampleWithBorder(_PrevTex, (uv - 0.5) * (1.0 - m) + 0.5, float4(0, 0, 0, 0));
			float4 c2 = tex2D(_MainTex, (uv - 0.5) * m + 0.5);
			return lerp(c1, c2, m);
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