#version 430 core

in vec3 position;
in vec3 normal;
in vec4 color;
in vec2 uv;
in int tex;

uniform mat4 PVM;

uniform int art;

uniform vec3 trans;
uniform vec3 scale;
uniform float scaleScalar;

out vec3 p_Color;
out vec2 p_UV;
out int p_Tex;

void main(void)
{
    gl_Position = PVM * vec4( (position+trans)*scale, 1.0 );
    p_Color = color.rgb;
    p_UV = uv;
    p_Tex = tex;
}
