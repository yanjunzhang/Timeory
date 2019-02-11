Shader "Unlit/RandomColor"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		// _ColorRamp_SampleTexture("Sample Texture", 2D)="white"{}
		_ColorRamp_Evaluation("Evaluation Position",Range(0,1))=0.5
		_IsScaning("Is Scaning",Int) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			// sampler2D _ColorRamp_SampleTexture;
			float4 _MainTex_ST;
			float _ColorRamp_Evaluation;
			int _IsScaning;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				float tmp = abs(cos(_Time*30));
				float2 positionOnRampTexture = float2(tmp,0.5);
				// fixed4 rampTextureColor = tex2D(_ColorRamp_SampleTexture,positionOnRampTexture);
				// col.rgb =col.rgb + rampTextureColor*0.01;
				fixed3 col2 = dot(col.rgb, fixed3(0.66, 0.33, 0.33));
				if(_IsScaning==1)
				{
					col.rgb=lerp(col,col2,tmp);
				}
				return col;

				// apply fog
				// UNITY_APPLY_FOG(i.fogCoord, col);
				// return col;
			}
			ENDCG
		}
	}
}
