/*
 * Copyright (c) 2014, Achim 'ahzf' Friedland <achim@graphdefined.org>
 * This file is part of OpenDataAPI <http://www.github.com/GraphDefined/OpenDataAPI>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#region Usings

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.OpenDataAPI.OverpassAPI;
using Newtonsoft.Json.Linq;

#endregion

namespace org.GraphDefined.OpenDataAPI.OSMImporter
{

    public static class Extentions
    {

        #region RunAll(this OverpassQuery, Filename)

        /// <summary>
        /// Standard workflow...
        /// </summary>
        /// <param name="OverpassQuery">An Overpass query.</param>
        /// <param name="FilenamePrefix">A file name prefix.</param>
        public static void RunAll(this OverpassQuery OverpassQuery,
                                    String FilenamePrefix)
        {

            OverpassQuery.
                ToFile(FilenamePrefix + ".json").
                ToGeoJSONFile(FilenamePrefix + ".geojson").
                ContinueWith(task => Console.WriteLine(FilenamePrefix + ".* files are ready!")).
                Wait();

        }

        #endregion

    }


    /// <summary>
    /// A little demo... can be tested via http://overpass-turbo.eu
    /// </summary>
    public class Program
    {

        /// <summary>
        /// Main...
        /// </summary>
        /// <param name="Arguments">CLI arguments...</param>
        public static async Task Main(String[] Arguments)
        {
            BoundingBox bboxAixTest = new BoundingBox(43.52705193777889, 5.446827714454083, 43.52763146564737, 5.44591576338902);
            var json = await GetBuildings(bboxAixTest);


            // -----------------------------------------------------------------

            Console.WriteLine("ready...");
            Console.ReadLine();

        }

        public static Task<JObject> GetBuildings(BoundingBox inputBBox)
        {
            try
            {
                BoundingBox bbox = new BoundingBox(Math.Min(inputBBox.YMin, inputBBox.YMax)
                    , Math.Min(inputBBox.XMin, inputBBox.XMax)
                    , Math.Max(inputBBox.YMin, inputBBox.YMax)
                    , Math.Max(inputBBox.XMin, inputBBox.XMax));
                Directory.CreateDirectory("output");


                return new OverpassQuery(bbox)
                    .WithWays("building")
                    .ToGeoJSON();

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public static void GetDepartements()
        {


            int msDelay = 2000;
            Directory.CreateDirectory("output");

            #region French "départements"
            Dictionary<string, string> v_frenchDepartements = new Dictionary<string, string>();
            v_frenchDepartements.Add("01", "Ain");
            v_frenchDepartements.Add("02", "Aisne");
            v_frenchDepartements.Add("03", "Allier");
            v_frenchDepartements.Add("04", "Alpes de Haute-Provence");
            v_frenchDepartements.Add("05", "Hautes-Alpes");
            v_frenchDepartements.Add("06", "Alpes-Maritimes");
            v_frenchDepartements.Add("07", "Ardèche");
            v_frenchDepartements.Add("08", "Ardennes");
            v_frenchDepartements.Add("09", "Ariège");
            v_frenchDepartements.Add("10", "Aube");
            v_frenchDepartements.Add("11", "Aude");
            v_frenchDepartements.Add("12", "Aveyron");
            v_frenchDepartements.Add("13", "Bouches du Rhône");
            v_frenchDepartements.Add("14", "Calvados");
            v_frenchDepartements.Add("15", "Cantal");
            v_frenchDepartements.Add("16", "Charente");
            v_frenchDepartements.Add("17", "Charente Maritime");
            v_frenchDepartements.Add("18", "Cher");
            v_frenchDepartements.Add("19", "Corrèze");
            v_frenchDepartements.Add("21", "Côte d'Or");
            v_frenchDepartements.Add("22", "Côtes d'Armor");
            v_frenchDepartements.Add("23", "Creuse");
            v_frenchDepartements.Add("24", "Dordogne");
            v_frenchDepartements.Add("25", "Doubs");
            v_frenchDepartements.Add("26", "Drôme");
            v_frenchDepartements.Add("27", "Eure");
            v_frenchDepartements.Add("28", "Eure-et-Loir");
            v_frenchDepartements.Add("29", "Finistère");
            v_frenchDepartements.Add("30", "Gard");
            v_frenchDepartements.Add("31", "Haute-Garonne");
            v_frenchDepartements.Add("32", "Gers");
            v_frenchDepartements.Add("33", "Gironde");
            v_frenchDepartements.Add("34", "Hérault");
            v_frenchDepartements.Add("35", "Ille-et-Vilaine");
            v_frenchDepartements.Add("36", "Indre");
            v_frenchDepartements.Add("37", "Indre-et-Loire");
            v_frenchDepartements.Add("38", "Isère");
            v_frenchDepartements.Add("39", "Jura");
            v_frenchDepartements.Add("40", "Landes");
            v_frenchDepartements.Add("41", "Loir-et-Cher");
            v_frenchDepartements.Add("42", "Loire");
            v_frenchDepartements.Add("43", "Haute-Loire");
            v_frenchDepartements.Add("44", "Loire-Atlantique");
            v_frenchDepartements.Add("45", "Loiret");
            v_frenchDepartements.Add("46", "Lot");
            v_frenchDepartements.Add("47", "Lot-et-Garonne");
            v_frenchDepartements.Add("48", "Lozère");
            v_frenchDepartements.Add("49", "Maine-et-Loire");
            v_frenchDepartements.Add("50", "Manche");
            v_frenchDepartements.Add("51", "Marne");
            v_frenchDepartements.Add("52", "Haute-Marne");
            v_frenchDepartements.Add("53", "Mayenne");
            v_frenchDepartements.Add("54", "Meurthe-et-Moselle");
            v_frenchDepartements.Add("55", "Meuse");
            v_frenchDepartements.Add("56", "Morbihan");
            v_frenchDepartements.Add("57", "Moselle");
            v_frenchDepartements.Add("58", "Nièvre");
            v_frenchDepartements.Add("59", "Nord");
            v_frenchDepartements.Add("60", "Oise");
            v_frenchDepartements.Add("61", "Orne");
            v_frenchDepartements.Add("62", "Pas-de-Calais");
            v_frenchDepartements.Add("63", "Puy-de-Dôme");
            v_frenchDepartements.Add("64", "Pyrénées-Atlantiques");
            v_frenchDepartements.Add("65", "Hautes-Pyrénées");
            v_frenchDepartements.Add("66", "Pyrénées-Orientales");
            v_frenchDepartements.Add("67", "Bas-Rhin");
            v_frenchDepartements.Add("68", "Haut-Rhin");
            v_frenchDepartements.Add("69", "Rhône");
            v_frenchDepartements.Add("70", "Haute-Saône");
            v_frenchDepartements.Add("71", "Saône-et-Loire");
            v_frenchDepartements.Add("72", "Sarthe");
            v_frenchDepartements.Add("73", "Savoie");
            v_frenchDepartements.Add("74", "Haute-Savoie");
            v_frenchDepartements.Add("75", "Paris");
            v_frenchDepartements.Add("76", "Seine-Maritime");
            v_frenchDepartements.Add("77", "Seine-et-Marne");
            v_frenchDepartements.Add("78", "Yvelines");
            v_frenchDepartements.Add("79", "Deux-Sèvres");
            v_frenchDepartements.Add("80", "Somme");
            v_frenchDepartements.Add("81", "Tarn");
            v_frenchDepartements.Add("82", "Tarn-et-Garonne");
            v_frenchDepartements.Add("83", "Var");
            v_frenchDepartements.Add("84", "Vaucluse");
            v_frenchDepartements.Add("85", "Vendée");
            v_frenchDepartements.Add("86", "Vienne");
            v_frenchDepartements.Add("87", "Haute-Vienne");
            v_frenchDepartements.Add("88", "Vosges");
            v_frenchDepartements.Add("89", "Yonne");
            v_frenchDepartements.Add("90", "Territoire-de-Belfort");
            v_frenchDepartements.Add("91", "Essonne");
            v_frenchDepartements.Add("92", "Hauts-de-Seine");
            v_frenchDepartements.Add("93", "Seine-St-Denis");
            v_frenchDepartements.Add("94", "Val-de-Marne");
            v_frenchDepartements.Add("95", "Val-d'Oise");
            v_frenchDepartements.Add("2A", "Corse du Sud");
            v_frenchDepartements.Add("2B", "Haute-Corse");
            #endregion


            foreach (var dep in v_frenchDepartements)
            {
                var areaId = new OverpassQuery(dep.Value).AreaId;

                string dirname = string.Concat("output/", dep.Key);

                Directory.CreateDirectory(dirname);

                new OverpassQuery(areaId)
                    .SelectFilter(".relations")
                    .WithNodesHavingRelation("admin_centre")
                    .ToGeoJSONFile(dirname + "/admin_centre.geojson")
                    .RunNow();

                Delay(msDelay);


                //new OverpassQuery(areaId)
                //	 .WithNodes("natural", "peak")
                //		.ToGeoJSONFile(dirname + "/peak.geojson")
                //		.RunNow();

                //Delay(msDelay);

                //new OverpassQuery(areaId)
                //	 .WithNodes("natural", "saddle")
                //		.ToGeoJSONFile(dirname + "/saddle.geojson")
                //		.RunNow();

                //Delay(msDelay);

                //new OverpassQuery(areaId)
                // .WithWays("waterway", "river")
                //	.WithWays("natural", "water")
                //	.ToGeoJSONFile(dirname + "/rivers.geojson")
                //	.RunNow();

                //Delay(msDelay);

                //new OverpassQuery(areaId)
                //	.WithRelations("landuse", "reservoir")
                //	.WithRelations("natural", "water")
                //	.ToGeoJSONFile(dirname + "/lakes.geojson")
                //	.RunNow();

                //Delay(msDelay);

                Console.WriteLine($"{dep.Key} {dep.Value} done");
            }


            BoundingBox bboxLauzannier = new BoundingBox(44.34815879690078, 6.780796051025391, 44.45878010882453, 6.961898803710937);
            // new OverpassQuery(bboxLauzannier).WithNodes("natural", "peak")
            //	.RunAll("output/natural.peak");

            //new OverpassQuery(bboxLauzannier).WithNodes("natural", "saddle")
            //   .RunAll("output/natural.saddle");

            //new OverpassQuery(bboxLauzannier).WithWays("waterway", "river")
            //   .RunAll("output/waterway.river");

            //new OverpassQuery(bboxLauzannier).WithWays("natural", "water")
            //   .RunAll("output/natural.water");

            //new OverpassQuery(bboxLauzannier).WithRelations("landuse", "reservoir")
            //   .RunAll("output/landuse.reservoir");

            new OverpassQuery(bboxLauzannier)
                .WithNodes("natural", "peak")
                .WithNodes("natural", "saddle")
                .WithWays("waterway", "river")
                .WithWays("natural", "water")
                .WithRelations("landuse", "reservoir")
                .WithRelations("natural", "water")
                .ToGeoJSONFile("output/fullBbox.geojson")
                 .RunNow();




            // -----------------------------------------------------------------

            Console.WriteLine("ready...");
            Console.ReadLine();

        }

        private static void Delay(int msDelay)
        {
            Console.Write($"Waiting {msDelay} ms...");
            Task.Delay(msDelay).Wait();
            Console.Write("OK!");
        }
    }

}
