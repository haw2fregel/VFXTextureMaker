Shader "GUI/PreviewTexture"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DrawChannel ("DrawChannel", Vector) = (1,1,1,1)
    }
    SubShader
    {

        Pass
        {
            blend SrcAlpha OneMinusSrcAlpha

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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _DrawChannel;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                col.rgb = col.rgb * _DrawChannel.rgb;
                col.a = 1 - (1 - col.a) * (_DrawChannel.a);
                return col;
            }
            ENDCG
        }
    }
}
