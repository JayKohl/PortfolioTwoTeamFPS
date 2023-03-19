// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/SH_Energy_01"
{
	Properties
	{
		_Texture2("Texture 2", 2D) = "white" {}
		_Texture3("Texture 3", 2D) = "white" {}
		_Alpha("Alpha", 2D) = "white" {}
		_intensity("intensity", Range( 0 , 30)) = 6
		_Opacity("Opacity", Range( 0 , 1)) = 0.5
		_Emmision("Emmision", Range( 0 , 5)) = 1
		_Refract("Refract %", Range( 0 , 1)) = 0.2
		_T_Blobs("T_Blobs", 2D) = "white" {}
		_T_Gradient_Radial_01("T_Gradient_Radial_01", 2D) = "white" {}
		_Panner("Panner", Vector) = (0.04,-0.2,0,0)
		_Speed("Speed", Vector) = (0,0,0,0)
		_Offset("Offset", Vector) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit alpha:fade keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform sampler2D _Alpha;
		uniform float2 _Speed;
		uniform float2 _Offset;
		uniform float _Refract;
		uniform sampler2D _T_Blobs;
		uniform float2 _Panner;
		uniform sampler2D _Texture2;
		uniform sampler2D _Texture3;
		uniform float _intensity;
		uniform float _Emmision;
		uniform sampler2D _T_Gradient_Radial_01;
		uniform float4 _T_Gradient_Radial_01_ST;
		uniform float _Opacity;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_TexCoord66 = i.uv_texcoord + _Offset;
			float2 uv_TexCoord21 = i.uv_texcoord * float2( 1,0.5 );
			float2 panner19 = ( 1.0 * _Time.y * _Panner + uv_TexCoord21);
			float2 temp_output_67_0 = ( uv_TexCoord66 + ( _Refract * tex2D( _T_Blobs, panner19 ).g ) );
			float2 panner91 = ( 1.0 * _Time.y * _Speed + temp_output_67_0);
			float cos86 = cos( _SinTime.x );
			float sin86 = sin( _SinTime.x );
			float2 rotator86 = mul( temp_output_67_0 - float2( 0,1 ) , float2x2( cos86 , -sin86 , sin86 , cos86 )) + float2( 0,1 );
			float cos71 = cos( _SinTime.y );
			float sin71 = sin( _SinTime.y );
			float2 rotator71 = mul( temp_output_67_0 - float2( 1,0 ) , float2x2( cos71 , -sin71 , sin71 , cos71 )) + float2( 1,0 );
			float4 temp_output_18_0 = ( tex2D( _Alpha, panner91 ) * ( ( tex2D( _Texture2, rotator86 ).r * tex2D( _Texture3, rotator71 ) ) * _intensity ) );
			o.Emission = ( ( temp_output_18_0 * _Emmision ) * i.vertexColor ).rgb;
			float2 uv_T_Gradient_Radial_01 = i.uv_texcoord * _T_Gradient_Radial_01_ST.xy + _T_Gradient_Radial_01_ST.zw;
			float4 clampResult26 = clamp( ( i.vertexColor.a * ( ( temp_output_18_0 * tex2D( _T_Gradient_Radial_01, uv_T_Gradient_Radial_01 ) ) * _Opacity ) ) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			o.Alpha = clampResult26.r;
		}

		ENDCG
	}
	//CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15401
279;108;1411;1004;4821.906;931.288;3.110366;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;21;-4341.486,380.3373;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,0.5;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;95;-4330.936,608.0146;Float;False;Property;_Panner;Panner;9;0;Create;True;0;0;False;0;0.04,-0.2;0.2,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;19;-3964.802,458.4041;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.04,-0.2;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;96;-3636.289,-30.21401;Float;False;Property;_Offset;Offset;11;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;64;-3731.487,308.2951;Float;False;Property;_Refract;Refract %;6;0;Create;True;0;0;False;0;0.2;0.035;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;63;-3749.587,454.6004;Float;True;Property;_T_Blobs;T_Blobs;7;0;Create;True;0;0;False;0;1363fc979baa3f9419a9bcdd43f30c58;1363fc979baa3f9419a9bcdd43f30c58;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;66;-3369.786,59.81805;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;-0.1,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;65;-3416.786,428.9995;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinTimeNode;87;-2698.443,265.4969;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SinTimeNode;69;-2741.284,568.9982;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;67;-3105.185,219.5995;Float;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RotatorNode;86;-2537.243,172.9976;Float;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,1;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RotatorNode;71;-2533.784,497.3995;Float;False;3;0;FLOAT2;0,0;False;1;FLOAT2;1,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;7;-2290.102,436.2002;Float;True;Property;_Texture3;Texture 3;1;0;Create;True;0;0;False;0;3b583d8b056a89945ba9e0cdbdcee87e;3b583d8b056a89945ba9e0cdbdcee87e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;4;-2284.201,190.2;Float;True;Property;_Texture2;Texture 2;0;0;Create;True;0;0;False;0;3b583d8b056a89945ba9e0cdbdcee87e;3b583d8b056a89945ba9e0cdbdcee87e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;97;-2964.369,-134.3385;Float;False;Property;_Speed;Speed;10;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;91;-2633.117,-137.538;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-0.27;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-1935.101,313.9999;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-1951.674,546.7001;Float;False;Property;_intensity;intensity;3;0;Create;True;0;0;False;0;6;7.5;0;30;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-1681.6,311.4;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;15;-2114.499,-164.9001;Float;True;Property;_Alpha;Alpha;2;0;Create;True;0;0;False;0;33c85d3bc54b5364184b396cf6f9cc76;828c91b5c28d98744852a67a4b5c4d3e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;92;-1678.955,679.2322;Float;True;Property;_T_Gradient_Radial_01;T_Gradient_Radial_01;8;0;Create;True;0;0;False;0;49370287c839b6243b38cf12361482c8;49370287c839b6243b38cf12361482c8;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-1423.699,209.0002;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;93;-1224.628,476.069;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-1123.178,736.8316;Float;False;Property;_Opacity;Opacity;4;0;Create;True;0;0;False;0;0.5;0.426;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-1095.577,-345.3721;Float;False;Property;_Emmision;Emmision;5;0;Create;True;0;0;False;0;1;1.44;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-839.4217,390.6881;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;88;-1084.942,-87.27937;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;89;-622.5413,2.3206;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-767.9754,-551.2724;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;90;-363.3423,-196.0794;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;26;-377.3993,181.3006;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;Effects/SH_Energy;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;19;0;21;0
WireConnection;19;2;95;0
WireConnection;63;1;19;0
WireConnection;66;1;96;0
WireConnection;65;0;64;0
WireConnection;65;1;63;2
WireConnection;67;0;66;0
WireConnection;67;1;65;0
WireConnection;86;0;67;0
WireConnection;86;2;87;0
WireConnection;71;0;67;0
WireConnection;71;2;69;2
WireConnection;7;1;71;0
WireConnection;4;1;86;0
WireConnection;91;0;67;0
WireConnection;91;2;97;0
WireConnection;10;0;4;1
WireConnection;10;1;7;0
WireConnection;11;0;10;0
WireConnection;11;1;12;0
WireConnection;15;1;91;0
WireConnection;18;0;15;0
WireConnection;18;1;11;0
WireConnection;93;0;18;0
WireConnection;93;1;92;0
WireConnection;24;0;93;0
WireConnection;24;1;25;0
WireConnection;89;0;88;4
WireConnection;89;1;24;0
WireConnection;32;0;18;0
WireConnection;32;1;27;0
WireConnection;90;0;32;0
WireConnection;90;1;88;0
WireConnection;26;0;89;0
WireConnection;0;2;90;0
WireConnection;0;9;26;0
ASEEND*/
//CHKSM=EF6862963E73D10D9C25C6AE464E5D79454A50A9