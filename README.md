OSM Importer
============

(forked from https://github.com/OffenesJena/OSMImports)
A tiny tool to query the Open Street map database via the Overpass API and to convert the result to GeoJSON. 

- added BBox filter
- removed all blockchain related dependencies

The examples shows how to query natural features (peaks, saddles, lakes, ...)

In order to query all those features in a bbox : 

    BoundingBox bboxLauzannier = new BoundingBox(44.34815879690078, 6.780796051025391, 44.45878010882453, 6.961898803710937);
     
    new OverpassQuery(bboxLauzannier)
				.WithNodes("natural", "peak")
				.WithNodes("natural", "saddle")
				.WithWays("waterway", "river")
				.WithWays("natural", "water")
				.WithRelations("landuse", "reservoir")
				.WithRelations("natural", "water")
				.ToGeoJSONFile("output/full.geojson")
			   .RunNow();
