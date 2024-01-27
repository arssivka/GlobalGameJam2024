shader_type spatial;
uniform float progress = 0.0;
uniform vec3 ProgressColor = vec3(0, 1, 0);
uniform vec3 BarColor = vec3(0.1, 0.1, 0.1);

void vertex() {
	// Camera-facing orientation would be implemented here later...
}

void fragment() {
	ALBEDO = ProgressColor;
	if (UV.y > clamp(progress, 0, 1))
		{
		ALBEDO = BarColor;
		}
}