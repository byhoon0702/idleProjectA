Shader "Screen Transition/Line Reveal" {
	Properties {
		[HideInInspector] _MainTex ("Base (RGB)", 2D) = "white" {}
		[HideInInspector] _PrevTex ("Previous Scene", 2D) = "white" {}
		_Progress ("Progress", Range(0, 1)) = 0
		_LineOrigin ("Line Origin", Vector) = (-0.2, -0.2, 0, 0)
		_LineNormal ("Line Normal", Vector) = (1, 1, 0, 0)
		_LineOffset ("Line Offset", Vector) = (1.4, 1.4, 0, 0)
		_FuzzyAmount ("Fuzzy Amount", Range(0, 1)) = 0.5
	}
	CGINCLUDE
		#include "UnityCG.cginc"
		#include "ScreenTransition.cginc"
		
		float2 _LineOrigin, _LineNormal, _LineOffset;
		float _FuzzyAmount;
		
       	float4 frag (v2f_img i) : COLOR
       	{
			float4 c1 = tex2D(_PrevTex, i.uv);
			float4 c2 = tex2D(_MainTex, i.uv);

			float2 origin = lerp(_LineOrigin, _LineOffset, _Progress);
			float2 nl = normalize(_LineNormal);
			float d = dot(nl, i.uv - origin);
			float p = saturate((d + _FuzzyAmount) / (2.0 * _FuzzyAmount));
			return lerp(c2, c1, p);
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