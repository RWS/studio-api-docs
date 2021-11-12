param(
[string]$ACTOR="user",
[string]$TOKEN="password"
)

write-host  "Build the documentation site" 
write-host  "`ACTOR is $ACTOR" 

$SOURCE_DIR=$psscriptroot
$TEMP_REPO_DIR=[System.IO.Path]::GetFullPath("$psscriptroot/../docs-gh-pages")

$remote_repo="https://github-actions:${TOKEN}@github.com/sdl/studio-api-docs.git"

write-host "Removing temporary doc directory $TEMP_REPO_DIR"
Remove-Item $TEMP_REPO_DIR -Force -Recurse -ErrorAction Ignore
mkdir $TEMP_REPO_DIR

write-host "Cloning the repo $remote_repo with the gh-pages-vers_test branch"
git clone $remote_repo --branch gh-pages-vers_test $TEMP_REPO_DIR

#write-host "Clear repo directory"
#Set-Location $TEMP_REPO_DIR
#git rm -r *

write-host "Copy documentation into the repo"
Copy-Item "$SOURCE_DIR\_site\*" . -Recurse -force

write-host "Push the new docs to the remote branch"
git config --local user.email "github-actions[bot]@users.noreply.sdl.com"
git config --local user.name "github-actions[bot]"
git add . -A
git commit -m "Update generated documentation"

git push "$remote_repo" HEAD:gh-pages-vers_test