Shader "ScriptBoy/Unlit/ExplosionWaveShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_ColorA ("Color A", Color) = (1,1,1,1)
		_ColorB ("Color B", Color) = (1,1,1,1)
		_ColorC ("Color C", Color) = (1,1,1,1)
		_R ("R", Range(0,1)) = 0
		_RLength ("RLength", float) = 0
    }
    SubShader
    {
   		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "False"
		}

		Cull off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

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
				float4 vertexColor : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				float r : float;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

			float4 _ColorA;
			float4 _ColorB;
			float4 _ColorC;

			float _R;
			float _RLength;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.r = v.vertexColor.a;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float deltaX = i.uv.x - 0.5f;
				float deltaY = i.uv.y - 0.5f;
				float magnitude = sqrt( deltaX * deltaX + deltaY * deltaY);

				float r = _R * i.r; 
				float fade2 = clamp(r / _RLength,0,1);
				float rLength = _RLength * fade2;
	
				float t = (magnitude / 0.5f);
				float alpha  = 1 - abs((r - t)/rLength);
				alpha = clamp(alpha,0,1);

				float fadeR = 1 - rLength * 2 ;

				if(t >  fadeR)
				{
					alpha *= clamp(1 - (t - fadeR) / rLength * 2 - rLength, 0, 1);
				}

                return lerp(lerp(_ColorA, _ColorB, r), lerp(_ColorB, _ColorC, r)*2, r) * alpha;
            }
            ENDCG
        }
    }
}