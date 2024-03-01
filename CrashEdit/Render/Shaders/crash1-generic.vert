#version 430 core

in vec3 position;
in vec3 normal;
in vec4 color;
in vec2 uv;
in ivec2 tex;

uniform mat4 PVM;

uniform int art;

uniform vec3 trans;
uniform vec3 scale;
uniform float scaleScalar;

uniform int blendmask;
uniform int enableTex;

out vec4 p_Color;
out vec2 p_UV;
out flat int p_Tex;
out flat int p_TexPage;

void main(void)
{
    p_Color = vec4(color.rgb, 1.0);
    p_UV = uv;
    p_Tex = tex.x;
    p_TexPage = tex.y;
    
    if (p_TexPage >= 0 && enableTex != 0 && ((p_Tex >> 2) & 0x3) != blendmask && blendmask != 3) {
        // discard wrong blend modes, but always let textures pass on solid render
        gl_Position = vec4(0);
    } else {
        gl_Position = PVM * vec4( (position+trans)*scale, 1.0 );
    }
}
