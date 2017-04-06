/*
 * Copyright (C) 2014 Francisco Manuel Garcia Moreno
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using UnityEngine;
using System.Collections;

public class Materials {
	public static void changeSharedMaterial(Renderer _renderer, int index, Material material){
		Material[] materials = _renderer.sharedMaterials;
		materials[index] = material;
		_renderer.materials = materials;
	}

	public static void changeSharedMaterial(Renderer _renderer, Material[] _materials){
		Material[] materials = _renderer.sharedMaterials;
		materials = _materials;
		_renderer.materials = materials;
	}
}
