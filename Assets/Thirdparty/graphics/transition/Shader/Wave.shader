Shader "Screen Transition/Wave" {
	Properties {
		[HideInInspector] _MainTex ("Base (RGB)", 2D) = "white" {}
		[HideInInspector] _PrevTex ("Previous Scene", 2D) = "white" {}
		_Progress ("Progress", Range(0, 1)) = 0
		_Amplitude ("Amplitude", Range(0, 1)) = 0.1
		_Phase ("Phase", Range(1, 32)) = 14
		_Frequence ("Frequence", Range(1, 64)) = 20
	}
	CGINCLUDE
		#include "UnityCG.cginc"
		#include "ScreenTransition.cginc"
		float _Amplitude, _Phase, _Frequence;
       	float4 frag (v2f_img i) : COLOR
       	{
			float MAG = _Amplitude;
			float PHASE = _Phase;
			float FREQ = _Frequence;
	
			float2 uv = i.uv + float2(MAG * _Progress * sin(FREQ * i.uv.y + PHASE * _Progress), 0);
			float4 c1 = SampleWithBorder(_PrevTex, uv, float4(0, 0, 0, 0));
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