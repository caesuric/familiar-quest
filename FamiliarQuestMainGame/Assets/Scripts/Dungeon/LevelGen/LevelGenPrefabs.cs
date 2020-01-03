using System.Collections.Generic;
using UnityEngine;

public class LevelGenPrefabs {
    public static Dictionary<string, Dictionary<string, Dictionary<string, GameObject>>> prefabs = new Dictionary<string, Dictionary<string, Dictionary<string, GameObject>>> {
        { "castle", new Dictionary<string, Dictionary<string, GameObject>> {
            { "floor", new Dictionary<string, GameObject> {
                { "Castle/Floor1", null },
                { "Castle/Floor2", null },
                { "Castle/Floor3", null },
            } },
            { "hallwayFloor", new Dictionary<string, GameObject> {
                { "Castle/Floor4", null }
            } },
            { "doubleWall", new Dictionary<string, GameObject> {
                { "Castle/DoubleWall1", null },
                { "Castle/DoubleWall2", null }
            } },
            { "wall", new Dictionary<string, GameObject> {
                { "Castle/SingleWall", null }
            } },
            { "oneTileWall", new Dictionary<string, GameObject> {
                { "Castle/OneTileWall", null }
            } },
            { "cornerWall", new Dictionary<string, GameObject> {
                { "Castle/CornerWall", null }
            } },
            { "reverseCornerWall", new Dictionary<string, GameObject> {
                { "Castle/ReverseCornerWall", null }
            } },
            { "door", new Dictionary<string, GameObject> {
                { "Castle/Door", null }
            } },
            { "entrance", new Dictionary<string, GameObject> {
                { "Castle/Entrance", null }
            } },
            { "stairsToNext", new Dictionary<string, GameObject> {
                { "Castle/StairsUp", null }
            } },
            { "stairsToLast", new Dictionary<string, GameObject> {
                { "Castle/StairsDown", null }
            } },
            { "beds1", new Dictionary<string, GameObject> {
                { "Dressing/Beds/1/1", null },
                { "Dressing/Beds/1/2", null },
                { "Dressing/Beds/1/3", null }
            } },
            { "beds2", new Dictionary<string, GameObject> {
                { "Dressing/Beds/2/1", null },
                { "Dressing/Beds/2/2", null },
                { "Dressing/Beds/2/3", null },
                { "Dressing/Beds/2/4", null }
            } },
            { "beds3", new Dictionary<string, GameObject> {
                { "Dressing/Beds/3/1", null },
                { "Dressing/Beds/3/2", null },
                { "Dressing/Beds/3/3", null },
                { "Dressing/Beds/3/4", null }
            } },
            { "beds4", new Dictionary<string, GameObject> {
                { "Dressing/Beds/4/1", null },
                { "Dressing/Beds/4/2", null },
                { "Dressing/Beds/4/3", null },
                { "Dressing/Beds/4/4", null }
            } },
            { "beds5", new Dictionary<string, GameObject> {
                { "Dressing/Beds/5/1", null },
                { "Dressing/Beds/5/2", null },
                { "Dressing/Beds/5/3", null }
            } },
            { "beds6", new Dictionary<string, GameObject> {
                { "Dressing/Beds/6/1", null },
                { "Dressing/Beds/6/2", null },
                { "Dressing/Beds/6/3", null }
            } },
            { "beds7", new Dictionary<string, GameObject> {
                { "Dressing/Beds/7/1", null },
                { "Dressing/Beds/7/2", null },
                { "Dressing/Beds/7/3", null }
            } },
            { "beds8", new Dictionary<string, GameObject> {
                { "Dressing/Beds/8/1", null },
                { "Dressing/Beds/8/2", null },
                { "Dressing/Beds/8/3", null }
            } },
            { "beds9", new Dictionary<string, GameObject> {
                { "Dressing/Beds/9/1", null },
                { "Dressing/Beds/9/2", null },
                { "Dressing/Beds/9/3", null }
            } },
            { "torch", new Dictionary<string, GameObject> {
                { "Dressing/Torch1", null }
            } },
            { "livingQuarterLighting", new Dictionary<string, GameObject> {
                { "Dressing/Lit_Candelabra", null }
            } },
            { "livingQuarterSideFurniture", new Dictionary<string, GameObject> {
                { "Dressing/Benches/1", null },
                { "Dressing/Benches/2", null },
                { "Dressing/Chairs/1", null },
                { "Dressing/Chairs/2", null },
                { "Dressing/Chairs/3", null },
                { "Dressing/Chairs/4", null },
                { "Dressing/Dressers/1", null },
                { "Dressing/Dressers/2", null },
                { "Dressing/Dressers/3", null },
                { "Dressing/Stool", null }
            } },
            { "livingQuarterSideFurnitureFancy", new Dictionary<string, GameObject> {
                { "Dressing/Benches/1", null },
                { "Dressing/Benches/2", null },
                { "Dressing/Chairs/F1", null },
                { "Dressing/Chairs/F2", null },
                { "Dressing/Dressers/F1", null },
                { "Dressing/Dressers/F2", null },
                { "Dressing/Dressers/F3", null },
                { "Dressing/Dressers/F4", null },
                { "Dressing/Stool", null }
            } },
            { "commonSpaceSideFurniture", new Dictionary<string, GameObject> {
                { "Dressing/Shelving/1", null },
                { "Dressing/Shelving/2", null },
                { "Dressing/Shelving/3", null },
                { "Dressing/Shelving/4", null },
                { "Dressing/Shelving/5", null },
                { "Dressing/Shelving/6", null },
                { "Dressing/Shelving/7", null },
                { "Dressing/Shelving/8", null },
                { "Dressing/Shelving/9", null }
            } },
            { "commonSpaceSideFurnitureFancy", new Dictionary<string, GameObject> {
                { "Dressing/Chairs/F3", null },
                { "Dressing/Chairs/F4", null },
                { "Dressing/Chairs/F5", null },
                { "Dressing/Shelving/F1", null },
                { "Dressing/Shelving/F2", null },
                { "Dressing/Shelving/F3", null },
                { "Dressing/Shelving/F4", null },
                { "Dressing/Shelving/F5", null },
                { "Dressing/Shelving/F6", null },
                { "Dressing/Shelving/F7", null },
                { "Dressing/Shelving/F8", null },
                { "Dressing/Shelving/F9", null }
            } },
            { "chairs", new Dictionary<string, GameObject> {
                { "Dressing/Chairs/1", null },
                { "Dressing/Chairs/2", null },
                { "Dressing/Chairs/3", null },
                { "Dressing/Chairs/4", null }
            } },
            { "chairsFancy", new Dictionary<string, GameObject> {
                { "Dressing/Chairs/F1", null },
                { "Dressing/Chairs/F2", null }
            } },
            { "nightstandClutter", new Dictionary<string, GameObject> {
                { "Dressing/TableClutter/Books/1", null },
                { "Dressing/TableClutter/Books/2", null },
                { "Dressing/TableClutter/Books/3", null },
                { "Dressing/TableClutter/Books/4", null },
                { "Dressing/TableClutter/Books/5", null },
                { "Dressing/TableClutter/Books/6", null },
                { "Dressing/TableClutter/Candles/1", null },
                { "Dressing/TableClutter/Candles/2", null },
                { "Dressing/TableClutter/Candles/3", null },
                { "Dressing/TableClutter/Candles/4", null },
                { "Dressing/TableClutter/Candles/5", null },
                { "Dressing/TableClutter/Candles/6", null },
                { "Dressing/TableClutter/Candles/7", null },
                { "Dressing/TableClutter/Candles/8", null },
                { "Dressing/TableClutter/Candles/Lit/1", null },
                { "Dressing/TableClutter/Candles/Lit/2", null },
                { "Dressing/TableClutter/Candles/Lit/3", null },
                { "Dressing/TableClutter/Candles/Lit/4", null },
                { "Dressing/TableClutter/Candles/Lit/5", null },
                { "Dressing/TableClutter/Candles/Lit/6", null },
                { "Dressing/TableClutter/Candles/Lit/7", null },
                { "Dressing/TableClutter/Plants/1", null },
                { "Dressing/TableClutter/Plants/2", null }
            } },
            { "tableClutter", new Dictionary<string, GameObject> {
                { "Dressing/TableClutter/Books/1", null },
                { "Dressing/TableClutter/Books/2", null },
                { "Dressing/TableClutter/Books/3", null },
                { "Dressing/TableClutter/Books/4", null },
                { "Dressing/TableClutter/Books/5", null },
                { "Dressing/TableClutter/Books/6", null },
                { "Dressing/TableClutter/Candles/1", null },
                { "Dressing/TableClutter/Candles/2", null },
                { "Dressing/TableClutter/Candles/3", null },
                { "Dressing/TableClutter/Candles/4", null },
                { "Dressing/TableClutter/Candles/5", null },
                { "Dressing/TableClutter/Candles/6", null },
                { "Dressing/TableClutter/Candles/7", null },
                { "Dressing/TableClutter/Candles/8", null },
                { "Dressing/TableClutter/Candles/Lit/1", null },
                { "Dressing/TableClutter/Candles/Lit/2", null },
                { "Dressing/TableClutter/Candles/Lit/3", null },
                { "Dressing/TableClutter/Candles/Lit/4", null },
                { "Dressing/TableClutter/Candles/Lit/5", null },
                { "Dressing/TableClutter/Candles/Lit/6", null },
                { "Dressing/TableClutter/Candles/Lit/7", null },
                { "Dressing/TableClutter/Plants/1", null },
                { "Dressing/TableClutter/Plants/2", null },
                { "Dressing/TableClutter/Paper/1", null },
                { "Dressing/TableClutter/Paper/2", null },
                { "Dressing/TableClutter/Paper/3", null },
                { "Dressing/TableClutter/Paper/4", null },
                { "Dressing/TableClutter/Paper/5", null },
                { "Dressing/TableClutter/Paper/6", null },
                { "Dressing/TableClutter/Paper/7", null },
                { "Dressing/TableClutter/Paper/8", null },
                { "Dressing/TableClutter/Paper/9", null }
            } },
            { "tables", new Dictionary<string, GameObject> {
                { "Dressing/Tables/1", null },
                { "Dressing/Tables/2", null },
                { "Dressing/Tables/3", null },
                { "Dressing/Tables/4", null },
                { "Dressing/Tables/5", null },
                { "Dressing/Tables/6", null },
                { "Dressing/Tables/7", null },
                { "Dressing/Tables/8", null },
                { "Dressing/Tables/9", null },
                { "Dressing/Tables/10", null },
                { "Dressing/Tables/11", null },
                { "Dressing/Tables/12", null }
            } },
            { "tablesFancy", new Dictionary<string, GameObject> {
                { "Dressing/Tables/Fancy/1", null },
                { "Dressing/Tables/Fancy/2", null },
                { "Dressing/Tables/Fancy/3", null },
                { "Dressing/Tables/Fancy/4", null },
                { "Dressing/Tables/Fancy/5", null },
                { "Dressing/Tables/Fancy/6", null }
            } }
        } }
    };
    public static Dictionary<string, Dictionary<string, Dictionary<string, float>>> prefabProbability = new Dictionary<string, Dictionary<string, Dictionary<string, float>>> {
        { "castle", new Dictionary<string, Dictionary<string, float>> {
            { "floor", new Dictionary<string, float> {
                { "Castle/Floor1", 0.9f },
                { "Castle/Floor2", 0.05f },
                { "Castle/Floor3", 0.05f }
            } },
            { "hallwayFloor", new Dictionary<string, float> {
                { "Castle/Floor4", 1f }
            } },
            { "doubleWall", new Dictionary<string, float> {
                { "Castle/DoubleWall1", 0.5f },
                { "Castle/DoubleWall2", 0.5f }
            } },
            { "wall", new Dictionary<string, float> {
                { "Castle/SingleWall", 1f }
            } },
            { "oneTileWall", new Dictionary<string, float> {
                { "Castle/OneTileWall", 1f }
            } },
            { "cornerWall", new Dictionary<string, float> {
                { "Castle/CornerWall", 1f }
            } },
            { "reverseCornerWall", new Dictionary<string, float> {
                { "Castle/ReverseCornerWall", 1f }
            } },
            { "door", new Dictionary<string, float> {
                { "Castle/Door", 1f }
            } },
            { "entrance", new Dictionary<string, float> {
                { "Castle/Entrance", 1f }
            } },
            { "stairsToNext", new Dictionary<string, float> {
                { "Castle/StairsUp", 1f }
            } },
            { "stairsToLast", new Dictionary<string, float> {
                { "Castle/StairsDown", 1f }
            } },
            { "beds1", new Dictionary<string, float> {
                { "Dressing/Beds/1/1", 0.45f },
                { "Dressing/Beds/1/2", 0.45f },
                { "Dressing/Beds/1/3", 0.1f }
            } },
            { "beds2", new Dictionary<string, float> {
                { "Dressing/Beds/2/1", 0.45f },
                { "Dressing/Beds/2/2", 0.45f },
                { "Dressing/Beds/2/3", 0.05f },
                { "Dressing/Beds/2/4", 0.05f }
            } },
            { "beds3", new Dictionary<string, float> {
                { "Dressing/Beds/3/1", 0.45f },
                { "Dressing/Beds/3/2", 0.45f },
                { "Dressing/Beds/3/3", 0.05f },
                { "Dressing/Beds/3/4", 0.05f }
            } },
            { "beds4", new Dictionary<string, float> {
                { "Dressing/Beds/4/1", 0.45f },
                { "Dressing/Beds/4/2", 0.45f },
                { "Dressing/Beds/4/3", 0.05f },
                { "Dressing/Beds/4/4", 0.05f }
            } },
            { "beds5", new Dictionary<string, float> {
                { "Dressing/Beds/5/1", 0.45f },
                { "Dressing/Beds/5/2", 0.45f },
                { "Dressing/Beds/5/3", 0.1f }
            } },
            { "beds6", new Dictionary<string, float> {
                { "Dressing/Beds/6/1", 0.45f },
                { "Dressing/Beds/6/2", 0.45f },
                { "Dressing/Beds/6/3", 0.1f }
            } },
            { "beds7", new Dictionary<string, float> {
                { "Dressing/Beds/7/1", 0.45f },
                { "Dressing/Beds/7/2", 0.45f },
                { "Dressing/Beds/7/3", 0.1f }
            } },
            { "beds8", new Dictionary<string, float> {
                { "Dressing/Beds/8/1", 0.45f },
                { "Dressing/Beds/8/2", 0.45f },
                { "Dressing/Beds/8/3", 0.1f }
            } },
            { "beds9", new Dictionary<string, float> {
                { "Dressing/Beds/9/1", 0.45f },
                { "Dressing/Beds/9/2", 0.45f },
                { "Dressing/Beds/9/3", 0.1f }
            } },
            { "torch", new Dictionary<string, float> {
                { "Dressing/Torch1", 1f }
            } },
            { "livingQuarterLighting", new Dictionary<string, float> {
                { "Dressing/Lit_Candelabra", 1f }
            } },
            { "livingQuarterSideFurniture", new Dictionary<string, float> {
                { "Dressing/Benches/1", 0.1f },
                { "Dressing/Benches/2", 0.1f },
                { "Dressing/Chairs/1", 0.1f },
                { "Dressing/Chairs/2", 0.1f },
                { "Dressing/Chairs/3", 0.1f },
                { "Dressing/Chairs/4", 0.1f },
                { "Dressing/Dressers/1", 0.1f },
                { "Dressing/Dressers/2", 0.1f },
                { "Dressing/Dressers/3", 0.1f },
                { "Dressing/Stool", 0.1f }
            } },
            { "livingQuarterSideFurnitureFancy", new Dictionary<string, float> {
                { "Dressing/Benches/1", 0.11f },
                { "Dressing/Benches/2", 0.11f },
                { "Dressing/Chairs/F1", 0.11f },
                { "Dressing/Chairs/F2", 0.11f },
                { "Dressing/Dressers/F1", 0.11f },
                { "Dressing/Dressers/F2", 0.11f },
                { "Dressing/Dressers/F3", 0.11f },
                { "Dressing/Dressers/F4", 0.11f },
                { "Dressing/Stool", 0.12f }
            } },
            { "commonSpaceSideFurniture", new Dictionary<string, float> {
                { "Dressing/Shelving/1", 0.11f },
                { "Dressing/Shelving/2", 0.11f },
                { "Dressing/Shelving/3", 0.11f },
                { "Dressing/Shelving/4", 0.11f },
                { "Dressing/Shelving/5", 0.11f },
                { "Dressing/Shelving/6", 0.11f },
                { "Dressing/Shelving/7", 0.11f },
                { "Dressing/Shelving/8", 0.11f },
                { "Dressing/Shelving/9", 0.12f }
            } },
            { "commonSpaceSideFurnitureFancy", new Dictionary<string, float> {
                { "Dressing/Chairs/F3", 0.083f },
                { "Dressing/Chairs/F4", 0.083f },
                { "Dressing/Chairs/F5", 0.083f },
                { "Dressing/Shelving/F1", 0.083f },
                { "Dressing/Shelving/F2", 0.083f},
                { "Dressing/Shelving/F3", 0.083f },
                { "Dressing/Shelving/F4", 0.083f },
                { "Dressing/Shelving/F5", 0.083f },
                { "Dressing/Shelving/F6", 0.083f },
                { "Dressing/Shelving/F7", 0.083f },
                { "Dressing/Shelving/F8", 0.083f },
                { "Dressing/Shelving/F9", 0.087f }
            } },
            { "chairs", new Dictionary<string, float> {
                { "Dressing/Chairs/1", 0.25f },
                { "Dressing/Chairs/2", 0.25f },
                { "Dressing/Chairs/3", 0.25f },
                { "Dressing/Chairs/4", 0.25f }
            } },
            { "chairsFancy", new Dictionary<string, float> {
                { "Dressing/Chairs/F1", 0.5f },
                { "Dressing/Chairs/F2", 0.5f }
            } },
            { "nightstandClutter", new Dictionary<string, float> {
                { "Dressing/TableClutter/Books/1", 0.04f },
                { "Dressing/TableClutter/Books/2", 0.04f },
                { "Dressing/TableClutter/Books/3", 0.04f },
                { "Dressing/TableClutter/Books/4", 0.04f },
                { "Dressing/TableClutter/Books/5", 0.04f },
                { "Dressing/TableClutter/Books/6", 0.05f },
                { "Dressing/TableClutter/Candles/1", 0.0166f },
                { "Dressing/TableClutter/Candles/2", 0.0166f },
                { "Dressing/TableClutter/Candles/3", 0.0166f },
                { "Dressing/TableClutter/Candles/4", 0.0166f },
                { "Dressing/TableClutter/Candles/5", 0.0166f },
                { "Dressing/TableClutter/Candles/6", 0.0166f },
                { "Dressing/TableClutter/Candles/7", 0.0166f },
                { "Dressing/TableClutter/Candles/8", 0.0176f },
                { "Dressing/TableClutter/Candles/Lit/1", 0.0166f },
                { "Dressing/TableClutter/Candles/Lit/2", 0.0166f },
                { "Dressing/TableClutter/Candles/Lit/3", 0.0166f },
                { "Dressing/TableClutter/Candles/Lit/4", 0.0166f },
                { "Dressing/TableClutter/Candles/Lit/5", 0.0166f },
                { "Dressing/TableClutter/Candles/Lit/6", 0.0166f },
                { "Dressing/TableClutter/Candles/Lit/7", 0.0166f },
                { "Dressing/TableClutter/Plants/1", 0.125f },
                { "Dressing/TableClutter/Plants/2", 0.125f }
            } },
            { "tableClutter", new Dictionary<string, float> {
                { "Dressing/TableClutter/Books/1", 0.047f },
                { "Dressing/TableClutter/Books/2", 0.047f },
                { "Dressing/TableClutter/Books/3", 0.047f },
                { "Dressing/TableClutter/Books/4", 0.047f },
                { "Dressing/TableClutter/Books/5", 0.047f },
                { "Dressing/TableClutter/Books/6", 0.047f },
                { "Dressing/TableClutter/Candles/1", 0.017f },
                { "Dressing/TableClutter/Candles/2", 0.017f },
                { "Dressing/TableClutter/Candles/3", 0.017f },
                { "Dressing/TableClutter/Candles/4", 0.017f },
                { "Dressing/TableClutter/Candles/5", 0.017f },
                { "Dressing/TableClutter/Candles/6", 0.017f },
                { "Dressing/TableClutter/Candles/7", 0.017f },
                { "Dressing/TableClutter/Candles/8", 0.017f },
                { "Dressing/TableClutter/Candles/Lit/1", 0.02f },
                { "Dressing/TableClutter/Candles/Lit/2", 0.02f },
                { "Dressing/TableClutter/Candles/Lit/3", 0.02f },
                { "Dressing/TableClutter/Candles/Lit/4", 0.02f },
                { "Dressing/TableClutter/Candles/Lit/5", 0.02f },
                { "Dressing/TableClutter/Candles/Lit/6", 0.02f },
                { "Dressing/TableClutter/Candles/Lit/7", 0.02f },
                { "Dressing/TableClutter/Plants/1", 0.07f },
                { "Dressing/TableClutter/Plants/2", 0.07f },
                { "Dressing/TableClutter/Paper/1", 0.03f },
                { "Dressing/TableClutter/Paper/2", 0.03f },
                { "Dressing/TableClutter/Paper/3", 0.03f },
                { "Dressing/TableClutter/Paper/4", 0.03f },
                { "Dressing/TableClutter/Paper/5", 0.03f },
                { "Dressing/TableClutter/Paper/6", 0.03f },
                { "Dressing/TableClutter/Paper/7", 0.03f },
                { "Dressing/TableClutter/Paper/8", 0.03f },
                { "Dressing/TableClutter/Paper/9", 0.062f }
            } },
            { "tables", new Dictionary<string, float> {
                { "Dressing/Tables/1", 0.087f },
                { "Dressing/Tables/2", 0.083f },
                { "Dressing/Tables/3", 0.083f },
                { "Dressing/Tables/4", 0.083f },
                { "Dressing/Tables/5", 0.083f },
                { "Dressing/Tables/6", 0.083f },
                { "Dressing/Tables/7", 0.083f },
                { "Dressing/Tables/8", 0.083f },
                { "Dressing/Tables/9", 0.083f },
                { "Dressing/Tables/10", 0.083f },
                { "Dressing/Tables/11", 0.083f },
                { "Dressing/Tables/12", 0.083f }
            } },
            { "tablesFancy", new Dictionary<string, float> {
                { "Dressing/Tables/Fancy/1", 0.17f },
                { "Dressing/Tables/Fancy/2", 0.166f },
                { "Dressing/Tables/Fancy/3", 0.166f },
                { "Dressing/Tables/Fancy/4", 0.166f },
                { "Dressing/Tables/Fancy/5", 0.166f },
                { "Dressing/Tables/Fancy/6", 0.166f }
            } }
        } }
    };
}
