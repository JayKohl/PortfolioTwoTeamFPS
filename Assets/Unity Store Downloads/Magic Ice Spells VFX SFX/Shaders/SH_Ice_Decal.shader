// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/SH_Ice_Decal"
{
	Properties
	{
		_IceNormal("Ice Normal", 2D) = "bump" {}
		_Ice_Alpha("Ice_Alpha", 2D) = "white" {}
		_ReflectionTile("Reflection Tile", Vector) = (0.07,0.1,0.1,0)
		_Specular("Specular", Range( 0 , 1)) = 0
		_Alpha_Amount("Alpha_Amount", Range( 0 , 1)) = 1
		_Ammisive("Ammisive", Range( 0 , 1)) = 0
		_Reflections("Reflections", CUBE) = "white" {}
		_T_Ice_D("T_Ice_D", 2D) = "white" {}
		_Color("Color", Color) = (0.7176471,0.8789786,0.9803922,0)
		_Notmal_Tail("Notmal_Tail", Vector) = (5,5,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float3 worldRefl;
			INTERNAL_DATA
		};

		uniform sampler2D _IceNormal;
		uniform float2 _Notmal_Tail;
		uniform sampler2D _T_Ice_D;
		uniform float4 _T_Ice_D_ST;
		uniform float4 _Color;
		uniform samplerCUBE _Reflections;
		uniform float3 _ReflectionTile;
		uniform float _Ammisive;
		uniform float _Specular;
		uniform sampler2D _Ice_Alpha;
		uniform float4 _Ice_Alpha_ST;
		uniform float _Alpha_Amount;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TexCoord83 = i.uv_texcoord * _Notmal_Tail;
			float3 tex2DNode4 = UnpackNormal( tex2D( _IceNormal, uv_TexCoord83 ) );
			o.Normal = tex2DNode4;
			float2 uv_T_Ice_D = i.uv_texcoord * _T_Ice_D_ST.xy + _T_Ice_D_ST.zw;
			float4 tex2DNode78 = tex2D( _T_Ice_D, uv_T_Ice_D );
			o.Albedo = ( tex2DNode78 * _Color ).rgb;
			o.Emission = ( texCUBE( _Reflections, ( WorldReflectionVector( i , tex2DNode4 ) + _ReflectionTile ) ) * _Ammisive ).rgb;
			o.Smoothness = ( tex2DNode78 * _Specular ).r;
			float2 uv_Ice_Alpha = i.uv_texcoord * _Ice_Alpha_ST.xy + _Ice_Alpha_ST.zw;
			o.Alpha = ( tex2D( _Ice_Alpha, uv_Ice_Alpha ) * _Alpha_Amount ).r;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows 

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
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
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
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
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
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldRefl = -worldViewDir;
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
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
441;50;1651;1004;1770.956;807.6979;1.768854;True;True
Node;AmplifyShaderEditor.Vector2Node;84;-2191.152,-88.11986;Float;False;Property;_Notmal_Tail;Notmal_Tail;9;0;Create;True;0;0;False;0;5,5;5,5;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;83;-1938.795,-175.8969;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;4;-1625.962,-196.9312;Float;True;Property;_IceNormal;Ice Normal;0;0;Create;True;0;0;False;0;None;035a3a98d5495104d86fcf697b1a57cb;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;31;-1315.225,464.5356;Float;False;Property;_ReflectionTile;Reflection Tile;2;0;Create;True;0;0;False;0;0.07,0.1,0.1;0.3,0.3,0.3;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldReflectionVector;33;-1273.807,136.6076;Float;False;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;34;-1014.864,257.5176;Float;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;40;-774.5816,162.0685;Float;True;Property;_Reflections;Reflections;6;0;Create;True;0;0;False;0;0f1ce361444c2834992ab857df569338;0f1ce361444c2834992ab857df569338;True;0;False;white;Auto;False;Object;-1;Auto;Cube;6;0;SAMPLER2D;;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;51;-828.6103,984.5621;Float;False;Property;_Alpha_Amount;Alpha_Amount;4;0;Create;True;0;0;False;0;1;0.85;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;45;-700.2479,-273.9733;Float;False;Property;_Color;Color;8;0;Create;True;0;0;False;0;0.7176471,0.8789786,0.9803922,0;1,1,1,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;49;-658.1514,722.8984;Float;False;Property;_Specular;Specular;3;0;Create;True;0;0;False;0;0;0.607;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;23;-836.6207,1122.498;Float;True;Property;_Ice_Alpha;Ice_Alpha;1;0;Create;True;0;0;False;0;a0955ebf85aca2241af0d9b6fc727f98;a0955ebf85aca2241af0d9b6fc727f98;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;27;-375.5984,374.1487;Float;False;Property;_Ammisive;Ammisive;5;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;78;-769.3304,-50.43057;Float;True;Property;_T_Ice_D;T_Ice_D;7;0;Create;True;0;0;False;0;None;b6c6155db38fbea439cfb395bc672aa1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;80;-68.43051,-437.3293;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;-313.3894,887.1757;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;-53.53796,491.3682;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;8.062536,216.9768;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;26;643.4969,62.11356;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Effects/Ice_Decal;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;83;0;84;0
WireConnection;4;1;83;0
WireConnection;33;0;4;0
WireConnection;34;0;33;0
WireConnection;34;1;31;0
WireConnection;40;1;34;0
WireConnection;80;0;78;0
WireConnection;80;1;45;0
WireConnection;50;0;23;0
WireConnection;50;1;51;0
WireConnection;48;0;78;0
WireConnection;48;1;49;0
WireConnection;28;0;40;0
WireConnection;28;1;27;0
WireConnection;26;0;80;0
WireConnection;26;1;4;0
WireConnection;26;2;28;0
WireConnection;26;4;48;0
WireConnection;26;9;50;0
ASEEND*/
//CHKSM=0F98CF7B186A43C89184958B72EAAFC845691F9F