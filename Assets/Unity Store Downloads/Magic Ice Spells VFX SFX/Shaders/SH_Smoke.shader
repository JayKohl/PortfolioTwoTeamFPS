// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/SH_Smoke"
{
	Properties
	{
		_Depth("Depth", Range( 0 , 5)) = 0.25
		_DistortionOffset("Distortion Offset", Range( 0 , 5)) = 1.2
		_T_SmokeColored_04("T_SmokeColored_04", 2D) = "white" {}
		_T_Gradient_Radial_04("T_Gradient_Radial_04", 2D) = "white" {}
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_T_SmokeSoft_Explo_02("T_SmokeSoft_Explo_02", 2D) = "white" {}
		_T_Gradient_Radial_03("T_Gradient_Radial_03", 2D) = "white" {}
		_Emission("Emission", Range( 0 , 5)) = 0
		_Panner_UpL("Panner_UpL", Vector) = (0.2,0.2,0,0)
		_Panner_DownR("Panner_DownR", Vector) = (0.2,0.2,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform sampler2D _T_Gradient_Radial_03;
		uniform float4 _T_Gradient_Radial_03_ST;
		uniform float _Emission;
		uniform sampler2D _T_SmokeColored_04;
		uniform float _DistortionOffset;
		uniform sampler2D _T_SmokeSoft_Explo_02;
		uniform float2 _Panner_DownR;
		uniform sampler2D _TextureSample1;
		uniform float2 _Panner_UpL;
		uniform float _Depth;
		uniform sampler2D _T_Gradient_Radial_04;
		uniform float4 _T_Gradient_Radial_04_ST;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_T_Gradient_Radial_03 = i.uv_texcoord * _T_Gradient_Radial_03_ST.xy + _T_Gradient_Radial_03_ST.zw;
			o.Emission = ( ( tex2D( _T_Gradient_Radial_03, uv_T_Gradient_Radial_03 ) * i.vertexColor ) * _Emission ).rgb;
			float2 uv_TexCoord1 = i.uv_texcoord * float2( 0.73,0.73 ) + float2( 0.1,0.1 );
			float2 appendResult42 = (float2(( (uv_TexCoord1).x + -0.2 ) , ( (uv_TexCoord1).y + -0.2 )));
			float2 uv_TexCoord24 = i.uv_texcoord * float2( 1.2,1.2 );
			float2 panner26 = ( 1.0 * _Time.y * _Panner_DownR + uv_TexCoord24);
			float2 uv_TexCoord27 = i.uv_texcoord * float2( 0.7,0.7 );
			float2 panner29 = ( 1.0 * _Time.y * _Panner_UpL + uv_TexCoord27);
			float2 appendResult43 = (float2(tex2D( _T_SmokeSoft_Explo_02, panner26 ).r , tex2D( _TextureSample1, panner29 ).r));
			float2 uv_T_Gradient_Radial_04 = i.uv_texcoord * _T_Gradient_Radial_04_ST.xy + _T_Gradient_Radial_04_ST.zw;
			float4 clampResult23 = clamp( ( i.vertexColor.a * ( tex2D( _T_SmokeColored_04, ( ( appendResult42 * _DistortionOffset ) + ( appendResult43 * _Depth ) ) ) * tex2D( _T_Gradient_Radial_04, uv_T_Gradient_Radial_04 ) ) ) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			o.Alpha = clampResult23.r;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Unlit alpha:fade keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				half4 color : COLOR0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.color = v.color;
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.vertexColor = IN.color;
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	//CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15401
659;24;1461;1004;3987.186;685.2713;1.838545;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;1;-3108.516,-438.8803;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.73,0.73;False;1;FLOAT2;0.1,0.1;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;27;-3479.445,588.0251;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.7,0.7;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;24;-3507.717,143.5567;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1.2,1.2;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;44;-3404.363,382.9232;Float;False;Property;_Panner_DownR;Panner_DownR;9;0;Create;True;0;0;False;0;0.2,0.2;0.2,0.2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;45;-3413.558,783.7263;Float;False;Property;_Panner_UpL;Panner_UpL;8;0;Create;True;0;0;False;0;0.2,0.2;-0.1,-0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.ComponentMaskNode;2;-2770.113,-568.6804;Float;True;True;False;False;False;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-2705.614,-120.3419;Float;False;Constant;_V;V;0;0;Create;True;0;0;False;0;-0.2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;29;-3145.945,591.5358;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.1,-0.1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-2693.113,-387.6804;Float;False;Constant;_U;U;0;0;Create;True;0;0;False;0;-0.2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;26;-3107.456,332.1843;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.2,0.2;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ComponentMaskNode;3;-2781.113,-305.6804;Float;True;False;True;False;False;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;28;-2849.377,585.7579;Float;True;Property;_TextureSample1;Texture Sample 1;4;0;Create;True;0;0;False;0;None;22d3bf82fc2abc84585d2d570fc84b1f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;6;-2497.115,-565.6804;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;7;-2489.615,-304.1804;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;25;-2836.487,352.0066;Float;True;Property;_T_SmokeSoft_Explo_02;T_SmokeSoft_Explo_02;5;0;Create;True;0;0;False;0;None;22d3bf82fc2abc84585d2d570fc84b1f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;32;-2516.362,687.1645;Float;False;Property;_Depth;Depth;0;0;Create;True;0;0;False;0;0.25;0.72;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-2264.001,-196.988;Float;False;Property;_DistortionOffset;Distortion Offset;1;0;Create;True;0;0;False;0;1.2;0.7600004;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;42;-2255.16,-527.4555;Float;True;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;43;-2406.121,436.4148;Float;True;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-2024.415,-463.8804;Float;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-2084.962,468.0229;Float;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;12;-1796.368,-158.184;Float;True;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;13;-1492.789,-113.4185;Float;True;Property;_T_SmokeColored_04;T_SmokeColored_04;2;0;Create;True;0;0;False;0;None;ee40115ae3508224ab4c5fd6bbc8651c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;14;-1480.846,135.4187;Float;True;Property;_T_Gradient_Radial_04;T_Gradient_Radial_04;3;0;Create;True;0;0;False;0;None;c6dfe99b29a432f4d99063043d6a10ba;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;16;-1162.047,-334.9424;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-1146.246,-107.0423;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;33;-903.1181,-537.6855;Float;True;Property;_T_Gradient_Radial_03;T_Gradient_Radial_03;6;0;Create;True;0;0;False;0;None;355b73d804975b643a7ad55b0385f7a5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-904.6454,-97.4423;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-521.4171,-422.7852;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;41;-519.0192,-186.6853;Float;False;Property;_Emission;Emission;7;0;Create;True;0;0;False;0;0;1.94;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;23;-585.4887,5.456543;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;-184.6187,-453.8853;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;Effects/SH_Smoke;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;2;0;1;0
WireConnection;29;0;27;0
WireConnection;29;2;45;0
WireConnection;26;0;24;0
WireConnection;26;2;44;0
WireConnection;3;0;1;0
WireConnection;28;1;29;0
WireConnection;6;0;2;0
WireConnection;6;1;5;0
WireConnection;7;0;3;0
WireConnection;7;1;8;0
WireConnection;25;1;26;0
WireConnection;42;0;6;0
WireConnection;42;1;7;0
WireConnection;43;0;25;1
WireConnection;43;1;28;1
WireConnection;11;0;42;0
WireConnection;11;1;10;0
WireConnection;31;0;43;0
WireConnection;31;1;32;0
WireConnection;12;0;11;0
WireConnection;12;1;31;0
WireConnection;13;1;12;0
WireConnection;15;0;13;0
WireConnection;15;1;14;0
WireConnection;17;0;16;4
WireConnection;17;1;15;0
WireConnection;34;0;33;0
WireConnection;34;1;16;0
WireConnection;23;0;17;0
WireConnection;40;0;34;0
WireConnection;40;1;41;0
WireConnection;0;2;40;0
WireConnection;0;9;23;0
ASEEND*/
//CHKSM=6DB8D6F5A9E5302BA106D30BB12B7F21221DDAFB