using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.IO;
public class RemoveLd64Flag
{
[PostProcessBuild(9999)]  // SEHR hohe Priorität
public static void OnPostProcessBuild(BuildTarget buildTarget, string pathToBuiltProject)
{
if (buildTarget != BuildTarget.iOS)
return;
string projectPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
PBXProject project = new PBXProject();
project.ReadFromFile(projectPath);
string unityFrameworkGuid = project.GetUnityFrameworkTargetGuid();
// ALLE 4 Build Configurations behandeln!
string[] configNames = new string[] { 
"Debug", 
"Release", 
"ReleaseForProfiling", 
"ReleaseForRunning" 
};
foreach (string configName in configNames)
{
string configGuid = project.BuildConfigByName(unityFrameworkGuid, configName);
if (!string.IsNullOrEmpty(configGuid))
{
project.UpdateBuildPropertyForConfig(configGuid, "OTHER_LDFLAGS", 
null, new string[] { "-ld64", "-ld_classic", "-weak-lSystem" });
UnityEngine.Debug.Log($"✓ Removed flags from {configName}");
}
}
project.WriteToFile(projectPath);
UnityEngine.Debug.Log("=== All legacy linker flags removed from all configs ===");
}
}