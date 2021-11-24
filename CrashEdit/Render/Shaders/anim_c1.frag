#version 430 core

uniform layout(binding=4, r16ui) readonly uimage2D vram16;

in vec3 pass_Color;
in vec3 pass_UVC;

out vec4 f_col;



void main()
{
    if (pass_UVC.s < 0) {
        f_col = vec4(pass_Color, 1.0);
    } else {
        vec3 texel_color;
        int colorMode = int(pass_UVC.z);
        int u = int(pass_UVC.x);
        int v = int(pass_UVC.y);
        if (colorMode == 0) {
            // 4 bit
            uvec4 t = imageLoad(vram16, ivec2(u/4, v));

            texel_color = vec3(t.r, t.r, t.r);
        } else if (colorMode == 1) {
            // 8 bit
            uvec4 t = imageLoad(vram16, ivec2(u/2, v));
            texel_color = vec3(1.0);
        } else {
            // 16 bit
            uvec4 t = imageLoad(vram16, ivec2(u, v));
            texel_color = vec3(1.0);
        }
        f_col = vec4(pass_Color*2*texel_color, 1.0);
    }
}
