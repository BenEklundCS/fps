shader_type spatial;

uniform sampler2D box_texture;
uniform float tiling_factor : hint_range(0.0f, 50.0f);
uniform float redshift_amount : hint_range(0.0f, 1.0f);

void fragment() {
    vec4 pixel_color = texture(box_texture, UV * tiling_factor);

	pixel_color.g *= (1.0 - redshift_amount);
    pixel_color.b *= (1.0 - redshift_amount);

    ALBEDO = pixel_color.rgb;
    ALPHA = pixel_color.a;
}