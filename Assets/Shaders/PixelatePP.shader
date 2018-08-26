Shader "Hidden/PixelatePP"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Factor("Factor", Range(0.0, 1.0)) = 0
		_Resolution("Resolution", Range(1.0, 300)) = 100
	}

	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;
			uniform float _Factor;
			float _Resolution;

			fixed4 frag(v2f i) : SV_Target
			{
				float factor = 1 - _Factor;
				//float2 factorScaled = factor * _ScreenParams.xy;
				//i.uv = round(i.uv * factorScaled) / factorScaled;
				float factorScaled = factor * _Resolution;
				i.uv = lerp(i.uv, round(i.uv * factorScaled) / factorScaled, _Factor);

				fixed4 col = tex2D(_MainTex, i.uv);
				col.rgb = saturate(col.rgb * factor);
				return col;
			}
			ENDCG
		}
	}
}
