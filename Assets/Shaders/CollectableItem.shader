Shader "Custom/CollectableItem" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_HighLightFac ("HighLight Factor", Range(0,1))  = 0.0
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}

	SubShader {
		Pass {
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			sampler2D _MainTex;
			half _Glossiness;
			half _Metallic;
			fixed4 _Color;
			uniform float _HighLightFac;

			struct vs_in
			{
				float4 pos : POSITION;
				float2 tex : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct vs_out
			{
				float4 pos : POSITION;
				float2 tex : TEXCOORD0;
				float4 worldtex : TEXCOORD1;
				float3 normal : NORMAL;
			};

			vs_out vert(vs_in IN)
			{
				vs_out OUT;
				OUT.pos = UnityObjectToClipPos(IN.pos);
				OUT.normal = normalize(mul(IN.normal, UNITY_MATRIX_M));
				OUT.tex = IN.tex;
				OUT.worldtex = OUT.pos;// mul(UNITY_MATRIX_M, IN.pos);
				return OUT;
			}

			#define PI 3.141592653589793238462
			float4 frag(vs_out IN) : COLOR
			{
				float4 colortex = tex2D(_MainTex, IN.tex);
				float diffuse = 0.7 +1.1*saturate(dot(_WorldSpaceLightPos0, IN.normal) * 4);

				//calculate highlight with screen UVs
				float timer = _Time.y * 0.3;
				float2 screenUV = IN.worldtex.xy / IN.worldtex.w;
				float offsety = frac(timer) * 2; //screen UV goes -1..1
				float txy = screenUV.y - offsety;
				float show = sign(saturate(sin(txy * PI)));
				float highlight = show * saturate(sin(txy * 2 * PI));
				highlight *= highlight * 0.5 * _HighLightFac;

				float4 color;
				color = saturate(colortex * diffuse * _Color * unity_AmbientSky +highlight);
				// Metallic and smoothness come from slider variables
				//o.Metallic = _Metallic;
				//o.Smoothness = _Glossiness;
				return color;
			}
			ENDCG
		}
	}
}
