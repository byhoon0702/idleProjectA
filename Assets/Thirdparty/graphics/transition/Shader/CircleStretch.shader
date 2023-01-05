Shader "Screen Transition/Circle Stretch" {
	Properties {
		[HideInInspector] _MainTex ("Base (RGB)", 2D) = "white" {}
		[HideInInspector] _PrevTex ("Previous Scene", 2D) = "white" {}
		_Progress ("Progress", Range(0, 1)) = 0
	}
	CGINCLUDE
		#include "UnityCG.cginc"
		#include "ScreenTransition.cginc"
		
		float DistanceFromCenterToSquareEdge (float2 dir)
		{
			dir = abs(dir);
			float dist = dir.x > dir.y ? dir.x : dir.y;
			return dist;
		}
       	fixed4 frag (v2f_img i) : COLOR
       	{
			float2 center = float2(0.5, 0.5);
			float radius = _Progress * 0.70710678;
			float2 dir = i.uv - center;
			float len = length(dir);
			float2 normDir = dir / len;
	
			if (len < radius)
			{
				float dfcte = DistanceFromCenterToSquareEdge(normDir) / 2.0;
				float2 ep = center + dfcte * normDir;
				float2 newUV = lerp(center, ep, len / min(radius, dfcte));
				return tex2D(_MainTex, newUV);
			}
			else
			{
				float dfcte = DistanceFromCenterToSquareEdge(normDir);
				float2 ep = center + dfcte * normDir;
				float2 radiusToUV = i.uv - (center + radius * normDir);
				float2 newUV = lerp(center, ep, length(radiusToUV) / (dfcte - radius));
				return tex2D(_PrevTex, newUV);
			}
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