using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AutoScriptAnnotation : UnityEditor.AssetModificationProcessor {


	private static string AnnotationStr =
		"// * ============================================================= *\r\n"
		+ "// * 描 述：\r\n"
		+ "// * 作 者：\r\n"
		+ "// * 版 本：v 1.0\r\n"
		+ "// * 创建时间：#CreateTime#\r\n"
		+ "// * ============================================================= *\r\n";

	public static void OnWillCreateAsset(string path){
		path = path.Replace (".meta", "");

		if (path.EndsWith (".cs")) {
		
			AnnotationStr += File.ReadAllText (path);

			AnnotationStr = AnnotationStr.Replace ("#CreateTime#",System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

			File.WriteAllText (path,AnnotationStr);
		}
	}

}
