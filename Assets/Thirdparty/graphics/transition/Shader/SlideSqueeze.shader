Shader "Screen Transition/SlideSqueeze" {
	Properties {
		_MainTex ("Base", 2D) = "white" {}
		_PrevTex ("Previous", 2D) = "white" {}
		_Progress ("Progress", Range(0, 1)) = 0
	}
	CGINCLUDE
		#include "UnityCG.cginc"
		#include "ScreenTransition.cginc"
       	float4 frag (v2f_img i) : COLOR
       	{
			float2 uv;
			float4 c;
			uv.y = i.uv.y;
			if (_Progress < i.uv.x)
			{
				uv.x = (1.0 / (1.0 - _Progress)) * (i.uv.x - _Progress);
				c = tex2D(_PrevTex, uv);
			}
			else
			{
				uv.x = saturate((1.0 / _Progress) * i.uv.x);
				c = tex2D(_MainTex, uv);
			}
			return c;
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