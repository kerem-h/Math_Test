using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    #region Singleton
    public static BuildManager Instance;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }
    #endregion

    public enum BuildType
    {
     CSO,
     Tours1,
     Tours2,
     Eopan,
     Debug,
     Alat,
     Suites
    }
    public BuildType buildType;
    
    public void SetBuildOptions() {
        switch (buildType)
        {
            case BuildType.CSO:
                // GameData.QuestionDatabaseUrls = new List<string> { "https://storage.googleapis.com/math-database/cso.csv" };
                GameData.QuestionDatabaseUrls = new List<string> { "https://devenez-pilote.fr/download_csv.php?file=cso.csv" };

                GameData.TestTimes = new[] { 1080f };
                GameData.QuestionCount = new[] {26};
                break;
            case BuildType.Tours1:
                // GameData.QuestionDatabaseUrls = new List<string> { "https://storage.googleapis.com/math-database/tours_1.csv" };
                GameData.QuestionDatabaseUrls = new List<string> { "https://devenez-pilote.fr/download_csv.php?file=tours_1.csv" };

                GameData.TestTimes = new[] { 2400f };
                GameData.QuestionCount = new[] {25};
                break;
            case BuildType.Tours2:
                // GameData.QuestionDatabaseUrls = new List<string> { "https://storage.googleapis.com/math-database/tours_2.csv" };
                GameData.QuestionDatabaseUrls = new List<string> { "https://devenez-pilote.fr/download_csv.php?file=tours_2.csv" };

                GameData.TestTimes = new[] { 2400f };
                GameData.QuestionCount = new[] {25};
                break;
            case BuildType.Eopan:
                GameData.QuestionDatabaseUrls = new List<string> { "https://devenez-pilote.fr/download_csv.php?file=eopan.csv" };

                GameData.TestTimes = new[] { 1080f };
                GameData.QuestionCount = new[] {24};
                break;
            case BuildType.Debug:
                // GameData.QuestionDatabaseUrls = new List<string> { "https://storage.googleapis.com/math-database/debug.csv" };
                GameData.QuestionDatabaseUrls = new List<string> { "https://devenez-pilote.fr/download_csv.php?file=debug.csv" };
                DebugManager.Instance.IsDebugBuild = true;
                // GameData.QuestionCount = new[] {25};
                GameData.TestTimes = new[] { 99999f };
                break;
            case BuildType.Suites:
                GameData.QuestionDatabaseUrls = new List<string> { "https://devenez-pilote.fr/download_csv.php?file=suites.csv" };
                // DebugManager.Instance.IsDebugBuild = true;
                GameData.QuestionCount = new[] {20};
                GameData.TestTimes = new[] { 720f };
                break;
            case BuildType.Alat:
                GameData.QuestionDatabaseUrls = new List<string> { "https://devenez-pilote.fr/download_csv.php?file=suites.csv" };
                // DebugManager.Instance.IsDebugBuild = true;
                GameData.QuestionCount = new[] { 20 };
                GameData.TestTimes = new[] { 720f };
                break;

        }
    }
}
