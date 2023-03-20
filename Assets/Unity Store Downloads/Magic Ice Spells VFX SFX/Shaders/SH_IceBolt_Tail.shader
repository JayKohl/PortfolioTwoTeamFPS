// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/SH_IceBolt_Tail"
{
	Properties
	{
		_Offset("Offset", Range( 0 , 5)) = 1
		_2("2", 2D) = "white" {}
		_Untitled3("Untitled-3", 2D) = "white" {}
		_T_Gradient_Radial_01("T_Gradient_Radial_01", 2D) = "white" {}
		_Vector1("Vector 1", Vector) = (1.2,0.5,0.2,0)
		_Emission("Emission", Range( 0 , 3)) = 0
		_Opacity("Opacity", Range( 0 , 1)) = 0
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
			float4 vertexColor : COLOR;
			float2 uv_texcoord;
		};

		uniform float3 _Vector1;
		uniform sampler2D _Untitled3;
		uniform float _Offset;
		uniform sampler2D _2;
		uniform sampler2D _T_Gradient_Radial_01;
		uniform float _Emission;
		uniform float _Opacity;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_TexCoord18 = i.uv_texcoord + float2( -0.15,0.06 );
			float2 uv_TexCoord1 = i.uv_texcoord * float2( 2,2 );
			float2 panner2 = ( 1.0 * _Time.y * float2( -3.5,-0.5 ) + uv_TexCoord1);
			float2 uv_TexCoord47 = i.uv_texcoord + float2( 0.1,0 );
			float4 temp_output_42_0 = ( tex2D( _Untitled3, ( ( float2( -0.1,-0.3 ) + uv_TexCoord18 ) + ( _Offset * (tex2D( _2, panner2 )).rg ) ) ) * tex2D( _T_Gradient_Radial_01, uv_TexCoord47 ) );
			o.Emission = ( ( ( i.vertexColor * float4( _Vector1 , 0.0 ) ) * temp_output_42_0 ) * _Emission ).rgb;
			float4 clampResult45 = clamp( ( temp_output_42_0 * _Opacity ) , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
			o.Alpha = ( i.vertexColor.a * clampResult45 ).r;
		}

		ENDCG
	}
	//CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15401
7;29;1906;1004;2368.676;699.5693;1.666979;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;1;-3367.793,763.9796;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;2,2;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;2;-3089.596,747.2797;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-3.5,-0.5;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;6;-2874.097,720.2797;Float;True;Property;_2;2;1;0;Create;True;0;0;False;0;None;3b583d8b056a89945ba9e0cdbdcee87e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;9;-2770.62,536.3654;Float;False;Property;_Offset;Offset;0;0;Create;True;0;0;False;0;1;0.67;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;17;-2302.193,92.87752;Float;False;Constant;_Vector0;Vector 0;2;0;Create;True;0;0;False;0;-0.1,-0.3;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.ComponentMaskNode;7;-2555.794,719.2794;Float;True;True;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;18;-2345.992,289.2768;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;-0.15,0.06;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-2251.594,625.6793;Float;True;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;16;-2036.393,117.6768;Float;True;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;39;-1781.963,553.3723;Float;True;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;47;-1898.09,866.7321;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0.1,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;41;-1495.352,665.6596;Float;True;Property;_T_Gradient_Radial_01;T_Gradient_Radial_01;3;0;Create;True;0;0;False;0;49370287c839b6243b38cf12361482c8;49370287c839b6243b38cf12361482c8;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;38;-1497.517,377.2141;Float;True;Property;_Untitled3;Untitled-3;2;0;Create;True;0;0;False;0;None;3195e59bcb9a3e645835922ab92314a3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;43;-1350.363,-373.2239;Float;False;Property;_Vector1;Vector 1;4;0;Create;True;0;0;False;0;1.2,0.5,0.2;1.4,1.65,2;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;44;-1182.584,902.756;Float;False;Property;_Opacity;Opacity;6;0;Create;True;0;0;False;0;0;0.69;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;-1159.076,428.2899;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;33;-1329.437,16.31307;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-955.2792,-348.2827;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-898.4922,610.0229;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;45;-582.0789,475.3047;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,1;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;-690.3271,-156.7192;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;49;-469.9869,-6.106117;Float;False;Property;_Emission;Emission;5;0;Create;True;0;0;False;0;0;0.69;0;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-342.4473,186.2269;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;-184.9336,-297.8274;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;Effects/SH_IceBolt_Tail;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;2;0;1;0
WireConnection;6;1;2;0
WireConnection;7;0;6;0
WireConnection;8;0;9;0
WireConnection;8;1;7;0
WireConnection;16;0;17;0
WireConnection;16;1;18;0
WireConnection;39;0;16;0
WireConnection;39;1;8;0
WireConnection;41;1;47;0
WireConnection;38;1;39;0
WireConnection;42;0;38;0
WireConnection;42;1;41;0
WireConnection;34;0;33;0
WireConnection;34;1;43;0
WireConnection;46;0;42;0
WireConnection;46;1;44;0
WireConnection;45;0;46;0
WireConnection;40;0;34;0
WireConnection;40;1;42;0
WireConnection;37;0;33;4
WireConnection;37;1;45;0
WireConnection;48;0;40;0
WireConnection;48;1;49;0
WireConnection;0;2;48;0
WireConnection;0;9;37;0
ASEEND*/
//CHKSM=3A553551171D207CD9B70AA3EE5ECED284972F81