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

write-host "Cloning the repo $remote_repo with the gh-pages branch"
git clone $remote_repo --branch gh-pages $TEMP_REPO_DIR

#write-host "Clear repo directory"
#Set-Location $TEMP_REPO_DIR
#git rm -r *

write-host "Copy documentation into the repo"
mkdir "15.2"
Copy-Item "$SOURCE_DIR\_site\15.2\*" .\15.2\ -Recurse -force

write-host "Push the new docs to the remote branch"
git config --local user.email "github-actions[bot]@users.noreply.sdl.com"
git config --local user.name "github-actions[bot]"
git add .\15.2 -A
git commit -m "Update generated documentation"

git fetch              # update 'gh-pages' from remote
git tag base gh-pages    # mark our base point
git rebase -i gh-pages   # rewrite some commits
git push --force-with-lease=gh-pages:base gh-pages:gh-pages
