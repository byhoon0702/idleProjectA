Shader "Screen Transition/RadialBlur" {
	Properties {
		[HideInInspector] _MainTex ("Base (RGB)", 2D) = "white" {}
		[HideInInspector] _PrevTex ("Previous Scene", 2D) = "white" {}
		_Progress ("Progress", Range(0, 1)) = 0
	}
	CGINCLUDE
		#include "UnityCG.cginc"
		#include "ScreenTransition.cginc"
       	float4 frag (v2f_img i) : COLOR
       	{
			float2 toUV = i.uv - float2(0.5, 0.5);
			float4 c1 = float4(0, 0, 0, 0);
			float s = _Progress * 0.05;
			for (int n = 0; n < 24; n++)
			{
				c1 += tex2D(_PrevTex, i.uv - toUV * s * n);
			}
			c1 /= 24;
			float4 c2 = tex2D(_MainTex, i.uv);
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