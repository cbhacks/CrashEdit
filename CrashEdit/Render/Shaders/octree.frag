#version 430 core

layout(binding=3) uniform sampler2D tColors;
layout(binding=4) uniform isampler3D tNodes;

in vec3 pNode;

uniform float uNodeShadeMax;

out vec4 fColor;

void main()
{
    int node = texelFetch(tNodes, ivec3(pNode), 0).r;
    if (node == 0) {
        discard;
    } else {
        int node_color = (node >> 1) & 0x7FFF;
        fColor = texelFetch(tColors, ivec2(node_color % 256, node_color >> 8), 0);

        // we got the color, now we add a bit of shading to help with depth perception
        // this 'packs' nodes such that they will accurately represent their layer in the octree
        vec3 packed_node = pNode / vec3(1 << ((node >> 16) & 0x1F), 1 << ((node >> 21) & 0x1F), 1 << ((node >> 26) & 0x1F));
        vec3 packed_fracs = vec3(packed_node.x - trunc(packed_node.x), packed_node.y - trunc(packed_node.y), packed_node.z - trunc(packed_node.z));
        // because the nodes are rendered on axis-aligned quads, only 2 axes can be non-integers, so we divide by 2 and not 3
        float shade = (packed_fracs.x + packed_fracs.y + packed_fracs.z) / 2 * uNodeShadeMax;
        fColor.rgb *= 1 - shade;
    }
}
