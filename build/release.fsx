// include Fake lib
#r @"../tools/FAKE/FakeLib.dll"
open Fake

// Custom function (see https://github.com/fsharp/FAKE/blob/master/src/app/FakeLib/MSBuildHelper.fs#L413-413)
let MSBuildReleaseConfigured solutionDir outputPath targets projects = MSBuild outputPath targets 
                                                                               [ "Configuration", "Release"; 
                                                                                 "SolutionDir", solutionDir; 
                                                                                 "BuildProjectReferences", "false" 
                                                                               ] 
                                                                               projects

// Utility function
let toProject name = name + "/" + name + ".csproj"

// Target directories
let buildDir = "../.binaries/"
let sdkDir = buildDir + "motoi.sdk/"

// Project sets
let srcFolder = "../src/"
let libsFolder = srcFolder + "libs/"
let pluginsFolder = srcFolder + "plug-ins/"
let sdkProjectSet = [ libsFolder + toProject "PTP" 
                      libsFolder + toProject "Tessa"
                    ]

let platformProjectSet = [ pluginsFolder + toProject "motoi.platform.application"
                         ]

// Clean dir
Target "CleanDir" (fun _ -> 
    CleanDir sdkDir
)

// Default target
Target "Default" (fun _ ->
    MSBuildRelease sdkDir "Build" sdkProjectSet |> Log "Build: "
    let solutionDir = (directoryInfo ".").Parent.FullName + "/src/"
    MSBuildReleaseConfigured solutionDir sdkDir "Build" platformProjectSet |> Log "Build: "
)

// Dependencies
"CleanDir"
    ==> "Default"

// start build
RunTargetOrDefault "Default"