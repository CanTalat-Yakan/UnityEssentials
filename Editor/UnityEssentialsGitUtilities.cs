#if UNITY_EDITOR
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;

namespace UnityEssentials
{
    public enum UnityEssentialsCloneMode
    {
        SkipIfExists = 0,
        PullIfExists = 1,
        RecloneIfExists = 2,
    }

    public static class UnityEssentialsGitUtilities
    {
        public readonly struct GitResult
        {
            public readonly int exitCode;
            public readonly string stdOut;
            public readonly string stdErr;

            public bool IsSuccess => exitCode == 0;

            public GitResult(int exitCode, string stdOut, string stdErr)
            {
                this.exitCode = exitCode;
                this.stdOut = stdOut;
                this.stdErr = stdErr;
            }
        }

        public static bool IsGitAvailable(out string versionOrError)
        {
            try
            {
                var r = RunGit("--version", workingDirectory: null, timeoutSeconds: 10);
                if (r.IsSuccess)
                {
                    versionOrError = string.IsNullOrWhiteSpace(r.stdOut) ? "git" : r.stdOut.Trim();
                    return true;
                }

                versionOrError = string.IsNullOrWhiteSpace(r.stdErr) ? "git not found" : r.stdErr.Trim();
                return false;
            }
            catch (Exception e)
            {
                versionOrError = e.Message;
                return false;
            }
        }

        public static string GetRepositoriesRootAbsolute()
        {
            var projectRoot = Directory.GetParent(Application.dataPath)?.FullName;
            if (string.IsNullOrEmpty(projectRoot))
                projectRoot = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));

            return Path.GetFullPath(Path.Combine(projectRoot, "Assets", "Repositories"));
        }

        public static string SanitizeFolderName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return "Repo";

            foreach (var c in Path.GetInvalidFileNameChars())
                name = name.Replace(c, '_');

            // Avoid trailing dots/spaces on Windows.
            return name.Trim().TrimEnd('.');
        }

        public static bool CloneOrUpdate(
            string repoHttpsUrl,
            string branch,
            string destinationPathAbsolute,
            UnityEssentialsCloneMode mode,
            out string userFacingError)
        {
            userFacingError = null;

            if (string.IsNullOrWhiteSpace(repoHttpsUrl))
            {
                userFacingError = "Repo URL is empty.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(destinationPathAbsolute))
            {
                userFacingError = "Destination path is empty.";
                return false;
            }

            destinationPathAbsolute = Path.GetFullPath(destinationPathAbsolute);
            var destParent = Directory.GetParent(destinationPathAbsolute);
            if (destParent == null)
            {
                userFacingError = "Invalid destination path.";
                return false;
            }

            Directory.CreateDirectory(destParent.FullName);

            bool destExists = Directory.Exists(destinationPathAbsolute);
            bool isGitRepo = Directory.Exists(Path.Combine(destinationPathAbsolute, ".git"));

            if (destExists)
            {
                switch (mode)
                {
                    case UnityEssentialsCloneMode.SkipIfExists:
                        return true;

                    case UnityEssentialsCloneMode.PullIfExists:
                        if (!isGitRepo)
                        {
                            userFacingError = "Destination exists but is not a git repo (missing .git).";
                            return false;
                        }

                        if (!string.IsNullOrWhiteSpace(branch))
                        {
                            var checkout = RunGit($"checkout {EscapeArg(branch)}", destinationPathAbsolute);
                            if (!checkout.IsSuccess)
                            {
                                userFacingError = checkout.stdErr;
                                return false;
                            }
                        }

                        var pull = RunGit("pull --ff-only", destinationPathAbsolute);
                        if (!pull.IsSuccess)
                        {
                            userFacingError = pull.stdErr;
                            return false;
                        }

                        return true;

                    case UnityEssentialsCloneMode.RecloneIfExists:
                        try
                        {
                            Directory.Delete(destinationPathAbsolute, true);
                        }
                        catch (Exception e)
                        {
                            userFacingError = "Failed to delete existing folder: " + e.Message;
                            return false;
                        }
                        break;
                }
            }

            var branchArg = string.IsNullOrWhiteSpace(branch) ? string.Empty : $" --branch {EscapeArg(branch)}";
            var clone = RunGit($"clone{branchArg} {EscapeArg(repoHttpsUrl)} {EscapeArg(destinationPathAbsolute)}", workingDirectory: destParent.FullName, timeoutSeconds: 60 * 20);
            if (!clone.IsSuccess)
            {
                userFacingError = string.IsNullOrWhiteSpace(clone.stdErr) ? clone.stdOut : clone.stdErr;
                return false;
            }

            return true;
        }

        private static GitResult RunGit(string arguments, string workingDirectory, int timeoutSeconds = 60)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "git",
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = string.IsNullOrEmpty(workingDirectory) ? Environment.CurrentDirectory : workingDirectory,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8,
            };

            using var p = new Process { StartInfo = psi };
            var output = new StringBuilder();
            var error = new StringBuilder();

            p.OutputDataReceived += (_, e) => { if (e.Data != null) output.AppendLine(e.Data); };
            p.ErrorDataReceived += (_, e) => { if (e.Data != null) error.AppendLine(e.Data); };

            if (!p.Start())
                return new GitResult(-1, string.Empty, "Failed to start git process.");

            p.BeginOutputReadLine();
            p.BeginErrorReadLine();

            if (!p.WaitForExit(timeoutSeconds * 1000))
            {
                try
                {
                    // Older Unity/.NET profiles may not have Process.Kill(bool).
                    p.Kill();
                }
                catch (Exception)
                {
                    // Ignore kill failures (the process may have already exited).
                }

                return new GitResult(-1, output.ToString(), $"Git command timed out after {timeoutSeconds}s." + (error.Length > 0 ? "\n" + error : string.Empty));
            }

            return new GitResult(p.ExitCode, output.ToString(), error.ToString());
        }

        private static string EscapeArg(string arg)
        {
            if (arg == null) return "\"\"";
            return "\"" + arg.Replace("\\", "\\\\").Replace("\"", "\\\"") + "\"";
        }
    }
}
#endif

