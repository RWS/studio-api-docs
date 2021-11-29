param(
[string]$ACTOR="user",
[string]$TOKEN="password"
)

write-host  "Build the documentation site" 
write-host  "`ACTOR is $ACTOR" 

$SOURCE_DIR=$psscriptroot
$TEMP_REPO_DIR=[System.IO.Path]::GetFullPath("$psscriptroot/../docs-gh-pages")

$remote_repo="https://github-actions:${TOKEN}@github.com/sdl/studio-api-docs.git"

write-host "Cloning the repo $remote_repo with the gh-pages branch"
git clone $remote_repo --branch gh-pages $TEMP_REPO_DIR
Set-Location $TEMP_REPO_DIR
git checkout -b gh-pages_temp
$items = ls
foreach ($item in $items){
 if (($item -ne "15.2") -and ($item -ne "16.1")){
  git -rm $item -r
 }
}
write-host "Copy documentation into the repo"

Copy-Item "$SOURCE_DIR\_site\*" .\ -Recurse -force

write-host "Push the new docs to the remote branch"
git config --local user.email "github-actions[bot]@users.noreply.sdl.com"
git config --local user.name "github-actions[bot]"
git add .\ -A
git commit -m "Update generated documentation"
git push "$remote_repo" HEAD:gh-pages_temp
Write-Output (${TOKEN}) | gh auth login --with-token
gh pr create --title "Update generated documentation" --body "Update generated documentation" -H gh-pages_temp -B gh-pages
