#version 330 core

in vec2 fragTexCoord; // Texture coordinates
in vec3 fragNormal;  // Surface normal

out vec4 fragColor;  // Final fragment color

uniform vec3 lightDir; // Direction of the light source

void main()
{
    // Normalize the surface normal and the light direction
    vec3 normal = normalize(fragNormal);
    vec3 lightDirection = normalize(lightDir);

    // Calculate the light intensity (dot product)
    float intensity = max(dot(normal, lightDirection), 0.0);

    // Diffuse color of the material (you can replace this with your own color)
    vec3 materialColor = vec3(1.0, 1.0, 1.0); // White in RGB

    // Calculate the final color with lighting
    vec3 finalColor = materialColor * intensity;

    fragColor = vec4(finalColor, 1.0);
}

