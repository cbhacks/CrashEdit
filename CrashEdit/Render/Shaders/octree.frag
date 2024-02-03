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
        node = node >> 1;
        fColor = texelFetch(tColors, ivec2(node % 256, node >> 8), 0);
        // because the nodes are rendered on axis-aligned quads, only 2 axes can be non-integers, so we divide by 2 and not 3
        float shade = (pNode.x - trunc(pNode.x) + pNode.y - trunc(pNode.y) + pNode.z - trunc(pNode.z)) / 2 * uNodeShadeMax;
        fColor *= 1 - shade;
        fColor.a = 1.0;
    }
}
