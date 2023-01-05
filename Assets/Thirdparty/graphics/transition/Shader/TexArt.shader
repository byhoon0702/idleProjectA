Shader "Screen Transition/TexArt" {   // crossfade based on black-white texture
	Properties {
		_MainTex ("Base", 2D) = "white" {}
		_PrevTex ("Previous", 2D) = "white" {}
		_MixTex ("Mix", 2D) = "white" {}
		_Progress ("Progress", Range(0, 1)) = 0
		_Threshold ("Threshold", Range(0, 1)) = 0.1
	}
	CGINCLUDE
		#include "UnityCG.cginc"
		#include "ScreenTransition.cginc"
		sampler2D _MixTex;
		float _Threshold;
       	float4 frag (v2f_img i) : COLOR
       	{
			float2 uv = i.uv;
			float4 c1 = tex2D(_MainTex, uv);
			float4 c2 = tex2D(_PrevTex, uv);
			float4 m = tex2D(_MixTex, uv);
			float r = _Progress * (1.0 + _Threshold * 2.0) - _Threshold;
			float f = saturate((m.r - r) * (1.0 / _Threshold));
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