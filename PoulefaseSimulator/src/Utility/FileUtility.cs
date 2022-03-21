using Newtonsoft.Json;
using System;
using System.IO;

namespace PouleSimulator
{
    public static class FileUtility
    {
        public static bool TryGetFileContentsFromPath(string path, out string fileContents) {
            fileContents = default;

            if(string.IsNullOrEmpty(path)) return false;

            try {
                if(File.Exists(path)) {
                    fileContents = File.ReadAllText(path);
                    return true;
                }
            } catch(Exception e) {
                Console.WriteLine($"[Exception] {e}");
            }

            return false;
        }

        public static bool TryGetFileContentsFromPath(string path, out byte[] fileContents) {
            fileContents = default;

            if(string.IsNullOrEmpty(path)) return false;

            try {
                if(File.Exists(path)) {
                    fileContents = File.ReadAllBytes(path);
                    return true;
                }
            } catch(Exception e) {
                Console.WriteLine($"[Exception] {e}");
            }

            return false;
        }

        public static bool TryGetFileContentsFromJsonFilePath<T>(string path, out T jsonSerializedObject) {
            jsonSerializedObject = default;

            if(!TryGetFileContentsFromPath(path, out string fileContent))
                return false;

            try {
                jsonSerializedObject = JsonConvert.DeserializeObject<T>(fileContent);
                return true;
            } catch(Exception e) {
                Console.WriteLine($"[Exception] {e}");
            }

            return false;
        }

        public static bool TrySaveFileContentsToPath(string path, string fileContents) {
            if(string.IsNullOrEmpty(path)) return false;

            try {
                Directory.CreateDirectory(new FileInfo(path).Directory.FullName);
                File.WriteAllText(path, fileContents);
                return true;
            } catch(Exception e) {
                Console.WriteLine($"[Exception] {e}");
                return false;
            }
        }

        public static bool TrySaveFileContentsToPath(string path, byte[] fileContents) {
            if(string.IsNullOrEmpty(path)) return false;

            try {
                Directory.CreateDirectory(new FileInfo(path).Directory.FullName);
                File.WriteAllBytes(path, fileContents);
                return true;
            } catch(Exception e) {
                Console.WriteLine($"[Exception] {e}");
                return false;
            }
        }

        public static bool TrySaveObjectToPath<T>(string path, T jsonSerializableObject, Formatting formatting = Formatting.None) {
            string fileContents;
            try {
                fileContents = JsonConvert.SerializeObject(jsonSerializableObject, formatting);
            } catch(Exception e) {
                Console.WriteLine($"[Exception] {e}");
                return false;
            }

            if(!TrySaveFileContentsToPath(path, fileContents)) return false;

            return true;
        }

        /// <summary>
        /// Normalizes a path (equalizes slashes/corrects capitalization/does canonicalization) 
        /// </summary>
        /// <param name="path">A relative or absolute path</param>
        /// <returns>Absolute normalized path</returns>
        public static string GetNormalizedPath(string path) {
            return new FileInfo(path).FullName;
        }
    }
}
