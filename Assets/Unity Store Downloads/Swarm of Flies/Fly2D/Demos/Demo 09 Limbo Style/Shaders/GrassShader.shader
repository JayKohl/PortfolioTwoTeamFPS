Shader "Unlit/Grass"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_WaveAmplitude ("Wave Amplitude", float) = 0
		_WaveLength ("Wave Length", float) = 0
		_WaveSpeed ("Wave Speed", float) = 0
    }
    SubShader
    {
		Tags
		{ 
			"Queue"="Transparent" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		//Blend One OneMinusSrcAlpha
		Blend SrcAlpha OneMinusSrcAlpha 

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
				float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float _WaveAmplitude;
			float _WaveLength;
			float _WaveSpeed;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float2 uv = i.uv;


				uv.x += sin( (uv.y + _Time * _WaveSpeed) * (2 * UNITY_PI / _WaveLength)) * _WaveAmplitude * uv.y;
				uv.y += sin( (uv.x + _Time * _WaveSpeed) * (2 * UNITY_PI / _WaveLength)) * _WaveAmplitude * uv.y;

                fixed4 col = tex2D(_MainTex, uv);
                return col * i.color;
            }
            ENDCG
        }
    }
}
