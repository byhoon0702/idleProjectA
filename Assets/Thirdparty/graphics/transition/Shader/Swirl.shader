Shader "Screen Transition/Swirl" {
	Properties {
		[HideInInspector] _MainTex ("Base (RGB)", 2D) = "white" {}
		[HideInInspector] _PrevTex ("Previous Scene", 2D) = "white" {}
		_Progress ("Progress", Range(0, 1)) = 0
		_AmountCenter ("Amount Center", Vector) = (32, 32, 0.5, 0.5)
	}
	CGINCLUDE
		#include "UnityCG.cginc"
		#include "ScreenTransition.cginc"
		float4 _AmountCenter;
       	float4 frag (v2f_img i) : COLOR
       	{
			float2 amount = _AmountCenter.xy;
			float2 center = _AmountCenter.zw;
			float2 toUV = i.uv - center;
			float dist = length(toUV);
			float2 normToUV = toUV / dist;
			float angle = atan2(normToUV.y, normToUV.x);
	
			angle += dist * dist * amount * _Progress;
			float2 newUV;
			sincos(angle, newUV.y, newUV.x);
			newUV *= dist;
			newUV += center;
	
			float4 c1 = SampleWithBorder(_PrevTex, newUV, float4(0, 0, 0, 0));
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