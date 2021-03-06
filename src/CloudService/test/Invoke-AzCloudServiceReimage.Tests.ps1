$loadEnvPath = Join-Path $PSScriptRoot 'loadEnv.ps1'
if (-Not (Test-Path -Path $loadEnvPath)) {
    $loadEnvPath = Join-Path $PSScriptRoot '..\loadEnv.ps1'
}
. ($loadEnvPath)
$TestRecordingFile = Join-Path $PSScriptRoot 'Invoke-AzCloudServiceReimage.Recording.json'
$currentPath = $PSScriptRoot
while(-not $mockingPath) {
    $mockingPath = Get-ChildItem -Path $currentPath -Recurse -Include 'HttpPipelineMocking.ps1' -File
    $currentPath = Split-Path -Path $currentPath -Parent
}
. ($mockingPath | Select-Object -First 1).FullName

Describe 'Invoke-AzCloudServiceReimage' {

    It 'Reimage cloud service' {
        Invoke-AzCloudServiceReimage -ResourceGroupName $env.ResourceGroupName -CloudServiceName $env.CloudServiceName -RoleInstance $env.RoleInstanceName
    }

    It 'Reimage cloud service via identity' {
        $cloudService = Get-AzCloudService -ResourceGroupName $env.ResourceGroupName -CloudServiceName $env.CloudServiceName
        Invoke-AzCloudServiceReimage -InputObject $cloudService.Id -RoleInstance $env.RoleInstanceName
    }
}
