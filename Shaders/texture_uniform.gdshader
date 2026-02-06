shader_type spatial;

uniform sampler2D texture_uniform;
uniform float tiling_factor : hint_range(1.0, 50.0) = 3.0;

void fragment() {
    vec4 pixel_color = texture(texture_uniform, UV * tiling_factor);
    ALBEDO = pixel_color.rgb;
}