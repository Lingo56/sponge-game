Shader "UI/Dissolve"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("Dissolve Noise", 2D) = "white" {}
        _Dissolve ("Dissolve Threshold", Range(0,1)) = 0.5
        _Color ("Tint", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        LOD 100
        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            sampler2D _NoiseTex;
            float4 _MainTex_ST;
            float4 _NoiseTex_ST;
            float _Dissolve;
            float4 _Color;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the font texture (SDF data)
                fixed4 texColor = tex2D(_MainTex, i.uv);
                
                // Apply SDF thresholding for crisp text
                // The 0.5 threshold is standard for SDF fonts, adjust if needed
                float smoothWidth = 0.05;
                float distance = texColor.a - 0.5;
                float alpha = smoothstep(0 - smoothWidth, 0 + smoothWidth, distance);
                
                // Create final color with proper SDF alpha
                fixed4 col = fixed4(_Color.rgb, alpha * _Color.a);
                
                // Sample the noise texture
                float noise = tex2D(_NoiseTex, i.uv).r;
                
                // Apply dissolve effect
                if (noise < _Dissolve)
                    discard;
                
                return col;
            }
            ENDCG
        }
    }
}
