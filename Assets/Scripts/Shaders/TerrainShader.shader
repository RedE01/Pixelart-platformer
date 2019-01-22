Shader "Custom/Terrain Sprite" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Color("Color", Color) = (1, 1, 1, 1)
	}
		SubShader{
			Tags
			 {
				 "Queue" = "Transparent"
				 "IgnoreProjector" = "True"
				 "RenderType" = "Transparent"
				 "PreviewType" = "Plane"
				 "CanUseSpriteAtlas" = "True"
			 }
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Blend One OneMinusSrcAlpha

			Pass {

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"

				sampler2D _MainTex;

				struct v2f {
					float4 pos : SV_POSITION;
					half2 uv : TEXCOORD0;
				};

				v2f vert(appdata_base v) {
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.uv = v.texcoord;
					return o;
				}

				fixed4 _Color;
				float4 _MainTex_TexelSize;

				fixed4 frag(v2f i) : COLOR
				{
					half4 c = tex2D(_MainTex, i.uv);
					c *= c.a;

					return c;
				}

				ENDCG
			}
		}
			FallBack "Diffuse"
}