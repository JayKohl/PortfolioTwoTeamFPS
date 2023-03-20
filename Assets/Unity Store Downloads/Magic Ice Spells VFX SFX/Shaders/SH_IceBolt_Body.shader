// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/SH_IceBolt_Body"
{
	Properties
	{
		_Float1("Float 1", Range( 0 , 5)) = 1
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_Offset_01("Offset_01", Vector) = (-0.25,-0.3,0,0)
		_Offset_02("Offset_02", Vector) = (0.15,0.1,0,0)
		_2("2", 2D) = "white" {}
		_T_Explsion_01("T_Explsion_01", 2D) = "white" {}
		_TextureSample3("Texture Sample 3", 2D) = "white" {}
		_T_Gradient_Cyrcle("T_Gradient_Cyrcle", 2D) = "white" {}
		_color("color", Vector) = (1.2,0.5,0.2,0)
		_Lenth("Lenth", Range( 0 , 5)) = 13.5
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

		uniform float3 _color;
		uniform sampler2D _TextureSample3;
		uniform sampler2D _TextureSample1;
		uniform float _Float1;
		uniform float2 _Offset_01;
		uniform sampler2D _T_Explsion_01;
		uniform float2 _Offset_02;
		uniform sampler2D _2;
		uniform sampler2D _T_Gradient_Cyrcle;
		uniform float4 _T_Gradient_Cyrcle_ST;
		uniform float _Lenth;
		uniform float _Opacity;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_TexCoord12 = i.uv_texcoord * float2( 0.5,0.5 );
			float2 panner14 = ( 1.0 * _Time.y * float2( 0.5,1 ) + uv_TexCoord12);
			float2 uv_TexCoord23 = i.uv_texcoord * float2( 1.15,1.15 ) + float2( 0.03,0.05 );
			float2 uv_TexCoord18 = i.uv_texcoord * float2( 1.2,1.2 ) + float2( -0.1,-0.05 );
			float2 uv_TexCoord1 = i.uv_texcoord * float2( 0.65,0.65 );
			float2 panner2 = ( 1.0 * _Time.y * float2( -0.5,-1.5 ) + uv_TexCoord1);
			float2 uv_T_Gradient_Cyrcle = i.uv_texcoord * _T_Gradient_Cyrcle_ST.xy + _T_Gradient_Cyrcle_ST.zw;
			float4 clampResult27 = clamp( ( tex2D( _T_Gradient_Cyrcle, uv_T_Gradient_Cyrcle ) * _Lenth ) , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
			float4 temp_output_30_0 = ( ( tex2D( _TextureSample3, ( ( (tex2D( _TextureSample1, panner14 )).rg * _Float1 ) + ( uv_TexCoord23 + _Offset_01 ) ) ) + tex2D( _T_Explsion_01, ( ( uv_TexCoord18 + _Offset_02 ) - ( _Float1 * (tex2D( _2, panner2 )).rg ) ) ) ) * clampResult27 );
			o.Emission = ( i.vertexColor * ( float4( _color , 0.0 ) * temp_output_30_0 ) ).rgb;
			o.Alpha = ( i.vertexColor.a * ( temp_output_30_0 * _Opacity ) ).r;
		}

		ENDCG
	}
	//CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15401
291;29;1461;1004;3803.567;523.3046;1.3;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;12;-4159.064,-371.1657;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.5,0.5;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;1;-4240.343,946.8549;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.65,0.65;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;14;-3880.864,-387.8655;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.5,1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;2;-3962.145,930.155;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.5,-1.5;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;13;-3665.365,-414.8656;Float;True;Property;_TextureSample1;Texture Sample 1;1;0;Create;True;0;0;False;0;None;22d3bf82fc2abc84585d2d570fc84b1f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;6;-3746.646,903.155;Float;True;Property;_2;2;4;0;Create;True;0;0;False;0;None;22d3bf82fc2abc84585d2d570fc84b1f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;7;-3428.343,902.1547;Float;True;True;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;23;-3008.139,-180.0474;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1.15,1.15;False;1;FLOAT2;0.03,0.05;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;9;-3862.542,256.0546;Float;False;Property;_Float1;Float 1;0;0;Create;True;0;0;False;0;1;1.78;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;18;-3071.337,517.9529;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1.2,1.2;False;1;FLOAT2;-0.1,-0.05;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;11;-3347.062,-415.866;Float;True;True;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;17;-3020.86,308.2498;Float;False;Property;_Offset_02;Offset_02;3;0;Create;True;0;0;False;0;0.15,0.1;0.15,0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;24;-3003.73,124.481;Float;False;Property;_Offset_01;Offset_01;2;0;Create;True;0;0;False;0;-0.25,-0.3;-0.25,-0.3;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-2990.863,-422.3667;Float;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-3124.143,808.5547;Float;True;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;16;-2692.938,378.952;Float;True;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;22;-2685.137,-134.8473;Float;True;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;15;-2473.637,529.8532;Float;True;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;21;-2499.137,-339.847;Float;True;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;26;-1874.428,397.6462;Float;True;Property;_T_Gradient_Cyrcle;T_Gradient_Cyrcle;7;0;Create;True;0;0;False;0;None;19cc9be09be5f01409df6ace7637ca37;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;29;-1852.735,626.5548;Float;False;Property;_Lenth;Lenth;9;0;Create;True;0;0;False;0;13.5;0.98;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;25;-2208.837,-133.3462;Float;True;Property;_TextureSample3;Texture Sample 3;6;0;Create;True;0;0;False;0;ab2105ef18b233740a0db1dcd3e8d4bb;ab2105ef18b233740a0db1dcd3e8d4bb;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;19;-2216.238,223.953;Float;True;Property;_T_Explsion_01;T_Explsion_01;5;0;Create;True;0;0;False;0;ab2105ef18b233740a0db1dcd3e8d4bb;ab2105ef18b233740a0db1dcd3e8d4bb;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-1470.435,402.2551;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;27;-1248.539,225.8589;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,1;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;20;-1824.937,14.65322;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;39;-969.9473,468.5575;Float;False;Property;_Opacity;Opacity;10;0;Create;True;0;0;False;0;0;0.559;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-956.9353,65.55516;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector3Node;32;-1085.732,-111.9619;Float;False;Property;_color;color;8;0;Create;True;0;0;False;0;1.2,0.5,0.2;1.4,1.65,2;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-725.535,-98.24535;Float;True;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;33;-510.4367,81.35393;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;-595.5471,380.5575;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-4077.745,558.6545;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-306.9563,268.8574;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-326.3351,-118.0459;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-4310.744,527.6545;Float;False;Constant;_Float0;Float 0;0;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;4;-4298.344,812.4545;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;Effects/SH_IceBolt_Body;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;14;0;12;0
WireConnection;2;0;1;0
WireConnection;13;1;14;0
WireConnection;6;1;2;0
WireConnection;7;0;6;0
WireConnection;11;0;13;0
WireConnection;10;0;11;0
WireConnection;10;1;9;0
WireConnection;8;0;9;0
WireConnection;8;1;7;0
WireConnection;16;0;18;0
WireConnection;16;1;17;0
WireConnection;22;0;23;0
WireConnection;22;1;24;0
WireConnection;15;0;16;0
WireConnection;15;1;8;0
WireConnection;21;0;10;0
WireConnection;21;1;22;0
WireConnection;25;1;21;0
WireConnection;19;1;15;0
WireConnection;28;0;26;0
WireConnection;28;1;29;0
WireConnection;27;0;28;0
WireConnection;20;0;25;0
WireConnection;20;1;19;0
WireConnection;30;0;20;0
WireConnection;30;1;27;0
WireConnection;31;0;32;0
WireConnection;31;1;30;0
WireConnection;38;0;30;0
WireConnection;38;1;39;0
WireConnection;3;0;5;0
WireConnection;3;1;4;0
WireConnection;37;0;33;4
WireConnection;37;1;38;0
WireConnection;34;0;33;0
WireConnection;34;1;31;0
WireConnection;0;2;34;0
WireConnection;0;9;37;0
ASEEND*/
//CHKSM=02847708ACAE6572476F95D0F95D6D9E3AC313C4