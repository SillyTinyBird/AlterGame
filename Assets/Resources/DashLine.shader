Shader "Unlit/DashLine"
{
    Properties
    {
        _Color1 ("Color 1",Color) = (1,1,1,1)
        _Color2 ("Color 2",Color) = (1,1,1,1)
        _freq ("Frequency",Range(2, 100)) = 0.1
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

            float4 _Color1;
            float4 _Color2;
            float _freq;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                
                float fractions = frac(i.uv.x * _freq); 
                float4 col = fractions > 0.5 ? _Color1 : _Color2;
                return col;
            }
            ENDCG
        }
    }
}
