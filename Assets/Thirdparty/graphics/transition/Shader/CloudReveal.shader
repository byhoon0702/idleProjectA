Shader "Screen Transition/CloudReveal" {
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
			float n = tex2D(_NoiseTex, i.uv).r;
			float4 c1 = tex2D(_PrevTex, i.uv);
			float4 c2 = tex2D(_MainTex, i.uv);
			float a;
			if (_Progress < 0.5)
				a = lerp(0, n, _Progress / 0.5);
			else
				a = lerp(n, 1, (_Progress - 0.5) / 0.5);
			return (a < 0.5) ? c1 : c2;
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