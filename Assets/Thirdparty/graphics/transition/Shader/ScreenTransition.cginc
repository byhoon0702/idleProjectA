sampler2D _MainTex, _PrevTex;
float _Progress;

float4 SampleWithBorder (sampler2D tex, float2 uv, float4 border)
{
	if (any(saturate(uv) - uv))
		return border;
	else
		return tex2D(tex, uv);
}