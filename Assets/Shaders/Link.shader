Shader "Custom/Link"
{
	Properties
	{
		_MainTex("Diffuse Tex", 2D) = "white" {}
		_Color("Tint", Color) = (0.1, 0.8, 0.3, 1.0)
		_AltColor("AltTint", Color) = (0.1, 0.8, 0.3, 1.0)

	}
		CGINCLUDE

#include "UnityCG.cginc"

	sampler2D _MainTex;
	float4 _MainTex_ST;
	float4 _Color;
	float4 _AltColor;
	float _LightBlur;
	float _Scale;
	float _selected;
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
		float2 uv = i.uv;
		uv.x *= _Scale;
		uv.x -= _Time.g;

		float4 tex = tex2D(_MainTex, uv);

		o.col0 = tex;
		o.col0.rgb *= _Color.rgb;
		o.col0.a *= (0.3f + (_selected * 0.4f));
		return o;
	}

	ENDCG


		SubShader
	{
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent+2" }
		LOD 200
		Blend SrcAlpha One
		ZWrite Off
		ZTest LEqual

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
