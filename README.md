> ### [Project Requirements](https://tdquang7.notion.site/Project-batch-rename-2022-9dc9eb9c9d674dbdb4a988a3794d1335)

# Setup
1. Use Visual Studio 2022 with **.NET 7.0** installed.

2. Pull the repo using GitHub Desktop.

3. Open the project in VS.

4. Make sure to use **Debug** build mode.

5. *Tools > NuGet Package Manager > Manage ... > Installed*.

6. Check if `WindowsAPICodePack-Shell` had been installed for **FinalProject** (if not, install it).

![](doc/res/vs-nuget.png)

7. ***Clean solution** > Build solution*. Copy all **.dll files** of every plugin into FinalProject's Debug folder (`FinalProject\bin\Debug\net6.0-windows`). :warning::warning::warning: _**This step must be repeated everytime there are any changes to the IRule interface or any rule file!**_ :warning::warning::warning:

8. Pray that it runs :smiling_face_with_tear:


# Contribute
1. On [GitHub website](https://github.com/itsdmd/CS202-Final), go to [**Issues**](https://github.com/itsdmd/CS202-Final/issues).

2. Find an issue you want to work on. If it's not there, create one.

3. Click on the issue and self assign.

![](doc/res/gh-self-assign.png)

---

4. Add `wip` label.

![](doc/res/gh-wip-label.png)

---

5. Create a branch with a short descriptive name (eg. 'Adding a suffix' > 'suffix' as branch name).

![](doc/res/gh-create-branch.png)

---

6. In GitHub Desktop, switch to the branch you just created and start editing.

![](doc/res/ghd-switch-branch.png)

---

7. After finish editing, commit and push all changes. After that, create a PR.

![](doc/res/ghd-create-pr.png)

---

8. Remove `wip` label and add `pending` label.

![](doc/res/gh-pending-label.png)
