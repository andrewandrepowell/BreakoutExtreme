# !/bin/bash -e

branch=$(git rev-parse --abbrev-ref HEAD)
echo $branch

if [[ "$branch" != "master" ]]; then
  echo "Must be on the master branch."
  exit 1;	
fi

git branch -D gh-pages
git checkout -b gh-pages
mkdir docs
cp -r bin/Release/net8.0/browser-wasm/publish/wwwroot/* docs
touch docs/.nojekyll
git add docs
git commit -m "Updating GitHub Pages with Game."
git push origin gh-pages --force
git checkout master
