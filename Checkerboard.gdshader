shader_type spatial;

uniform float checkSize = 2;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	// Called for every pixel the material is visible on.
	vec4 WorldUV = INV_VIEW_MATRIX * vec4(VERTEX, 1)/vec4(100);
	float remainder = mod(floor(checkSize * WorldUV.x) +
		floor(checkSize * WorldUV.z), 2.0f);
	float col = max(sign(remainder), 0.0f);
	ALBEDO = vec3(col);
}

//void light() {
	// Called for every pixel for every light affecting the material.
	// Uncomment to replace the default light processing function with this one.
//}
