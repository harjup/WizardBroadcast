using UnityEditor;

public class Force2DAudio : AssetPostprocessor {
    void OnPreprocessAudio () {
        var importer = (AudioImporter) assetImporter;
        importer.threeD = false;
    }
}
