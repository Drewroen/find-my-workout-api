param($commit_message)
cd ../..

write-host "Committing/pushing to github..."

git add .
git commit -m $commit_message
git push

write-host "Pushed to github"

write-host "Building artifact..."

.\build.ps1

write-host "Artifact built"

write-host "Deploying using serverless..."

serverless deploy -v

write-host "Deployed using serverless"

cd .\find-my-workout-api\WhatsTheWorkout