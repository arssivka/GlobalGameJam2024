shader_type spatial;

uniform float checkSize = 2;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	// Called for every pixel the material is visible on.
	float remainder = mod(floor(checkSize * UV.x) +
		floor(checkSize * UV.y), 2.0f);
	float col = max(sign(remainder), 0.0f);
	ALBEDO = vec3(col);
}

//void light() {
	// Called for every pixel for every light affecting the material.
	// Uncomment to replace the default light processing function with this one.
//}
