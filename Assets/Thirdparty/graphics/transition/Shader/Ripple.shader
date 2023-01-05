Shader "Screen Transition/Ripple" {
	Properties {
		[HideInInspector] _MainTex ("Base (RGB)", 2D) = "white" {}
		[HideInInspector] _PrevTex ("Previous Scene", 2D) = "white" {}
		_Progress ("Progress", Range(0, 1)) = 0
		_Frequency ("Frequency", Range(8, 32)) = 20
		_Speed ("Speed", Range(2, 16)) = 10
		_Amplitude ("Amplitude", Range(0, 1)) = 0.1
	}
	CGINCLUDE
		#include "UnityCG.cginc"
		#include "ScreenTransition.cginc"
		float _Frequency, _Speed, _Amplitude;
       	float4 frag (v2f_img i) : COLOR
       	{
			float2 center = float2(0.5, 0.5);

			float2 toUV = i.uv - center;
			float dsf = length(toUV);
			float2 normToUV = toUV / dsf;

			float wave = cos(_Frequency * dsf - _Speed * _Progress);
			float offset1 = _Progress * wave * _Amplitude;
			float offset2 = (1 - _Progress) * wave * _Amplitude;
	
			float2 newUV1 = center + normToUV * (dsf + offset1);
			float2 newUV2 = center + normToUV * (dsf + offset2);
	
			float4 c1 = tex2D(_PrevTex, newUV1); 
			float4 c2 = tex2D(_MainTex, newUV2);
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