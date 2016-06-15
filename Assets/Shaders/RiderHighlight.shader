Shader "Custom/RiderHighlight"
{
	Properties
	{
		_MainTex("Diffuse Tex", 2D) = "white" {}
		_Color("Tint", Color) = (0.1, 0.8, 0.3, 1.0)
		_AltColor("AltTint", Color) = (0.1, 0.8, 0.3, 1.0)
		_AnimSpeed("Animation Speed", Range(0.2, 3.0)) = 1.0

	}
		CGINCLUDE

#include "UnityCG.cginc"

		sampler2D _MainTex;
	float4 _MainTex_ST;
	float4 _Color;
	float4 _AltColor;
	float _AnimSpeed;
	sampler2D _UIScreenTex;

	struct v2f
	{
		float4 pos : SV_POSITION;
		fixed4 color : COLOR;
		float2  uv : TEXCOORD0;
		float3 normal : TEXCOORD1;
	};

	struct f2c
	{
		float4 col0 : COLOR0;
		fixed4 col1 : COLOR1;
	};

	v2f vert(appdata_full v)
	{
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.color = v.color;
		o.normal = v.normal;// * 0.5 + 0.5;

		o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

		return o;
	}

	f2c frag(v2f i)
	{
		f2c o;

		float2 remap0 = abs(i.uv.xy * 2.0f - 1.0f);
		float2 remap1 = abs(i.uv.xy * 2.0f - 1.0f);

		float timeFrac = frac(_Time.g * _AnimSpeed);

		remap0 = ceil(remap0 - (0.2f + timeFrac *0.8f));
		remap1 = ceil(remap1 - (0.3f + timeFrac *0.8f));


		float val = max(1.0f - max(remap0.x, remap0.y), max(remap1.x, remap1.y)) * (_Time.g * 2.0f);

		o.col0 = lerp(_Color, _AltColor, val);
		o.col0.a *= 1.0f - timeFrac;
		//val = i.color.a;
		return o;
	}

	ENDCG


		SubShader
	{
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
		LOD 200
		Blend SrcAlpha One
		ZWrite Off

		Pass
	{
		CGPROGRAM
#pragma glsl
#pragma vertex vert
#pragma fragment frag
		ENDCG
	}


	}

		Fallback off
}
