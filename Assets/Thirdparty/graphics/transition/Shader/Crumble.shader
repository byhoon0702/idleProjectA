Shader "Screen Transition/Crumble" {
	Properties {
		[HideInInspector] _MainTex ("Base (RGB)", 2D) = "white" {}
		[HideInInspector] _PrevTex ("Previous Scene", 2D) = "white" {}
		_NoiseTex ("Noise Texture", 2D) = "white" {}
		_Progress ("Progress", Range(0, 1)) = 0
		_Density ("Density", Range(1, 20)) = 8
	}
	CGINCLUDE
		#include "UnityCG.cginc"
		#include "ScreenTransition.cginc"
		sampler2D _NoiseTex;
		float _Density;
		fixed4 frag (v2f_img i) : COLOR
       	{
			float2 offset = tex2D(_NoiseTex, float2(i.uv.x / _Density, frac(i.uv.y / _Density + 0.95))).xy * 2 - 1;
			float p = _Progress * 2;
			if (p > 1.0)
				p = 1.0 - (p - 1.0);
			float4 c1 = tex2D(_PrevTex, frac(i.uv + offset * p));
			float4 c2 = tex2D(_MainTex, frac(i.uv + offset * p));
			return lerp(c1, c2, _Progress);
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