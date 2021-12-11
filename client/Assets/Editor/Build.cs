using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System;
using System.Collections.Generic;
//using XZXD;

public class Build
{

//	public enum ComPlarmID
//	{
//		AppleStore,
//		GooglePlay,
//	}
//
//		
	#if UNITY_IOS
	[MenuItem ("Build/BuildAppleStore")]
	public static void BuildAppleStore ()
	{
		BulidTarget (PlatformId.Appstore, "IOS");
	}
	#endif

	#if UNITY_ANDROID
	[MenuItem ("Build/BuildGooglePlay")]
	public static void BuildAndroidAppStore ()
	{
		BulidTarget (PlatformId.GooglePlay, "Android");
	}

	[MenuItem ("Build/BuildAndroidNormal")]
	public static void BuildAndroidNormal ()
	{
		BulidTarget (PlatformId.AndroidNormal, "Android");
	}
	#endif
	static public void setState( PlatformId platformId)
	{
		UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Client/Scenes/game.unity");
		Main main = GameObject.FindObjectOfType<Main>();
		main.platformId = platformId;
		EditorApplication.SaveScene();
	}

	//这里封装了一个简单的通用方法。
	static void BulidTarget (PlatformId pid, string target)
	{
		//==================这里是比较重要的东西=======================
		Debug.Log (Application.dataPath);
		setState (pid);
//		TextureAndAtlasProduce.ProduceAtlasMd5 ();
//		TextureAndAtlasProduce.ProduceTextureMd5 ();
//		AutoGenerateCSResDependence.ProduceUIPart_Txt ();
		AssetDatabase.Refresh ();
		string sourceJarPath = "";


		string app_name = pid.ToString () + string.Format ("{0:yyyyMMddHHmmssffff}", DateTime.Now);
		string target_dir = "";
		string target_name = "";
		BuildOptions buildOptions = BuildOptions.None;
		BuildTarget buildTarget = BuildTarget.Android;
		BuildTargetGroup buildTargetGroup = BuildTargetGroup.Android;
		string applicationPath = Application.dataPath.Replace ("/Assets", "");
		switch (pid) {
		case PlatformId.GooglePlay:
			buildOptions = BuildOptions.AcceptExternalModificationsToPlayer;
			break;
		}
		if (target == "Android") {
			if (!string.IsNullOrEmpty (sourceJarPath)) {
				//每次build删除之前的残留
				string sourceJarPathTarget = Application.dataPath + "/Plugins/Android/";
				if (Directory.Exists (sourceJarPathTarget)) {
					Directory.Delete (sourceJarPathTarget, true);
					AssetDatabase.Refresh ();
				}

				CopyFolder (sourceJarPath, sourceJarPathTarget);
				AssetDatabase.Refresh ();
			}
			target_dir = applicationPath + "/TargetAndroid";
			target_name = app_name + ".apk";
			buildTarget = BuildTarget.Android;
			buildTargetGroup = BuildTargetGroup.Android;



			if (pid == PlatformId.GooglePlay) {
				//==================这里是比较重要的东西=======================
//				PlayerSettings.Android.keystoreName = Application.dataPath + "/../xzxd_android_studio/totem.jks";
//				PlayerSettings.Android.keystorePass = "totem123456";
//				PlayerSettings.Android.keyaliasName = "com.simplegame.mzxz.google";
//				PlayerSettings.Android.keyaliasPass = "123456";
				PlayerSettings.Android.keystoreName = Application.dataPath + "/../../user.keystore";
				PlayerSettings.Android.keystorePass = "smgame";
				PlayerSettings.Android.keyaliasName = "z2048";
				PlayerSettings.Android.keyaliasPass = "123456";

				PlayerSettings.applicationIdentifier = "com.simplegame.tzfn.google";
			} else {
				//==================这里是比较重要的东西=======================
//				PlayerSettings.Android.keystoreName = Application.dataPath + "/../xzxd_android_studio/totem.jks";
//				PlayerSettings.Android.keystorePass = "totem123456";
//				PlayerSettings.Android.keyaliasName = "com.simplegame.mzxz";
//				PlayerSettings.Android.keyaliasPass = "123456";
				PlayerSettings.Android.keystoreName = Application.dataPath + "/../../user.keystore";
				PlayerSettings.Android.keystorePass = "smgame";
				PlayerSettings.Android.keyaliasName = "z2048";
				PlayerSettings.Android.keyaliasPass = "123456";

				PlayerSettings.applicationIdentifier = "com.simplegame.tzfn";
			}
			PlayerSettings.Android.bundleVersionCode += 1;
		}
		if (target == "IOS") {
			target_dir = applicationPath + "/TargetIOS";
			target_name = app_name;
			buildTarget = BuildTarget.iOS;
			buildTargetGroup = BuildTargetGroup.iOS;
			PlayerSettings.iOS.buildNumber =  (int.Parse(PlayerSettings.iOS.buildNumber) +1).ToString();
		}

		//每次build删除之前的残留
		if (Directory.Exists (target_dir)) {
			if (File.Exists (target_name)) {
				File.Delete (target_name);
			}
		} else {
			Directory.CreateDirectory (target_dir);
		}

//		string lastCode =  PlayerSettings.bundleVersion;
//		List<int> lastCodes = CommonUtil.GetIntSplitString (lastCode, '.');
//		lastCodes [lastCodes.Count - 1] = lastCodes [lastCodes.Count - 1] + 1;
//		var str = CommonUtil.CombineList (lastCodes, '.');
//		str = str.Substring (0, str.Length - 1);
//		PlayerSettings.bundleVersion = str;

		//==================这里是比较重要的东西=======================

		//开始Build场景，等待吧～
		//得到工程中所有场景名称
		string[] SCENES = FindEnabledEditorScenes ();
		if (SCENES.Length != 0) {
			Debug.Log (string.Format ("{0:yyyyMMddHHmmssffff}", DateTime.Now));

			GenericBuild (SCENES, target_dir + "/" + target_name, buildTarget, buildOptions);
		}
	}

	private static string[] FindEnabledEditorScenes ()
	{
		List<string> EditorScenes = new List<string> ();
		foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes) {
			if (!scene.enabled)
				continue;
			EditorScenes.Add (scene.path);
		}
		return EditorScenes.ToArray ();
	}

	static void GenericBuild (string[] scenes, string target_dir, BuildTarget build_target, BuildOptions build_options)
	{
		//EditorUserBuildSettings.SwitchActiveBuildTarget(build_target);
		var res = BuildPipeline.BuildPlayer (scenes, target_dir, build_target, build_options);

		// if (res..Length > 0) {
		// 	throw new Exception ("BuildPlayer failure: " + res);
		// }
	}

	private static void CopyFolder (string from, string to)
	{
		if (!Directory.Exists (to))
			Directory.CreateDirectory (to);

		// 子文件夹
		foreach (string sub in Directory.GetDirectories(from))
			CopyFolder (sub + "/", to + Path.GetFileName (sub) + "/");

		// 文件
		foreach (string file in Directory.GetFiles(from))
			File.Copy (file, to + Path.GetFileName (file), true);
	}
}
