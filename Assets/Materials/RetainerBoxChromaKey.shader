Shader "Custom/RetainerBoxChromaKey"
{
    Properties {
        [PerRendererData] _MainTex("MainTex", 2D) = "white" {}
        _ChromaColor ("ChromaColor", Color) = (0,1,0,1)
        _ScanForSemiTransparentColor("ScanForSemiTransparentColor", float) = 0
        _SemiTransparentColor("SemiTransparentColor", Color) = (0,0,0,0)
        _SemiTransparentReplaceColor("SemiTransparentReplaceColor", Color) = (0,0,0,0)
    }
    SubShader {
        Blend SrcAlpha OneMinusSrcAlpha // Alpha blending
        Cull Off ZWrite Off ZTest Off

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            fixed4 _ChromaColor;
            fixed _ScanForSemiTransparentColor;
            fixed4 _SemiTransparentColor;
            fixed4 _SemiTransparentReplaceColor;

            struct appdata {
                float4 vertex:POSITION;
                float2 uv:TEXCOORD0;
            };

            struct v2f {
                float2 uv:TEXCOORD0;
                float4 vertex:SV_POSITION;
            };

            v2f vert(appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                if (_ScanForSemiTransparentColor > 0)
                {
                    col = distance(col, _SemiTransparentColor) < 0.7 ? _SemiTransparentReplaceColor : col;
                }
                col = distance(tex2D(_MainTex, i.uv), _ChromaColor) < 0.0001 ? fixed4(0, 0, 0, 0) : col;
                return col;
            }
            ENDCG
        }
    }
}