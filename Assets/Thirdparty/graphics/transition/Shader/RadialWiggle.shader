Shader "Screen Transition/Radial Wiggle" {
	Properties {
		[HideInInspector] _MainTex ("Base (RGB)", 2D) = "white" {}
		[HideInInspector] _PrevTex ("Previous Scene", 2D) = "white" {}
		_NoiseTex ("Noise Texture", 2D) = "white" {}
		_Progress ("Progress", Range(0, 1)) = 0
		_Seed ("Seed", Range(0, 1)) = 0.5
	}
	CGINCLUDE
		#include "UnityCG.cginc"
		#include "ScreenTransition.cginc"

		sampler2D _NoiseTex;
		float _Seed;

		float4 frag (v2f_img i) : COLOR
		{
			float2 center = float2(0.5,0.5);
			float2 dir = i.uv - center;
			float dist = length(dir);

			float2 normDir = dir / dist;
			float angle = (atan2(normDir.y, normDir.x) + 3.141592) / (2.0 * 3.141592);
			float offset1 = tex2D(_NoiseTex, float2(angle, frac(_Progress / 3 + dist / 5 + _Seed))).x * 2.0 - 1.0;
			float offset2 = offset1 * 2.0 * min(0.3, (1 - _Progress)) * dist;
			offset1 = offset1 * 2.0 * min(0.3, _Progress) * dist;

			float4 c1 = tex2D(_PrevTex, frac(center + normDir * (dist + offset1))); 
			float4 c2 = tex2D(_MainTex, frac(center + normDir * (dist + offset2)));

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