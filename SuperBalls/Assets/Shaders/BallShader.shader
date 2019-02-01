Shader "Unlit/BallShader"
{
    Properties
    {
        _MainColor ("Color", Color) = (1, 0, 0, 1) 
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 screenPos : TEXCOORD1;
            };

            float4 _MainColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.screenPos = ComputeScreenPos(o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float brightness = max((i.screenPos.y - 0.5) * 2, 0);
                float4 col = _MainColor  + float4(brightness, brightness, brightness, 0);
                return col;
            }
            ENDCG
        }
    }
}
